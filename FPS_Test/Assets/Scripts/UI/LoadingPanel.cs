using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : BaseDialog
{
    [SerializeField]
    private CanvasGroup mCanvasGroup = null;

    [SerializeField]
    private Image mBlurBG = null;

    [SerializeField]
    private Image mProgressBarFG = null;

    public bool IsLoadingCompleted { get; private set; }
    private Action OnLoadingComplete = null;

    public override void Refresh()
    {
        
    }

    public void ShowLoadingPanel(float duration, Action onComplete)
    {
        mProgressBarFG.fillAmount = 0.0f;
        OnLoadingComplete = onComplete;
        mProgressBarFG.DOFillAmount(1.0f, duration)
                    .OnComplete(() =>
                    {
                        Hide(0.5f);
                        IsLoadingCompleted = true;
                    })
                    .Play();
        gameObject.SetActive(true);
        IsLoadingCompleted = false;
        Show();
    }

    public void Show()
    {
        mCanvasGroup.DOFade(1, 1).Play();
        mCanvasGroup.blocksRaycasts = true;
    }

    public void Hide(float duration = 0.25f)
    {
        mBlurBG
            .DOFade(0, duration)
            .Play();

        mCanvasGroup.blocksRaycasts = false;
        mCanvasGroup
            .DOFade(0, duration)
            .OnComplete(() =>
            {
                mProgressBarFG.fillAmount = 0;
                if (OnLoadingComplete != null)
                {
                    OnLoadingComplete.Invoke();
                    OnLoadingComplete = null;
                }
                gameObject.SetActive(false);
            })
            .Play();
    }
}
