using System;
using UnityEngine;

namespace Tetris.Tetramino
{
  public class TetraminoView : MonoBehaviour
  {
    [SerializeField] private Vector2 _size;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private TetraminoType _type;
    [SerializeField] private bool _isOnlyTwoSideRotation;
    [SerializeField] private Vector2 _spawnOffset;

    private Block[] _blocks;
    public Block[] BLocks => _blocks;

    public TetraminoType TetraminoType => _type;
    public Vector2 Size => _size;
    public Vector3 Offset => _offset;
    public Vector2 SpawnOffset => _spawnOffset;
    public bool IsOnlyTwoSideRotation => _isOnlyTwoSideRotation;

    private void Awake()
    {
      _blocks = GetComponentsInChildren<Block>();
      foreach (var block in _blocks)
      {
        block.Init(this);
      }
    }

    public void Rotation(int angle)
    {
      transform.eulerAngles = Vector3.forward * angle;
    }
    
    public void Move(Vector2 direction)
    {
      Vector3 dirMove = new Vector3(direction.x, direction.y);
      transform.position += dirMove;
    }

    public Bound GetBound()
    {
      Transform trans = GetComponent<Transform>();
      Vector3 pos = trans.position;
      Vector3 offset = trans.right * _offset.x;
      int multiply = Math.Abs(trans.eulerAngles.z - 90) < 0.01f ? -1 : 1;
      offset += trans.up * _offset.y * multiply;
      pos += offset;
      
      Bound bound = new Bound
      {
        xMin = pos.x - _size.x / 2,
        xMax = pos.x + _size.x / 2,
        yMin = pos.y - _size.y / 2,
        yMax = pos.y + _size.y / 2,
      };

      return bound;
    }

    private void OnDrawGizmos()
    {
      Transform trans = GetComponent<Transform>();
      Vector3 pos = trans.position;
      // Vector3 offset = trans.right * _offset.x;
      // int multiply = Math.Abs(trans.eulerAngles.z - 90) < 0.01f ? -1 : 1;
      // offset += trans.up * _offset.y * multiply;
      // pos += offset;
      Gizmos.color = Color.green;
      // Gizmos.DrawLine(pos + _offset, pos + trans.up * _size.y);
      // Gizmos.DrawLine(pos + _offset, pos + trans.right * _size.x);
      
      // Gizmos.DrawWireCube(pos, _size);
      Gizmos.DrawLine(pos, new Vector3(GetBound().xMin, pos.y));
      Gizmos.DrawLine(pos, new Vector3(GetBound().xMax, pos.y));
      Gizmos.DrawLine(pos, new Vector3(pos.x, GetBound().yMin));
      Gizmos.DrawLine(pos, new Vector3(pos.x, GetBound().yMax));
    }
  }

  public struct Bound
  {
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
  }
}