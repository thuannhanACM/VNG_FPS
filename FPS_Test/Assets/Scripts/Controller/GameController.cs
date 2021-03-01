using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    public const string FIRE_RATE_DESC = "Increase Fire Rate";
    public const string DAMAGE_DESC = "Increase Base Damage";
    public const string CRIT_RATE_DESC = "Increase Crit Rate";
    public const string DOUBLE_BULLET_DESC = "Increase Bullets";
    public enum UPGRADE { FIRE_RATE, DAMAGE, CRIT_RATE, DOUBLE_BULLET };

    public Toggle mBGMToggle;
    public Toggle mSFXToggle;
    public AudioSource mSFXAudioSource;
    public AudioSource mBGMAudioSource;
    public AudioClip mSFXConfirmClip;

    private Player mPlayer;

    public GameObject[] mStartObjs;
    public WayPoint[] mEnemiesGroup;
    public GameObject[] mEnemiesPrefab;
    public GameObject mDamageTextPrefab;

    private int numFinshedgroup = 0;
    private bool mSFX = true;
    public bool SFX { get { return mSFX; } }
    private bool mBGM = true;

    private bool mIsGameOver = false;
    public bool IsGameOver { get { return mIsGameOver; } }
    private Action mOnUpgradeSelected = null;

    #region boosting
    private float mFireRate = 0.0f;
    public float BOOST_FIRERATE { get { return mFireRate; } }
    private float mAdditiveDamage = 0.0f;
    public float BOOST_DAMAGE { get { return mAdditiveDamage; } }
    private float mCritRate = 0.0f;
    public float BOOST_CRITRATE { get { return mCritRate; } }
    private bool mHasDoubleBullet = false;
    public bool IS_DOUBLE_BULLET { get { return mHasDoubleBullet; } }
    #endregion

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

    public bool IsCrit()
    {
        if (BOOST_CRITRATE > 0.0f)
            return (UnityEngine.Random.Range(0.0f, 1.0f) > BOOST_CRITRATE);
        return false;
    }

    public float GetPlayerDamage()
    {
        return mPlayer.BASEDAMAGE + BOOST_DAMAGE;
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

    public void PrepareUpgradeDialog(Action onUpgradeSelected)
    {
        mOnUpgradeSelected = onUpgradeSelected;

        List<UPGRADE> upgrades = new List<UPGRADE>(){ UPGRADE.FIRE_RATE, UPGRADE.DAMAGE, UPGRADE.CRIT_RATE };
        if (!mHasDoubleBullet)
            upgrades.Add(UPGRADE.DOUBLE_BULLET);

        if (!mHasRicochet)
            upgrades.Add(UPGRADE.RICOCHET);

        UPGRADE[] upgradesArray = upgrades.ToArray();
        System.Random r = new System.Random();
        upgradesArray = upgradesArray.OrderBy(x => r.Next()).ToArray();

        UIManager.Instance.ShowUpgradeDialog(upgradesArray);
    }

    public void SelectUpgrade(UPGRADE selected)
    {
        switch (selected)
        {
            case UPGRADE.DAMAGE:
                mAdditiveDamage += 5;
                break;

            case UPGRADE.FIRE_RATE:
                mFireRate += 0.1f;
                break;

            case UPGRADE.CRIT_RATE:
                mCritRate += 0.25f;
                break;

            case UPGRADE.DOUBLE_BULLET:
                mHasDoubleBullet = true;
                break;

            default:
                break;
        }

        StartCoroutine(DelayResumeGame());
    }

    public IEnumerator DelayResumeGame()
    {
        yield return new WaitForSeconds(0.5f);

        if (mOnUpgradeSelected != null)
        {
            mOnUpgradeSelected();
            mOnUpgradeSelected = null;
        }
    }

    public void ShowDamageText(Vector3 worldPos, float damage, bool isCrit = false)
    {
        GameObject damageTextObj = Instantiate(mDamageTextPrefab);
        damageTextObj.transform.parent = transform;
        damageTextObj.transform.position = worldPos;
        damageTextObj.transform.localScale = Vector3.one;

        TextMeshPro text = damageTextObj.GetComponent<TextMeshPro>();
        text.text = ((int)damage).ToString();
        text.color = isCrit ? Color.red : Color.white;
    }
}
    
