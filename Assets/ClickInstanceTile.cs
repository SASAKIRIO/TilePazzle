// ビルドエラー対策用スニペット
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]//ExecuteInEditModeを付ける事でOnEnableが再生していなくても実行されるようになる
public class ClickInstanceTile : MonoBehaviour
{
    [Header("使用しているモニターがRetinaモニターの場合trueにしてください"), SerializeField] 
    private bool _Monitor_is_Retina;

    [Header("マウスX倍率"), SerializeField]
    private float X = 1.25f;

    [Header("マウスY倍率"), SerializeField]
    private float Y = 1.25f;

    [Header("どのキーでタイルモードにするかどうか"), SerializeField]
    private KeyCode _keyCode;

    // タイルモード
    //private bool _TileMode = false;

    // 生成したタイルの親オブジェクト
    private GameObject MapParent;


    // 実質UpdateになるEnable関数
    private void OnEnable()
    {
        // 生成したタイルをまとめる親オブジェクトを設定
        MapParent = GameObject.FindGameObjectWithTag("MapParent");
        //シーンビュー上のイベントを取得するため、メソッドを設定
        SceneView.duringSceneGui += EventOnSceneView;
    }


    /// <summary>
    /// シーンビュー上でイベントが発生した
    /// </summary>
    /// <param name="scene">現在のシーン、自動保持</param>
    private void EventOnSceneView(SceneView scene)
    {
        // 左Ctrlを押しながらクリックでタイル生成。
        //Event.current.type == EventType.MouseDown &&


        /*
         *
         *【問題点】
         * EventTypeを使った多重分岐ができない
         * 
         * logのように同時にtrueになることがなさそう
         * 
         * 処置としてbool型に一時保存する形にする。
         * 
         */
        // Debug.Log("キー:" + (Event.current.type == EventType.KeyDown) + "  MouseDown" + (Event.current.type == EventType.MouseDown));



        // タイルモードの状態で、クリックするとタイルを生成。
        if (CreateTileEditor._tileMode && Event.current.type == EventType.MouseDown)
        {
            InstancePrefab();
        }
    }



    /// <summary>
    /// プレハブの生成メソッド
    /// </summary>
    private void InstancePrefab()
    {
        // イベントからマウスの位置取得
        Vector3 mousePosition = Event.current.mousePosition;
        
        // 使用者のモニターがRetinaモニターの場合
        if (_Monitor_is_Retina)
        {
            mousePosition.x *= X;
            mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y * Y;
        }
        // 使用者のモニターがRetinaモニターでない場合
        else
        {
            mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
        }

        // マウスの位置をシーン上の座標に変換
        mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);

        // 奥行を排除
        mousePosition.z = 0;

        // シーン上の座標に変換後、グリッド座標に正規化
        mousePosition = new Vector3(
                                    Mathf.Round(mousePosition.x),
                                    Mathf.Round(mousePosition.y),
                                    Mathf.Round(mousePosition.z)
                                    );

        try
        {
            // ScriptableObjectで設定したPrefabを出して、tileに設定
            GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(CreateTileEditor._selectedTile);

            // 生成したオブジェクトを子オブジェクトに設定
            tile.transform.parent = MapParent.transform;

            // 生成したオブジェクトを選択済みの状態にする。 
            Selection.activeGameObject = tile;

            // 生成したオブジェクトをマウス位置に移動させる。
            tile.transform.position = mousePosition;

            // Ctrl＋Zで戻れるようにする為に内部のUndoリストに登録する。
            Undo.RegisterCreatedObjectUndo(tile, "Create Tile");
        }
        catch{ }

    }
}

#endif