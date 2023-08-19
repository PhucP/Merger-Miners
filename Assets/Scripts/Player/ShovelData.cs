using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class ShovelData
{
    [FormerlySerializedAs("Type")] public ShovelType type;
    [FormerlySerializedAs("Prefab")] public GameObject prefab;
    [FormerlySerializedAs("Damage")] public int damage;
    [FormerlySerializedAs("Heal")] public int heal;
    [FormerlySerializedAs("Cost")] public int cost;
}

[System.Serializable]
public enum ShovelType
{
    Lv1,
    Lv2,
    Lv3,
    Lv4,
    Lv5,
    Lv6,
    Lv7,
    Lv8,
    Lv9,
    Lv10
}
