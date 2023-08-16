using TMPro;
using UnityEngine;

namespace Tetris.Gameplay
{
  public class GameplayUiView : MonoBehaviour
  {
    [SerializeField] private TextMeshPro _levelText;
    [SerializeField] private TextMeshPro _scoreText;
    [SerializeField] private TextMeshPro _linesText;

    public int Level
    {
      set => _levelText.text = $"Level: {value}";
    }
    
    public int Score
    {
      set => _scoreText.text = $"Score: {value}";
    }
    
    public int Lines
    {
      set => _linesText.text = $"Lines: {value}";
    }
  }
}