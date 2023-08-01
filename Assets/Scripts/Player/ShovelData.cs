using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShovelData
{
    public ShovelType ShovelType;
    public GameObject Shovel;
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
