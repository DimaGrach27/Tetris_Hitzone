using System;
using System.Collections.Generic;
using Tetris.Tetramino;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tetris.Gameplay
{
  public class BlockSpawner : MonoBehaviour
  {
    [SerializeField] private TetraminoView[] _tetraminoPrefabs;
    [SerializeField] private int _spawnHeight = 4;

    private readonly Dictionary<TetraminoType, TetraminoView> _tetraminoMap = new();

    private void Awake()
    {
      foreach (var tetramino in _tetraminoPrefabs)
      {
        _tetraminoMap.Add(tetramino.TetraminoType, tetramino);
      }
    }

    public TetraminoView Spawn()
    {
      int lenght = Enum.GetValues(typeof(TetraminoType)).Length;
      TetraminoType type = (TetraminoType)Random.Range(1, lenght);
      TetraminoView prefab = _tetraminoMap[type];
      Vector2 spawnPoint = Vector2.up * _spawnHeight + prefab.SpawnOffset;
      TetraminoView tetraminoView = Instantiate(prefab, spawnPoint, Quaternion.identity);

      return tetraminoView;
    }
  }
}