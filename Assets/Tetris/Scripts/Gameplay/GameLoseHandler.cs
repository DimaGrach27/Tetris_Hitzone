using Tetris.Tetramino;
using Tetris.Tiles;
using UnityEngine;

namespace Tetris.Gameplay
{
  public class GameLoseHandler
  {
    private readonly TileMapService _tileMapService;
    private readonly LoseScreen _loseScreen;

    public GameLoseHandler(TileMapService tileMapService, LoseScreen loseScreen)
    {
      _tileMapService = tileMapService;
      _loseScreen = loseScreen;
    }

    public bool CheckIsCollision(TetraminoView tetraminoView)
    {
      foreach (var block in tetraminoView.BLocks)
      {
        Tile tile = _tileMapService.GetTileByPos(block.Position);
        if (tile.Block != null)
        {
          Lose();
          return true;
        }
      }
      
      return false;
    }

    private void Lose()
    {
      _loseScreen.Show();
    }
  }
}