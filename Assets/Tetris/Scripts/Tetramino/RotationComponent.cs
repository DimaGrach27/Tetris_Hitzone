using Tetris.Gameplay;
using Tetris.Global;
using Tetris.Interfaces;
using Tetris.Tiles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tetris.Tetramino
{
  public class RotationComponent : IComponent, IInit, IDestroy
  {
    private readonly TileMapService _tileMapService;
    private readonly ArrowPanel _arrowPanel;
    private readonly PauseHandler _pauseHandler;
    
    private TetraminoView _tetraminoView;
    private InputKeys _inputKeys;

    private int _currentRotation;
    
    public RotationComponent(TileMapService tileMapService, ArrowPanel arrowPanel, PauseHandler pauseHandler)
    {
      _tileMapService = tileMapService;
      _arrowPanel = arrowPanel;
      _pauseHandler = pauseHandler;
    }

    public void SetTetramino(TetraminoView tetraminoView)
    {
      _tetraminoView = tetraminoView;
      _currentRotation = 0;
    }
    
    public void Init()
    {
      _inputKeys = new InputKeys();
      _inputKeys.Enable();
      
      _inputKeys.Move.ChnageForm.performed += ChangeFormOnPerformed;
      _arrowPanel.OnClickArrow += OnClickArrowHandler;
    }

    private void OnClickArrowHandler(Vector2 dir)
    {
      if (dir != Vector2.up)
      {
        return;
      }
      
      ChangeForm();
    }

    private void ChangeFormOnPerformed(InputAction.CallbackContext callbackContext)
    {
      ChangeForm();
    }

    private void ChangeForm()
    {
      if (_pauseHandler.IsPause)
      {
        return;
      }
      
      if (_tetraminoView == null)
      {
        return;
      }
      
      if (!CheckBound() || !CheckOtherBlocks())
      {
        return;
      }
      
      _currentRotation += 90;

      if (_tetraminoView.IsOnlyTwoSideRotation && _currentRotation > 90)
      {
        _currentRotation = 0;
      }
      
      _tetraminoView.Rotation(_currentRotation);
    }

    private bool CheckOtherBlocks()
    {
      Bound bound = _tetraminoView.GetBound();

      for (float x = bound.xMin; x < bound.xMax; x++)
      {
        for (float y = bound.yMin; y < bound.yMax; y++)
        {
          Vector2 pos = new Vector2(x, y);
          Block block = _tileMapService.GetTileByPos(pos).Block;

          if (block != null && block.Owner != _tetraminoView)
          {
            return false;
          }
        }
      }

      return true;
    }

    private bool CheckBound()
    {
      Bound bound = _tetraminoView.GetBound();

      if (bound.xMax > Constants.WIDTH_FIELD / 2)
      {
        return false;
      }
      
      if (bound.xMin < -Constants.WIDTH_FIELD / 2)
      {
        return false;
      }
      
      if (bound.yMax > Constants.HEIGHT_FIELD / 2)
      {
        return false;
      }
      
      if (bound.yMin < -Constants.HEIGHT_FIELD / 2)
      {
        return false;
      }
      
      return true;
    }

    public void Disable()
    {
      _tetraminoView = null;
    }

    public void Destroy()
    {
      _arrowPanel.OnClickArrow -= OnClickArrowHandler;

      _inputKeys.Move.ChnageForm.performed -= ChangeFormOnPerformed;
      _inputKeys.Disable();
    }
  }
}