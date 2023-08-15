using System;
using System.Collections.Generic;
using Tetris.Tetramino;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tetris.Tetris.Scripts.Tiles
{
  public class TileMapController : MonoBehaviour
  {
    [SerializeField] private Tilemap _tilemap;

    private readonly Dictionary<Vector3Int, Tile> _tilesMap = new();

    private void Awake()
    {
      BoundsInt bounds = _tilemap.cellBounds;

      for (int x = bounds.xMin; x < bounds.xMax; x++)
      {
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
          Vector2 pos = new Vector2(x, y);
          Vector3Int cell = WorldToCell(pos);
          TileBase tile = _tilemap.GetTile(cell);
          
          if (tile != null)
          {
            _tilesMap.Add(cell, new Tile());
            CreateDebugCoord(pos);
          }
        }
      }
      
      print("end");
    }

    private void CreateDebugCoord(Vector2 pos)
    {
      GameObject gm = new GameObject("Coord");
      gm.transform.position = pos + Vector2.one * 0.5f;
      TextMeshPro tm = gm.AddComponent<TextMeshPro>();
      tm.text = $"({pos.x}, {pos.y})";
      tm.fontSize = 3.0f;
      tm.alignment = TextAlignmentOptions.Center;
      tm.sortingOrder = 100;
      tm.color = Color.black;
      RectTransform rectTransform = gm.GetComponent<RectTransform>();
      rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1.0f);
      rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1.0f);
    }

    public void AddBlock(Vector2 pos, Block block)
    {
      Vector3Int cell = WorldToCell(pos);
      _tilesMap[cell].Block = block;
    }

    public void RemoveBlock(Vector2 pos)
    {
      Vector3Int cell = WorldToCell(pos);
      _tilesMap[cell].Block = null;
    }

    public Tile GetTileByPos(Vector2 pos)
    {
      Vector3Int cell = WorldToCell(pos);

      return _tilesMap.TryGetValue(cell, out Tile tile) ? tile : null;
    }

    private Vector3Int WorldToCell(Vector3 worldPos)
    {
      return _tilemap.WorldToCell(worldPos);
    }
    
    private Vector3 CellToWorld(Vector3Int cellPos)
    {
      return _tilemap.CellToWorld(cellPos);
    }
  }
}