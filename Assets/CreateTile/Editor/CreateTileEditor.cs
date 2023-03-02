// ビルドエラー対策用スニペット
//#if UNITY_EDITOR
using UnityEditor;
//#endif

using UnityEngine;

// 起動時にstaticコンストラクターメソッドを実行。
[InitializeOnLoad]
public static class CreateTileEditor
{

//#if UNITY_EDITOR
    // タイルの色のenum変数
    public enum Status
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

    // 現在ボタンで選択している色を保存する変数
    public static Status _status = Status.None;

    // 指定したタイルを格納する変数
    public static GameObject _selectedTile = default;

    // タイルデータが格納されているスクリプタブルオブジェクトを格納
    private static TileDataTable _tileDataTable;

    // タイル編集モードが否かを判定するbool変数
    public static bool _tileMode = false;

    public static string TileDataTablePath {get;set;} = "Assets/CreateTile/Data/TileDataTable.asset";

    // ボタンの多きさ
    static float loc_buttonSize = 60;

    // ボタンとボタンの間隔(px)
    static float loc_padding = 2;

    /// <summary>
    /// コンストラクターメソッド　SceneGUIにOnGUIメソッドを加算（重ねる）
    /// </summary>
    static CreateTileEditor()
    {
        // SceneGUIにOnGUIメソッドを加算（重ねる）
        SceneView.duringSceneGui += OnGUI;
    }



    /// <summary>
    /// SceneGUIに加算するGUIを作成
    /// </summary>
    /// <param name="sceneView">シーンビュー（自動保存）</param>
    private static void OnGUI(SceneView sceneView)
    {
        ParamUpdate();

        // タイルモードの切り替えをするボタンの大きさを設定するfloatローカル変数
        float loc_switchButtonSizeX = 80;
        float loc_switchButtonSizeY = 60;

        /*
         * タイルモードのON、OFFができるボタンを表示する拡張
         */

        // タイルモードでない時
        if (_tileMode == false)
        {
            // ボタン描画の開始
            Handles.BeginGUI();

            // ボタン位置をRect変数に設定
            Rect rect = new Rect(8, 300, loc_switchButtonSizeX, loc_switchButtonSizeY);

            // ボタンの挙動、押したらTileモードがONになる
            if (GUI.Button(rect, "TileモードOFF"))
            {
                _tileMode = true;
            }

            // ボタン描画の終了
            Handles.EndGUI();
            return;
        }
        else
        {
            // ボタン描画の開始
            Handles.BeginGUI();

            // ボタン位置をRect変数に設定
            Rect rect = new Rect(8, 300, loc_switchButtonSizeX, loc_switchButtonSizeY);

            // ボタンの挙動、押したらTileモードがOFFになる
            if (GUI.Button(rect, "TileモードON"))
            {
                _tileMode = false;
            }

            // ボタン描画の終了
            Handles.EndGUI();
        }

        // スクリプタブルオブジェクトが空の場合
        if (_tileDataTable == null)
        {
            // スクリプタブルオブジェクトをアセットデータベースから再格納。
            _tileDataTable = LoadDataTable();
        }


        // ボタン描画の開始
        Handles.BeginGUI();

        // ボタンを出す。
        ShowButtons(sceneView.position.size);

        // ボタン描画の終了
        Handles.EndGUI();
    }

    private static void ParamUpdate()
    {
        loc_buttonSize = CreateTileEditorWindow._buttonSize;
        loc_padding = CreateTileEditorWindow._padding;
        TileDataTablePath = CreateTileEditorWindow._tileDataTablePath;
    }

    /// <summary>
    /// スクリプタブルオブジェクトを取得
    /// </summary>
    /// <returns>パス指定したオブジェクトデータを取得</returns>
    private static TileDataTable LoadDataTable()
    {
        // 指定したパスにあるScriptableObjectからデータを引っ張る。
        return AssetDatabase.LoadAssetAtPath<TileDataTable>(TileDataTablePath);
    }


