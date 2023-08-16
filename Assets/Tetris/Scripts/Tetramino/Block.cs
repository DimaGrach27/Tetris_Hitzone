using UnityEngine;

namespace Tetris.Tetramino
{
  public class Block : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private TetraminoView _tetraminoView;
    
    public Vector2 Position => transform.position;
    public Vector2 LeftDir => -Vector2.right + Position;
    public Vector2 RightDir => Vector2.right + Position;
    public Vector2 DownDir => -Vector2.up + Position;

    public TetraminoView Owner => _tetraminoView;
    
    public void Init(TetraminoView tetraminoView)
    {
      _tetraminoView = tetraminoView;
      _spriteRenderer.color = tetraminoView.Color;
    }

    public void SetPosition(Vector2 pos)
    {
      transform.position = pos;
    }

    private void OnDrawGizmos()
    {
      Transform trans = GetComponent<Transform>();
      Vector3 pos = trans.position;
      
      Gizmos.color = Color.yellow;
      Gizmos.DrawLine(pos, pos + Vector3.right);
      Gizmos.DrawLine(pos, pos -Vector3.right);
      
      Gizmos.color = Color.blue;
      Gizmos.DrawLine(pos, pos + Vector3.up);
      Gizmos.DrawLine(pos, pos -Vector3.up);
    }
  }
}