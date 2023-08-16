using Tetris.Tetramino;
using Tetris.Tiles;
using UnityEngine;

namespace Tetris.Gameplay
{
  public class BlockManager
  {
    private readonly TileMapService _tileMapService;

    private Transform _blockContainer;

    public BlockManager(TileMapService tileMapService)
    {
      _tileMapService = tileMapService;
    }
    public void OnPlacedHandler(TetraminoView tetraminoView)
    {
      foreach (var block in tetraminoView.BLocks)
      {
        _tileMapService.AddBlock(block.Position, block);
      }
      
      tetraminoView.DeattachBlocks(_blockContainer);
    }
  }
}