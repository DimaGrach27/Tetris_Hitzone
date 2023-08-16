using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Services
{
  public class ServiceLocator
  {
    public static ServiceLocator Instance => _serviceLocator ??= new ServiceLocator();
    private static ServiceLocator _serviceLocator;
    
    private readonly Dictionary<Type, IService> _servicesMap = new();

    public void AddService(IService service)
    {
      Type type = service.GetType();

      if (_servicesMap.ContainsKey(type))
      {
        Debug.LogError($"Service map already contains service with {type}!");
        return;
      }
      
      _servicesMap.Add(type, service);
    }

    public T GetService<T>() where T : IService
    {
      Type type = typeof(T);

      if (_servicesMap.ContainsKey(type))
      {
        return (T)_servicesMap[type];
      }
      Debug.LogError($"Service map doesn't contains service with {type}!");

      return default;
    }
  }
}