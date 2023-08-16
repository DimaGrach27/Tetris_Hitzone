using Tetris.Interfaces;

namespace Tetris.Gameplay
{
  public class RestartHandler
  {
    private readonly IRestart[] _restarts;

    public RestartHandler(IRestart[] restarts)
    {
      _restarts = restarts;
    }

    public void Restart()
    {
      foreach (var restart in _restarts)
      {
        restart.Restart();
      }
    }
  }
}