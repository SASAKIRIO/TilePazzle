using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.;

public class Player : MonoBehaviour
{
    //Debug.Log("");
    [SerializeField] private MapArrayEditor mapArrayEditor;
    [Header("何秒でタイル移動するか")]
    [SerializeField] private float MoveSpeedTime;

    [Header("香り")]
    [SerializeField] private string Smell = default;


    private int[,] Map = default;

    private bool _isEvent = false;
    private bool _isMoveOK = true;

    private void Start()
    {
        Map = mapArrayEditor.Map;
    }

    private void Update()
    {
        bool UpArrow    = Input.GetKey(KeyCode.UpArrow);
        bool DownArrow  = Input.GetKey(KeyCode.DownArrow);
        bool LeftArrow  = Input.GetKey(KeyCode.LeftArrow);
        bool RightArrow = Input.GetKey(KeyCode.RightArrow);


        if (_isEvent) return;

        if      (RightArrow) StartCoroutine(MoveLerp(Vector3.right));
        else if (LeftArrow)  StartCoroutine(MoveLerp(Vector3.left));
        else if (UpArrow)    StartCoroutine(MoveLerp(Vector3.up));
        else if (DownArrow)  StartCoroutine(MoveLerp(Vector3.down));

    }


    private IEnumerator MoveLerp(Vector3 Direction)
    {
        // イベント状態にする
        _isEvent = true;

        // 今の位置と移動しようとしている位置を格納
        Vector3 NowPos = transform.position;
        Vector3 NextPos = NowPos + Direction;

        //移動先が移動可能か
        CanMove(NextPos);

        if (!_isMoveOK)
        {
            _isEvent = false;
            yield break;
        }

        // Lerpで移動
        for (float i = 0; i <= 1; i += 0.01f)
        {
            transform.position = Vector3.Lerp(NowPos, NextPos, i);
            yield return new WaitForSeconds(Time.deltaTime * MoveSpeedTime);
        }

        // 位置ずれ防止の為に修正
        transform.position = NextPos;

        // イベント状態を切る
        _isEvent = false;
    }



    private void CanMove(Vector3 PlanPos)
    {
        //PlanPosのxとyを格納
        int x = (int)PlanPos.x;
        int y = (int)PlanPos.y;

        try
        {
            int tileType = Map[x, y];

            //もし赤いタイル、何もタイルが無い時
            if (tileType == (int)Tile.Tile_Type.Red ||
                tileType == (int)Tile.Tile_Type.None)
            {
                // 進行許可しない
                _isMoveOK = false;
                return;
            }

            //それ以外の場合進行許可する
            _isMoveOK = true;
        }
        catch
        {
            // 進行許可しない
            _isMoveOK = false;
        }
    }
}
