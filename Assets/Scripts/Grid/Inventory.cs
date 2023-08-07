using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Vector2Int Position;
    [SerializeField] private Shovel _currentShovel;

    private Game game => Game.Instance;

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
        CreateNewShovel((ShovelType)_currentShovel.Type + 1);
    }

    public void CreateNewShovel(ShovelType shovelType, bool isOldInv = false)
    {
        var newShovelData = game.GetShovelData(shovelType);
        GameObject newShovel = Instantiate(newShovelData.Prefab, transform.position, Quaternion.identity, game.weaponParent);

        Vector3 pos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (!isOldInv)
        {
            var upgradeVFX = Instantiate(game.Data.listVFX[1], pos, Quaternion.Euler(-90, 0, 0));
            Destroy(upgradeVFX, 2f);
        }

        Shovel oldShovel = null;

        if (_currentShovel != null)
        {
            oldShovel = _currentShovel;
            oldShovel.gameObject.SetActive(false);
        }

        Shovel newShovelScript = newShovel.GetComponent<Shovel>();
        game.ListShovel.Add(newShovelScript);
        _currentShovel = newShovelScript;
        newShovelScript.CurrentInventory = this;

        if (oldShovel != null)
        {
            game.ListShovel.Remove(oldShovel);
            oldShovel.gameObject.SetActive(false);
            Destroy(oldShovel.gameObject);
        }
    }
}
