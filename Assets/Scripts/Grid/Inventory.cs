using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
    [FormerlySerializedAs("Position")] public Vector2Int position;
    [FormerlySerializedAs("_currentShovel")] [SerializeField] private Shovel currentShovel;

    private Game Game => Game.Instance;

    public Shovel CurrentShovel
    {
        get => currentShovel;
        set => currentShovel = value;
    }

    public void SetShovel(Shovel shovel)
    {
        currentShovel = shovel;
    }

    public void ChangeShovel()
    {
        currentShovel = null;
    }

    public void MergeShovel()
    {
        CreateNewShovel((ShovelType)currentShovel.Type + 1);
    }

    public void CreateNewShovel(ShovelType shovelType, bool isOldInv = false)
    {
        var newShovelData = Game.GetShovelData(shovelType);

        if (Game.data.saveData.gold < newShovelData.cost) return;

        GameObject newShovel = Instantiate(newShovelData.prefab, transform.position, Quaternion.identity, Game.weaponParent);

        Vector3 pos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (!isOldInv)
        {
            var upgradeVFX = Instantiate(Game.data.listVFX[1], pos, Quaternion.Euler(-90, 0, 0));
            Destroy(upgradeVFX, 2f);
            Game.data.saveData.gold -= newShovelData.cost;

            UIManager.Instance.UpdateCoinText(Game.data.saveData.gold);
        }

        Shovel oldShovel = null;

        if (currentShovel != null)
        {
            oldShovel = currentShovel;
            oldShovel.gameObject.SetActive(false);
        }

        Shovel newShovelScript = newShovel.GetComponent<Shovel>();
        Game.listShovel.Add(newShovelScript);
        currentShovel = newShovelScript;
        newShovelScript.CurrentInventory = this;

        if (oldShovel != null)
        {
            Game.listShovel.Remove(oldShovel);
            oldShovel.gameObject.SetActive(false);
            Destroy(oldShovel.gameObject);
        }
    }
}
