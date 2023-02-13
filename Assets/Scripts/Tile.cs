using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.;

public class Tile : MonoBehaviour
{
    [Tooltip("タイルのタイプ")]
    public enum Tile_Type
    {
        /// <summary>
        /// なにもタイルがない
        /// </summary>
        None,
        /// <summary>
        /// 効果なし
        /// </summary>
        Pink,
        /// <summary>
        /// 効果なし、(パズル後戦闘)
        /// </summary>
        Green,
        /// <summary>
        /// 一つ前のタイルに戻される
        /// </summary>
        Yellow,
        /// <summary>
        /// オレンジの香りがつく
        /// </summary>
        Orange,
        /// <summary>
        /// 石鹸ですべる。レモンの香りがつく
        /// </summary>
        Purple,
        /// <summary>
        /// 黄色と隣接 -> ひとつ前に戻る
        /// オレンジの香りがついている -> 一つ前に戻る
        /// </summary>
        Blue,
        /// <summary>
        /// 通行禁止
        /// </summary>
        Red,
    }

    public enum Tile_Feature
    {
        None,
        Bounce,
        Slip,
    }


    [Header("タイルの種類")]
    [SerializeField] public Tile_Type _tileType = default;

    
}
