// �r���h�G���[�΍��p�X�j�y�b�g
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

// �N������static�R���X�g���N�^�[���\�b�h�����s�B
[InitializeOnLoad]
public static class CreateTileEditor
{
    // �^�C���̐F��enum�ϐ�
    public enum Status
    {
        None,
        White,
        Black,
        Green,
        Pink,
        Red,
        Yellow,
        Blue,
        Orange,
        Purple,
    }

    // ���݃{�^���őI�����Ă���F��ۑ�����ϐ�
    public static Status _status = Status.None;

    // �w�肵���^�C�����i�[����ϐ�
    public static GameObject _selectedTile = default;

    // �^�C���f�[�^���i�[����Ă���X�N���v�^�u���I�u�W�F�N�g���i�[
    private static TileDataTable _tileDataTable;

    // �^�C���ҏW���[�h���ۂ��𔻒肷��bool�ϐ�
    public static bool _tileMode = false;



    /// <summary>
    /// �R���X�g���N�^�[���\�b�h�@SceneGUI��OnGUI���\�b�h�����Z�i�d�˂�j
    /// </summary>
    static CreateTileEditor()
    {
        // SceneGUI��OnGUI���\�b�h�����Z�i�d�˂�j
        SceneView.duringSceneGui += OnGUI;
    }



    /// <summary>
    /// SceneGUI�ɉ��Z����GUI���쐬
    /// </summary>
    /// <param name="sceneView">�V�[���r���[�i�����ۑ��j</param>
    private static void OnGUI(SceneView sceneView)
    {
        // �^�C�����[�h�̐؂�ւ�������{�^���̑傫����ݒ肷��float���[�J���ϐ�
        float loc_switchButtonSizeX = 80;
        float loc_switchButtonSizeY = 60;

        /*
         * �^�C�����[�h��ON�AOFF���ł���{�^����\������g��
         */

        // �^�C�����[�h�łȂ���
        if (_tileMode == false)
        {
            // �{�^���`��̊J�n
            Handles.BeginGUI();

            // �{�^���ʒu��Rect�ϐ��ɐݒ�
            Rect rect = new Rect(8, 300, loc_switchButtonSizeX, loc_switchButtonSizeY);

            // �{�^���̋����A��������Tile���[�h��ON�ɂȂ�
            if (GUI.Button(rect, "Tile���[�hOFF"))
            {
                _tileMode = true;
            }

            // �{�^���`��̏I��
            Handles.EndGUI();
            return;
        }
        else
        {
            // �{�^���`��̊J�n
            Handles.BeginGUI();

            // �{�^���ʒu��Rect�ϐ��ɐݒ�
            Rect rect = new Rect(8, 300, loc_switchButtonSizeX, loc_switchButtonSizeY);

            // �{�^���̋����A��������Tile���[�h��OFF�ɂȂ�
            if (GUI.Button(rect, "Tile���[�hON"))
            {
                _tileMode = false;
            }

            // �{�^���`��̏I��
            Handles.EndGUI();
        }

        // �X�N���v�^�u���I�u�W�F�N�g����̏ꍇ
        if (_tileDataTable == null)
        {
            // �X�N���v�^�u���I�u�W�F�N�g���A�Z�b�g�f�[�^�x�[�X����Ċi�[�B
            _tileDataTable = LoadDataTable();
        }


        // �{�^���`��̊J�n
        Handles.BeginGUI();

        // �{�^�����o���B
        ShowButtons(sceneView.position.size);

        // �{�^���`��̏I��
        Handles.EndGUI();
    }



    /// <summary>
    /// �X�N���v�^�u���I�u�W�F�N�g���擾
    /// </summary>
    /// <returns>�p�X�w�肵���I�u�W�F�N�g�f�[�^���擾</returns>
    private static TileDataTable LoadDataTable()
    {
        // �w�肵���p�X�ɂ���ScriptableObject����f�[�^����������B
        return AssetDatabase.LoadAssetAtPath<TileDataTable>("Assets/Data/TileDataTable.asset");
    }


    /// <summary>
    /// �{�^���̕`��֐�
    /// </summary>
    private static void ShowButtons(Vector2 sceneSize)
    {
        // Tile�̌����i�[�@ScriptableObject�̗v�f���Q��
        int loc_count = _tileDataTable.dataList.Count;

        // �{�^���̑�����
        float loc_buttonSize = 60;

        // �{�^���ƃ{�^���̊Ԋu(px)
        float loc_padding = 2;

        for (var i = 0; i < loc_count; i++)
        {
            // ScriptableObject�̗v�f���̈���i�[
            TileData _tileData = _tileDataTable.dataList[i];

            // �{�^���ʒu
            Rect loc_rect = new Rect(sceneSize.x / 2 - loc_buttonSize * loc_count / 2 + loc_buttonSize * i + loc_padding * i,
                                 sceneSize.y - loc_buttonSize * 1.6f,
                                 loc_buttonSize,
                                 loc_buttonSize);



            // �{�^�����������Ƃ��B
            if (GUI.Button(loc_rect, _tileData.icon.texture))
            {
                // �ʒu�ɂ���ď�����ς���B
                switch (i + 1)
                {
                    case (int)Status.White:

                        // �I�����Ă���F�𔒂ɂ���B
                        _status = Status.White;
                        break;

                    case (int)Status.Black:

                        // �I�����Ă���F�����ɂ���B
                        _status = Status.Black;
                        break;

                    case (int)Status.Green:

                        // �I�����Ă���F��΂ɂ���B
                        _status = Status.Green;
                        break;

                    case (int)Status.Pink:

                        // �I�����Ă���F���s���N�ɂ���B
                        _status = Status.Pink;
                        break;

                    case (int)Status.Red:

                        // �I�����Ă���F��Ԃɂ���B
                        _status = Status.Red;
                        break;

                    case (int)Status.Yellow:

                        // �I�����Ă���F�����F�ɂ���B
                        _status = Status.Yellow;
                        break;

                    case (int)Status.Blue:

                        // �I�����Ă���F��ɂ���B
                        _status = Status.Blue;
                        break;

                    case (int)Status.Orange:

                        // �I�����Ă���F���I�����W�ɂ���B
                        _status = Status.Orange;
                        break;

                    case (int)Status.Purple:

                        // �I�����Ă���F�����ɂ���B
                        _status = Status.Purple;
                        break;

                    default:

                        // �I�����Ă���F�𖳂��ɂ���B
                        _status = Status.None;
                        break;

                }

                // �w��^�C�����i�[
                _selectedTile = _tileData.gameObject;

                // �^�C��������Click�X�N���v�g�Ő����B
            }
        }
    }
}
#endif