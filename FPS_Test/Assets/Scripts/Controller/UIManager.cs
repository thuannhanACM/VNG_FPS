using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField]
    private GameObject mGameOverPanel;

    [SerializeField]
    private GameObject mGameWinPanel;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < mDialogs.Length; i++)
            mDialogs[i].Init();
        mLoadingPanel.ShowLoadingPanel(mFakeLoadingDuration, OnLoadingCompleted);
    }

    private void OnLoadingCompleted()
    {
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        mDialogs[(int)Dialog.MainMenu].Open(1.0f);
    }

    public void UpdateGameProgress(int completeGroup, int totalGroup)
    {
        ShowGameProgressCanvas(true);
        float progress = (float)completeGroup / (float)totalGroup;

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

    public void OnGameOver()
    {
        mGameOverPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    public void OnGameWin()
    {
        mGameWinPanel.SetActive(true);
    }
}
