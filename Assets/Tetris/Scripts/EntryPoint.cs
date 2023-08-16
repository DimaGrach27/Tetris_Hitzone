using System.Collections.Generic;
using Tetris.Gameplay;
using Tetris.Services;
using Tetris.Tetramino;
using Tetris.Tools;
using UnityEngine;

namespace Tetris
{
  public class EntryPoint : MonoBehaviour
  {
    [SerializeField] private TetraminosHolderConfig _tetraminosHolderConfig;
    [SerializeField] private CoroutineHelper _coroutineHelper;
    
    private SceneService _sceneService;

    private void Awake()
    {
      CreateServices();
      _sceneService.LoadScene(SceneName.MainMenu);
    }

    private void CreateServices()
    {
      _sceneService = new SceneService();
      
      List<IService> services = new List<IService>
      {
        new BlockSpawnerService(_tetraminosHolderConfig),
        _sceneService,
        _coroutineHelper,
      };

      foreach (var service in services)
      {
        ServiceLocator.Instance.AddService(service);
      }
    }
  }
}