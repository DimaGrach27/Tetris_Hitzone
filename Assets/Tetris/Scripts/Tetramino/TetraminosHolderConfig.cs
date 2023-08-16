using UnityEngine;

namespace Tetris.Tetramino
{
  [CreateAssetMenu(fileName = "newTetraminsConfig", menuName = "Tetris/Create 'new Tetramino Config'", order = 0)]
  public class TetraminosHolderConfig : ScriptableObject
  {
    [SerializeField] private TetraminoView[] _tetraminoPrefabs;

    public TetraminoView[] TetraminoPrefabs => _tetraminoPrefabs;
  }
}