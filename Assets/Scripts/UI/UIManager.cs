using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public event Action PlayAction;
    public GameObject WinPanel;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _buyButton;
    [SerializeField] private GameObject _winPanel, _losePanel;
    [SerializeField] private TMP_Text _coinText;
 
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
        Game.OnLose += OnLoseEvent;
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
        Game.IsLose();
        _playButton.gameObject.SetActive(false);
        _buyButton.gameObject.SetActive(false);
    }

    public void OnInitEvent()
    {
        _playButton.gameObject.SetActive(true);
        _buyButton.gameObject.SetActive(true);

        _winPanel.SetActive(false);
        _losePanel.SetActive(false);
    }

    public void OnWinEvent()
    {
        _winPanel.SetActive(true);
    }

    public void OnLoseEvent()
    {
        _losePanel.SetActive(true);
    }

    public void UpdateCoinText(int coin)
    {
        _coinText.SetText("COIN: {}", coin);
    }
}
