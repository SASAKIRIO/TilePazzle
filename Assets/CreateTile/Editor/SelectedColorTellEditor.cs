using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



/// <summary>
/// タイルを選択した時にそれは何のタイルかを表示してくれるEditor
/// </summary>
[InitializeOnLoad]
public class SelectedColorTellEditor
{
    private static GameObject _labelObject;
    private static string _tagName = "Label";

    static SelectedColorTellEditor()
    {
        // Tagが付いてるオブジェクトを取得
        _labelObject = GameObject.FindGameObjectWithTag(_tagName);

        //シーンビュー上のイベントを取得するため、メソッドを設定
        SceneView.duringSceneGui += EventOnSceneView;
    }


    /// <summary>
    /// SceneView上でイベントが発生した時に呼ばれる関数
    /// </summary>
    /// <param name="scene"></param>
    private static void EventOnSceneView(SceneView scene)
    {
        // タイルモードでないときreturn
        if (!CreateTileEditor._tileMode) return;

        // マウス座標にラベルオブジェクトを移動
        _labelObject.transform.position = ClickInstanceTile.GetMousePosition();

        // リッチテキストを許可するためのGUIStyle変数
        GUIStyle style = new GUIStyle(EditorStyles.label);
        style.richText = true;

        // Tipsを表示
        Handles.Label(_labelObject.transform.position + Vector3.up, CreateTileEditor._tipsString, style);

        // TipsTextureを表示
        Handles.Label(_labelObject.transform.position+Vector3.up, CreateTileEditor._tipsTexture);
    }
}
