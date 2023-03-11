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

    // �w�肵���^�C������ݒ肷��ϐ�
    public static string _selectTileName = default;

    // �w�肵���^�C����ݒ肷��ϐ�
    public static GameObject _selectedTile = default;

    // �w�肵���^�C����string�^tips��ݒ肷��ϐ�
    public static string _tipsString = default;

    // �w�肵���^�C����Texture�^tips��ݒ肷��ϐ�
    public static Texture _tipsTexture = default;

    // �^�C���f�[�^���i�[����Ă���X�N���v�^�u���I�u�W�F�N�g���i�[
    private static TileDataTable _tileDataTable;

    // �^�C���ҏW���[�h���ۂ��𔻒肷��bool�ϐ�
    public static bool _tileMode = false;

    public static string _tileDataTablePath {get;set;} = "Assets/CreateTile/Data/TileDataTable.asset";

    // �{�^���̑�����
    private static float _buttonSize = CreateTileEditorWindow._buttonSize;

    // �{�^���ƃ{�^���̊Ԋu(px)
    private static float _padding = CreateTileEditorWindow._padding;

    private static Rect _tileModeButtonRect = CreateTileEditorWindow._tileModeButtonRect;

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

        /*
         * �^�C�����[�h��ON�AOFF���ł���{�^����\������g��
         */

        // �^�C�����[�h�łȂ���
        if (_tileMode == false)
        {
            // �{�^���`��̊J�n
            Handles.BeginGUI();

            // �{�^���̋����A��������Tile���[�h��ON�ɂȂ�
            if (GUI.Button(_tileModeButtonRect, "Tile���[�hOFF"))
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

            // �{�^���̋����A��������Tile���[�h��OFF�ɂȂ�
            if (GUI.Button(_tileModeButtonRect, "Tile���[�hON"))
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
    /// CreateTileEditorWindow����l���X�V
    /// </summary>
    private static void ParamUpdate()
    {
        _buttonSize = CreateTileEditorWindow._buttonSize;
        _padding = CreateTileEditorWindow._padding;
        _tileDataTablePath = CreateTileEditorWindow._tileDataTablePath;
        _tileModeButtonRect = CreateTileEditorWindow._tileModeButtonRect;
    }

    /// <summary>
    /// �X�N���v�^�u���I�u�W�F�N�g���擾
    /// </summary>
    /// <returns>�p�X�w�肵���I�u�W�F�N�g�f�[�^���擾</returns>
    private static TileDataTable LoadDataTable()
    {
        // �w�肵���p�X�ɂ���ScriptableObject����f�[�^����������B
        return AssetDatabase.LoadAssetAtPath<TileDataTable>(_tileDataTablePath);
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



            //// �{�^���ʒu(�}�C�N��)
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
                                     sceneSize.x - _buttonSize * 2,
                                     // y
                                     //sceneSize.y / 2 - loc_buttonSize * loc_count / 2 + loc_buttonSize * i + loc_padding * i,
                                     _buttonSize * i + _padding * i,
                                     // width ���T�C�Y
                                     _buttonSize,
                                     // height �c�T�C�Y
                                     _buttonSize

                                    );






            /*[   /�{�^���ʒu�A�i�I�ׂ�p�ɂ���j   ]*/

            // �{�^�����������Ƃ��B
            if (GUI.Button(loc_rect, _tileData.icon.texture))
            {
                // �w��^�C�����i�[
                _selectTileName = _tileData.Name;
                _selectedTile = _tileData.gameObject;
                _tipsString = _tileData.tipsString;
                _tipsTexture = _tileData.tipsTexture;

                // �^�C��������Click�X�N���v�g�Ő����B
            }
        }
    }
//#endif
}



/// <summary>
/// �{�^���̐ݒ�E�B���h�E�B
/// </summary>
public sealed class CreateTileEditorWindow : EditorWindow
{
    [MenuItem("Window/Sasaki/CreateTileEditorWindow")]
    private static void ShowParamWindow()
    {
        // Window��\��
        GetWindow<CreateTileEditorWindow>().Show();
    }

    // �{�^���̑�����
    //public static float _buttonSize { get; set; } = 40;
    public static float _buttonSize;

