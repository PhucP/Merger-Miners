using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Block : MonoBehaviour, ICollision
{
    private Game Game => Game.Instance;
    private BlockData _blockData;
    private int _heal;
    private int _damage;

    [SerializeField] private BlockType _type;

    public int Damage { get => _damage; }

    public int Heal { get => _heal; }
    public BlockType Type
    {
        get => _type;
        set => _type = value;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _blockData = Game.Data.ListBlockConfig.FirstOrDefault(block => block.Type == _type);
        _heal = _blockData.Heal;
        _damage = _blockData.Damage;
    }

    public void TakeDamage(int damage)
    {
        _heal -= damage;
        DestroyByHeal();
    }

    public void DestroyByHeal()
    {
        if (_heal < 1)
        {
            Destroy(this.gameObject);
        }
    }
}

[System.Serializable]
public enum BlockType
{
    BLOCK1,
    BLOCK2,
    BLOCK3,
    BLOCK4,
    BLOCK5,
    BLOCK6,
    BLOCK7,
    BLOCK8
}
