using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.;

public class Player : MonoBehaviour
{
    //Debug.Log("");
    [SerializeField] private MapArrayEditor mapArrayEditor;
    [Header("���b�Ń^�C���ړ����邩")]
    [SerializeField] private float MoveSpeedTime;

    [Header("����")]
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
        // �C�x���g��Ԃɂ���
        _isEvent = true;

        // ���̈ʒu�ƈړ����悤�Ƃ��Ă���ʒu���i�[
        Vector3 NowPos = transform.position;
        Vector3 NextPos = NowPos + Direction;

        //�ړ��悪�ړ��\��
        CanMove(NextPos);

        if (!_isMoveOK)
        {
            _isEvent = false;
            yield break;
        }

        // Lerp�ňړ�
        for (float i = 0; i <= 1; i += 0.01f)
        {
            transform.position = Vector3.Lerp(NowPos, NextPos, i);
            yield return new WaitForSeconds(Time.deltaTime * MoveSpeedTime);
        }

        // �ʒu����h�~�ׂ̈ɏC��
        transform.position = NextPos;

        // �C�x���g��Ԃ�؂�
        _isEvent = false;
    }



    private void CanMove(Vector3 PlanPos)
    {
        //PlanPos��x��y���i�[
        int x = (int)PlanPos.x;
        int y = (int)PlanPos.y;

        try
        {
            int tileType = Map[x, y];

            //�����Ԃ��^�C���A�����^�C����������
            if (tileType == (int)Tile.Tile_Type.Red ||
                tileType == (int)Tile.Tile_Type.None)
            {
                // �i�s�����Ȃ�
                _isMoveOK = false;
                return;
            }

            //����ȊO�̏ꍇ�i�s������
            _isMoveOK = true;
        }
        catch
        {
            // �i�s�����Ȃ�
            _isMoveOK = false;
        }
    }
}
