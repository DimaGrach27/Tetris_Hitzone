using Tetris.Gameplay;
using Tetris.Interfaces;
using Tetris.Tetramino;
using Tetris.Tetris.Scripts.Tiles;
using UnityEngine;

namespace Tetris.Tetris.Scripts
{
  public class TetraminoController : MonoBehaviour
  {
    [SerializeField] private TileMapController _tileMapController;
    [SerializeField] private BlockSpawner _blockSpawner;

    private ControlComponent _controlComponent;
    private MoveComponent _moveComponent;
    private RotationComponent _rotationComponent;

    private TetraminoView _currentTetramino;

    private IInit[] _inits;
    private IDestroy[] _destroys;
    private IComponent[] _components;

    private void Awake()
    {
      _controlComponent = new ControlComponent(_tileMapController);
      _moveComponent = new MoveComponent(_tileMapController);
      _rotationComponent = new RotationComponent(_tileMapController);

      _inits = new IInit[]
      {
        _controlComponent,
        _rotationComponent
      };

      _destroys = new IDestroy[]
      {
        _controlComponent,
        _rotationComponent
      };

      _components = new IComponent[]
      {
        _controlComponent,
        _moveComponent,
        _rotationComponent
      };
    }

    private void Start()
    {
      foreach (var init in _inits)
      {
        init.Init();
      }

      SetNewTetramino();
      
      _moveComponent.OnPlaced += OnPlacedHandler;
    }

    private void OnPlacedHandler()
    {
      foreach (var component in _components)
      {
        component.Disable();
      }
      
      foreach (var block in _currentTetramino.BLocks)
      {
        _tileMapController.AddBlock(block.Position, block);
      }

      SetNewTetramino();
    }

    private void SetNewTetramino()
    {
      _currentTetramino = _blockSpawner.Spawn();

      foreach (var component in _components)
      {
        component.SetTetramino(_currentTetramino);
      }
    }

    private void Update()
    {
      _moveComponent.Tick();
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