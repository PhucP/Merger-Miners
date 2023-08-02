using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Merge MIner/LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    [Header("Size")]
    public int Width;
    public int GridHeight, InventoryHeight;
    public float Space;

    [Header("Gird")]
    public List<GridData> ListGrid;

    [Header("Camera Tranform")]
    public float AnchorsPosY, FieldOfView;
}

[System.Serializable]
public class GridData
{
    public BlockType Type;
    public Vector2Int Position;
}
