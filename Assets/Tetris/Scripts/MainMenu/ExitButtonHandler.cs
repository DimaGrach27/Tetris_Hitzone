using Tetris.Interfaces;
using UnityEngine;

namespace Tetris.MainMenu
{
  public class ExitButtonHandler : IInit
  {
    private readonly MainMenuView _mainMenuView;

    public ExitButtonHandler(MainMenuView mainMenuView)
    {
      _mainMenuView = mainMenuView;
    }
    
    public void Init()
    {
      _mainMenuView.SubscribeExitButton(ClickExit);
    }

    private void ClickExit()
    {
      Application.Quit();
    }
  }
}