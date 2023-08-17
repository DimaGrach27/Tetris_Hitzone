using System;
using UnityEngine;

namespace Tetris.Gameplay
{
  public class ArrowPanel : MonoBehaviour
  {
    public event Action<Vector2> OnClickArrow;
    public event Action<Vector2, bool> OnStatePress;
    
    [SerializeField] private ButtonWithEvents _upArrowButton;
    [SerializeField] private ButtonWithEvents _downArrowButton;
    [SerializeField] private ButtonWithEvents _leftArrowButton;
    [SerializeField] private ButtonWithEvents _rightArrowButton;

    private void Awake()
    {
      _upArrowButton.onClick.AddListener(ClickArrowUp);
      _downArrowButton.onClick.AddListener(ClickArrowDown);
      _leftArrowButton.onClick.AddListener(ClickArrowLeft);
      _rightArrowButton.onClick.AddListener(ClickArrowRight);

      _upArrowButton.OnStatePress += OnPressArrowUp;
      _downArrowButton.OnStatePress += OnPressArrowDown;
      _leftArrowButton.OnStatePress += OnPressArrowLeft;
      _rightArrowButton.OnStatePress += OnPressArrowRight;
    }

    private void OnDestroy()
    {
      _upArrowButton.OnStatePress -= OnPressArrowUp;
      _downArrowButton.OnStatePress -= OnPressArrowDown;
      _leftArrowButton.OnStatePress -= OnPressArrowLeft;
      _rightArrowButton.OnStatePress -= OnPressArrowRight;
    }

    private void ClickArrowUp() => OnClickArrow?.Invoke(Vector2.up);
    private void ClickArrowDown() => OnClickArrow?.Invoke(Vector2.down);
    private void ClickArrowLeft() => OnClickArrow?.Invoke(Vector2.left);
    private void ClickArrowRight() => OnClickArrow?.Invoke(Vector2.right);
    private void OnPressArrowUp(bool isPressed) => OnStatePress?.Invoke(Vector2.up, isPressed);
    private void OnPressArrowDown(bool isPressed) => OnStatePress?.Invoke(Vector2.down, isPressed);
    private void OnPressArrowLeft(bool isPressed) => OnStatePress?.Invoke(Vector2.left, isPressed);
    private void OnPressArrowRight(bool isPressed) => OnStatePress?.Invoke(Vector2.right, isPressed);
  }
}