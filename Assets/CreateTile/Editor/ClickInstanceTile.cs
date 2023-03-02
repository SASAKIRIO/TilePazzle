// �r���h�G���[�΍��p�X�j�y�b�g
//#if UNITY_EDITOR
using UnityEditor;
//#endif
using UnityEngine;


[InitializeOnLoad]
public static class ClickInstanceTile
{
//#if UNITY_EDITOR

     /*
      * 
      * [���_]
      * 
      * Header���Ă�p�����[�^��Serialize��
      * 
      */


     [Header("�g�p���Ă��郂�j�^�[��Retina���j�^�[�̏ꍇtrue�ɂ��Ă�������"), SerializeField] 
    private static bool _Monitor_is_Retina = true;

    [Header("��ʊg�嗦"), SerializeField]
    public static float WindowZoomPercent = GetDpi();


    // ���������^�C���̐e�I�u�W�F�N�g
    private static GameObject MapParent;

    // �}�b�v�z��A�^�C�������݂���Ƃ����notnull,�^�C�������݂��Ȃ��Ƃ����null
    public static GameObject[,] ExistMap { get; set; } = new GameObject[100, 100];

    private static GameObject[] Tiles;

    // ���d�ɒu�����Ƃ�h���ׂɂ���B�@�O�t���[���̃}�E�X�ʒu��ݒ肷��Vector3�ϐ�
    private static Vector3 BeforeMousePosition;




    static ClickInstanceTile()
    {
        // ���������^�C�����܂Ƃ߂�e�I�u�W�F�N�g��ݒ�
        MapParent = GameObject.FindGameObjectWithTag("MapParent");

        //�V�[���r���[��̃C�x���g���擾���邽�߁A���\�b�h��ݒ�
        SceneView.duringSceneGui += EventOnSceneView;
    }



    /// <summary>
    /// DPI���擾���郁�\�b�h
    /// </summary>
    public static float GetDpi()
    {
        // dpi�ϐ�
        float dpi;

        dpi = Screen.dpi;

        Debug.Log("�g�嗦��" + (dpi / 96) * 100 + "%�ł��I");

        return dpi / 96;

    }



    /// <summary>
    /// �V�[���r���[��ŃC�x���g����������
    /// </summary>
    /// <param name="scene">���݂̃V�[���A�����ێ�</param>
    private static void EventOnSceneView(SceneView scene)
    {

        /*
         *
         *�y���_�z
         * EventType���g�������d���򂪂ł��Ȃ�
         * 
         * log�̂悤�ɓ�����true�ɂȂ邱�Ƃ��Ȃ�����
         * 
         * ���u�Ƃ���bool�^�Ɉꎞ�ۑ�����`�ɂ���B
         * 
         */


        // �g�嗦���X�V
        WindowZoomPercent = GetDpi();


        // �^�C�����[�h�̏�ԂŁA�N���b�N����ƃ^�C���𐶐��B
        // ���͏���
        if (CreateTileEditor._tileMode && (Event.current.type == EventType.Used))
        {
            // �v���n�u�̐���
            InstancePrefab();
        }

        // �S�[�X�g�͕\������B
        OnGUIGhostTile();
    }



    /// <summary>
    /// �v���n�u�̐������\�b�h
    /// </summary>
    private static void InstancePrefab()
    {
        

        // �C�x���g����}�E�X�̈ʒu�擾
        Vector3 mousePosition = GetMousePosition();

        // �V�[����̍��W�ɕϊ���A�O���b�h���W�ɐ��K��
        mousePosition = new Vector3(
                                    Mathf.Round(mousePosition.x),
                                    Mathf.Round(mousePosition.y),
                                    Mathf.Round(mousePosition.z)
                                    );

        // �O�t���[���ƃ}�E�X�ʒu�������Ȃ�
        if (mousePosition == BeforeMousePosition) return;

        // �^�C��������Ă��邩�m�F
        if (_isExistTile(mousePosition))
        {

            int x = (int)mousePosition.x;
            int y = (int)mousePosition.y;

            // ����Ă���I�u�W�F�N�g��j�����āA������Undo���X�g�ɋL�^
            Undo.DestroyObjectImmediate(ExistMap[x, y]);
        }


        try
        {
            // ScriptableObject�Őݒ肵��Prefab���o���āAtile�ɐݒ�
            GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(CreateTileEditor._selectedTile);

            // ���������I�u�W�F�N�g���q�I�u�W�F�N�g�ɐݒ�
            tile.transform.parent = MapParent.transform;

            // ���������I�u�W�F�N�g��I���ς݂̏�Ԃɂ���B 
            //Selection.activeGameObject = tile;
            Selection.activeGameObject = null;

            // ���������I�u�W�F�N�g���}�E�X�ʒu�Ɉړ�������B
            tile.transform.position = mousePosition;

            // Ctrl�{Z�Ŗ߂��悤�ɂ���ׂɓ�����Undo���X�g�ɓo�^����B
            Undo.RegisterCreatedObjectUndo(tile, "Create Tile");
        }
        catch{ }

        // �^�C�������݂��Ă��邩���Ȃ����𔻒f����z���ݒ�
        SettingExistTileArray();

        BeforeMousePosition = mousePosition;
    }



