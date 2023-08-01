using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [Header("Level Config")]
    public SaveData _saveData;

    [Header("Shovel Config")]
    public List<ShovelData> ListShoveConfig;
}
