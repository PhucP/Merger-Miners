using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public event Action PlayAction;

    [SerializeField] private Button next;
    [SerializeField] private Button replay;
    [SerializeField] private Button replayLose;
    [SerializeField] private Button option;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject playGroup;
    [SerializeField] private GameObject pause;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text level;

    private Game Game => Game.Instance;


    private void Awake()
    {
        Singleton();
    }

    private void Start()
    {
        winPanel.SetActive(false);

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
        Game.CheckGameStat();
        playGroup.SetActive(false);
    }

    public void OnInitEvent()
    {
        playGroup.SetActive(true);

        var image = losePanel.GetComponent<Image>();
        image.DOFade(0, 0f);
        var image2 = winPanel.GetComponent<Image>();
        image2.DOFade(0, 0f);

        replayLose.transform.localScale = Vector3.zero;
        replay.transform.localScale = Vector3.zero;
        next.transform.localScale = Vector3.zero;

        pause.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        SetLevel();
    }

    private void OnWinEvent()
    {
        var image = winPanel.GetComponent<Image>();
        winPanel.SetActive(true);
        image.DOFade(1, 2f).SetEase(Ease.Linear);

        replay.transform.DOScale(1, 2.5f).SetEase(Ease.OutBounce);
        next.transform.DOScale(1, 2.5f).SetEase(Ease.OutBounce);
    }

    private void OnLoseEvent()
    {
        var image = losePanel.GetComponent<Image>();
        losePanel.SetActive(true);
        image.DOFade(1, 2f).SetEase(Ease.Linear);
        replayLose.transform.DOScale(1, 2.5f).SetEase(Ease.OutBounce);
    }

    public void UpdateCoinText(int coin)
    {
        coinText.SetText("{}", coin);
    }

    private void SetLevel()
    {
        level.SetText("LEVEL {}", Game.Instance.data.saveData.level);
    }

    public void Option()
    {
        pause.SetActive(!pause.activeSelf);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        pause.SetActive(false);
        Time.timeScale = 1;
    }
}
