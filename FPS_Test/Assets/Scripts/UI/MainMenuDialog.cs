using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuDialog : BaseDialog
{
    [SerializeField]
    private Ease mEaseType;

    [SerializeField]
    private RectTransform rectTransform;

    public override void Init()
    {
        base.Init();
        Alpha = 1;
    }

    public override void Refresh()
    {
        //
    }

    public override void Open(float sp = 1)
    {
        UIManager.Instance.ShowCrossHair(false);
        BlocksRaycasts = true;
        Interactable = true;
        rectTransform
            .DOScale(1.0f, sp)
            .SetEase(mEaseType)
            .Play();
        Refresh();
    }

    public override void Close(float sp = 1, Action callback = null)
    {
        BlocksRaycasts = true;
        Interactable = true;
        rectTransform
            .DOScale(0.0f, sp)
            .SetEase(mEaseType)
            .Play();
        Refresh();
    }

    public void PlayGame()
    {
        Close();
        StartCoroutine(GameController.Instance.DelayStartGame());
    }
}
