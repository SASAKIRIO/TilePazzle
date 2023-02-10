using UnityEngine;

[SelectionBase]
public class SnapMove : MonoBehaviour
{
    // GameConfig.GridSize ��1�ڐ���Ƃ����ꍇ�́A�O���b�h���W
    [SerializeField] public Vector2Int gridPos = Vector2Int.zero;


    /// <summary>
    /// �O���b�h�ړ��ʂ��w�肵�Ĉړ�����
    /// </summary>
    public void Move(Vector2Int vec2)
    {
        gridPos += vec2;
        transform.position = GetGlobalPosition(gridPos);
    }

    /// <summary>
    /// �O���b�h���W��Global���W�ɕϊ�����
    /// </summary>
    public static Vector3 GetGlobalPosition(Vector2Int gridPos)
    {
        return new Vector3(
            gridPos.x,
            gridPos.y,
            0);
    }
}