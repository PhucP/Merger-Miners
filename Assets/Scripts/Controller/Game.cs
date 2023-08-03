using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class Game : MonoBehaviour
{
    public static Game Instance;

    public GameData Data;
    public List<Inventory> ListInventory;
    public List<Shovel> ListShovel;
    public Transform weaponParent;

    public event Action OnInit;

    private void Awake()
    {
        Singleton();

        ListInventory = new List<Inventory>();
    }

    private void Singleton()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        OnInit?.Invoke();
    }

    public ShovelData GetShovelData(ShovelType shovelType)
    {
        var shovelData = Data.ListShoveConfig.FirstOrDefault(shovelData => shovelData.Type == shovelType);
        return shovelData;
    }

    public BlockData GetBlockData(BlockType blockType)
    {
        var blockData = Data.ListBlockConfig.FirstOrDefault(blockData => blockData.Type == blockType);
        return blockData;
    }

    public LevelData GetLevelDataByLevel(int lv)
    {
        return Data.LevelConfig.ListLevelData[lv - 1];
    }
}
