using Tetris.Interfaces;
using Tetris.Services;

namespace Tetris.MainMenu
{
  public class PlayButtonHandler : IInit
  {
    private readonly SceneService _sceneService;
    private readonly MainMenuView _mainMenuView;

    public PlayButtonHandler(SceneService sceneService, MainMenuView mainMenuView)
    {
      _sceneService = sceneService;
      _mainMenuView = mainMenuView;
    }

    public void Init()
    {
      _mainMenuView.SubscribePlayButton(ClickPlay);
    }

    private void ClickPlay()
    {
      _sceneService.LoadScene(SceneName.Gameplay);
    }
  }
}