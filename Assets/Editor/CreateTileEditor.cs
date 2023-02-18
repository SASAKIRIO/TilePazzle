using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class CreateTileEditor
{
    private enum Status
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

    private static Status _status = Status.None;

    private static TileDataTable _tileDataTable;
    static CreateTileEditor()
    {
        SceneView.duringSceneGui += OnGui;
    }

    private static void OnGui(SceneView sceneView)
    {
        if (_tileDataTable == null)
        {
            _tileDataTable = LoadDataTable();
        }

        Handles.BeginGUI();

        ShowButtons(sceneView.position.size);

        Handles.EndGUI();
    }

    private static TileDataTable LoadDataTable()
    {
        // 指定したパスにあるScriptableObjectからデータを引っ張る。
        return AssetDatabase.LoadAssetAtPath<TileDataTable>("Assets/Data/TileDataTable.asset");
    }


    /// <summary>
    /// ボタンの描画関数
    /// </summary>
    private static void ShowButtons(Vector2 sceneSize)
    {
        // Tileの個数を格納　ScriptableObjectの要素数参照
        int count = _tileDataTable.dataList.Count;

        // ボタンの多きさ
        float buttonSize = 60;

        // ボタンとボタンの間隔(px)
        float padding = 2;

        for (var i = 0; i < count; i++)
        {
            // ScriptableObjectの要素数の一つを格納
            TileData data = _tileDataTable.dataList[i];

            // ボタン位置
            Rect rect = new Rect(sceneSize.x / 2 - buttonSize * count / 2 + buttonSize * i + padding * i,
                                 sceneSize.y - buttonSize * 1.6f, 
                                 buttonSize,
                                 buttonSize);

            

            // ボタンを押したとき。
            if (GUI.Button(rect, data.icon.texture))
            {
                switch (i+1)
                {
                    case (int)Status.White:
                        _status = Status.White;
                        Debug.Log("White");
                        break;
                    case (int)Status.Black:
                        _status = Status.Black;
                        Debug.Log("Black");
                        break;
                    case (int)Status.Green:
                        _status = Status.Green;
                        Debug.Log("Green");
                        break;
                    case (int)Status.Pink:
                        _status = Status.Pink;
                        Debug.Log("Pink");
                        break;
                    case (int)Status.Red:
                        _status = Status.Red;
                        Debug.Log("Red");
                        break;
                    case (int)Status.Yellow:
                        _status = Status.Yellow;
                        Debug.Log("Yellow");
                        break;
                    case (int)Status.Blue:
                        _status = Status.Blue;
                        Debug.Log("Blue");
                        break;
                    case (int)Status.Orange:
                        _status = Status.Orange;
                        Debug.Log("Orange");
                        break;
                    case (int)Status.Purple:
                        _status = Status.Purple;
                        Debug.Log("Purple");
                        break;
                    default:
                        _status = Status.None;
                        Debug.Log("None");
                        break;
                }
                // ScriptableObjectで設定したPrefabを出して、goに格納。
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(data.gameObject);

                // 生成したオブジェクトを選択済みの状態にする。 
                Selection.activeGameObject = go;

                // Ctrl＋Zで戻れるようにする。
                Undo.RegisterCreatedObjectUndo(go, "Create Tile");

                Debug.Log("おされた" + _status);
            }
        }
    }
}


/// <summary>
/// ウィンドウに出すsealedクラス
/// </summary>
public sealed class CreateTileEditorWindow : EditorWindow
{
    private void OnGUI()
    {
        GUILayout.Label("工事中");


        if (GUILayout.Button("Close"))
        {
            // ウィンドウを閉じる
            Close();
        }
    }



    [MenuItem("Window/CreateTileEditor/WindowShow")]
    private static void ShowWindow()
    {
        GetWindow<CreateTileEditorWindow>().Show();
    }
}
