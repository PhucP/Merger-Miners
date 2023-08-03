using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class GridBase : MonoBehaviour
{
    public BlockType Type;
    public Vector2Int Position;

    private CreateMap create => CreateMap.Instance;

    private void OnMouseDown()
    {
        ChangeTypeOfGrid();
    }

    public void ChangeTypeOfGrid()
    {
        Type = GetNextBlockType();
        GameObject blockPrefab = GetBlockPrefabByType();

        GameObject oldGO = this.gameObject;
        oldGO.SetActive(false);
        create._listGrid.Remove(this);

        GameObject newGO = Instantiate(blockPrefab, this.transform.position, Quaternion.identity, create._parentGrid);
        newGO.GetComponent<GridBase>().Position = Position;
        create._listGrid.Add(newGO.GetComponent<GridBase>());

        Destroy(oldGO);
    }

    private BlockType GetNextBlockType()
    {
        BlockType maxType = System.Enum.GetValues(typeof(BlockType)).Cast<BlockType>().Max();
        int numOfType = (int)maxType + 1;
        return (BlockType)(((int)Type + 1) % numOfType);
    }

    public GameObject GetBlockPrefabByType()
    {
        var grid = create.ListGridPrefab.FirstOrDefault(grid => grid.GetComponent<GridBase>().Type == Type);
        return grid;
    }

    public GridBase Clone()
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, this);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return (GridBase)formatter.Deserialize(memoryStream);
        }
    }
}

[System.Serializable]
public class GridData
{
    public BlockType Type;
    public Vector2Int Position;
}
