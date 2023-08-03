using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShovelData
{
    public ShovelType Type;
    public GameObject Prefab;
    public int Damage;
    public int Heal;
}

[System.Serializable]
public enum ShovelType
{
    LV1,
    LV2,
    LV3,
    LV4,
    LV5,
    LV6,
    LV7,
    LV8,
    LV9,
    LV10
}
