using System;
using Tetris.Gameplay;
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
    private readonly ArrowPanel _arrowPanel;
    private readonly PauseHandler _pauseHandler;

    private TetraminoView _tetraminoView;
    private InputKeys _inputKeys;
    
    private bool _isBoostActive = false;
    
    private float _deltaTimeDur;

    public ControlComponent(
      TileMapService tileMapService, 
      MoveComponent moveComponent, 
      ArrowPanel arrowPanel,
      PauseHandler pauseHandler
      )
    {
      _tileMapService = tileMapService;
      _moveComponent = moveComponent;
      _arrowPanel = arrowPanel;
      _pauseHandler = pauseHandler;
    }
    
    public void Init()
    {
      _inputKeys = new InputKeys();
      _inputKeys.Enable();
      
      _inputKeys.Move.MoveLeft.performed += MoveLeftOnPerformed;
      _inputKeys.Move.MoveRight.performed += MoveRightOnPerformed;
      _inputKeys.Move.Boost.started += BoostOnStarted;
      _inputKeys.Move.Boost.canceled += BoostOnCanceled;
      
      _arrowPanel.OnClickArrow += OnClickArrowHandler;
      _arrowPanel.OnStatePress += OnStatePressHandler;
    }

    private void OnStatePressHandler(Vector2 dir, bool isPressed)
    {
      if (dir != Vector2.down)
      {
        return;
      }
      
      _isBoostActive = isPressed;
      _moveComponent.SetIsBlockControl(isPressed);
    }

    private void OnClickArrowHandler(Vector2 dir)
    {
      if (dir == Vector2.right)
      {
        MoveRight();
        return;
      }
      
      if (dir == Vector2.left)
      {
        MoveLeft();
      }
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
      if (_pauseHandler.IsPause)
      {
        return;
      }
      
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
      MoveLeft();
    }
    
    private void MoveRightOnPerformed(InputAction.CallbackContext callbackContext)
    {
      MoveRight();
    }

    private void MoveLeft()
    {
      if (_pauseHandler.IsPause)
      {
        return;
      }
      
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

    private void MoveRight()
    {
      if (_pauseHandler.IsPause)
      {
        return;
      }
      
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
      _arrowPanel.OnClickArrow -= OnClickArrowHandler;
      _arrowPanel.OnStatePress -= OnStatePressHandler;

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