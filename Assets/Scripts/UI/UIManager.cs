using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public event Action PlayAction;
    public GameObject WinPanel;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _buyButton;
    [SerializeField] private GameObject _winPanel;
 
    private Game Game => Game.Instance;


    private void Awake()
    {
        Singleton();
    }

    private void Start()
    {
        WinPanel.SetActive(false);

        Game.OnInit += OnInitEvent;
        Game.OnWin += OnWinEvent;
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
        Game.Save(false);

        PlayAction?.Invoke();
        _playButton.gameObject.SetActive(false);
        _buyButton.gameObject.SetActive(false);
    }

    public void OnInitEvent()
    {
        _playButton.gameObject.SetActive(true);
        _buyButton.gameObject.SetActive(true);

        _winPanel.SetActive(false);
    }

    public void OnWinEvent()
    {
        _winPanel.SetActive(true);
    }

    
}
