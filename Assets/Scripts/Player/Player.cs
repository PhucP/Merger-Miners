using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    private Game game => Game.Instance;
    public void BuyShovel()
    {
        Inventory emptyInv = game.ListInventory.FirstOrDefault(inv => inv.CurrentShovel == null);
        if(emptyInv != null)
        {
            emptyInv.CreateNewShovel(ShovelType.LV1);
        }
    }
}
