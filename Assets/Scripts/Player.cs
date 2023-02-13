using System.Collections;
using UnityEngine;
//using UnityEngine.;

public class Player : MonoBehaviour
{
    // 変数部ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    // マップ配列を格納する為のスクリプトを取得 
    [SerializeField] 
    private MapArray _mapArray = default;

    [Header("スタート地点")]
    [SerializeField]
    private GameObject _startPos = default;

    [Header("ゴール地点")]
    [SerializeField]
    private GameObject _goalPos = default;

    [Header("何秒でタイル移動するか")]
    [SerializeField] 
    private float _MoveSpeed = default;

    // 香りのenum情報
    private enum Smell_type
    {
        None,
        Orange,
        Lemon,
    }

    [Header("香り")]
    [SerializeField] 
    private Smell_type _smellType = Smell_type.None;

    // マップを格納する為の二次元配列を格納
    private int[,] _Map = default;

    [Header("確認用")]
    // 現在地の格納場所
    private Vector3 _NowPos = default;

    // 移動先の格納場所
    private Vector3 _NextPos = default;

    // 移動先の格納場所の延長１マス
    private Vector3 _AfterNextPos = default;

    // 移動先の格納場所の延長１マス
    private Vector3 _PurpleNextPos = default;

    // コルーチンが稼働中かどうかを取るbool型変数
    private bool[] _isCoroutineFlag = new bool[2];

    // 進行方向
    private Vector3 _Direction = Vector3.zero;

    // 連続している紫の数
    private int PurpleLength = 1;

    // 滑り先が黄色だった時にあげるフラグ
    private bool _SlipYellow = false;

    // ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // 関数部ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    private void Start()
    {
        // マップを格納
        _Map = _mapArray.Map;

        // スタート位置に
        transform.position = _startPos.transform.position;
    }



    private void Update()
    {

        // bool型変数に入力変数を格納　ここをいじれば入力処理の変更が可能
        bool UpArrow    = Input.GetKeyDown(KeyCode.UpArrow);
        bool DownArrow  = Input.GetKeyDown(KeyCode.DownArrow);
        bool LeftArrow  = Input.GetKeyDown(KeyCode.LeftArrow);
        bool RightArrow = Input.GetKeyDown(KeyCode.RightArrow);

        // 何かしらのコルーチンが稼働しているか
        bool _isMoving = _isCoroutineFlag[0] || _isCoroutineFlag[1];

        // 移動中の時は入力を受け付けずreturnで返す
        if (_isMoving) return;

        // 入力されていない時に
        if (!(UpArrow || DownArrow || LeftArrow || RightArrow)) return;

        // Moveコルーチンで移動する。
        // _Directionに進行方向を格納する。
        if (RightArrow)
        {
            _Direction = Vector3.right;
            Move(_Direction);
        }
        else if (LeftArrow)
        {
            _Direction = Vector3.left;
            Move(_Direction);
        }
        else if (UpArrow)
        {
            _Direction = Vector3.up;
            Move(_Direction);
        }
        else if (DownArrow)
        {
            _Direction = Vector3.down;
            Move(_Direction);
        }

    }



    /// <summary>
    /// 移動コルーチン
    /// </summary>
    /// <param name="Direction"></param>
    /// <returns></returns>
    private void Move(Vector3 Direction)
    {
        // 移動中状態にする
        _isCoroutineFlag[0] = true;

        // 今の位置と移動しようとしている位置を格納
        _NowPos = transform.position;
        _NextPos = _NowPos + Direction;
        _AfterNextPos = _NextPos + Direction;

        // 紫の個数を格納
        GetPurpleLength(_NowPos, Direction);

        // 紫のスライド移動先
        _PurpleNextPos = _NowPos + Direction * (PurpleLength);

        // 移動先が移動可能かを判定し、可能であれば移動する。
        CanMove(_NextPos);

        // 移動中状態を切る
        _isCoroutineFlag[0] = false;

    }



