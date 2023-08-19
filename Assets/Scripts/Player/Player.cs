using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    private Game Game => Game.Instance;
    public void BuyShovel()
    {
        List<Inventory> emptyInv = Game.listInventory.FindAll(inv => inv.CurrentShovel == null);
        if (emptyInv.Count != 0)
        {
            int ranIndex = (int)Random.Range(0, emptyInv.Count);
            emptyInv[ranIndex].CreateNewShovel(ShovelType.Lv1);
        }
    }
}
