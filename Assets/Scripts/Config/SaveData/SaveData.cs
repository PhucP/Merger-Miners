using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Merge MIner/SaveData", order = 0)]
public class SaveData : ScriptableObject
{
    public int Level;
    public LevelConfig LevelConfig;
}