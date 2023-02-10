using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using UnityEngine.;

[CustomEditor(typeof(SnapMove))]
[CanEditMultipleObjects]
public class SnapMoveEditor : Editor
{
    //Debug.Log("");

    private SnapMove[] instances;
    private Vector3 Center = Vector3.zero;
    private readonly int GridSize = 16;
    private void OnEnable()
    {
        instances = targets.Cast<SnapMove>().ToArray();
    }


    /// <summary>
    /// �V�[���r���[��GUI
    /// </summary>
    [System.Obsolete]
    private void OnSceneGUI()
    {
        Tools.current = Tool.None;

        Center = GetCenterOfInstances(instances);

        FreeHandle();
        AxisHandle(Color.red, Vector2Int.right);
        AxisHandle(Color.green, Vector2Int.up);
        AxisHandle(Color.cyan, Vector2Int.left);
        AxisHandle(Color.yellow, Vector2Int.down);
    }

    [System.Obsolete]
    private void FreeHandle()
    {
        Handles.color = Color.magenta;

        EditorGUI.BeginChangeCheck();
        var pos = Handles.FreeMoveHandle(Center, Quaternion.identity, 1f, Vector3.one,
            Handles.CircleHandleCap);

        
        if (EditorGUI.EndChangeCheck())
        {
            MoveObject(pos - Center);
        }
    }

    /// <summary>
    /// �����̃C���X�^���X�̒��S��Ԃ�
    /// </summary>
    /// <param name="instances"></param>
    /// <returns></returns>
    private static Vector3 GetCenterOfInstances(SnapMove[] instances)
    {
        float x = 0f, y = 0f;

        foreach(SnapMove ins in instances)
        {
            Vector3 pos = ins.transform.position;
            x += pos.x;
            y += pos.y;
        }
        return new Vector3(x / instances.Length,
                           y / instances.Length,
                           0);
    }



    private void AxisHandle(Color color,Vector2 direction)
    {
        Handles.color = color;
        EditorGUI.BeginChangeCheck();
        var deltaMovement = Handles.Slider(Center, new Vector3(direction.x, direction.y, 0)) - Center;

        if (EditorGUI.EndChangeCheck())
        {
            var dot = Vector2.Dot(deltaMovement, direction);
            if (!(Mathf.Abs(dot) > Mathf.Epsilon)) return;

            MoveObject(dot * direction);
        }
    }



    private void MoveObject(Vector3 vec3)
    {
        var vec2 = new Vector2Int(Mathf.RoundToInt(vec3.x/GridSize),Mathf.RoundToInt(vec3.y / GridSize));

        if (vec2 == Vector2.zero) return;

        foreach(var ins in instances)
        {
            Object[] objects = { ins, ins.transform };
            Undo.RecordObjects(objects, "オブジェクトの移動");
            ins.Move(vec2);
        }
    }
}
