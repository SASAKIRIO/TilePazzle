// �r���h�G���[�΍��p�X�j�y�b�g
//#if UNITY_EDITOR
using UnityEditor;
//#endif

using UnityEngine;

// �N������static�R���X�g���N�^�[���\�b�h�����s�B
[InitializeOnLoad]
public static class CreateTileEditor
{

//#if UNITY_EDITOR
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

    public static string TileDataTablePath {get;set;} = "Assets/CreateTile/Data/TileDataTable.asset";

    // �{�^���̑�����
    static float loc_buttonSize = 60;

    // �{�^���ƃ{�^���̊Ԋu(px)
    static float loc_padding = 2;

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
        ParamUpdate();

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

    private static void ParamUpdate()
    {
        loc_buttonSize = CreateTileEditorWindow._buttonSize;
        loc_padding = CreateTileEditorWindow._padding;
        TileDataTablePath = CreateTileEditorWindow._tileDataTablePath;
    }

    /// <summary>
    /// �X�N���v�^�u���I�u�W�F�N�g���擾
    /// </summary>
    /// <returns>�p�X�w�肵���I�u�W�F�N�g�f�[�^���擾</returns>
    private static TileDataTable LoadDataTable()
    {
        // �w�肵���p�X�ɂ���ScriptableObject����f�[�^����������B
        return AssetDatabase.LoadAssetAtPath<TileDataTable>(TileDataTablePath);
    }


    /// <summary>
    /// �{�^���̕`��֐�
    /// </summary>
    private static void ShowButtons(Vector2 sceneSize)
    {
        // Tile�̌����i�[�@ScriptableObject�̗v�f���Q��
        int loc_count = _tileDataTable.dataList.Count;


        for (var i = 0; i < loc_count; i++)
        {
            // ScriptableObject�̗v�f���̈���i�[
            TileData _tileData = _tileDataTable.dataList[i];


            /*[   �{�^���ʒu�A�i�I�ׂ�p�ɂ���j   ]*/

            //// �{�^���ʒu
            //Rect loc_rect = new Rect(
            //                        // x
            //                        sceneSize.x / 2 - loc_buttonSize * loc_count / 2 + loc_buttonSize * i + loc_padding * i,
            //                         // y�i�}�[�W���j
            //                         sceneSize.y - loc_buttonSize * 1.6f,
            //                         // width ���T�C�Y
            //                         loc_buttonSize,
            //                         // height �c�T�C�Y
            //                         loc_buttonSize);

            //// �{�^���ʒu(�E��z�u)
            Rect loc_rect = new Rect(

                                     // x�i�}�[�W���j
                                     sceneSize.x - loc_buttonSize * 2,
                                     // y
                                     //sceneSize.y / 2 - loc_buttonSize * loc_count / 2 + loc_buttonSize * i + loc_padding * i,
                                     loc_buttonSize * i + loc_padding * i,
                                     // width ���T�C�Y
                                     loc_buttonSize,
                                     // height �c�T�C�Y
                                     loc_buttonSize

                                    );


            /*[   /�{�^���ʒu�A�i�I�ׂ�p�ɂ���j   ]*/

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
//#endif
}



public sealed class CreateTileEditorWindow : EditorWindow
{
    [MenuItem("Window/Sasaki/CreateTileEditorWindow")]
    private static void ShowParamWindow()
    {
        // Window��\��
        GetWindow<CreateTileEditorWindow>().Show();
    }

    // �{�^���̑�����
    public static float _buttonSize { get; set; } = 40;

    // �{�^���̊Ԋu (px)
    public static float _padding { get; set; } = 2;

    // �^�C����񂪓����Ă���X�N���v�^�u���I�u�W�F�N�g�̃p�X
    public static string _tileDataTablePath { get; set; } = "Assets/CreateTile/Data/TileDataTable.asset";

    public enum Language
    {
        Japanese,
        English,
        Chinese,
    }

    Language language = Language.Japanese;

    private void OnGUI()
    {�@
        // ����ݒ�
        language = (Language)EditorGUILayout.EnumPopup("Language", language);
        
        EditorGUILayout.Space();
        
        // ����ɂ���ĕ\����ς���B
        switch (language)
        {
            case Language.Japanese:
                EditorGUILayout.LabelField("�{�^���ݒ�");

                EditorGUILayout.Space();

                _buttonSize = EditorGUILayout.FloatField("�{�^���̑傫��", _buttonSize);
                _padding = EditorGUILayout.FloatField("�{�^���ƃ{�^���̊Ԋu", _padding);

                EditorGUILayout.Space();


                EditorGUILayout.Space();

                EditorGUILayout.LabelField("�^�C���̏�񂪓����Ă���X�N���v�^�u���I�u�W�F�N�g�̃p�X(�g���q���Y�ꂸ��)");
                _tileDataTablePath = EditorGUILayout.TextField("", _tileDataTablePath);

                break;

            case Language.English:
                EditorGUILayout.LabelField("Button Settings");

                EditorGUILayout.Space();

                _buttonSize = EditorGUILayout.FloatField("size", _buttonSize);
                _padding = EditorGUILayout.FloatField("padding", _padding);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Tile information Path");
                _tileDataTablePath = EditorGUILayout.TextField("", _tileDataTablePath);

                break;
        }
    }
}