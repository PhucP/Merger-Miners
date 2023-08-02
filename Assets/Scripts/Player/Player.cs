using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    private Game game => Game.Instance;
    public void BuyShovel()
    {
        List<Inventory> emptyInv = game.ListInventory.FindAll(inv => inv.CurrentShovel == null);
        if (emptyInv.Count != 0)
        {
            int ranIndex = (int)Random.Range(0, emptyInv.Count);
            emptyInv[ranIndex].CreateNewShovel(ShovelType.LV1);
        }
    }
}