    /// <summary>
    /// 現在のポジションから特定のポジションへLerpで動かす
    /// </summary>
    /// <param name="NowPos">開始のポジション</param>
    /// <param name="NextPos">終了のポジション</param>
    /// <param name="tileFeature">特殊なタイルの場合を取るTileFeature型変数</param>
    /// <returns></returns>
    private IEnumerator NowPos_To_NextPos(Vector3 NowPos, Vector3 NextPos, Tile.Tile_Feature tileFeature = Tile.Tile_Feature.None)
    {
        // コルーチンの稼働を宣言
        _isCoroutineFlag[1] = true;

        // 紫のスリップ移動以外なら
        if (tileFeature != Tile.Tile_Feature.Slip)
        {
            // Lerpで移動
            for (float i = 0; i <= 1; i += _MoveSpeed / PurpleLength)
            {
                // ポジションをLerpで移動
                transform.position = Vector3.Lerp(NowPos, NextPos, i);
                yield return null;
            }
        }
        // 紫のスリップ移動なら
        else
        {
            // 紫タイルの最終移動先を移動先に設定
            NextPos = _PurpleNextPos;

            // Lerpで移動
            for (float i = 0; i <= 1; i += _MoveSpeed / PurpleLength)
            {
                // ポジションをLerpで移動
                transform.position = Vector3.Lerp(NowPos, NextPos, i);
                yield return null;
            }

            // 移動先のタイルの香りをつける
            try
            {
                int x = (int)NextPos.x;
                int y = (int)NextPos.y;
                int tileType = _Map[x, y];

                SmellTile(tileType);
            }
            catch　{}
        }


        // 位置ずれ防止の為に修正
        transform.position = NextPos;

        // 戻るタイルの場合、再帰的に関数を呼び、場所を戻す
        if (tileFeature == Tile.Tile_Feature.Bounce || _SlipYellow)
        {
            // 方向を反転させる。
            _Direction *= -1;

            // 滑った先が黄色だったかというフラグを下げる
            _SlipYellow = false;

            // もどる。
            yield return NowPos_To_NextPos(NextPos, NowPos);
        }

        GoalCheck(NextPos);
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

        }
    }



    /// <summary>
    /// タイルのロジック処理
    /// </summary>
    /// <param name="tileType">移動先のタイルの種類</param>
    private void TileLogic(int tileType)
    {
        switch (tileType)
        {
            case (int)Tile.Tile_Type.None:// 完成
                // 進行許可しない
                return;

            case (int)Tile.Tile_Type.Red:// 完成
                // 進行許可しない
                return;

            case (int)Tile.Tile_Type.Orange:// 完成
                SmellTile(tileType);
                break;

            case (int)Tile.Tile_Type.Purple:// 完成

                SmellTile(tileType);

                // 滑る
                StartCoroutine(NowPos_To_NextPos(_NowPos, _AfterNextPos, Tile.Tile_Feature.Slip));

                return;

            case (int)Tile.Tile_Type.Yellow:// 完成

                // 一回行って帰って来る
                StartCoroutine(NowPos_To_NextPos(_NowPos, _NextPos, Tile.Tile_Feature.Bounce));

                return;

            case (int)Tile.Tile_Type.Blue:// 完成

                // 周囲に黄色がある時
                if (GetNearYellowTile(_NextPos))
                {
                    // 感電により、一回行って帰って来る
                    StartCoroutine(NowPos_To_NextPos(_NowPos, _NextPos, Tile.Tile_Feature.Bounce));
                }

                // オレンジの香りがついている時
                if (_smellType == Smell_type.Orange)
                {
                    // 一回行って帰って来る。
                    StartCoroutine(NowPos_To_NextPos(_NowPos, _NextPos, Tile.Tile_Feature.Bounce));

                    return;
                }

                // レモンの香りか香りがついていない時には普通に通過できる

                break;

        }

        // Lerpで移動
        StartCoroutine(NowPos_To_NextPos(_NowPos, _NextPos));
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
            
        }
    }



    /// <summary>
    /// 紫がどれだけ連続して続いてるかをとるメソッド
    /// </summary>
    /// <param name="NowPos">現在地</param>
    /// <param name="Direction">進む方向</param>
    private void GetPurpleLength(Vector3 NowPos,Vector3 Direction)
    {
        try
        {
            // タイルの種類を取得する
            int x = (int)NowPos.x;
            int y = (int)NowPos.y;
            int tileType = _Map[x, y];

            // 香りを付ける
            SmellTile(tileType);


            // 紫の連続数をリセット
            PurpleLength = 1;

            while (true)
            {
                // 方向によって探索方向を変える。
                if      (Direction == Vector3.up)    y++;
                else if (Direction == Vector3.down)  y--;
                else if (Direction == Vector3.left)  x--;
                else if (Direction == Vector3.right) x++;

                // 探索先のタイルを配列から探す
                tileType = _Map[x, y];

                // タイルが無くなった時紫の連続数を調整。
                if (tileType == (int)Tile.Tile_Type.None)
                {
                    // 直前にする為に減算
                    PurpleLength--;
                    break;
                }

                // 紫から続いている黄色であるか
                if (tileType == (int)Tile.Tile_Type.Yellow && PurpleLength >= 2)
                {
                    // 滑り先が黄色であることを知らせるフラグをtrueに
                    _SlipYellow = true;
                    break;
                }

                // それが紫じゃない時にループをブレイクする
                if (tileType != (int)Tile.Tile_Type.Purple) break;

                // 加算
                PurpleLength++;
            }
        }
        catch
        {
            // 紫の連続数を調整
            PurpleLength --;
        }
    }



    /// <summary>
    /// 周囲に黄色パネルがあるかを返すboolメソッド
    /// </summary>
    /// <param name="NextPos"></param>
    /// <returns></returns>
    private bool GetNearYellowTile(Vector3 NextPos)
    {
        int x = (int)NextPos.x;
        int y = (int)NextPos.y;

        // 選んだポジションの上に黄色がある場合trueを返す
        try { if (_Map[x + 1, y] == (int)Tile.Tile_Type.Yellow) return true; }
        catch { }

        // 選んだポジションの下に黄色がある場合trueを返す
        try { if (_Map[x - 1, y] == (int)Tile.Tile_Type.Yellow) return true; }
        catch { }

        // 選んだポジションの右に黄色がある場合trueを返す
        try { if (_Map[x, y + 1] == (int)Tile.Tile_Type.Yellow) return true; }
        catch { }

        // 選んだポジションの左に黄色がある場合trueを返す
        try { if (_Map[x, y - 1] == (int)Tile.Tile_Type.Yellow) return true; }
        catch { }

        // 前後左右に無い場合falseを返す
        return false;
    }



    /// <summary>
    /// クリアチェック
    /// </summary>
    /// <param name="NextPos"></param>
    private void GoalCheck(Vector3 NextPos)
    {
        try
        {
            // タイルの種類を取得する
            int x = (int)NextPos.x;
            int y = (int)NextPos.y;
            int tileType = _Map[x, y];

            if (tileType == (int)Tile.Tile_Type.Goal)
            {
                Debug.Log("くりあーーー");
            }
        }
        catch { }
    }
    // ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
}
