using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.;

public class Player : MonoBehaviour
{
    // �ϐ����[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

    // �}�b�v�z����i�[����ׂ̃X�N���v�g���擾 
    [SerializeField] private MapArrayEditor _mapArrayEditor;

    [Header("���b�Ń^�C���ړ����邩")]
    [SerializeField] private float _MoveSpeedTime;

    // �����enum���
    private enum Smell_type
    {
        None,
        Orange,
        Lemon,
    }

    [Header("����")]
    [SerializeField] private Smell_type _smellType = Smell_type.None;

    // �}�b�v���i�[����ׂ̓񎟌��z����i�[
    private int[,] _Map = default;

    // �ړ������ǂ����𔻒f����bool�^�ϐ�, true�Ńv���C���[��Lerp�ړ����Afalse�Ńv���C���[���ړ�����
    //private bool _isMoving = false;

    // �^�C�����ړ��\���𔻒f����bool�^�ϐ�
    private bool _isSafeTile = true;

    // ���ݒn�̊i�[�ꏊ
    private Vector3 _NowPos = default;

    // �ړ���̊i�[�ꏊ
    private Vector3 _NextPos = default;

    // �R���[�`�����ғ������ǂ��������bool�^�ϐ�
    private bool[] _isCoroutineFlag = new bool[2];

    // �[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
    // �֐����[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

    private void Start()
    {
        // �}�b�v���i�[
        _Map = _mapArrayEditor.Map;
    }



    private void Update()
    {
        // bool�^�ϐ��ɓ��͕ϐ����i�[�@������������Γ��͏����̕ύX���\
        bool UpArrow    = Input.GetKey(KeyCode.UpArrow);
        bool DownArrow  = Input.GetKey(KeyCode.DownArrow);
        bool LeftArrow  = Input.GetKey(KeyCode.LeftArrow);
        bool RightArrow = Input.GetKey(KeyCode.RightArrow);

        // ��������̃R���[�`�����ғ����Ă��邩
        bool _isMoving = _isCoroutineFlag[0] || _isCoroutineFlag[1];

        // �ړ����̎��͓��͂��󂯕t����return�ŕԂ�
        if (_isMoving) return;

        // Move�R���[�`���ňړ�����B
        if      (RightArrow) StartCoroutine(Move(Vector3.right));
        else if (LeftArrow)  StartCoroutine(Move(Vector3.left));
        else if (UpArrow)    StartCoroutine(Move(Vector3.up));
        else if (DownArrow)  StartCoroutine(Move(Vector3.down));

    }



    /// <summary>
    /// �ړ��R���[�`��
    /// </summary>
    /// <param name="Direction"></param>
    /// <returns></returns>
    private IEnumerator Move(Vector3 Direction)
    {
        // �ړ�����Ԃɂ���
        _isCoroutineFlag[0] = true;

        // ���̈ʒu�ƈړ����悤�Ƃ��Ă���ʒu���i�[
        _NowPos = transform.position;
        _NextPos = _NowPos + Direction;

        // �ړ��悪�ړ��\��
        CanMove(_NextPos);

        // �ړ��ł��Ȃ���Ԃ̏ꍇbreak����
        if (!_isSafeTile)
        {
            _isCoroutineFlag[0] = false;
            yield break;
        }

        // Lerp�ňړ�
        yield return NowPos_To_NextPos(_NowPos, _NextPos);

        // �ړ�����Ԃ�؂�
        _isCoroutineFlag[0] = false;
    }



    /// <summary>
    /// ���݂̃|�W�V�����������̃|�W�V������Lerp�œ�����
    /// </summary>
    /// <param name="NowPos">�J�n�̃|�W�V����</param>
    /// <param name="NextPos">�I���̃|�W�V����</param>
    /// <param name="_isBackTile">�s���Ė߂邩�ۂ���bool�^�ϐ�</param>
    /// <returns></returns>
    private IEnumerator NowPos_To_NextPos(Vector3 NowPos, Vector3 NextPos, bool _isBackTile = false)
    {
        // �R���[�`���̉ғ���錾
        _isCoroutineFlag[1] = true;

        // Lerp�ňړ�
        for (float i = 0; i <= 1; i += 0.01f)
        {
            // �|�W�V������Lerp�ňړ�
            transform.position = Vector3.Lerp(NowPos, NextPos, i);
            yield return new WaitForSeconds(Time.deltaTime * _MoveSpeedTime);
        }

        // �ʒu����h�~�ׂ̈ɏC��
        transform.position = NextPos;

        // �߂�^�C���̏ꍇ�A�ċA�I�Ɋ֐����ĂсA�ꏊ��߂�
        if (_isBackTile)
        {
            yield return NowPos_To_NextPos(NextPos, NowPos);
        }

        // �R���[�`���̏I����錾
        _isCoroutineFlag[1] = false;
    }



    /// <summary>
    /// ������^�C�����m�F
    /// </summary>
    /// <param name="PlanPos"></param>
    private void CanMove(Vector3 PlanPos)
    {
        // PlanPos��x��y���i�[
        int x = (int)PlanPos.x;
        int y = (int)PlanPos.y;

        // �z����̏ꍇ
        try
        {
            // �^�C���̎�ނ��i�[�BTile.Tile_Type��enum�ϐ��Ŕ�r���邱��
            int tileType = _Map[x, y];

            // �^�C���̌��ʂ𔭓�������
            TileLogic(tileType);
        }
        // �z��O�̏ꍇ
        catch
        {
            // �i�s�����Ȃ�
            _isSafeTile = false;
        }
    }



    /// <summary>
    /// �^�C���̃��W�b�N����
    /// </summary>
    /// <param name="tileType">�ړ���̃^�C���̎��</param>
    private void TileLogic(int tileType)
    {
        //�����Ԃ��^�C���A�����^�C����������
        if (tileType == (int)Tile.Tile_Type.Red ||
            tileType == (int)Tile.Tile_Type.None)
        {
            // �i�s�����Ȃ�
            _isSafeTile = false;
            return;
        }
        //���������I�����W�̃^�C���̏ꍇ
        else if (tileType == (int)Tile.Tile_Type.Orange ||
                 tileType == (int)Tile.Tile_Type.Purple)
        {
            SmellTile(tileType);
        }
        //�������F�̃^�C���̏ꍇ
        else if (tileType == (int)Tile.Tile_Type.Yellow)
        {
            // ���s���ċA���ė���B
            StartCoroutine(NowPos_To_NextPos(_NowPos, _NextPos, true));

            // �i�s�����Ȃ�
            _isSafeTile = false;
            return;
        }

        //����ȊO�̏ꍇ�i�s������
        _isSafeTile = true;
    }



    /// <summary>
    /// ��������郍�W�b�N
    /// </summary>
    /// <param name="tileType"></param>
    private void SmellTile(int tileType)
    {
        // �^�C���̎�ނ��I�����W�̏ꍇ
        if (tileType == (int)Tile.Tile_Type.Orange)
        {
            // �I�����W�̍����t����
            _smellType = Smell_type.Orange;
        }
        // �^�C���̎�ނ����̏ꍇ
        else if(tileType == (int)Tile.Tile_Type.Purple)
        {
            // �������̍����t����
            _smellType = Smell_type.Lemon;
        }
        // ����ȊO�̎�
        else
        {
            Debug.Log("����̂��^�C���ł͂Ȃ��ł�");
        }
    }

    // �[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
}
