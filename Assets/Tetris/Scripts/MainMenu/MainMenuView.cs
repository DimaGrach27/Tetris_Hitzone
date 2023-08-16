using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.MainMenu
{
  public class MainMenuView : MonoBehaviour
  {
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;

    public void SubscribePlayButton(Action action)
    {
      _playButton.onClick.AddListener(() => action?.Invoke());
    }
    
    public void SubscribeExitButton(Action action)
    {
      _exitButton.onClick.AddListener(() => action?.Invoke());
    }
  }
}