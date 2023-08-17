using Tetris.Interfaces;

namespace Tetris.Gameplay
{
  public class GamePlayMenuController : IInit, IDestroy
  {
    private readonly ButtonPanel _buttonPanel;
    private readonly ExitHandler _exitHandler;
    private readonly PauseHandler _pauseHandler;

    public GamePlayMenuController(ButtonPanel buttonPanel, ExitHandler exitHandler, PauseHandler pauseHandler)
    {
      _buttonPanel = buttonPanel;
      _exitHandler = exitHandler;
      _pauseHandler = pauseHandler;
    }

    public void Init()
    {
      _buttonPanel.OnClickPause += OnClickPauseHandler;
      _buttonPanel.OnClickExit += OnClickExitHandler;
    }

    private void OnClickExitHandler()
    {
      _exitHandler.Exit();
    }

    private void OnClickPauseHandler()
    {
      _pauseHandler.PauseGame();
    }

    public void Destroy()
    {
      _buttonPanel.OnClickPause -= OnClickPauseHandler;
      _buttonPanel.OnClickExit -= OnClickExitHandler;
    }
  }
}