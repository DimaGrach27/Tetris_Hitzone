using Tetris.Services;

namespace Tetris.Gameplay
{
  public class ExitHandler
  {
    private readonly SceneService _sceneService;

    public ExitHandler(SceneService sceneService)
    {
      _sceneService = sceneService;
    }

    public void Exit()
    {
      _sceneService.LoadScene(SceneName.MainMenu);
    }
  }
}