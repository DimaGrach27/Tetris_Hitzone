using Tetris.Global;
using UnityEngine;

namespace Tetris.Gameplay
{
  public class GameDifficultyManager
  {
    private readonly GameplayUiView _gameplayUiView;
    private readonly GameplayModel _gameplayModel;

    private int _linesToLevel = Constants.LINES_TO_LEVEL;

    public GameDifficultyManager(GameplayUiView gameplayUiView, GameplayModel gameplayModel)
    {
      _gameplayUiView = gameplayUiView;
      _gameplayModel = gameplayModel;
    }

    public void SetLines(int lines)
    {
      _linesToLevel -= lines;

      if (_linesToLevel > 0)
      {
        return;
      }

      int linesDelta = Mathf.Abs(_linesToLevel);
      UpdateLevel();
      
      SetLines(linesDelta);
    }

    private void UpdateLevel()
    {
      _linesToLevel = Constants.LINES_TO_LEVEL;
      
      _gameplayModel.Level++;
      _gameplayUiView.Level = _gameplayModel.Level;

      CalculateSpeed();
    }

    private void CalculateSpeed()
    {
      _gameplayModel.MoveTick *= 0.7f;
    }
  }
}