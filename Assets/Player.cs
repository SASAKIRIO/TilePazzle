using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.;

public class Player : MonoBehaviour
{
    // 変数部ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    // マップ配列を格納する為のスクリプトを取得 
    [SerializeField] private MapArrayEditor _mapArrayEditor;

    [Header("何秒でタイル移動するか")]
    [SerializeField] private float _MoveSpeedTime;

    // 香りのenum情報
    private enum Smell_type
    {
        None,
        Orange,
        Lemon,
    }

    [Header("香り")]
    [SerializeField] private Smell_type _smellType = Smell_type.None;

    // マップを格納する為の二次元配列を格納
    private int[,] _Map = default;

    // 移動中かどうかを判断するbool型変数, trueでプレイヤーがLerp移動中、falseでプレイヤーが移動完了
    //private bool _isMoving = false;

    // タイルが移動可能かを判断するbool型変数
    private bool _isSafeTile = true;

    // 現在地の格納場所
    private Vector3 _NowPos = default;

    // 移動先の格納場所
    private Vector3 _NextPos = default;

    // コルーチンが稼働中かどうかを取るbool型変数
    private bool[] _isCoroutineFlag = new bool[2];

    // ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // 関数部ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    private void Start()
    {
        // マップを格納
        _Map = _mapArrayEditor.Map;
    }



    private void Update()
    {
        // bool型変数に入力変数を格納　ここをいじれば入力処理の変更が可能
        bool UpArrow    = Input.GetKey(KeyCode.UpArrow);
        bool DownArrow  = Input.GetKey(KeyCode.DownArrow);
        bool LeftArrow  = Input.GetKey(KeyCode.LeftArrow);
        bool RightArrow = Input.GetKey(KeyCode.RightArrow);

        // 何かしらのコルーチンが稼働しているか
        bool _isMoving = _isCoroutineFlag[0] || _isCoroutineFlag[1];

        // 移動中の時は入力を受け付けずreturnで返す
        if (_isMoving) return;

        // Moveコルーチンで移動する。
        if      (RightArrow) StartCoroutine(Move(Vector3.right));
        else if (LeftArrow)  StartCoroutine(Move(Vector3.left));
        else if (UpArrow)    StartCoroutine(Move(Vector3.up));
        else if (DownArrow)  StartCoroutine(Move(Vector3.down));

    }



    /// <summary>
    /// 移動コルーチン
    /// </summary>
    /// <param name="Direction"></param>
    /// <returns></returns>
    private IEnumerator Move(Vector3 Direction)
    {
        // 移動中状態にする
        _isCoroutineFlag[0] = true;

        // 今の位置と移動しようとしている位置を格納
        _NowPos = transform.position;
        _NextPos = _NowPos + Direction;

        // 移動先が移動可能か
        CanMove(_NextPos);

        // 移動できない状態の場合breakする
        if (!_isSafeTile)
        {
            _isCoroutineFlag[0] = false;
            yield break;
        }

        // Lerpで移動
        yield return NowPos_To_NextPos(_NowPos, _NextPos);

        // 移動中状態を切る
        _isCoroutineFlag[0] = false;
    }



    /// <summary>
    /// 現在のポジションから特定のポジションへLerpで動かす
    /// </summary>
    /// <param name="NowPos">開始のポジション</param>
    /// <param name="NextPos">終了のポジション</param>
    /// <param name="_isBackTile">行って戻るか否かのbool型変数</param>
    /// <returns></returns>
    private IEnumerator NowPos_To_NextPos(Vector3 NowPos, Vector3 NextPos, bool _isBackTile = false)
    {
        // コルーチンの稼働を宣言
        _isCoroutineFlag[1] = true;

        // Lerpで移動
        for (float i = 0; i <= 1; i += 0.01f)
        {
            // ポジションをLerpで移動
            transform.position = Vector3.Lerp(NowPos, NextPos, i);
            yield return new WaitForSeconds(Time.deltaTime * _MoveSpeedTime);
        }

        // 位置ずれ防止の為に修正
        transform.position = NextPos;

        // 戻るタイルの場合、再帰的に関数を呼び、場所を戻す
        if (_isBackTile)
        {
            yield return NowPos_To_NextPos(NextPos, NowPos);
        }

        // コルーチンの終了を宣言
        _isCoroutineFlag[1] = false;
    }



    /// <summary>
    /// 動けるタイルか確認
    /// </summary>
    /// <param name="PlanPos"></param>
    private void CanMove(Vector3 PlanPos)
    {
        // PlanPosのxとyを格納
        int x = (int)PlanPos.x;
        int y = (int)PlanPos.y;

        // 配列内の場合
        try
        {
            // タイルの種類を格納。Tile.Tile_Typeのenum変数で比較すること
            int tileType = _Map[x, y];

            // タイルの効果を発動させる
            TileLogic(tileType);
        }
        // 配列外の場合
        catch
        {
            // 進行許可しない
            _isSafeTile = false;
        }
    }



    /// <summary>
    /// タイルのロジック処理
    /// </summary>
    /// <param name="tileType">移動先のタイルの種類</param>
    private void TileLogic(int tileType)
    {
        //もし赤いタイル、何もタイルが無い時
        if (tileType == (int)Tile.Tile_Type.Red ||
            tileType == (int)Tile.Tile_Type.None)
        {
            // 進行許可しない
            _isSafeTile = false;
            return;
        }
        //もし紫かオレンジのタイルの場合
        else if (tileType == (int)Tile.Tile_Type.Orange ||
                 tileType == (int)Tile.Tile_Type.Purple)
        {
            SmellTile(tileType);
        }
        //もし黄色のタイルの場合
        else if (tileType == (int)Tile.Tile_Type.Yellow)
        {
            // 一回行って帰って来る。
            StartCoroutine(NowPos_To_NextPos(_NowPos, _NextPos, true));

            // 進行許可しない
            _isSafeTile = false;
            return;
        }

        //それ以外の場合進行許可する
        _isSafeTile = true;
    }



    /// <summary>
    /// 香りをつけるロジック
    /// </summary>
    /// <param name="tileType"></param>
    private void SmellTile(int tileType)
    {
        // タイルの種類がオレンジの場合
        if (tileType == (int)Tile.Tile_Type.Orange)
        {
            // オレンジの香りを付ける
            _smellType = Smell_type.Orange;
        }
        // タイルの種類が紫の場合
        else if(tileType == (int)Tile.Tile_Type.Purple)
        {
            // レモンの香りを付ける
            _smellType = Smell_type.Lemon;
        }
        // それ以外の時
        else
        {
            Debug.Log("香りのつくタイルではないです");
        }
    }

    // ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
}
