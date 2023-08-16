using Tetris.Interfaces;
using Tetris.Services;
using UnityEngine;

namespace Tetris.MainMenu
{
  public class MainMenuSetup : MonoBehaviour
  {
    [SerializeField] private MainMenuView _mainMenuView;

    private PlayButtonHandler _playButtonHandler;
    private ExitButtonHandler _exitButtonHandler;

    private IInit[] _inits;
    
    private void Awake()
    {
      SceneService sceneService = ServiceLocator.Instance.GetService<SceneService>();
      _playButtonHandler = new PlayButtonHandler(sceneService, _mainMenuView);
      _exitButtonHandler = new ExitButtonHandler(_mainMenuView);

      _inits = new IInit[]
      {
        _playButtonHandler,
        _exitButtonHandler
      };
    }

    private void Start()
    {
      foreach (var init in _inits)
      {
        init.Init();
      }
    }
  }
}