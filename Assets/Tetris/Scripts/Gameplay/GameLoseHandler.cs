using Tetris.Tetramino;
using Tetris.Tiles;

namespace Tetris.Gameplay
{
  public class GameLoseHandler
  {
    private readonly TileMapService _tileMapService;

    public GameLoseHandler(TileMapService tileMapService)
    {
      _tileMapService = tileMapService;
    }

    public bool CheckIsCollision(TetraminoView tetraminoView)
    {
      foreach (var block in tetraminoView.BLocks)
      {
        Tile tile = _tileMapService.GetTileByPos(block.Position);
        if (tile.Block != null)
        {
          return true;
        }
      }
      
      return false;
    }
  }
}