#if UNITY_EDITOR
//SceneViewを取得するために宣言、エディタ外では使えないのでUNITY_EDITORで囲む
using UnityEditor;
#endif

using UnityEngine;

/// <summary>
/// SceneView上でクリックした座標を取得するクラス
/// </summary>
public class Click : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        //マウスのクリックがあったら処理
        if (Event.current == null || Event.current.type != EventType.MouseUp)
        {
            return;
        }

        //処理中のイベントからマウスの位置取得
        Vector3 mousePosition = Event.current.mousePosition;

        //シーン上の座標に変換
        mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
        mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);

        Debug.Log("座標 : " + mousePosition.x.ToString("F2") + ", " + mousePosition.y.ToString("F2"));

        mousePosition.z = 0;
        GameObject newObject = Instantiate(Prefab);
        newObject.transform.position = mousePosition;
    }
#endif

}