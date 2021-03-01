using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    public Toggle mBGMToggle;
    public Toggle mSFXToggle;
    public AudioSource mSFXAudioSource;
    public AudioSource mBGMAudioSource;
    public AudioClip mSFXConfirmClip;

    private Player mPlayer;
    
    public GameObject[] mStartObjs;
    public WayPoint[] mEnemiesGroup;
    public GameObject[] mEnemiesPrefab;

    private int numFinshedgroup = 0;
    private bool mSFX = true;
    public bool SFX { get { return mSFX; } }
    private bool mBGM = true;

    private bool mIsGameOver = false;
    public bool IsGameOver { get { return mIsGameOver; } }

    public void Start()
    {
        mBGMToggle.enabled = mBGM;
        mSFXToggle.enabled = mSFX;
    }

    public void MovePlayerToPosition(Vector3 targetPos)
    {
        mPlayer.MoveToPosition(targetPos);
    }

    public Vector3 GetPlayerPos()
    {
        return mPlayer.transform.position;
    }

    public void RegisterPlayer(Player p)
    {
        mPlayer = p;
    }

    public int GetPlayerLevel()
    {
        return mPlayer.LEVEL;
    }

    public float GetPlayerDamage()
    {
        return mPlayer.BASEDAMAGE;
    }

    public void OnWayPointActive()
    {
        mPlayer.StopMove();
    }

    public void PlayDeadAnimation()
    {
        Debug.LogError("Player Dead");

        UIManager.Instance.OnGameOver();
    }

    public void OnSFX()
    {
        mSFX = !mSFX;
        if (mSFX)
            mSFXAudioSource.PlayOneShot(mSFXConfirmClip);
    }

    public void OnBGM()
    {
        mBGM = !mBGM;
        if (!mBGM)
            mBGMAudioSource.Stop();
        else
            mBGMAudioSource.Play();
    }

    public IEnumerator DelayStartGame()
    {
        yield return new WaitForSeconds(2f);
        StartGame();
    }

    public void StartGame()
    {
        UIManager.Instance.ShowCrossHair(true);

        foreach (var obj in mStartObjs)
            obj.SetActive(true);

        foreach (var waypoint in mEnemiesGroup)
        {
            waypoint.OnFinishedGroup += OnFinishedGroup;
        }

        UIManager.Instance.UpdateGameProgress(0, mEnemiesGroup.Length);
    }

    public void OnFinishedGroup()
    {
        numFinshedgroup++;
        if (numFinshedgroup >= mEnemiesGroup.Length)
            StartCoroutine(DelayShowGameWin(0.5f));
    }

    public GameObject GetRandomEnemy()
    {
        return mEnemiesPrefab[UnityEngine.Random.Range(0, mEnemiesPrefab.Length)];
    }

    private IEnumerator DelayShowGameWin(float duration)
    {
        yield return new WaitForSeconds(duration);
        UIManager.Instance.OnGameWin();
    }
}
