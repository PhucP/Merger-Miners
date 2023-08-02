using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public event Action PlayAction;
    public GameObject WinPanel;

    private void Awake()
    {
        Singleton();

        WinPanel.SetActive(false);
    }

    private void Singleton()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    public void OnPlay()
    {
        PlayAction?.Invoke();
    }
}
