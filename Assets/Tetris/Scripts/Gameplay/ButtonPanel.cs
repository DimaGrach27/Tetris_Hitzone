using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Gameplay
{
  public class ButtonPanel : MonoBehaviour
  {
    public event Action OnClickPause;
    public event Action OnClickExit;

    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TextMeshProUGUI _pauseText;
    [SerializeField] private CanvasGroup _pauseScreen;

    private Coroutine _fadePauseScreen;

    private void Awake()
    {
      _pauseButton.onClick.AddListener(OnClickPauseButton);
      _exitButton.onClick.AddListener(OnClickExitButton);
    }

    public bool IsPause
    {
      set
      {
        string pauseStatus = value ? "Resume" : "Pause";
        _pauseText.text = pauseStatus;

        if (_fadePauseScreen != null)
        {
          StopCoroutine(_fadePauseScreen);
        }

        _fadePauseScreen = StartCoroutine(FadeScreenRoutine(value));
      }
    }

    private IEnumerator FadeScreenRoutine(bool isPause)
    {
      float startFade = _pauseScreen.alpha;
      float endFade = isPause ? 1.0f : 0.0f;
      
      for (float i = 0; i < 1.0f; i += Time.deltaTime * 4)
      {
        yield return null;
        _pauseScreen.alpha = Mathf.Lerp(startFade, endFade, i);
      }
      
      _pauseScreen.alpha = endFade;
    }

    private void OnClickPauseButton() => OnClickPause?.Invoke();
    private void OnClickExitButton() => OnClickExit?.Invoke();
  }
}