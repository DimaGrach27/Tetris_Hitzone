using System.Collections.Generic;
using Tetris.Gameplay;
using Tetris.Services;
using Tetris.Tetramino;
using Tetris.Tools;

namespace Tetris.StateMachine.States
{
  public class BootState : State
  {
    private readonly CoroutineHelper _coroutineHelper;
    private readonly TetraminosHolderConfig _tetraminosHolderConfig;
    
    private SceneService _sceneService;

    public BootState(
      StateMachine stateMachine,
      CoroutineHelper coroutineHelper, 
      TetraminosHolderConfig tetraminosHolderConfig) 
      : base(stateMachine)
    {
      _coroutineHelper = coroutineHelper;
      _tetraminosHolderConfig = tetraminosHolderConfig;
    }

    public override void Enter()
    {
      CreateServices();
      
      _sceneService.LoadScene(SceneName.MainMenu, true);
      StateMachine.ChangeState<MainMenuState>();
    }

    public override void Exit()
    {
      _sceneService.UnloadScene(SceneName.Boot);
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