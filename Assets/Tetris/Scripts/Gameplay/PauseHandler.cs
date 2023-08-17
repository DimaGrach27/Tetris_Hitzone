namespace Tetris.Gameplay
{
  public class PauseHandler
  {
    private readonly ButtonPanel _buttonPanel;

    public bool IsPause { get; private set; }
    
    public PauseHandler(ButtonPanel buttonPanel)
    {
      _buttonPanel = buttonPanel;
    }
    
    public void PauseGame()
    {
      IsPause = !IsPause;

      _buttonPanel.IsPause = IsPause;
    }
  }
}