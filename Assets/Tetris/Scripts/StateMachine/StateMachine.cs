using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.StateMachine
{
  public class StateMachine
  {
    private readonly Dictionary<Type, State> _statesMap = new();
    private State _currentState;

    public void AddState(State state)
    {
      Type type = state.GetType();

      if (_statesMap.ContainsKey(type))
      {
        Debug.LogError($"StateMachine already contains state {type}.");
        return;
      }

      _statesMap.Add(type, state);
    }

    public void ChangeState<T>() where T : State
    {
      Type type = typeof(T);

      if (!_statesMap.ContainsKey(type))
      {
        Debug.LogError($"StateMachine doesn't contains state {type}.");
        return;
      }
      
      _currentState?.Exit();

      _currentState = _statesMap[type];
      _currentState.Enter();
    }

    public void Tick(float deltaTime)
    {
      _currentState?.Tick(deltaTime);
    }
  }
}