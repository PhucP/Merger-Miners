using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Shovel _currentShovel;
    public Shovel CurrentShovel
    {
        get => _currentShovel;
        set => _currentShovel = value;
    }

    public void SetShovel(Shovel shovel)
    {
        _currentShovel = shovel;
    }

    public void ChangeShovel()
    {
        _currentShovel = null;
    }

    public void MergeShovel()
    {
        //instantiate new shovel

        //delete current shovel
        _currentShovel.LevelUpForShavel();
    }
}
