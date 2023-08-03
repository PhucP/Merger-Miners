using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Transform _blockParent;
    [SerializeField] private Transform _inventoryParent;

    private Game Game => Game.Instance;
    private void Start()
    {
        Game.OnInit += OnInitEvent;
    }
    public void OnInitEvent()
    {
        SpawnMap();
    }

    public void SpawnMap()
    {
        var saveData = Game.Data._saveData;
        var levelData = Game.GetLevelDataByLevel(saveData.Level);
        var startPos = CalucalteStartPos(levelData.Width, levelData.Space);

        SpawnInventory(levelData, startPos, saveData);
        SpawnGrid(startPos, levelData);
    }

    public void SpawnInventory(LevelData levelData, float startPos, SaveData saveData)
    {
        for (int i = 0; i < levelData.InventoryHeight; i++)
        {
            for (int j = 0; j < levelData.Width; j++)
            {
                GameObject newInv = Instantiate(Game.Data.ListInventoryPrefab[0], Vector3.zero, Quaternion.identity, _inventoryParent);
                newInv.transform.localPosition = new Vector3(startPos + j * levelData.Space, -i * levelData.Space, 0);

                Game.ListInventory.Add(newInv.GetComponent<Inventory>());
            }
        }

        if (saveData.Level != 1)
        {

        }
    }

    public void SpawnGrid(float startPos, LevelData levelData)
    {
        foreach (var block in levelData.ListGrid)
        {
            BlockData findBlock = Game.GetBlockData(block.Type);
            GameObject newBlock = Instantiate(findBlock.BlockPrefab, Vector3.zero, Quaternion.identity, _blockParent);

            newBlock.transform.localPosition = new Vector3(startPos + block.Position.x * levelData.Space, -block.Position.y * levelData.Space, 0);
        }
    }

    private float CalucalteStartPos(int _width, float Space)
    {
        float startX = 0;
        startX = (_width % 2 != 0) ? -_width / 2 * Space : -(_width / 2 - 0.5f) * Space;
        return startX;
    }
}
