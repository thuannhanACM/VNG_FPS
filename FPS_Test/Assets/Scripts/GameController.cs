using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    public Transform MainMenu;

    public Toggle mBGMToggle;
    public Toggle mSFXToggle;
    public AudioSource mSFXAudioSource;
    public AudioClip mSFXConfirmClip;

    private Player mPlayer;
    
    public GameObject[] mStartObjs;

    public WayPoint[] mEnemiesGroup;

    private int numFinshedgroup = 0;
    private bool mSFX = true;
    public bool SFX { get { return mSFX; } }
    private bool mBGM = true;

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
    }

    public void StartGame()
    {
        foreach (var obj in mStartObjs)
            obj.SetActive(true);

        foreach (var waypoint in mEnemiesGroup)
        {
            waypoint.OnFinishedGroup += OnFinishedGroup;
        }

        UIManager.Instance.UpdateGameProgress(0, mEnemiesGroup.Length, 1, 1);
    }

    public void OnFinishedGroup()
    {
        numFinshedgroup++;
    }

    public void UpdateEnemyKill(int numkill, int total)
    {
        UIManager.Instance.UpdateGameProgress(numFinshedgroup, mEnemiesGroup.Length, numkill, total);
    }
}
