using System;
using Tetris.Global;
using Tetris.Interfaces;
using Tetris.Tiles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tetris.Tetramino
{
  public class ControlComponent : IComponent, IInit, IDestroy, ICondition, ITickable
  {
    private const float BOOST_MOVE_TICK = 0.025f;
    public event Action<TetraminoView> OnBLockPLaced;

    private readonly TileMapService _tileMapService;
    private readonly MoveComponent _moveComponent;
    
    private TetraminoView _tetraminoView;
    private InputKeys _inputKeys;
    
    private bool _isBoostActive = false;
    
    private float _deltaTimeDur;

    public ControlComponent(TileMapService tileMapService, MoveComponent moveComponent)
    {
      _tileMapService = tileMapService;
      _moveComponent = moveComponent;
    }
    
    public void Init()
    {
      _inputKeys = new InputKeys();
      _inputKeys.Enable();
      
      _inputKeys.Move.MoveLeft.performed += MoveLeftOnPerformed;
      _inputKeys.Move.MoveRight.performed += MoveRightOnPerformed;
      _inputKeys.Move.Boost.started += BoostOnStarted;
      _inputKeys.Move.Boost.canceled += BoostOnCanceled;
    }
    
    private void BoostOnStarted(InputAction.CallbackContext objCallbackContext)
    {
      _isBoostActive = true;
      _moveComponent.SetIsBlockControl(true);
    }
    
    private void BoostOnCanceled(InputAction.CallbackContext objCallbackContext)
    {
      _isBoostActive = false;
      _moveComponent.SetIsBlockControl(false);
    }
    
    private void DownFall()
    {
      if (_tetraminoView == null)
      {
        return;
      }
      
      if (!_isBoostActive)
      {
        return;
      }
      
      _deltaTimeDur += Time.deltaTime;

      if (_deltaTimeDur <= BOOST_MOVE_TICK)
      {
        return;
      }

      _deltaTimeDur -= BOOST_MOVE_TICK;
      
      if (!CheckDown())
      {
        OnBLockPLaced?.Invoke(_tetraminoView);
        return;
      }
      
      _tetraminoView.Move(Vector2.down);
    }

    private void MoveLeftOnPerformed(InputAction.CallbackContext callbackContext)
    {
      if (_tetraminoView == null)
      {
        return;
      }
      
      if (!CheckDir(true))
      {
        return;
      }
      
      _tetraminoView.Move(Vector2.left);
    }

    private void MoveRightOnPerformed(InputAction.CallbackContext callbackContext)
    {
      if (_tetraminoView == null)
      {
        return;
      }
      
      if (!CheckDir(false))
      {
        return;
      }
      
      _tetraminoView.Move(Vector2.right);
    }

    private bool CheckDir(bool isLeft)
    {
      foreach (var block in _tetraminoView.BLocks)
      {
        if (isLeft)
        {
          bool isCanMove = true;
          isCanMove &= block.LeftDir.x > -Constants.WIDTH_FIELD / 2 &&
                       _tileMapService.GetTileByPos(block.LeftDir).Block == null;

          if (!isCanMove)
          {
            return false;
          }
        }
        else
        {
          bool isCanMove = true;
          isCanMove &= block.RightDir.x < Constants.WIDTH_FIELD / 2 &&
                       _tileMapService.GetTileByPos(block.RightDir).Block == null;

          if (!isCanMove)
          {
            return false;
          }
        }
      }
      
      return true;
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

    public void SetTetramino(TetraminoView tetraminoView)
    {
      _tetraminoView = tetraminoView;
      _isBoostActive = false;
    }

    public void Disable()
    {
      _tetraminoView = null;
    }

    public void Destroy()
    {
      _inputKeys.Move.MoveLeft.performed -= MoveLeftOnPerformed;
      _inputKeys.Move.MoveRight.performed -= MoveRightOnPerformed;
      _inputKeys.Move.Boost.started -= BoostOnStarted;
      _inputKeys.Move.Boost.canceled -= BoostOnCanceled;

      _inputKeys.Disable();
    }

    public void Tick(float deltaTime)
    {
      DownFall();
    }
  }
}