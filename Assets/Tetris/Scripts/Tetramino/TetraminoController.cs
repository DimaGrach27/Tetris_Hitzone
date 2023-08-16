using Tetris.Gameplay;
using Tetris.Interfaces;
using UnityEngine;

namespace Tetris.Tetramino
{
  public class TetraminoController : IInit, IDestroy
  {
    private readonly BlockSpawnerService _blockSpawnerService;
    
    private readonly IComponent[] _components;
    private readonly ICondition[] _conditions;
    private readonly Transform _tetraminoContainer;
    private readonly GameLoseHandler _gameLoseHandler;
    private readonly BlockManager _blockManager;
    private readonly LineController _lineController;

    private TetraminoView _nextTetramino;
    
    public TetraminoController(
      BlockSpawnerService blockSpawnerService,
      IComponent[] components,
      ICondition[] conditions,
      Transform tetraminoContainer,
      GameLoseHandler gameLoseHandler,
      BlockManager blockManager,
      LineController lineController
    )
    {
      _blockSpawnerService = blockSpawnerService;
      _components = components;
      _conditions = conditions;
      _tetraminoContainer = tetraminoContainer;
      _gameLoseHandler = gameLoseHandler;
      _blockManager = blockManager;
      _lineController = lineController;
    }
    
    public void Init()
    {
      SetNewTetramino();

      foreach (var condition in _conditions)
      {
        condition.OnBLockPLaced += OnPlacedHandler;
      }
    }
    
    public void Destroy()
    {
      foreach (var condition in _conditions)
      {
        condition.OnBLockPLaced -= OnPlacedHandler;
      }
    }

    private void OnPlacedHandler(TetraminoView tetraminoView)
    {
      foreach (var component in _components)
      {
        component.Disable();
      }

      _blockManager.OnPlacedHandler(tetraminoView);
      _lineController.OnPlacedHandler(tetraminoView);
      
      Object.Destroy(tetraminoView.gameObject);

      SetNewTetramino();
    }

    private void SetNewTetramino()
    {
      TetraminoView tetraminoView = _nextTetramino;
      if (_nextTetramino == null)
      {
        tetraminoView = _blockSpawnerService.Spawn();
      }
      else
      {
        tetraminoView.SetPosition(_blockSpawnerService.GetSpawnPoint(tetraminoView));
      }

      SpawnNextTetramino();

      if (_gameLoseHandler.CheckIsCollision(tetraminoView))
      {
        Object.Destroy(tetraminoView.gameObject);
        Debug.Log("Game lose!");
        return;
      }
      
      foreach (var component in _components)
      {
        component.SetTetramino(tetraminoView);
      }
    }

    private void SpawnNextTetramino()
    {
      _nextTetramino = _blockSpawnerService.Spawn();
      Vector2 pos = _tetraminoContainer.position;
      pos += _nextTetramino.SpawnOffset;
      _nextTetramino.SetPosition(pos);
    }
  }
}