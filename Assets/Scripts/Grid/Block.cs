using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.Serialization;

public class Block : MonoBehaviour, ICollision
{
    private Game Game => Game.Instance;
    private BlockData _blockData;
    private int _heal;
    private int _damage;
    [FormerlySerializedAs("_pos")] [SerializeField]private Vector2Int pos;

    [FormerlySerializedAs("_type")] [SerializeField] private BlockType type;

    public int Damage { get => _damage; }
    public Vector2Int Pos { get => pos; set => pos = value; }

    public int Heal { get => _heal; }
    public BlockType Type
    {
        get => type;
        set => type = value;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _blockData = Game.data.listBlockConfig.FirstOrDefault(block => block.type == type);
        _heal = _blockData.heal;
        _damage = _blockData.damage;
    }

    public void TakeDamage(int damage)
    {
        _heal -= damage;
        DestroyByHeal();
        //MoveCamera();
    }

    public void MoveCamera()
    {
        int maxPosy = Game.GetLevelDataByLevel(Game.data.saveData.level).gridHeight - 3;

        if (pos.y > maxPosy) return;

        if(Camera.main.transform.position.y < transform.position.y) return;

        if (pos.y % 2 == 1 || pos.y == maxPosy)
        {
            Camera.main.transform.DOMoveY(this.transform.position.y, 1.5f).SetEase(Ease.Linear);
        }
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
    Block1,
    Block2,
    Block3,
    Block4,
    Block5,
    Block6,
    Block7,
    Block8
}
