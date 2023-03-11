using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



/// <summary>
/// �^�C����I���������ɂ���͉��̃^�C������\�����Ă����Editor
/// </summary>
[InitializeOnLoad]
public class SelectedColorTellEditor
{
    private static GameObject _labelObject;
    private static string _tagName = "Label";

    static SelectedColorTellEditor()
    {
        // Tag���t���Ă�I�u�W�F�N�g���擾
        _labelObject = GameObject.FindGameObjectWithTag(_tagName);

        //�V�[���r���[��̃C�x���g���擾���邽�߁A���\�b�h��ݒ�
        SceneView.duringSceneGui += EventOnSceneView;
    }


    /// <summary>
    /// SceneView��ŃC�x���g�������������ɌĂ΂��֐�
    /// </summary>
    /// <param name="scene"></param>
    private static void EventOnSceneView(SceneView scene)
    {
        // �^�C�����[�h�łȂ��Ƃ�return
        if (!CreateTileEditor._tileMode) return;

        // �}�E�X���W�Ƀ��x���I�u�W�F�N�g���ړ�
        _labelObject.transform.position = ClickInstanceTile.GetMousePosition();

        // ���b�`�e�L�X�g�������邽�߂�GUIStyle�ϐ�
        GUIStyle style = new GUIStyle(EditorStyles.label);
        style.richText = true;

        // Tips��\��
        Handles.Label(_labelObject.transform.position + Vector3.up, CreateTileEditor._tipsString, style);

        // TipsTexture��\��
        Handles.Label(_labelObject.transform.position+Vector3.up, CreateTileEditor._tipsTexture);
    }
}
