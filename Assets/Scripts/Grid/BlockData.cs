using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class BlockData
{
    [FormerlySerializedAs("BlockPrefab")] public GameObject blockPrefab;
    [FormerlySerializedAs("Heal")] public int heal;
    [FormerlySerializedAs("Type")] public BlockType type;
    [FormerlySerializedAs("Damage")] public int damage;
}