    /// <summary>
    /// �}�E�X�|�W�V�����̎擾
    /// </summary>
    /// <returns>�V�[����̃}�E�X���W</returns>
    private static Vector3 GetMousePosition()
    {
        /*
         * 
         * [���_]
         * �}�E�X�̍��W����
         * 
         * [����]
         * Windows�̐ݒ�̉�ʊg�嗦������������
         * X��Y�ɂ͊g�嗦������
         *
         */



        // �C�x���g����}�E�X�̈ʒu�擾
        Vector3 mousePosition = Event.current.mousePosition;



        // �g�p�҂̃��j�^�[��Retina���j�^�[�̏ꍇ
        if (_Monitor_is_Retina)
        {
            mousePosition.x *= WindowZoomPercent;
            mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y * WindowZoomPercent;
        }
        // �g�p�҂̃��j�^�[��Retina���j�^�[�łȂ��ꍇ
        else
        {
            mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
        }

        // �}�E�X�̈ʒu���V�[����̍��W�ɕϊ�
        mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);

        // ���s��r��
        mousePosition.z = 0;

        return mousePosition;
    }


    /// <summary>
    /// �^�C�������݂��Ă��邩���Ȃ����𔻒f����z���ݒ�
    /// </summary>
    private static void SettingExistTileArray()
    {
        /*
         * 
         * �œK���ΏہB�s�K�v�ȏ��܂ŒT�����Ă���B
         * �\�z�ł̓^�C���̍��W�̂������ꂼ��̈�Ԓ[���擾����
         * ���̕�����for���ŉ񂹂΂��������B
         * 
         */


        // Tile�^�O�������I�u�W�F�N�g���擾
        Tiles = GameObject.FindGameObjectsWithTag("Tile");

        // ��񂷂ׂĂ�null�ɂ���
        for(int i = 0; i < ExistMap.GetLength(0); i++)
        {
            for(int j = 0; j < ExistMap.GetLength(1); j++)
            {
                ExistMap[i, j] = null;
            }
        }
        // �^�C���̍��W�̏ꏊ�Ƀ^�C����ݒ�
        for (int i = 0; i < Tiles.Length; i++)
        {
            int x = 0;
            int y = 0;
            x = (int)Tiles[i].gameObject.transform.position.x;
            y = (int)Tiles[i].gameObject.transform.position.y;

            try
            {
                ExistMap[x, y] = Tiles[i];
            }
            catch { }

        }
    }



    /// <summary>
    /// mousePosition�Ŏw�肵����Ɋ��Ƀ^�C�������邩�ۂ����Ƃ�bool���\�b�h
    /// </summary>
    /// <param name="mousePosition">�}�E�X�Ŏw�肵���|�W�V����</param>
    /// <returns>���݂���ꍇtrue�A���݂��Ȃ��ꍇfalse��Ԃ�</returns>
    private static bool _isExistTile(Vector3 mousePosition)
    {
        int x = (int)mousePosition.x;
        int y = (int)mousePosition.y;

        // �������݂���ꍇtrue�A���݂��Ȃ��ꍇfalse��return����B
        try
        {
            if (ExistMap[x, y] != null) return true;
            else return false;
        }
        catch
        {
            return false;
        }

    }



    //########################################
    private static void OnGUIGhostTile()
    {
        // �C�x���g����}�E�X�̈ʒu�擾
        Vector3 mousePosition = GetMousePosition();

        //Debug.Log(mousePosition +")"+Screen.width+":"+Screen.height);
    }



    
//#endif
}



/// <summary>
/// ClickInstanceTile�X�N���v�g�̃p�����[�^���E�B���h�E�Ƀp�����[�^������N���X
/// </summary>
public sealed class ClickInstanceTileWindow : EditorWindow
{
    [UnityEditor.MenuItem("Window/Sasaki/ClickInstanceTileWindow")]
    private static void ShowParamWindow()
    {
        // Window��\��
        GetWindow<ClickInstanceTileWindow>().Show();
    }


    // �����������l�ɂȂ�
    public float WindowZoomPercent { get; set; } = ClickInstanceTile.WindowZoomPercent;

    public static bool foldout = false;

    

    private void OnGUI()
    {
        EditorGUILayout.LabelField("�E�B���h�E�ݒ�");

        WindowZoomPercent = EditorGUILayout.FloatField(WindowZoomPercent, "�g�嗦");

        EditorGUILayout.Space();

        // �l��\��
        //WindowZoomPercent = EditorGUILayout.FloatField("Windows�̃E�B���h�E�g�嗦", WindowZoomPercent);


        foldout = EditorGUILayout.Foldout(foldout, "�h���b�v�A�E�g");
        if (foldout)
        {
            EditorGUILayout.LabelField("����ɂ���");
        }
    }
}