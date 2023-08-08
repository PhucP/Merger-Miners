using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;
public class Game : MonoBehaviour
{
    public static Game Instance;

    public GameData Data;
    public Transform weaponParent;

    [Header("Object")]
    public List<Inventory> ListInventory;
    public List<Shovel> ListShovel;
    public List<Gift> ListGift;
    public List<Block> ListBlock;

    public event Action OnInit;
    public event Action OnQuit;
    public event Action OnWin;
    public event Action OnLose;

    [HideInInspector] public bool IsPlay;

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
        ListGift.Clear();
        ListInventory.Clear();
        ListShovel.Clear();
        ListBlock.Clear();
        IsPlay = false;

        UIManager.Instance.UpdateCoinText(Data._saveData.Gold);
        Camera.main.transform.position = new Vector3(0, 0, -10);

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
        return Data.LevelConfig.ListLevelData[lv % Data.LevelConfig.ListLevelData.Count()];
    }

    public GameObject GetGift(int index = 0)
    {
        return Data.ListGiftPrefab[index];
    }

    private void OnApplicationQuit()
    {
        OnQuit?.Invoke();
    }

    public void Win()
    {
        OnWin?.Invoke();
        Reset();
    }

    public void Reset()
    {
        RemoveOldObject(ListInventory);
        RemoveOldObject(ListShovel);
        RemoveOldObject(ListGift);
        RemoveOldObject(ListBlock);

        DOTween.KillAll();
    }

    public void RemoveOldObject<T>(List<T> list) where T : Component
    {
        foreach (var item in list)
        {
            if (item is Component component && item != null)
            {
                component.gameObject.SetActive(false);
                Destroy(component.gameObject);
            }
        }

        list.Clear();
    }

    public void RePlay()
    {
        Reset();
        Init();
    }

    public void NextLevel()
    {
        Reset();
        Data._saveData.Level += 1;
        Init();
    }

    public void Save(bool isResetData = true)
    {
        var saveData = Data._saveData;
        saveData.listInvData.Clear();

        if (isResetData) return;

        foreach (Inventory inventory in ListInventory)
        {
            if (inventory.CurrentShovel != null)
            {
                InventoryData inventoryData = new InventoryData();
                inventoryData.Position = inventory.Position;
                inventoryData.Type = inventory.CurrentShovel.Type;

                saveData.listInvData.Add(inventoryData);
            }
        }
    }

    public void ResetInven()
    {
        Data._saveData.listInvData.Clear();
        RePlay();
    }

    public Inventory GetInventoryByPos(Vector2Int pos)
    {
        return ListInventory.FirstOrDefault(inventory => inventory.Position == pos);
    }

    public void IsLose()
    {
        foreach (var weapon in ListShovel)
        {
            if(ListShovel.Count <= 1) break;
            if (weapon != null && weapon.gameObject.activeInHierarchy) return;
        }
        OnLose?.Invoke();
    }
}