    // �{�^���̊Ԋu (px)
    //public static float _padding { get; set; } = 2;
    public static float _padding;

    // �^�C����񂪓����Ă���X�N���v�^�u���I�u�W�F�N�g�̃p�X
    public static string _tileDataTablePath { get; set; } = "Assets/CreateTile/Data/TileDataTable.asset";

    // Rect���W
    public static Rect _tileModeButtonRect;
    

    public enum Language
    {
        Japanese,
        English,
    }

    Language _language = Language.Japanese;



    /// <summary>
    /// Prefs�ɕۑ����Ă��l���i�[����B
    /// </summary>
    private void OnEnable()
    {
        // �{�^���̑傫����Prefs�ۑ�
        _buttonSize = EditorPrefs.GetFloat(nameof(_buttonSize), _buttonSize);

        // �{�^���Ԋu��Prefs�ۑ�
        _padding = EditorPrefs.GetFloat(nameof(_padding), _padding);


        _tileModeButtonRect.x = EditorPrefs.GetFloat(nameof(_tileModeButtonRect) + "_x", _tileModeButtonRect.x);
        _tileModeButtonRect.y = EditorPrefs.GetFloat(nameof(_tileModeButtonRect) + "_y", _tileModeButtonRect.y);
        _tileModeButtonRect.width = EditorPrefs.GetFloat(nameof(_tileModeButtonRect) + "_w", _tileModeButtonRect.width);
        _tileModeButtonRect.height = EditorPrefs.GetFloat(nameof(_tileModeButtonRect) + "_h", _tileModeButtonRect.height);
    }



    private void OnGUI()
    {�@
        // ����ݒ�
        _language = (Language)EditorGUILayout.EnumPopup("Language", _language);
        
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();

        // ���b�`�e�L�X�g�������邽�߂�GUIStyle�ϐ�
        GUIStyle style = new GUIStyle(EditorStyles.label);
        style.richText = true;

        // ����ɂ���ĕ\����ς���B
        switch (_language)
        {
            case Language.Japanese:

                EditorGUILayout.LabelField("<size=20><b>CreateTileEditor�ւ悤����</b></size>", style);

                EditorGUILayout.Space();

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("�{�^���ݒ�",EditorStyles.boldLabel);

                EditorGUILayout.Space();

                _buttonSize = EditorGUILayout.FloatField("�{�^���̑傫��", _buttonSize);
                _padding = EditorGUILayout.FloatField("�{�^���ƃ{�^���̊Ԋu", _padding);

                EditorGUILayout.Space();

                _tileModeButtonRect = EditorGUILayout.RectField("���[�h�X�C�b�`�{�^����Rect�ʒu",_tileModeButtonRect);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("�^�C���̏�񂪓����Ă���X�N���v�^�u���I�u�W�F�N�g�̃p�X(�g���q���Y�ꂸ��)");
                _tileDataTablePath = EditorGUILayout.TextField("", _tileDataTablePath);

                break;

            case Language.English:

                EditorGUILayout.LabelField("<size=20><b>Welcome to CreateTileEditor</b></size>",style);

                EditorGUILayout.Space();

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Button Settings", EditorStyles.boldLabel);

                EditorGUILayout.Space();

                _buttonSize = EditorGUILayout.FloatField("size", _buttonSize);
                _padding = EditorGUILayout.FloatField("padding", _padding);

                EditorGUILayout.Space();

                _tileModeButtonRect = EditorGUILayout.RectField("Rect position to ModeSwitchButton", _tileModeButtonRect);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Tile information Path");
                _tileDataTablePath = EditorGUILayout.TextField("", _tileDataTablePath);

                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetFloat(nameof(_buttonSize), _buttonSize);
            EditorPrefs.SetFloat(nameof(_padding), _padding);

            EditorPrefs.SetFloat(nameof(_tileModeButtonRect) + "_x", _tileModeButtonRect.x);
            EditorPrefs.SetFloat(nameof(_tileModeButtonRect) + "_y", _tileModeButtonRect.y);
            EditorPrefs.SetFloat(nameof(_tileModeButtonRect) + "_w", _tileModeButtonRect.width);
            EditorPrefs.SetFloat(nameof(_tileModeButtonRect) + "_h", _tileModeButtonRect.height);

        }
    }
}