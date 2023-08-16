using System.Collections.Generic;
using Tetris.Tetramino;

namespace Tetris.Tiles
{
  public class CheckTilesLine
  {
    private readonly TileMapService _tileMapService;

    public CheckTilesLine(TileMapService tileMapService)
    {
      _tileMapService = tileMapService;
    }

    public int BLockPLacedHandler(TetraminoView tetraminoView)
    {
      List<int> lines = new List<int>();
      foreach (var block in tetraminoView.BLocks)
      {
        int yCoord = _tileMapService.WorldToCell(block.Position).y;

        if (!lines.Contains(yCoord))
        {
          lines.Add(yCoord);
        }
      }

      List<int> deleteLines = new List<int>();

      foreach (var yCoord in lines)
      {
        if (CheckLine(_tileMapService.GetLineByY(yCoord)))
        { 
          deleteLines.Add(yCoord);
        }
      }

      deleteLines.Sort();
      
      if (deleteLines.Count == 0)
      {
        return 0;
      }
      
      foreach (var y in deleteLines)
      {
        _tileMapService.DeleteLine(y);
      }

      _tileMapService.RefreshLines(deleteLines[0]);

      return deleteLines.Count;
    }

    private bool CheckLine(List<Tile> tiles)
    {
      foreach (var tile in tiles)
      {
        if (tile.Block == null)
        {
          return false;
        }
      }

      return true;
    }
  }
}