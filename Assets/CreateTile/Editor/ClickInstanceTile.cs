// ビルドエラー対策用スニペット
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
      * [問題点]
      * 
      * HeaderついてるパラメータをSerialize化
      * 
      */


     [Header("使用しているモニターがRetinaモニターの場合trueにしてください"), SerializeField] 
    private static bool _Monitor_is_Retina = true;

    [Header("画面拡大率"), SerializeField]
    public static float WindowZoomPercent = GetDpi();


    // 生成したタイルの親オブジェクト
    private static GameObject MapParent;

    // マップ配列、タイルが存在するところはnotnull,タイルが存在しないところはnull
    public static GameObject[,] ExistMap { get; set; } = new GameObject[100, 100];

    private static GameObject[] Tiles;

    // 多重に置くことを防ぐ為にある。　前フレームのマウス位置を設定するVector3変数
    private static Vector3 BeforeMousePosition;




    static ClickInstanceTile()
    {
        // 生成したタイルをまとめる親オブジェクトを設定
        MapParent = GameObject.FindGameObjectWithTag("MapParent");

        //シーンビュー上のイベントを取得するため、メソッドを設定
        SceneView.duringSceneGui += EventOnSceneView;
    }



    /// <summary>
    /// DPIを取得するメソッド
    /// </summary>
    public static float GetDpi()
    {
        // dpi変数
        float dpi;

        dpi = Screen.dpi;

        Debug.Log("拡大率は" + (dpi / 96) * 100 + "%です！");

        return dpi / 96;

    }



    /// <summary>
    /// シーンビュー上でイベントが発生した
    /// </summary>
    /// <param name="scene">現在のシーン、自動保持</param>
    private static void EventOnSceneView(SceneView scene)
    {

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


        // 拡大率を更新
        WindowZoomPercent = GetDpi();


        // タイルモードの状態で、クリックするとタイルを生成。
        // 入力処理
        if (CreateTileEditor._tileMode && (Event.current.type == EventType.Used))
        {
            // プレハブの生成
            InstancePrefab();
        }

        // ゴーストは表示する。
        OnGUIGhostTile();
    }



    /// <summary>
    /// プレハブの生成メソッド
    /// </summary>
    private static void InstancePrefab()
    {
        

        // イベントからマウスの位置取得
        Vector3 mousePosition = GetMousePosition();

        // シーン上の座標に変換後、グリッド座標に正規化
        mousePosition = new Vector3(
                                    Mathf.Round(mousePosition.x),
                                    Mathf.Round(mousePosition.y),
                                    Mathf.Round(mousePosition.z)
                                    );

        // 前フレームとマウス位置が同じなら
        if (mousePosition == BeforeMousePosition) return;

        // タイルが被っているか確認
        if (_isExistTile(mousePosition))
        {

            int x = (int)mousePosition.x;
            int y = (int)mousePosition.y;

            // 被っているオブジェクトを破棄して、内部のUndoリストに記録
            Undo.DestroyObjectImmediate(ExistMap[x, y]);
        }


        try
        {
            // ScriptableObjectで設定したPrefabを出して、tileに設定
            GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(CreateTileEditor._selectedTile);

            // 生成したオブジェクトを子オブジェクトに設定
            tile.transform.parent = MapParent.transform;

            // 生成したオブジェクトを選択済みの状態にする。 
            //Selection.activeGameObject = tile;
            Selection.activeGameObject = null;

            // 生成したオブジェクトをマウス位置に移動させる。
            tile.transform.position = mousePosition;

            // Ctrl＋Zで戻れるようにする為に内部のUndoリストに登録する。
            Undo.RegisterCreatedObjectUndo(tile, "Create Tile");
        }
        catch{ }

        // タイルが存在しているかいないかを判断する配列を設定
        SettingExistTileArray();

        BeforeMousePosition = mousePosition;
    }



    /// <summary>
    /// マウスポジションの取得
    /// </summary>
    /// <returns>シーン上のマウス座標</returns>
    private static Vector3 GetMousePosition()
    {
        /*
         * 
         * [問題点]
         * マウスの座標ずれ
         * 
         * [解決]
         * Windowsの設定の画面拡大率が原因だった
         * XとYには拡大率を入れる
         *
         */



        // イベントからマウスの位置取得
        Vector3 mousePosition = Event.current.mousePosition;



        // 使用者のモニターがRetinaモニターの場合
        if (_Monitor_is_Retina)
        {
            mousePosition.x *= WindowZoomPercent;
            mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y * WindowZoomPercent;
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

        return mousePosition;
    }


    /// <summary>
    /// タイルが存在しているかいないかを判断する配列を設定
    /// </summary>
    private static void SettingExistTileArray()
    {
        /*
         * 
         * 最適化対象。不必要な所まで探索している。
         * 予想ではタイルの座標のｘｙそれぞれの一番端を取得して
         * その分だけfor文で回せばいけそう。
         * 
         */


        // Tileタグがついたオブジェクトを取得
        Tiles = GameObject.FindGameObjectsWithTag("Tile");

        // 一回すべてをnullにする
        for(int i = 0; i < ExistMap.GetLength(0); i++)
        {
            for(int j = 0; j < ExistMap.GetLength(1); j++)
            {
                ExistMap[i, j] = null;
            }
        }
        // タイルの座標の場所にタイルを設定
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
    /// mousePositionで指定した先に既にタイルがあるか否かをとるboolメソッド
    /// </summary>
    /// <param name="mousePosition">マウスで指定したポジション</param>
    /// <returns>存在する場合true、存在しない場合falseを返す</returns>
    private static bool _isExistTile(Vector3 mousePosition)
    {
        int x = (int)mousePosition.x;
        int y = (int)mousePosition.y;

        // もし存在する場合true、存在しない場合falseをreturnする。
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
        // イベントからマウスの位置取得
        Vector3 mousePosition = GetMousePosition();

        //Debug.Log(mousePosition +")"+Screen.width+":"+Screen.height);
    }



    
//#endif
}



/// <summary>
/// ClickInstanceTileスクリプトのパラメータをウィンドウにパラメータ化するクラス
/// </summary>
public sealed class ClickInstanceTileWindow : EditorWindow
{
    [UnityEditor.MenuItem("Window/Sasaki/ClickInstanceTileWindow")]
    private static void ShowParamWindow()
    {
        // Windowを表示
        GetWindow<ClickInstanceTileWindow>().Show();
    }


    // ここが初期値になる
    public float WindowZoomPercent { get; set; } = ClickInstanceTile.WindowZoomPercent;

    public static bool foldout = false;

    

    private void OnGUI()
    {
        EditorGUILayout.LabelField("ウィンドウ設定");

        WindowZoomPercent = EditorGUILayout.FloatField(WindowZoomPercent, "拡大率");

        EditorGUILayout.Space();

        // 値を表示
        //WindowZoomPercent = EditorGUILayout.FloatField("Windowsのウィンドウ拡大率", WindowZoomPercent);


        foldout = EditorGUILayout.Foldout(foldout, "ドロップアウト");
        if (foldout)
        {
            EditorGUILayout.LabelField("こんにちは");
        }
    }
}