using System;
using Tetris.Gameplay;
using Tetris.Global;
using Tetris.Interfaces;
using Tetris.Tiles;
using UnityEngine;

namespace Tetris.Tetramino
{
  public class MoveComponent : IComponent, ICondition, ITickable
  {
    public event Action<TetraminoView> OnBLockPLaced;

    private readonly TileMapService _tileMapService;
    private readonly GameplayModel _gameplayModel;

    private float _deltaTimeDur;
    
    private bool _isCanMove = true;
    private bool _isBlockControl = false;

    private TetraminoView _tetraminoView;
    
    public MoveComponent(TileMapService tileMapService, GameplayModel gameplayModel)
    {
      _tileMapService = tileMapService;
      _gameplayModel = gameplayModel;
    }
    
    public void SetTetramino(TetraminoView tetraminoView)
    {
      _tetraminoView = tetraminoView;
      _isCanMove = true;
      _isBlockControl = false;
      _deltaTimeDur = 0.0f;
    }

    public void Disable()
    {
      _tetraminoView = null;
    }

    public void SetIsBlockControl(bool value)
    {
      _isBlockControl = value;
    }
    
    public void Tick(float deltaTime)
    {
      if (_tetraminoView == null)
      {
        return;
      }

      if (_isBlockControl)
      {
        return;
      }
      
      if (!_isCanMove)
      {
        return;
      }
      
      _deltaTimeDur += Time.deltaTime;

      if (_deltaTimeDur <= _gameplayModel.MoveTick)
      {
        return;
      }

      _deltaTimeDur -= _gameplayModel.MoveTick;
      
      if (!CheckDown())
      {
        _isCanMove = false;
        OnBLockPLaced?.Invoke(_tetraminoView);
        return;
      }
      
      _tetraminoView.Move(Vector2.down);
    }
    
    private bool CheckDown()
    {
      foreach (var block in _tetraminoView.BLocks)
      {
        if (block.DownDir.y <= -Constants.HEIGHT_FIELD / 2 || 
            _tileMapService.GetTileByPos(block.DownDir).Block != null)
        {
          return false;
        }
      }
      
      return true;
    }
  }
}