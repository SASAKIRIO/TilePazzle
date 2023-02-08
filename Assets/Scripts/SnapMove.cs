using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.;

[SelectionBase]
public class SnapMove : MonoBehaviour
{
    //Debug.Log("");
    [SerializeField] public Vector2Int GridPosition = Vector2Int.zero;

    public void OnAnimatorMove(Vector2Int vec2)
    {
        GridPosition += vec2;
        //transform.position=
    }



    //public static Vector3 GetGlobalPosition(Vector2Int GridPosition)
    //{
        
    //}
}
