using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public enum Dialog {
        MainMenu,
        InGameMessage
    }

    [SerializeField]
    private BaseDialog[] mDialogs = null;

    [SerializeField]
    private LoadingPanel mLoadingPanel = null;

    [SerializeField]
    private float mFakeLoadingDuration = 2.0f;

    [SerializeField]
    private Image mGameProgress = null;

    [SerializeField]
    private CanvasGroup mGameProgressCanvas = null;

    [SerializeField]
    private Image mCrossHairImg;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < mDialogs.Length; i++)
            mDialogs[i].Init();
        mLoadingPanel.ShowLoadingPanel(mFakeLoadingDuration, OnLoadingCompleted);
    }

    private void OnLoadingCompleted()
    {
        mDialogs[(int)Dialog.MainMenu].Open(1.0f);
    }

    public void UpdateGameProgress(int completeGroup, int totalGroup, int currentEnemyKillInGroup, int maxEnemyInGroup)
    {
        ShowGameProgressCanvas(true);
        float progressAfterFinishedGroup = (float)completeGroup / (float)totalGroup;
        float progress = ((float)currentEnemyKillInGroup / (float)maxEnemyInGroup) * progressAfterFinishedGroup;

        mGameProgress.fillAmount = progress;
    }

    public void ShowCrossHair(bool show)
    {
        mCrossHairImg.gameObject.SetActive(show);
    }

    public void ShowGameProgressCanvas(bool show)
    {
        mGameProgressCanvas.alpha = show ? 1.0f : 0.0f;
    }
}
