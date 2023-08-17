using Tetris.Interfaces;
using Tetris.Services;
using Tetris.Tetramino;
using Tetris.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tetris.Gameplay
{
  public class GameplaySetup : MonoBehaviour
  {
    [SerializeField] private Tilemap _gameplayTilemap;
    [SerializeField] private Transform _tetraminoContainer;
    [SerializeField] private GameplayUiView _gameplayUiView;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private ArrowPanel _arrowPanel;
    [SerializeField] private ButtonPanel _buttonPanel;

    private TileMapService _tileMapService;
    private ControlComponent _controlComponent;
    private MoveComponent _moveComponent;
    private RotationComponent _rotationComponent;
    private GameplayModel _gameplayModel;
    private CheckTilesLine _checkTilesLine;
    private TetraminoController _tetraminoController;
    private LineController _lineController;
    private GameLoseHandler _gameLoseHandler;
    private BlockManager _blockManager;
    private ScoreManager _scoreManager;
    private GameDifficultyManager _gameDifficultyManager;
    private RestartHandler _restartHandler;
    private ExitHandler _exitHandler;
    private PauseHandler _pauseHandler;
    private GamePlayMenuController _gamePlayMenuController;
    
    private IInit[] _inits;
    private IDestroy[] _destroys;
    private IComponent[] _components;
    private ICondition[] _conditions;
    private ITickable[] _tickables;
    private IRestart[] _restarts;
    
    private void Awake()
    {
      BlockSpawnerService spawnerService = ServiceLocator.Instance.GetService<BlockSpawnerService>();
      SceneService sceneService = ServiceLocator.Instance.GetService<SceneService>();
      BlockPool blockPool = ServiceLocator.Instance.GetService<BlockPool>();

      _pauseHandler = new PauseHandler(_buttonPanel);
      _gameplayModel = new GameplayModel();
      _tileMapService = new TileMapService(_gameplayTilemap, blockPool);
      _moveComponent = new MoveComponent(_tileMapService, _gameplayModel, _pauseHandler);
      _controlComponent = new ControlComponent(_tileMapService, _moveComponent, _arrowPanel, _pauseHandler);
      _rotationComponent = new RotationComponent(_tileMapService, _arrowPanel, _pauseHandler);
      _checkTilesLine = new CheckTilesLine(_tileMapService);
      _blockManager = new BlockManager(_tileMapService);
      _gameDifficultyManager = new GameDifficultyManager(_gameplayUiView, _gameplayModel);
      _gameLoseHandler = new GameLoseHandler(_tileMapService, _loseScreen);
      _gamePlayMenuController = new GamePlayMenuController(_buttonPanel, _exitHandler, _pauseHandler);
      
      _components = new IComponent[]
      {
        _controlComponent,
        _moveComponent,
        _rotationComponent
      };

      _conditions = new ICondition[]
      {
        _controlComponent,
        _moveComponent
      };
      
      _scoreManager = new ScoreManager(_gameplayUiView, _conditions, _gameplayModel);
      
      _lineController = new LineController
      (
        _checkTilesLine, 
        _gameplayUiView, 
        _scoreManager,
        _gameDifficultyManager
      );
      
      _tetraminoController = new TetraminoController
      (
        spawnerService,
        _components, 
        _conditions,
        _tetraminoContainer,
        _gameLoseHandler,
        _blockManager,
        _lineController,
        blockPool
        );
      
      _restarts = new IRestart[]
      {
        _tileMapService,
        _gameDifficultyManager,
        _lineController,
        _scoreManager,
        _tetraminoController,
      };

      _restartHandler = new RestartHandler(_restarts);
      _exitHandler = new ExitHandler(sceneService);
      
      _loseScreen.SetDependencies(_restartHandler, _exitHandler);
    }

    private void Start()
    {
      _inits = new IInit[]
      {
        _controlComponent,
        _rotationComponent,
        _tileMapService,
        _tetraminoController,
        _scoreManager,
        _gamePlayMenuController,
      };
      
      _tickables = new ITickable[]
      {
        _controlComponent,
        _moveComponent,
      };
      
      _destroys = new IDestroy[]
      {
        _controlComponent,
        _rotationComponent,
        _tetraminoController,
        _scoreManager,
        _gamePlayMenuController,
      };

      foreach (var init in _inits)
      {
        init.Init();
      }
    }
    
    private void Update()
    {
      foreach (var tickable in _tickables)
      {
        tickable.Tick(Time.deltaTime);
      }
    }
    
    private void OnDestroy()
    {
      foreach (var destroy in _destroys)
      {
        destroy.Destroy();
      }
    }
  }
}