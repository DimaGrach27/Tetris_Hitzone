using System;
using Tetris.Tetramino;

namespace Tetris.Interfaces
{
  public interface ICondition
  {
    public event Action<TetraminoView> OnBLockPLaced;
  }
}