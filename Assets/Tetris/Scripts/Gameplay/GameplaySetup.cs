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
    
    private IInit[] _inits;
    private IDestroy[] _destroys;
    private IComponent[] _components;
    private ICondition[] _conditions;
    private ITickable[] _tickables;
    
    private void Awake()
    {
      BlockSpawnerService spawnerService = ServiceLocator.Instance.GetService<BlockSpawnerService>();
      
      _gameplayModel = new GameplayModel();
      _tileMapService = new TileMapService(_gameplayTilemap);
      _moveComponent = new MoveComponent(_tileMapService, _gameplayModel);
      _controlComponent = new ControlComponent(_tileMapService, _moveComponent);
      _rotationComponent = new RotationComponent(_tileMapService);
      _checkTilesLine = new CheckTilesLine(_tileMapService);
      _blockManager = new BlockManager(_tileMapService);
      _gameDifficultyManager = new GameDifficultyManager(_gameplayUiView, _gameplayModel);
      _gameLoseHandler = new GameLoseHandler(_tileMapService);
      
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
        _lineController
        );
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