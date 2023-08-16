using Tetris.Interfaces;
using Tetris.Tetramino;

namespace Tetris.Gameplay
{
  public class ScoreManager : IInit, IDestroy, IRestart
  {
    private readonly GameplayUiView _gameplayUiView;
    private readonly ICondition[] _conditions;
    private readonly GameplayModel _gameplayModel;

    public ScoreManager(
      GameplayUiView gameplayUiView, 
      ICondition[] conditions,
      GameplayModel gameplayModel
      )
    {
      _gameplayUiView = gameplayUiView;
      _conditions = conditions;
      _gameplayModel = gameplayModel;
    }

    public void CalculateLinesScore(int lines)
    {
      int score = lines * lines * 100;

      _gameplayModel.Score += score;

      _gameplayUiView.Score = _gameplayModel.Score;
    }

    public void Init()
    {
      foreach (var condition in _conditions)
      {
        condition.OnBLockPLaced += OnPlacedHandler;
      }
    }

    private void OnPlacedHandler(TetraminoView tetraminoView)
    {
      _gameplayModel.Score += tetraminoView.BLocks.Length * 5;
      _gameplayUiView.Score = _gameplayModel.Score;
    }

    public void Destroy()
    {
      foreach (var condition in _conditions)
      {
        condition.OnBLockPLaced -= OnPlacedHandler;
      }
    }

    public void Restart()
    {
      _gameplayModel.Score = 0;
      _gameplayUiView.Score = _gameplayModel.Score;
    }
  }
}