// �r���h�G���[�΍��p�X�j�y�b�g
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]//ExecuteInEditMode��t���鎖��OnEnable���Đ����Ă��Ȃ��Ă����s�����悤�ɂȂ�
public class ClickInstanceTile : MonoBehaviour
{
    [Header("�g�p���Ă��郂�j�^�[��Retina���j�^�[�̏ꍇtrue�ɂ��Ă�������"), SerializeField] 
    private bool _Monitor_is_Retina;

    [Header("�}�E�XX�{��"), SerializeField]
    private float X = 1.25f;

    [Header("�}�E�XY�{��"), SerializeField]
    private float Y = 1.25f;

    [Header("�ǂ̃L�[�Ń^�C�����[�h�ɂ��邩�ǂ���"), SerializeField]
    private KeyCode _keyCode;

    // �^�C�����[�h
    //private bool _TileMode = false;

    // ���������^�C���̐e�I�u�W�F�N�g
    private GameObject MapParent;


    // ����Update�ɂȂ�Enable�֐�
    private void OnEnable()
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
    private void EventOnSceneView(SceneView scene)
    {
        // ��Ctrl�������Ȃ���N���b�N�Ń^�C�������B
        //Event.current.type == EventType.MouseDown &&


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
        // Debug.Log("�L�[:" + (Event.current.type == EventType.KeyDown) + "  MouseDown" + (Event.current.type == EventType.MouseDown));



        // �^�C�����[�h�̏�ԂŁA�N���b�N����ƃ^�C���𐶐��B
        if (CreateTileEditor._tileMode && Event.current.type == EventType.MouseDown)
        {
            InstancePrefab();
        }
    }



    /// <summary>
    /// �v���n�u�̐������\�b�h
    /// </summary>
    private void InstancePrefab()
    {
        // �C�x���g����}�E�X�̈ʒu�擾
        Vector3 mousePosition = Event.current.mousePosition;
        
        // �g�p�҂̃��j�^�[��Retina���j�^�[�̏ꍇ
        if (_Monitor_is_Retina)
        {
            mousePosition.x *= X;
            mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y * Y;
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

        // �V�[����̍��W�ɕϊ���A�O���b�h���W�ɐ��K��
        mousePosition = new Vector3(
                                    Mathf.Round(mousePosition.x),
                                    Mathf.Round(mousePosition.y),
                                    Mathf.Round(mousePosition.z)
                                    );

        try
        {
            // ScriptableObject�Őݒ肵��Prefab���o���āAtile�ɐݒ�
            GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(CreateTileEditor._selectedTile);

            // ���������I�u�W�F�N�g���q�I�u�W�F�N�g�ɐݒ�
            tile.transform.parent = MapParent.transform;

            // ���������I�u�W�F�N�g��I���ς݂̏�Ԃɂ���B 
            Selection.activeGameObject = tile;

            // ���������I�u�W�F�N�g���}�E�X�ʒu�Ɉړ�������B
            tile.transform.position = mousePosition;

            // Ctrl�{Z�Ŗ߂��悤�ɂ���ׂɓ�����Undo���X�g�ɓo�^����B
            Undo.RegisterCreatedObjectUndo(tile, "Create Tile");
        }
        catch{ }

    }
}

#endif