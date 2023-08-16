using UnityEngine.SceneManagement;

namespace Tetris.Services
{
  public class SceneService : IService
  {
    public void LoadScene(SceneName sceneName, bool isAddition = false)
    {
      LoadSceneMode loadSceneMode = isAddition ? LoadSceneMode.Additive : LoadSceneMode.Single;
      SceneManager.LoadSceneAsync(sceneName.ToString(), loadSceneMode);
    }

    public void UnloadScene(SceneName sceneName)
    {
      SceneManager.UnloadSceneAsync(sceneName.ToString());
    }
  }

  public enum SceneName
  {
    Boot,
    Gameplay,
    MainMenu
  }
}