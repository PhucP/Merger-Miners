using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Serialization;

public class GridBase : MonoBehaviour
{
    [FormerlySerializedAs("Type")] public BlockType type;
    [FormerlySerializedAs("Position")] public Vector2Int position;

    private CreateMap Create => CreateMap.Instance;

    private void OnMouseDown()
    {
        ChangeTypeOfGrid();
    }

    public void ChangeTypeOfGrid()
    {
        type = GetNextBlockType();
        GameObject blockPrefab = GetBlockPrefabByType();

        GameObject oldGo = this.gameObject;
        oldGo.SetActive(false);
        Create.listGrid.Remove(this);

        GameObject newGo = Instantiate(blockPrefab, this.transform.position, Quaternion.identity, Create.parentGrid);
        newGo.GetComponent<GridBase>().position = position;
        Create.listGrid.Add(newGo.GetComponent<GridBase>());

        Destroy(oldGo);
    }

    private BlockType GetNextBlockType()
    {
        BlockType maxType = System.Enum.GetValues(typeof(BlockType)).Cast<BlockType>().Max();
        int numOfType = (int)maxType + 1;
        return (BlockType)(((int)type + 1) % numOfType);
    }

    public GameObject GetBlockPrefabByType()
    {
        var grid = Create.listGridPrefab.FirstOrDefault(grid => grid.GetComponent<GridBase>().type == type);
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
    [FormerlySerializedAs("Type")] public BlockType type;
    [FormerlySerializedAs("Position")] public Vector2Int position;
}
