using System;

namespace Tetris.Interfaces
{
  public interface ICondition
  {
    public event Action OnBLockPLaced;
  }
}