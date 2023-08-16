using Tetris.Global;
using Tetris.Services;
using UnityEngine;
using UnityEngine.Pool;

namespace Tetris.Tetramino
{
  public class BlockPool : IService
  {
    private readonly Block _block;

    public BlockPool(Block block)
    {
      _block = block;
    }
    
    private const bool CollectionChecks = true;
    private const int MaxPoolSize = Constants.WIDTH_FIELD * Constants.HEIGHT_FIELD;

    private IObjectPool<Block> _pool;

    public IObjectPool<Block> Pool
    {
      get
      {
        if (_pool == null)
        {
          _pool = new ObjectPool<Block>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            CollectionChecks,
            4,
            MaxPoolSize);
        }

        return _pool;
      }
    }

    private Block CreatePooledItem()
    {
      Block block = Object.Instantiate(_block);

      return block;
    }
    
    private void OnReturnedToPool(Block block)
    {
      block.gameObject.SetActive(false);
    }
    
    private void OnTakeFromPool(Block block)
    {
      block.gameObject.SetActive(true);
    }

    private void OnDestroyPoolObject(Block block)
    {
      Object.Destroy(block.gameObject);
    }
  }
}