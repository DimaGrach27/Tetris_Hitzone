using Tetris.Interfaces;
using Tetris.Tetris.Scripts.Tiles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tetris.Tetramino
{
  public class ControlComponent : IComponent, IInit, IDestroy
  {
    private readonly TileMapController _tileMapController;
    private TetraminoView _tetraminoView;
    private InputKeys _inputKeys;

    public ControlComponent(TileMapController tileMapController)
    {
      _tileMapController = tileMapController;
    }
    
    public void Init()
    {
      _inputKeys = new InputKeys();
      _inputKeys.Enable();
      
      _inputKeys.Move.MoveLeft.performed += MoveLeftOnPerformed;
      _inputKeys.Move.MoveRight.performed += MoveRightOnPerformed;
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
          isCanMove &= block.LeftDir.x > -5.0f;
          isCanMove &= _tileMapController.GetTileByPos(block.LeftDir).Block == null;

          if (!isCanMove)
          {
            return false;
          }
        }
        else
        {
          bool isCanMove = true;
          isCanMove &= block.RightDir.x < 5.0f;
          isCanMove &= _tileMapController.GetTileByPos(block.RightDir).Block == null;

          if (!isCanMove)
          {
            return false;
          }
        }
      }
      
      return true;
    }

    public void SetTetramino(TetraminoView tetraminoView)
    {
      _tetraminoView = tetraminoView;
    }

    public void Disable()
    {
      _tetraminoView = null;
    }

    public void Destroy()
    {
      _inputKeys.Move.MoveLeft.performed -= MoveLeftOnPerformed;
      _inputKeys.Move.MoveRight.performed -= MoveRightOnPerformed;
      _inputKeys.Disable();
    }
  }
}