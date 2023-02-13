using System.Collections;
using UnityEngine;
//using UnityEngine.;

public class Player : MonoBehaviour
{
    // �ϐ����[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

    // �}�b�v�z����i�[����ׂ̃X�N���v�g���擾 
    [SerializeField] 
    private MapArray _mapArray = default;

    [Header("�X�^�[�g�n�_")]
    [SerializeField]
    private GameObject _startPos = default;

    [Header("�S�[���n�_")]
    [SerializeField]
    private GameObject _goalPos = default;

    [Header("���b�Ń^�C���ړ����邩")]
    [SerializeField] 
    private float _MoveSpeed = default;

    // �����enum���
    private enum Smell_type
    {
        None,
        Orange,
        Lemon,
    }

    [Header("����")]
    [SerializeField] 
    private Smell_type _smellType = Smell_type.None;

    // �}�b�v���i�[����ׂ̓񎟌��z����i�[
    private int[,] _Map = default;

    [Header("�m�F�p")]
    // ���ݒn�̊i�[�ꏊ
    private Vector3 _NowPos = default;

    // �ړ���̊i�[�ꏊ
    private Vector3 _NextPos = default;

    // �ړ���̊i�[�ꏊ�̉����P�}�X
    private Vector3 _AfterNextPos = default;

    // �ړ���̊i�[�ꏊ�̉����P�}�X
    private Vector3 _PurpleNextPos = default;

    // �R���[�`�����ғ������ǂ��������bool�^�ϐ�
    private bool[] _isCoroutineFlag = new bool[2];

    // �i�s����
    private Vector3 _Direction = Vector3.zero;

    // �A�����Ă��鎇�̐�
    private int PurpleLength = 1;

    // ����悪���F���������ɂ�����t���O
    private bool _SlipYellow = false;

    // �[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
    // �֐����[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

    private void Start()
    {
        // �}�b�v���i�[
        _Map = _mapArray.Map;

        // �X�^�[�g�ʒu��
        transform.position = _startPos.transform.position;
    }



