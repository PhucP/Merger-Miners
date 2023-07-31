using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Merge MIner/LevelConfig", order = 0)]
public class LevelConfig : ScriptableObject 
{
    public List<LevelData> ListLevelData;    
}
