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

    // 指定したタイル名を設定する変数
    public static string _selectTileName = default;

    // 指定したタイルを設定する変数
    public static GameObject _selectedTile = default;

    // 指定したタイルのstring型tipsを設定する変数
    public static string _tipsString = default;

    // 指定したタイルのTexture型tipsを設定する変数
    public static Texture _tipsTexture = default;

    // タイルデータが格納されているスクリプタブルオブジェクトを格納
    private static TileDataTable _tileDataTable;

    // タイル編集モードが否かを判定するbool変数
    public static bool _tileMode = false;

    public static string _tileDataTablePath {get;set;} = "Assets/CreateTile/Data/TileDataTable.asset";

    // ボタンの多きさ
    private static float _buttonSize = CreateTileEditorWindow._buttonSize;

    // ボタンとボタンの間隔(px)
    private static float _padding = CreateTileEditorWindow._padding;

    private static Rect _tileModeButtonRect = CreateTileEditorWindow._tileModeButtonRect;

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

        /*
         * タイルモードのON、OFFができるボタンを表示する拡張
         */

        // タイルモードでない時
        if (_tileMode == false)
        {
            // ボタン描画の開始
            Handles.BeginGUI();

            // ボタンの挙動、押したらTileモードがONになる
            if (GUI.Button(_tileModeButtonRect, "TileモードOFF"))
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

            // ボタンの挙動、押したらTileモードがOFFになる
            if (GUI.Button(_tileModeButtonRect, "TileモードON"))
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




    /// <summary>
    /// CreateTileEditorWindowから値を更新
    /// </summary>
    private static void ParamUpdate()
    {
        _buttonSize = CreateTileEditorWindow._buttonSize;
        _padding = CreateTileEditorWindow._padding;
        _tileDataTablePath = CreateTileEditorWindow._tileDataTablePath;
        _tileModeButtonRect = CreateTileEditorWindow._tileModeButtonRect;
    }

    /// <summary>
    /// スクリプタブルオブジェクトを取得
    /// </summary>
    /// <returns>パス指定したオブジェクトデータを取得</returns>
    private static TileDataTable LoadDataTable()
    {
        // 指定したパスにあるScriptableObjectからデータを引っ張る。
        return AssetDatabase.LoadAssetAtPath<TileDataTable>(_tileDataTablePath);
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



            //// ボタン位置(マイクラ)
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
                                     sceneSize.x - _buttonSize * 2,
                                     // y
                                     //sceneSize.y / 2 - loc_buttonSize * loc_count / 2 + loc_buttonSize * i + loc_padding * i,
                                     _buttonSize * i + _padding * i,
                                     // width 横サイズ
                                     _buttonSize,
                                     // height 縦サイズ
                                     _buttonSize

                                    );






            /*[   /ボタン位置、（選べる用にする）   ]*/

            // ボタンを押したとき。
            if (GUI.Button(loc_rect, _tileData.icon.texture))
            {
                // 指定タイルを格納
                _selectTileName = _tileData.Name;
                _selectedTile = _tileData.gameObject;
                _tipsString = _tileData.tipsString;
                _tipsTexture = _tileData.tipsTexture;

                // タイル生成はClickスクリプトで生成。
            }
        }
    }
//#endif
}



/// <summary>
/// ボタンの設定ウィンドウ。
/// </summary>
public sealed class CreateTileEditorWindow : EditorWindow
{
    [MenuItem("Window/Sasaki/CreateTileEditorWindow")]
    private static void ShowParamWindow()
    {
        // Windowを表示
        GetWindow<CreateTileEditorWindow>().Show();
    }

    // ボタンの多きさ
    //public static float _buttonSize { get; set; } = 40;
    public static float _buttonSize;

    // ボタンの間隔 (px)
    //public static float _padding { get; set; } = 2;
    public static float _padding;

    // タイル情報が入っているスクリプタブルオブジェクトのパス
    public static string _tileDataTablePath { get; set; } = "Assets/CreateTile/Data/TileDataTable.asset";

    // Rect座標
    public static Rect _tileModeButtonRect;
    

    public enum Language
    {
        Japanese,
        English,
    }

    Language _language = Language.Japanese;



    /// <summary>
    /// Prefsに保存してた値を格納する。
    /// </summary>
    private void OnEnable()
    {
        // ボタンの大きさのPrefs保存
        _buttonSize = EditorPrefs.GetFloat(nameof(_buttonSize), _buttonSize);

        // ボタン間隔のPrefs保存
        _padding = EditorPrefs.GetFloat(nameof(_padding), _padding);


        _tileModeButtonRect.x = EditorPrefs.GetFloat(nameof(_tileModeButtonRect) + "_x", _tileModeButtonRect.x);
        _tileModeButtonRect.y = EditorPrefs.GetFloat(nameof(_tileModeButtonRect) + "_y", _tileModeButtonRect.y);
        _tileModeButtonRect.width = EditorPrefs.GetFloat(nameof(_tileModeButtonRect) + "_w", _tileModeButtonRect.width);
        _tileModeButtonRect.height = EditorPrefs.GetFloat(nameof(_tileModeButtonRect) + "_h", _tileModeButtonRect.height);
    }



    private void OnGUI()
    {　
        // 言語設定
        _language = (Language)EditorGUILayout.EnumPopup("Language", _language);
        
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();

        // リッチテキストを許可するためのGUIStyle変数
        GUIStyle style = new GUIStyle(EditorStyles.label);
        style.richText = true;

        // 言語によって表示を変える。
        switch (_language)
        {
            case Language.Japanese:

                EditorGUILayout.LabelField("<size=20><b>CreateTileEditorへようこそ</b></size>", style);

                EditorGUILayout.Space();

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("ボタン設定",EditorStyles.boldLabel);

                EditorGUILayout.Space();

                _buttonSize = EditorGUILayout.FloatField("ボタンの大きさ", _buttonSize);
                _padding = EditorGUILayout.FloatField("ボタンとボタンの間隔", _padding);

                EditorGUILayout.Space();

                _tileModeButtonRect = EditorGUILayout.RectField("モードスイッチボタンのRect位置",_tileModeButtonRect);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("タイルの情報が入っているスクリプタブルオブジェクトのパス(拡張子も忘れずに)");
                _tileDataTablePath = EditorGUILayout.TextField("", _tileDataTablePath);

                break;

            case Language.English:

                EditorGUILayout.LabelField("<size=20><b>Welcome to CreateTileEditor</b></size>",style);

                EditorGUILayout.Space();

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Button Settings", EditorStyles.boldLabel);

                EditorGUILayout.Space();

                _buttonSize = EditorGUILayout.FloatField("size", _buttonSize);
                _padding = EditorGUILayout.FloatField("padding", _padding);

                EditorGUILayout.Space();

                _tileModeButtonRect = EditorGUILayout.RectField("Rect position to ModeSwitchButton", _tileModeButtonRect);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Tile information Path");
                _tileDataTablePath = EditorGUILayout.TextField("", _tileDataTablePath);

                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetFloat(nameof(_buttonSize), _buttonSize);
            EditorPrefs.SetFloat(nameof(_padding), _padding);

            EditorPrefs.SetFloat(nameof(_tileModeButtonRect) + "_x", _tileModeButtonRect.x);
            EditorPrefs.SetFloat(nameof(_tileModeButtonRect) + "_y", _tileModeButtonRect.y);
            EditorPrefs.SetFloat(nameof(_tileModeButtonRect) + "_w", _tileModeButtonRect.width);
            EditorPrefs.SetFloat(nameof(_tileModeButtonRect) + "_h", _tileModeButtonRect.height);

        }
    }
}