#if UNITY_EDITOR
//SceneView���擾���邽�߂ɐ錾�A�G�f�B�^�O�ł͎g���Ȃ��̂�UNITY_EDITOR�ň͂�
using UnityEditor;
#endif

using UnityEngine;

/// <summary>
/// SceneView��ŃN���b�N�������W���擾����N���X
/// </summary>
public class Click : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        //�}�E�X�̃N���b�N���������珈��
        if (Event.current == null || Event.current.type != EventType.MouseUp)
        {
            return;
        }

        //�������̃C�x���g����}�E�X�̈ʒu�擾
        Vector3 mousePosition = Event.current.mousePosition;

        //�V�[����̍��W�ɕϊ�
        mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
        mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);

        Debug.Log("���W : " + mousePosition.x.ToString("F2") + ", " + mousePosition.y.ToString("F2"));

        mousePosition.z = 0;
        GameObject newObject = Instantiate(Prefab);
        newObject.transform.position = mousePosition;
    }
#endif

}