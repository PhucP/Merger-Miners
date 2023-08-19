using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;
using UnityEditor;

public class Game : MonoBehaviour
{
    public static Game Instance;

    public GameData data;
    public Transform weaponParent;
    public List<Inventory> listInventory;
    public List<Shovel> listShovel;
    public List<Gift> listGift;
    public List<Block> listBlock;
    public List<Block> listHistBlock;

    public event Action OnInit;
    public event Action OnQuit;
    public event Action OnWin;
    public event Action OnLose;

    public bool isWin;
    public bool isPlay;

    private void Awake()
    {
        Singleton();

        listInventory = new List<Inventory>();
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
        listGift.Clear();
        listInventory.Clear();
        listShovel.Clear();
        listBlock.Clear();
        isPlay = false;
        isWin = false;
        listHistBlock.Clear();

        UIManager.Instance.UpdateCoinText(data.saveData.gold);
        Camera.main.transform.position = new Vector3(0, 0, -10);

        OnInit?.Invoke();
    }

    public ShovelData GetShovelData(ShovelType shovelType)
    {
        var shovelData = data.listShoveConfig.FirstOrDefault(shovelData => shovelData.type == shovelType);
        return shovelData;
    }

    public BlockData GetBlockData(BlockType blockType)
    {
        var blockData = data.listBlockConfig.FirstOrDefault(blockData => blockData.type == blockType);
        return blockData;
    }

    public LevelData GetLevelDataByLevel(int lv)
    {
        return data.levelConfig.listLevelData[lv % data.levelConfig.listLevelData.Count()];
    }

    public GameObject GetGift(int index = 0)
    {
        return data.listGiftPrefab[index];
    }

    private void OnApplicationQuit()
    {
        OnQuit?.Invoke();
    }

    public IEnumerator Win()
    {
        yield return new WaitForSeconds(2f);
        OnWin?.Invoke();
        //Reset();
    }

    public void Reset()
    {
        RemoveOldObject(listInventory);
        RemoveOldObject(listShovel);
        RemoveOldObject(listGift);
        RemoveOldObject(listBlock);
        RemoveOldObject(listHistBlock);

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
        UIManager.Instance.Continue();
    }

    public void NextLevel()
    {
        Reset();
        data.saveData.level += 1;
        Init();
    }

    public void Save(bool isResetData = true)
    {
        var saveData = data.saveData;
        saveData.listInvData.Clear();

        if (isResetData) return;

        foreach (Inventory inventory in listInventory)
        {
            if (inventory.CurrentShovel != null)
            {
                InventoryData inventoryData = new InventoryData();
                inventoryData.position = inventory.position;
                inventoryData.type = inventory.CurrentShovel.Type;

                saveData.listInvData.Add(inventoryData);
            }
        }
        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void ResetInven()
    {
        data.saveData.listInvData.Clear();
        RePlay();
    }

    public Inventory GetInventoryByPos(Vector2Int pos)
    {
        return listInventory.FirstOrDefault(inventory => inventory.position == pos);
    }

    public void CheckGameStat()
    {
        foreach (var weapon in listShovel)
        {
            if (listShovel.Count < 1)
            {
                StartCoroutine(Lose());
                return;
            }
            if (weapon != null && weapon.gameObject.activeInHierarchy) return;
        }

        if (isWin) StartCoroutine(Win());
        else StartCoroutine(Lose());
    }

    public IEnumerator Lose()
    {
        yield return new WaitForSeconds(2f);
        OnLose?.Invoke();
    }
}
