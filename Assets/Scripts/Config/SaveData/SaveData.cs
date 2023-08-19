using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SaveData", menuName = "Merge MIner/SaveData", order = 0)]
public class SaveData : ScriptableObject
{
    [FormerlySerializedAs("Level")] public int level;
    [FormerlySerializedAs("Gold")] public int gold;
    [FormerlySerializedAs("Exp")] public int exp;
    public List<InventoryData> listInvData;
}