    private void Update()
    {

        // bool�^�ϐ��ɓ��͕ϐ����i�[�@������������Γ��͏����̕ύX���\
        bool UpArrow    = Input.GetKeyDown(KeyCode.UpArrow);
        bool DownArrow  = Input.GetKeyDown(KeyCode.DownArrow);
        bool LeftArrow  = Input.GetKeyDown(KeyCode.LeftArrow);
        bool RightArrow = Input.GetKeyDown(KeyCode.RightArrow);

        // ��������̃R���[�`�����ғ����Ă��邩
        bool _isMoving = _isCoroutineFlag[0] || _isCoroutineFlag[1];

        // �ړ����̎��͓��͂��󂯕t����return�ŕԂ�
        if (_isMoving) return;

        // ���͂���Ă��Ȃ�����
        if (!(UpArrow || DownArrow || LeftArrow || RightArrow)) return;

        // Move�R���[�`���ňړ�����B
        // _Direction�ɐi�s�������i�[����B
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
    /// �ړ��R���[�`��
    /// </summary>
    /// <param name="Direction"></param>
    /// <returns></returns>
    private void Move(Vector3 Direction)
    {
        // �ړ�����Ԃɂ���
        _isCoroutineFlag[0] = true;

        // ���̈ʒu�ƈړ����悤�Ƃ��Ă���ʒu���i�[
        _NowPos = transform.position;
        _NextPos = _NowPos + Direction;
        _AfterNextPos = _NextPos + Direction;

        // ���̌����i�[
        GetPurpleLength(_NowPos, Direction);

        // ���̃X���C�h�ړ���
        _PurpleNextPos = _NowPos + Direction * (PurpleLength);

        // �ړ��悪�ړ��\���𔻒肵�A�\�ł���Έړ�����B
        CanMove(_NextPos);

        // �ړ�����Ԃ�؂�
        _isCoroutineFlag[0] = false;

    }



    /// <summary>
    /// ���݂̃|�W�V�����������̃|�W�V������Lerp�œ�����
    /// </summary>
    /// <param name="NowPos">�J�n�̃|�W�V����</param>
    /// <param name="NextPos">�I���̃|�W�V����</param>
    /// <param name="tileFeature">����ȃ^�C���̏ꍇ�����TileFeature�^�ϐ�</param>
    /// <returns></returns>
    private IEnumerator NowPos_To_NextPos(Vector3 NowPos, Vector3 NextPos, Tile.Tile_Feature tileFeature = Tile.Tile_Feature.None)
    {
        // �R���[�`���̉ғ���錾
        _isCoroutineFlag[1] = true;

        // ���̃X���b�v�ړ��ȊO�Ȃ�
        if (tileFeature != Tile.Tile_Feature.Slip)
        {
            // Lerp�ňړ�
            for (float i = 0; i <= 1; i += _MoveSpeed / PurpleLength)
            {
                // �|�W�V������Lerp�ňړ�
                transform.position = Vector3.Lerp(NowPos, NextPos, i);
                yield return null;
            }
        }
        // ���̃X���b�v�ړ��Ȃ�
        else
        {
            // ���^�C���̍ŏI�ړ�����ړ���ɐݒ�
            NextPos = _PurpleNextPos;

            // Lerp�ňړ�
            for (float i = 0; i <= 1; i += _MoveSpeed / PurpleLength)
            {
                // �|�W�V������Lerp�ňړ�
                transform.position = Vector3.Lerp(NowPos, NextPos, i);
                yield return null;
            }

            // �ړ���̃^�C���̍��������
            try
            {
                int x = (int)NextPos.x;
                int y = (int)NextPos.y;
                int tileType = _Map[x, y];

                SmellTile(tileType);
            }
            catch�@{}
        }


        // �ʒu����h�~�ׂ̈ɏC��
        transform.position = NextPos;

        // �߂�^�C���̏ꍇ�A�ċA�I�Ɋ֐����ĂсA�ꏊ��߂�
        if (tileFeature == Tile.Tile_Feature.Bounce || _SlipYellow)
        {
            // �����𔽓]������B
            _Direction *= -1;

            // �������悪���F���������Ƃ����t���O��������
            _SlipYellow = false;

            // ���ǂ�B
            yield return NowPos_To_NextPos(NextPos, NowPos);
        }

        GoalCheck(NextPos);
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

        }
    }



