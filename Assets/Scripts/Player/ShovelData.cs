using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShovelData
{
    public ShovelType ShovelType;
    public GameObject ShovelPrefab;
    public int Damage;
    public int Heal;
}

[System.Serializable]
public enum ShovelType
{
    LV1,
    LV2,
    LV3
}
