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

    [Header("��ʊg�嗦"), SerializeField]
    public static float WindowZoomPercent = GetDpi();

    // ���������^�C���̐e�I�u�W�F�N�g
    private static GameObject MapParent;

    // �}�b�v�z��A�^�C�������݂���Ƃ����notnull,�^�C�������݂��Ȃ��Ƃ����null
    public static GameObject[,] ExistMap { get; set; } = new GameObject[100, 100];

    // ���݂��Ă���^�C�����i�[����GameObject�z��
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


        // �^�C�����[�h�̏�ԂŁA�N���b�N�A�h���b�O����ƃ^�C���𐶐��B
        // ���͏���
        if (CreateTileEditor._tileMode && Event.current.type == EventType.Used)
        {
            // �v���n�u�̐���
            InstancePrefab();
        }

        // �E�N���b�N�ō폜
        if(CreateTileEditor._tileMode && Event.current.button == 1)
        {
            DeletePrefab();
        }
    }



    /// <summary>
    /// DPI�g�嗦���擾���郁�\�b�h
    /// </summary>
    public static float GetDpi()
    {
        // dpi��ݒ肷��B
        float loc_dpi = Screen.dpi;

        // Windows�W����DPI�l
        const int loc_defaultWindowsDpi = 96;

        // �W����DPI�l����ɂ����{����Ԃ��B
        return loc_dpi / loc_defaultWindowsDpi;

    }



    /// <summary>
    /// �v���n�u�̐������\�b�h
    /// </summary>
    private static void InstancePrefab()
    {
        

        // �C�x���g����}�E�X�̈ʒu�擾
        Vector3 mousePosition = GetMousePosition(true);

        // �O�t���[���ƃ}�E�X�ʒu�������Ȃ�
        if (mousePosition == BeforeMousePosition) return;

        // �^�C��������Ă��邩�m�F
        if (_isExistTile(mousePosition))
        {
            // ����Ă���^�C����j������
            DeletePrefab();
        }


        try
        {
            // ScriptableObject�Őݒ肵��Prefab���o���āAtile�ɐݒ�
            GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(CreateTileEditor._selectedTile);

            // ���������I�u�W�F�N�g���q�I�u�W�F�N�g�ɐݒ�
            tile.transform.parent = MapParent.transform;

            // ���������I�u�W�F�N�g��I��null�̏�Ԃɂ���B 
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
    /// �v���n�u�̍폜���\�b�h
    /// </summary>
    private static void DeletePrefab()
    {
        // �C�x���g����}�E�X�̈ʒu�擾
        Vector3 mousePosition = GetMousePosition(true);

        int x = (int)mousePosition.x;
        int y = (int)mousePosition.y;

        // ����Ă���I�u�W�F�N�g��j�����āA������Undo���X�g�ɋL�^
        try
        {
            Undo.DestroyObjectImmediate(ExistMap[x, y]);
        }
        catch { }
    }

    /// <summary>
    /// �}�E�X�|�W�V�����̎擾
    /// </summary>
    /// <returns>�V�[����̃}�E�X���W</returns>
    public static Vector3 GetMousePosition(bool _Round = false)
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

        // �g�嗦����Z����
        mousePosition.x *= WindowZoomPercent;

        // �g�嗦����Z����
        mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y * WindowZoomPercent;

        // �}�E�X�̈ʒu���V�[����̍��W�ɕϊ�
        mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);

        // ���s��r��
        mousePosition.z = 0;

        if (_Round)
        {
            // �V�[����̍��W�ɕϊ���A�O���b�h���W�ɐ��K��
            mousePosition = new Vector3(
                                        Mathf.Round(mousePosition.x),
                                        Mathf.Round(mousePosition.y),
                                        Mathf.Round(mousePosition.z)
                                        );
        }

        // �ԋp�l�Ƃ��ĕԂ�
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

        // for���Ŕz�����
        for(int i = 0; i < ExistMap.GetLength(0); i++)
        {
            for(int j = 0; j < ExistMap.GetLength(1); j++)
            {
                // �v�f����null�ɂ���
                ExistMap[i, j] = null;
            }
        }
        // �^�C���̍��W�̏ꏊ�Ƀ^�C����ݒ�
        for (int i = 0; i < Tiles.Length; i++)
        {
            // ���W��x���W��y���W���ۂ߂Ă��ꂼ��i�[
            int x = (int)Tiles[i].gameObject.transform.position.x;
            int y = (int)Tiles[i].gameObject.transform.position.y;

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
    //#endif
}