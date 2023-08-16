using Tetris.Services;
using UnityEngine;

namespace Tetris.Tools
{
  public class CoroutineHelper : MonoBehaviour, IService
  {
    private void Awake()
    {
      DontDestroyOnLoad(gameObject);
    }
  }
}