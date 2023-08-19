using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class GameData
{
    [FormerlySerializedAs("_saveData")] [Header("Level Config")]
    public SaveData saveData;
    [FormerlySerializedAs("LevelConfig")] public LevelConfig levelConfig;

    [FormerlySerializedAs("ListShoveConfig")] [Header("Shovel Config")]
    public List<ShovelData> listShoveConfig;

    [FormerlySerializedAs("ListBlockConfig")] [Header("Block Config")]
    public List<BlockData> listBlockConfig;
    [FormerlySerializedAs("ListInventoryPrefab")] public List<GameObject> listInventoryPrefab; 
    [FormerlySerializedAs("ListGiftPrefab")] public List<GameObject> listGiftPrefab;

    [Header("VFX")]
    public List<GameObject> listVFX; 
}
