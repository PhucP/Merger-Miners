using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Merge MIner/LevelConfig", order = 0)]
public class LevelConfig : ScriptableObject 
{
    [FormerlySerializedAs("ListLevelData")] public List<LevelData> listLevelData;    
}
