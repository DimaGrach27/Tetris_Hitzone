using Tetris.Interfaces;
using Tetris.Tetris.Scripts.Tiles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tetris.Tetramino
{
  public class RotationComponent : IComponent, IInit, IDestroy
  {
    private readonly TileMapController _tileMapController;
    private TetraminoView _tetraminoView;
    private InputKeys _inputKeys;

    private int _currentRotation;
    
    public RotationComponent(TileMapController tileMapController)
    {
      _tileMapController = tileMapController;
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
    }

    private void ChangeFormOnPerformed(InputAction.CallbackContext callbackContext)
    {
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
          Block block = _tileMapController.GetTileByPos(pos).Block;

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

      if (bound.xMax > 5.0f)
      {
        return false;
      }
      
      if (bound.xMin < -5.0f)
      {
        return false;
      }
      
      if (bound.yMax > 5.0f)
      {
        return false;
      }
      
      if (bound.yMin < -5.0f)
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
      _inputKeys.Move.ChnageForm.performed -= ChangeFormOnPerformed;
      _inputKeys.Disable();
    }
  }
}