    /// <summary>
    /// �^�C���̃��W�b�N����
    /// </summary>
    /// <param name="tileType">�ړ���̃^�C���̎��</param>
    private void TileLogic(int tileType)
    {
        switch (tileType)
        {
            case (int)Tile.Tile_Type.None:// ����
                // �i�s�����Ȃ�
                return;

            case (int)Tile.Tile_Type.Red:// ����
                // �i�s�����Ȃ�
                return;

            case (int)Tile.Tile_Type.Orange:// ����
                SmellTile(tileType);
                break;

            case (int)Tile.Tile_Type.Purple:// ����

                SmellTile(tileType);

                // ����
                StartCoroutine(NowPos_To_NextPos(_NowPos, _AfterNextPos, Tile.Tile_Feature.Slip));

                return;

            case (int)Tile.Tile_Type.Yellow:// ����

                // ���s���ċA���ė���
                StartCoroutine(NowPos_To_NextPos(_NowPos, _NextPos, Tile.Tile_Feature.Bounce));

                return;

            case (int)Tile.Tile_Type.Blue:// ����

                // ���͂ɉ��F�����鎞
                if (GetNearYellowTile(_NextPos))
                {
                    // ���d�ɂ��A���s���ċA���ė���
                    StartCoroutine(NowPos_To_NextPos(_NowPos, _NextPos, Tile.Tile_Feature.Bounce));
                }

                // �I�����W�̍��肪���Ă��鎞
                if (_smellType == Smell_type.Orange)
                {
                    // ���s���ċA���ė���B
                    StartCoroutine(NowPos_To_NextPos(_NowPos, _NextPos, Tile.Tile_Feature.Bounce));

                    return;
                }

                // �������̍��肩���肪���Ă��Ȃ����ɂ͕��ʂɒʉ߂ł���

                break;

        }

        // Lerp�ňړ�
        StartCoroutine(NowPos_To_NextPos(_NowPos, _NextPos));
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
            
        }
    }



    /// <summary>
    /// �����ǂꂾ���A�����đ����Ă邩���Ƃ郁�\�b�h
    /// </summary>
    /// <param name="NowPos">���ݒn</param>
    /// <param name="Direction">�i�ޕ���</param>
    private void GetPurpleLength(Vector3 NowPos,Vector3 Direction)
    {
        try
        {
            // �^�C���̎�ނ��擾����
            int x = (int)NowPos.x;
            int y = (int)NowPos.y;
            int tileType = _Map[x, y];

            // �����t����
            SmellTile(tileType);


            // ���̘A���������Z�b�g
            PurpleLength = 1;

            while (true)
            {
                // �����ɂ���ĒT��������ς���B
                if      (Direction == Vector3.up)    y++;
                else if (Direction == Vector3.down)  y--;
                else if (Direction == Vector3.left)  x--;
                else if (Direction == Vector3.right) x++;

                // �T����̃^�C����z�񂩂�T��
                tileType = _Map[x, y];

                // �^�C���������Ȃ��������̘A�����𒲐��B
                if (tileType == (int)Tile.Tile_Type.None)
                {
                    // ���O�ɂ���ׂɌ��Z
                    PurpleLength--;
                    break;
                }

                // �����瑱���Ă��鉩�F�ł��邩
                if (tileType == (int)Tile.Tile_Type.Yellow && PurpleLength >= 2)
                {
                    // ����悪���F�ł��邱�Ƃ�m�点��t���O��true��
                    _SlipYellow = true;
                    break;
                }

                // ���ꂪ������Ȃ����Ƀ��[�v���u���C�N����
                if (tileType != (int)Tile.Tile_Type.Purple) break;

                // ���Z
                PurpleLength++;
            }
        }
        catch
        {
            // ���̘A�����𒲐�
            PurpleLength --;
        }
    }



    /// <summary>
    /// ���͂ɉ��F�p�l�������邩��Ԃ�bool���\�b�h
    /// </summary>
    /// <param name="NextPos"></param>
    /// <returns></returns>
    private bool GetNearYellowTile(Vector3 NextPos)
    {
        int x = (int)NextPos.x;
        int y = (int)NextPos.y;

        // �I�񂾃|�W�V�����̏�ɉ��F������ꍇtrue��Ԃ�
        try { if (_Map[x + 1, y] == (int)Tile.Tile_Type.Yellow) return true; }
        catch { }

        // �I�񂾃|�W�V�����̉��ɉ��F������ꍇtrue��Ԃ�
        try { if (_Map[x - 1, y] == (int)Tile.Tile_Type.Yellow) return true; }
        catch { }

        // �I�񂾃|�W�V�����̉E�ɉ��F������ꍇtrue��Ԃ�
        try { if (_Map[x, y + 1] == (int)Tile.Tile_Type.Yellow) return true; }
        catch { }

        // �I�񂾃|�W�V�����̍��ɉ��F������ꍇtrue��Ԃ�
        try { if (_Map[x, y - 1] == (int)Tile.Tile_Type.Yellow) return true; }
        catch { }

        // �O�㍶�E�ɖ����ꍇfalse��Ԃ�
        return false;
    }



    /// <summary>
    /// �N���A�`�F�b�N
    /// </summary>
    /// <param name="NextPos"></param>
    private void GoalCheck(Vector3 NextPos)
    {
        try
        {
            // �^�C���̎�ނ��擾����
            int x = (int)NextPos.x;
            int y = (int)NextPos.y;
            int tileType = _Map[x, y];

            if (tileType == (int)Tile.Tile_Type.Goal)
            {
                Debug.Log("���肠�[�[�[");
            }
        }
        catch { }
    }
    // �[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
}
