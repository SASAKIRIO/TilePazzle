using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileData
{
    public Sprite icon;
    public GameObject gameObject;
}

[CreateAssetMenu(fileName = "TileDataTable", menuName = "ScriptableObject/TileDataTable")]
public class TileDataTable : ScriptableObject
{
    public List<TileData> dataList = new List<TileData>();
}