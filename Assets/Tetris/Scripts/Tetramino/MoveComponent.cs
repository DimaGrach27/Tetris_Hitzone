using System;
using Tetris.Interfaces;
using Tetris.Tetris.Scripts.Tiles;
using UnityEngine;

namespace Tetris.Tetramino
{
  public class MoveComponent : IComponent
  {
    public event Action OnPlaced;
    
    private readonly TileMapController _tileMapController;
    
    private float _speed = 1.0f;
    private float _deltaTimeDur;
    
    private bool _isCanMove = true;

    private TetraminoView _tetraminoView;
    
    public MoveComponent(TileMapController tileMapController)
    {
      _tileMapController = tileMapController;
    }
    
    public void SetTetramino(TetraminoView tetraminoView)
    {
      _tetraminoView = tetraminoView;
      _isCanMove = true;
      _deltaTimeDur = 0.0f;
    }

    public void Disable()
    {
      _tetraminoView = null;
    }
    
    public void Tick()
    {
      if (_tetraminoView == null)
      {
        return;
      }
      
      if (!_isCanMove)
      {
        return;
      }
      
      _deltaTimeDur += Time.deltaTime;

      if (_deltaTimeDur <= _speed)
      {
        return;
      }

      _deltaTimeDur -= _speed;
      
      if (!CheckDown())
      {
        _isCanMove = false;
        OnPlaced?.Invoke();
        return;
      }
      
      _tetraminoView.Move(Vector2.down);
    }
    
    private bool CheckDown()
    {
      foreach (var block in _tetraminoView.BLocks)
      {
        if (block.DownDir.y <= -5.0f || 
            _tileMapController.GetTileByPos(block.DownDir).Block != null)
        {
          return false;
        }
      }
      
      return true;
    }
  }
}