    /// <summary>
    /// ボタンの描画関数
    /// </summary>
    private static void ShowButtons(Vector2 sceneSize)
    {
        // Tileの個数を格納　ScriptableObjectの要素数参照
        int loc_count = _tileDataTable.dataList.Count;


        for (var i = 0; i < loc_count; i++)
        {
            // ScriptableObjectの要素数の一つを格納
            TileData _tileData = _tileDataTable.dataList[i];


            /*[   ボタン位置、（選べる用にする）   ]*/

            //// ボタン位置
            //Rect loc_rect = new Rect(
            //                        // x
            //                        sceneSize.x / 2 - loc_buttonSize * loc_count / 2 + loc_buttonSize * i + loc_padding * i,
            //                         // y（マージン）
            //                         sceneSize.y - loc_buttonSize * 1.6f,
            //                         // width 横サイズ
            //                         loc_buttonSize,
            //                         // height 縦サイズ
            //                         loc_buttonSize);

            //// ボタン位置(右上配置)
            Rect loc_rect = new Rect(

                                     // x（マージン）
                                     sceneSize.x - loc_buttonSize * 2,
                                     // y
                                     //sceneSize.y / 2 - loc_buttonSize * loc_count / 2 + loc_buttonSize * i + loc_padding * i,
                                     loc_buttonSize * i + loc_padding * i,
                                     // width 横サイズ
                                     loc_buttonSize,
                                     // height 縦サイズ
                                     loc_buttonSize

                                    );


            /*[   /ボタン位置、（選べる用にする）   ]*/

            // ボタンを押したとき。
            if (GUI.Button(loc_rect, _tileData.icon.texture))
            {
                // 位置によって処理を変える。
                switch (i + 1)
                {
                    case (int)Status.White:

                        // 選択している色を白にする。
                        _status = Status.White;
                        break;

                    case (int)Status.Black:

                        // 選択している色を黒にする。
                        _status = Status.Black;
                        break;

                    case (int)Status.Green:

                        // 選択している色を緑にする。
                        _status = Status.Green;
                        break;

                    case (int)Status.Pink:

                        // 選択している色をピンクにする。
                        _status = Status.Pink;
                        break;

                    case (int)Status.Red:

                        // 選択している色を赤にする。
                        _status = Status.Red;
                        break;

                    case (int)Status.Yellow:

                        // 選択している色を黄色にする。
                        _status = Status.Yellow;
                        break;

                    case (int)Status.Blue:

                        // 選択している色を青にする。
                        _status = Status.Blue;
                        break;

                    case (int)Status.Orange:

                        // 選択している色をオレンジにする。
                        _status = Status.Orange;
                        break;

                    case (int)Status.Purple:

                        // 選択している色を紫にする。
                        _status = Status.Purple;
                        break;

                    default:

                        // 選択している色を無しにする。
                        _status = Status.None;
                        break;

                }

                // 指定タイルを格納
                _selectedTile = _tileData.gameObject;

                // タイル生成はClickスクリプトで生成。
            }
        }
    }
//#endif
}



public sealed class CreateTileEditorWindow : EditorWindow
{
    [MenuItem("Window/Sasaki/CreateTileEditorWindow")]
    private static void ShowParamWindow()
    {
        // Windowを表示
        GetWindow<CreateTileEditorWindow>().Show();
    }

    // ボタンの多きさ
    public static float _buttonSize { get; set; } = 40;

    // ボタンの間隔 (px)
    public static float _padding { get; set; } = 2;

    // タイル情報が入っているスクリプタブルオブジェクトのパス
    public static string _tileDataTablePath { get; set; } = "Assets/CreateTile/Data/TileDataTable.asset";

    public enum Language
    {
        Japanese,
        English,
        Chinese,
    }

    Language language = Language.Japanese;

    private void OnGUI()
    {　
        // 言語設定
        language = (Language)EditorGUILayout.EnumPopup("Language", language);
        
        EditorGUILayout.Space();
        
        // 言語によって表示を変える。
        switch (language)
        {
            case Language.Japanese:
                EditorGUILayout.LabelField("ボタン設定");

                EditorGUILayout.Space();

                _buttonSize = EditorGUILayout.FloatField("ボタンの大きさ", _buttonSize);
                _padding = EditorGUILayout.FloatField("ボタンとボタンの間隔", _padding);

                EditorGUILayout.Space();


                EditorGUILayout.Space();

                EditorGUILayout.LabelField("タイルの情報が入っているスクリプタブルオブジェクトのパス(拡張子も忘れずに)");
                _tileDataTablePath = EditorGUILayout.TextField("", _tileDataTablePath);

                break;

            case Language.English:
                EditorGUILayout.LabelField("Button Settings");

                EditorGUILayout.Space();

                _buttonSize = EditorGUILayout.FloatField("size", _buttonSize);
                _padding = EditorGUILayout.FloatField("padding", _padding);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Tile information Path");
                _tileDataTablePath = EditorGUILayout.TextField("", _tileDataTablePath);

                break;
        }
    }
}