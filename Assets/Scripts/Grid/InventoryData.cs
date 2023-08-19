using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class InventoryData
{
    [FormerlySerializedAs("Position")] public Vector2Int position;
    [FormerlySerializedAs("Type")] public ShovelType type;
}
