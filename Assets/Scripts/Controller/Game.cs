using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;

    [Header("Shovel Config")]
    [SerializeField] List<ShovelData> ListShoveConfig;
    
    [Header("Level Config")]
    [SerializeField] private SaveData _saveData;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }
}
