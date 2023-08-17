using System;
using UnityEngine;

namespace Tetris.Tetramino
{
  public class TetraminoView : MonoBehaviour
  {
    [SerializeField] private Vector2 _size;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector2 _spawnOffset;
    
    [SerializeField] private TetraminoType _type;
    
    [SerializeField] private Color _color;
    
    [SerializeField] private bool _isOnlyTwoSideRotation;
    
    [SerializeField] private Transform[] _blocksPoint;

    private Block[] _blocks;
    public Block[] BLocks => _blocks;

    public TetraminoType TetraminoType => _type;
    public Vector2 SpawnOffset => _spawnOffset;
    public Color Color => _color;
    public bool IsOnlyTwoSideRotation => _isOnlyTwoSideRotation;

    public void Rotation(int angle)
    {
      transform.eulerAngles = Vector3.forward * angle;
    }
    
    public void Move(Vector2 direction)
    {
      Vector3 dirMove = new Vector3(direction.x, direction.y);
      transform.position += dirMove;
    }
    
    public void SetPosition(Vector2 pos)
    {
      transform.position = pos;
    }

    public void DeattachBlocks(Transform parent)
    {
      foreach (var block in _blocks)
      {
        block.transform.SetParent(parent);
      }
    }

    public void CreateBlocks(BlockPool blockPool)
    {
      _blocks = new Block[_blocksPoint.Length];
      
      for (int i = 0; i < _blocksPoint.Length; i++)
      {
        Block block = blockPool.Pool.Get();
        block.SetPosition(_blocksPoint[i].position);
        block.transform.SetParent(transform);
        block.Init(this);

        _blocks[i] = block;
      }
    }

    public Bound GetBound()
    {
      Transform trans = transform;
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
      Transform trans = GetComponent<Transform>();
      Vector3 pos = trans.position;

      Gizmos.color = Color.green;

      Gizmos.DrawLine(pos, new Vector3(GetBound().xMin, pos.y));
      Gizmos.DrawLine(pos, new Vector3(GetBound().xMax, pos.y));
      Gizmos.DrawLine(pos, new Vector3(pos.x, GetBound().yMin));
      Gizmos.DrawLine(pos, new Vector3(pos.x, GetBound().yMax));
    }
#endif
  }

  public struct Bound
  {
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
  }
}