using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Gift : MonoBehaviour
{
    [FormerlySerializedAs("IsUsed")] public bool isUsed;
    public Transform effect;
    public GameObject particalEffect;

    private void Awake() {
        isUsed = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!isUsed) 
        {
            Game.Instance.data.saveData.gold += (int)Random.Range(0f, 10f); 
            UIManager.Instance.UpdateCoinText(Game.Instance.data.saveData.gold);
            var pe = Instantiate(particalEffect, effect.position, Quaternion.Euler(-90, 0, 0));
            Destroy(pe, 2f);
        }
        isUsed = true;
        other.gameObject.SetActive(false);
        Game.Instance.isWin = true;
        other.GetComponent<Shovel>().DestroyByHeal();
    }
}
