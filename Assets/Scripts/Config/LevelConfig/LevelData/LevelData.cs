using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Merge MIner/LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    [Header("Inventory")]
    public int WidthInv;
    public int HeightInv;

    [Header("Grid")]
    public int WidthGrid;
    public int HeightGrid;
}
