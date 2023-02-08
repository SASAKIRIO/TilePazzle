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
    /// シーンビューのGUI
    /// </summary>
    private void OnSceneGUI()
    {
        //シーンビューでのツールを未選択にする
        Tools.current = Tool.None;

        Center = GetCenterOfInstances(instances);
        EditorGUI.BeginChangeCheck();

       
    }



    /// <summary>
    /// 複数のインスタンスの中心を返す
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

            
        }
    }
}
