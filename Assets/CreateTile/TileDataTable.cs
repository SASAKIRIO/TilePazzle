using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スクリプタブルオブジェクトのデータ
/// </summary>
[Serializable]
public class TileData
{
    // タイル名
    public string Name;

    // タイルアイコン
    public Sprite icon;
    
    // タイルのオブジェクト（プレハブ）
    public GameObject gameObject;

    // タイルのtips 文字列型
    public string tipsString;

    // タイルのtips Texture型
    public Texture tipsTexture;
}


/// <summary>
/// スクリプタブルオブジェクトの生成
/// </summary>
[CreateAssetMenu(fileName = "TileDataTable", menuName = "ScriptableObject/TileDataTable")]
public class TileDataTable : ScriptableObject
{
    public List<TileData> dataList = new List<TileData>();
}