using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Map : MonoBehaviour
{
    [FormerlySerializedAs("_blockParent")] [SerializeField] private Transform blockParent;
    [FormerlySerializedAs("_inventoryParent")] [SerializeField] private Transform inventoryParent;
    [FormerlySerializedAs("_giftParent")] [SerializeField] private Transform giftParent;


    private Game Game => Game.Instance;
    private void Awake()
    {
        Game.OnInit += OnInitEvent;
        Game.OnQuit += OnQuitEvent;
    }
    public void OnInitEvent()
    {
        SpawnMap();
    }

    public void SpawnMap()
    {
        var saveData = Game.data.saveData;
        var levelData = Game.GetLevelDataByLevel(saveData.level);
        var startPos = CalucalteStartPos(levelData.width, levelData.space);

        //set pos for inv and grid parent
        inventoryParent.position = new Vector3(inventoryParent.position.x, levelData.invPosY, inventoryParent.position.z);
        blockParent.position = new Vector3(blockParent.position.x, inventoryParent.position.y - (levelData.inventoryHeight + 1) * levelData.space, blockParent.position.z);
        giftParent.position = new Vector3(giftParent.position.x, blockParent.position.y - (levelData.gridHeight + 1) * levelData.space, giftParent.position.z);

        SpawnInventory(levelData, startPos, saveData);
        SpawnGrid(startPos, levelData);
        SpawnGift(startPos, levelData);

        Camera.main.fieldOfView = levelData.fieldOfView;
    }

    public void SpawnInventory(LevelData levelData, float startPos, SaveData saveData)
    {
        for (int i = 0; i < levelData.inventoryHeight; i++)
        {
            for (int j = 0; j < levelData.width; j++)
            {
                GameObject newInv = Instantiate(Game.data.listInventoryPrefab[0], Vector3.zero, Quaternion.identity, inventoryParent);
                newInv.transform.localPosition = new Vector3(startPos + j * levelData.space, -i * levelData.space, 0);

                Inventory newInvScript = newInv.GetComponent<Inventory>();
                newInvScript.position = new Vector2Int(i, j);
                Game.listInventory.Add(newInvScript);
            }
        }

        if (saveData.listInvData.Count != 0)
        {
            var listOldInv = saveData.listInvData;
            CreateOldWeapon(listOldInv);
        }
    }

    private void CreateOldWeapon(List<InventoryData> listOldInv)
    {
        foreach (var inventory in listOldInv)
        {
            Inventory currentInv = Game.GetInventoryByPos(inventory.position);
            if (currentInv != null)
            {
                currentInv.CreateNewShovel(inventory.type, true);
            }
            else
            {
                List<Inventory> emptyInv = Game.listInventory.FindAll(inv => inv.CurrentShovel == null);
                if (emptyInv.Count != 0)
                {
                    int ranIndex = (int)Random.Range(0, emptyInv.Count);
                    emptyInv[ranIndex].CreateNewShovel(inventory.type, true);
                }
            }
        }
    }

    public void SpawnGrid(float startPos, LevelData levelData)
    {
        foreach (var block in levelData.listGrid)
        {
            BlockData findBlock = Game.GetBlockData(block.type);
            GameObject newBlock = Instantiate(findBlock.blockPrefab, Vector3.zero, Quaternion.identity, blockParent);

            newBlock.transform.localPosition = new Vector3(startPos + block.position.x * levelData.space, -block.position.y * levelData.space, 0);

            var blockScript = newBlock.GetComponent<Block>();
            blockScript.Pos = block.position;
            Game.listBlock.Add(blockScript);
        }
    }

    public void SpawnGift(float startPos, LevelData levelData)
    {
        for (int i = 0; i < levelData.width; i++)
        {
            GameObject giftPrefab = Game.GetGift();
            GameObject newGift = Instantiate(giftPrefab, Vector3.zero, Quaternion.identity, giftParent);

            newGift.transform.localPosition = new Vector3(startPos + i * levelData.space - 0.175f, newGift.transform.position.y, 0);

            Game.listGift.Add(newGift.GetComponent<Gift>());
        }
    }

    private float CalucalteStartPos(int width, float space)
    {
        float startX = 0;
        startX = (width % 2 != 0) ? -width / 2 * space : -(width / 2 - 0.5f) * space;
        return startX;
    }

    public void OnQuitEvent()
    {

    }
}
