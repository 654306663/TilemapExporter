using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapBehaviour : MonoBehaviour
{
    private Tilemap tileMap;
    public Tilemap Tilemap
    {
        get
        {
            if (tileMap == null)
                tileMap = GetComponent<Tilemap>();
            return tileMap;
        }
    }

    public BoundsInt area;
}
