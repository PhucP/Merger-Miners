using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Game : MonoBehaviour
{
    public static Game Instance;

    public GameData Data;
    public List<Inventory> ListInventory;
    public List<Shovel> ListShovel;


    private void Awake()
    {
        Singleton();

        ListInventory = new List<Inventory>();
    }

    private void Singleton()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    public ShovelData GetShovelData(ShovelType shovelType)
    {
        var shovelData = Data.ListShoveConfig.FirstOrDefault(shovelData => shovelData.ShovelType == shovelType);
        return shovelData;
    }
}
