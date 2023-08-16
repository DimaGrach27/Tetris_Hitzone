using System.Collections.Generic;
using Tetris.Global;
using Tetris.Interfaces;
using Tetris.Services;
using Tetris.Tetramino;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace Tetris.Tiles
{
  public class TileMapService : IService, IInit
  {
    private readonly Tilemap _tilemap;

    private Transform _debugCoordContainer;

    private readonly Dictionary<Vector3Int, Tile> _tilesMap = new();

    public TileMapService(Tilemap tilemap)
    {
      _tilemap = tilemap;
    }
    
    public void Init()
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
    }

    private void CreateDebugCoord(Vector2 pos)
    {
      if (_debugCoordContainer == null)
      {
        _debugCoordContainer = new GameObject("CoordContainer").transform;
      }
      
      GameObject gm = new GameObject("Coord");
      gm.transform.position = pos + Vector2.one * 0.5f;
      gm.transform.SetParent(_debugCoordContainer);
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

    public List<Tile> GetLineByY(int yCoord)
    {
      List<Tile> tiles = new List<Tile>();

      foreach (var keyValue in _tilesMap)
      {
        if (keyValue.Key.y == yCoord)
        {
          tiles.Add(keyValue.Value);
        }
      }
      
      return tiles;
    }

    public void DeleteLine(int y)
    {
      foreach (var keyValue in _tilesMap)
      {
        if (keyValue.Key.y == y)
        {
          Object.Destroy(keyValue.Value.Block.gameObject);
          keyValue.Value.Block = null;
        }
      }
    }
    
    public void RefreshLines(int minY)
    {
      int y = minY + 1;
      int minEmpty = minY;
      
      while (y <= Constants.HEIGHT_FIELD / 2)
      {
        if (CheckEmptyLine(y))
        {
          y++;
          continue;
        }

        int offset = y - minEmpty;
        MoveLine(y, offset);

        minEmpty++;
      }
    }

    private void MoveLine(int fromY, int offset)
    {
      foreach (var keyValue in _tilesMap)
      {
        if (keyValue.Key.y == fromY)
        {
          Block block = keyValue.Value.Block;
            
          if(block != null)
          {
            Vector3 newPos = block.Position;
            newPos.y -= offset;
            block.SetPosition(newPos);
            _tilesMap[WorldToCell(newPos)].Block = block;
          }
            
          keyValue.Value.Block = null;
        }
      }
    }

    private bool CheckEmptyLine(int y)
    {
      foreach (var tile in GetLineByY(y))
      {
        if (tile.Block != null)
        {
          return false;
        }
      }
      
      return true;
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

    public Vector3Int WorldToCell(Vector3 worldPos)
    {
      return _tilemap.WorldToCell(worldPos);
    }
    
    public Vector3 CellToWorld(Vector3Int cellPos)
    {
      return _tilemap.CellToWorld(cellPos);
    }
  }
}