// ビルドエラー対策用スニペット
#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEditor;
//using UnityEngine.;

[CustomEditor(typeof(SnapMove))]
[CanEditMultipleObjects]
public class SnapMoveEditor : Editor
{
    //Debug.Log("");

    private SnapMove[] instances;
    //オブジェクトのどこを中心とするか
    private Vector3 Center = Vector3.zero;
    //オブジェクトを動かす時のデッドゾーン
    private readonly int GridSize = 1;
    // フリーハンドルの半径
    private float FreeHandleRadius = 0.5f;

    private void OnEnable()
    {
        instances = targets.Cast<SnapMove>().ToArray();
    }


    /// <summary>
    /// OnSceneGUI
    /// </summary>
    private void OnSceneGUI()
    {
        // 基本ツールの使用を規制する。
        Tools.current = Tool.None;

        // オブジェクトの中心座標をもとめて設定する。
        Center = GetCenterOfInstances(instances);

        //円形ハンドルの描画
        FreeHandle();

    }



    private void FreeHandle()
    {
        //円形の描画
        Handles.color = Color.magenta;

        EditorGUI.BeginChangeCheck();

        // フリーハンドルを生成　(非推奨らしい。変わりが見つからない...)
        Vector3 pos = Handles.FreeMoveHandle(
                                             // 中心位置に生成
                                             Center, 

                                             // 回転させないで生成
                                             Quaternion.identity, 

                                             // フリーハンドルの半径
                                             FreeHandleRadius, 

                                             // 
                                             Vector3.one,

                                             // サークルハンドルを描画
                                             Handles.CircleHandleCap
                                            );

        
        if (EditorGUI.EndChangeCheck())
        {
            MoveObject(pos - Center);
        }
    }

    /// <summary>
    /// 中心位置を求めるメソッド
    /// </summary>
    /// <param name="instances"></param>
    /// <returns></returns>
    private static Vector3 GetCenterOfInstances(SnapMove[] instances)
    {
        float x = 0f, y = 0f;


        foreach (SnapMove ins in instances)
        {
            Vector3 pos = ins.transform.position;
            x += pos.x;
            y += pos.y;
        }
        return new Vector3(x / instances.Length,
                            y / instances.Length,
                            0);
    }


    /// <summary>
    /// グリッドに合わせて動かす
    /// </summary>
    /// <param name="vec3"></param>
    private void MoveObject(Vector3 vec3)
    {
        // 移動量を丸める(Mathf重いから改善したい)
        Vector2Int RoundVector2 = new Vector2Int(Mathf.RoundToInt(vec3.x/GridSize),Mathf.RoundToInt(vec3.y / GridSize));

        // 移動量が無い場合returnする
        if (RoundVector2 == Vector2.zero) return;

        foreach(var ins in instances)
        {
            Object[] objects = { ins, ins.transform };
            Undo.RecordObjects(objects, "オブジェクトの移動");
            ins.Move(RoundVector2);
        }
    }
}
#endif