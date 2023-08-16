using System;
using System.Collections.Generic;
using Tetris.Global;
using Tetris.Services;
using Tetris.Tetramino;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Tetris.Gameplay
{
  public class BlockSpawnerService : IService
  {
    private readonly TetraminosHolderConfig _holderConfig;
    private readonly int _spawnHeight;

    private readonly Dictionary<TetraminoType, TetraminoView> _tetraminoMap = new();

    public BlockSpawnerService(TetraminosHolderConfig holderConfig)
    {
      _holderConfig = holderConfig;
      
      foreach (var tetramino in _holderConfig.TetraminoPrefabs)
      {
        _tetraminoMap.Add(tetramino.TetraminoType, tetramino);
      }
      
      _spawnHeight = Constants.HEIGHT_FIELD / 2 - 1;
    }

    public TetraminoView Spawn()
    {
      int lenght = Enum.GetValues(typeof(TetraminoType)).Length;
      TetraminoType type = (TetraminoType)Random.Range(1, lenght);
      TetraminoView prefab = _tetraminoMap[type];
      Vector2 spawnPoint = Vector2.up * _spawnHeight + prefab.SpawnOffset;
      TetraminoView tetraminoView = Object.Instantiate(prefab, spawnPoint, Quaternion.identity);

      return tetraminoView;
    }

    public Vector2 GetSpawnPoint(TetraminoView tetraminoView)
    {
      Vector2 spawnPoint = Vector2.up * _spawnHeight + tetraminoView.SpawnOffset;
      return spawnPoint;
    }
  }
}