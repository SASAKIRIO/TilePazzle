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
        // �w�肵���p�X�ɂ���ScriptableObject����f�[�^����������B
        return AssetDatabase.LoadAssetAtPath<TileDataTable>("Assets/Data/TileDataTable.asset");
    }


    /// <summary>
    /// �{�^���̕`��֐�
    /// </summary>
    private static void ShowButtons(Vector2 sceneSize)
    {
        // Tile�̌����i�[�@ScriptableObject�̗v�f���Q��
        int count = _tileDataTable.dataList.Count;

        // �{�^���̑�����
        float buttonSize = 60;

        // �{�^���ƃ{�^���̊Ԋu(px)
        float padding = 2;

        for (var i = 0; i < count; i++)
        {
            // ScriptableObject�̗v�f���̈���i�[
            TileData data = _tileDataTable.dataList[i];

            // �{�^���ʒu
            Rect rect = new Rect(sceneSize.x / 2 - buttonSize * count / 2 + buttonSize * i + padding * i,
                                 sceneSize.y - buttonSize * 1.6f, 
                                 buttonSize,
                                 buttonSize);

            

            // �{�^�����������Ƃ��B
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
                // ScriptableObject�Őݒ肵��Prefab���o���āAgo�Ɋi�[�B
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(data.gameObject);

                // ���������I�u�W�F�N�g��I���ς݂̏�Ԃɂ���B 
                Selection.activeGameObject = go;

                // Ctrl�{Z�Ŗ߂��悤�ɂ���B
                Undo.RegisterCreatedObjectUndo(go, "Create Tile");

                Debug.Log("�����ꂽ" + _status);
            }
        }
    }
}


/// <summary>
/// �E�B���h�E�ɏo��sealed�N���X
/// </summary>
public sealed class CreateTileEditorWindow : EditorWindow
{
    private void OnGUI()
    {
        GUILayout.Label("�H����");


        if (GUILayout.Button("Close"))
        {
            // �E�B���h�E�����
            Close();
        }
    }



    [MenuItem("Window/CreateTileEditor/WindowShow")]
    private static void ShowWindow()
    {
        GetWindow<CreateTileEditorWindow>().Show();
    }
}
