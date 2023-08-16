using Tetris.Interfaces;
using Tetris.Tetramino;
using Tetris.Tiles;

namespace Tetris.Gameplay
{
  public class LineController
  {
    private readonly CheckTilesLine _checkTilesLine;
    private readonly GameplayUiView _gameplayUiView;
    private readonly ScoreManager _scoreManager;
    private readonly GameDifficultyManager _gameDifficultyManager;

    private int _lines;

    public LineController(
      CheckTilesLine checkTilesLine, 
      GameplayUiView gameplayUiView,
      ScoreManager scoreManager,
      GameDifficultyManager gameDifficultyManager
      )
    {
      _checkTilesLine = checkTilesLine;
      _gameplayUiView = gameplayUiView;
      _scoreManager = scoreManager;
      _gameDifficultyManager = gameDifficultyManager;
    }

    public void OnPlacedHandler(TetraminoView tetraminoView)
    {
      int lineCount = _checkTilesLine.BLockPLacedHandler(tetraminoView);
      _lines += lineCount;

      _gameplayUiView.Lines = _lines;
      _scoreManager.CalculateLinesScore(lineCount);
      _gameDifficultyManager.SetLines(lineCount);
    }
  }
}