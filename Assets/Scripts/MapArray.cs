using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.;

public class MapArray : MonoBehaviour
{
    // マップエディタのモード
    public enum MapEditorMode
    {
        Create,
        Random,
    }

    // マップエディタのモード
    [SerializeField] private MapEditorMode _mapEditorMode = MapEditorMode.Create;

    [SerializeField] private GameObject[] Tiles;


    public int[,] Map { get; set; } = new int[100, 100];

    private void Start()
    {

        if (_mapEditorMode == MapEditorMode.Create)
        {
            Tiles = GameObject.FindGameObjectsWithTag("Tile");

            for (int i = 0; i < Tiles.Length; i++)
            {
                int x = 0;
                int y = 0;
                x = (int)Tiles[i].gameObject.transform.position.x;
                y = (int)Tiles[i].gameObject.transform.position.y;

                int _tileType = (int)Tiles[i].GetComponent<Tile>()._tileType;
                Map[x, y] = _tileType;
            }
        }
        else if (_mapEditorMode == MapEditorMode.Random)
        {

        }

    }
}
