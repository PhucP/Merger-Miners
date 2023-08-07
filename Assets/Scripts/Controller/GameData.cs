using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [Header("Level Config")]
    public SaveData _saveData;
    public LevelConfig LevelConfig;

    [Header("Shovel Config")]
    public List<ShovelData> ListShoveConfig;

    [Header("Block Config")]
    public List<BlockData> ListBlockConfig;
    public List<GameObject> ListInventoryPrefab; 
    public List<GameObject> ListGiftPrefab;

    [Header("VFX")]
    public List<GameObject> listVFX; 
}
