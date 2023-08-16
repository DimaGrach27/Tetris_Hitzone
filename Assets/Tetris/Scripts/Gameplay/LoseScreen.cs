using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Gameplay
{
  public class LoseScreen : MonoBehaviour
  {
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    private RestartHandler _restartHandler;
    private ExitHandler _exitHandler;

    private void Awake()
    {
      _restartButton.onClick.AddListener(RestartHandler);
      _exitButton.onClick.AddListener(ExitHandler);
    }

    private void RestartHandler()
    {
      Hide();
      _restartHandler.Restart();
    }

    private void ExitHandler()
    {
      _exitHandler.Exit();
    }

    public void Show()
    {
      gameObject.SetActive(true);
    }

    private void Hide()
    {
      gameObject.SetActive(false);
    }

    public void SetDependencies(RestartHandler restartHandler, ExitHandler exitHandler)
    {
      _restartHandler = restartHandler;
      _exitHandler = exitHandler;
    }
  }
}