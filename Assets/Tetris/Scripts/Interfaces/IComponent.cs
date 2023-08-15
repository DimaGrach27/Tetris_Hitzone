using Tetris.Tetramino;

namespace Tetris.Interfaces
{
  public interface IComponent
  {
    public void SetTetramino(TetraminoView tetraminoView);
    public void Disable();
  }
}