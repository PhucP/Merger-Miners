using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LevelData", menuName = "Merge MIner/LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    [FormerlySerializedAs("Width")] [Header("Size")]
    public int width;
    [FormerlySerializedAs("GridHeight")] public int gridHeight;
    [FormerlySerializedAs("InventoryHeight")] public int inventoryHeight;
    [FormerlySerializedAs("Space")] public float space;

    [FormerlySerializedAs("ListGrid")] [Header("Gird")]
    public List<GridData> listGrid;

    [FormerlySerializedAs("AnchorsPosY")] [Header("Tranform")]
    public float anchorsPosY;

    [FormerlySerializedAs("FieldOfView")] [Header("Tranform")]
    public float fieldOfView;

    [FormerlySerializedAs("InvPosY")] public float invPosY;
    [FormerlySerializedAs("BlockPosY")] public float blockPosY;
}
