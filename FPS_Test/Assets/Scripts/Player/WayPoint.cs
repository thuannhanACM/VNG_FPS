using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public int ID;

    public int[] mEnemies;
    public float[] mSpawnRate;
    public Transform[] mSpawnPoints;
    public float mHealthModifier = 1.0f;
    public float mSpeedModifier = 1.0f;
    public GameObject[] OnClearObjects;
    public bool mHasUpgrade = false;

    private List<Enemy> mActiveEnemies = new List<Enemy>();
    private float mSpawnTimer = 0.0f;
    private int mWaveCount = 0;
    private int mEnemyCount = 0;
    private bool mIsSpawnerActive = false;
    private int mNumEnemiesKilled = 0;
    private int mTotalEnemies = 0;

    public Action OnFinishedGroup = null;

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;

        mIsSpawnerActive = true;
        mWaveCount = 0;
        mEnemyCount = 0;
        mSpawnTimer = mSpawnRate[mWaveCount];

        GameController.Instance.OnWayPointActive();

        mTotalEnemies = 0;
        for (int i = 0; i < mEnemies.Length; i++)
            mTotalEnemies += mEnemies[i];
    }

    private void Update()
    {
        if (!mIsSpawnerActive)
            return;
        if (mSpawnTimer <= 0.0f && mEnemyCount < mEnemies[mWaveCount])
        {
            Vector3 pos = mSpawnPoints[UnityEngine.Random.Range(0, mSpawnPoints.Length)].position;
            SpawnEnemies(pos);
            mEnemyCount++;
            mSpawnTimer = mSpawnRate[mWaveCount];
        }
        else if (mSpawnTimer > 0.0f)
        {
            mSpawnTimer -= Time.deltaTime;
        }
    }

    private void OnEnemiesDeath(Enemy enemy)
    {
        mActiveEnemies.Remove(enemy);
        Destroy(enemy.gameObject);
        mNumEnemiesKilled++;

        if (mActiveEnemies.Count == 0 && mEnemyCount >= mEnemies[mWaveCount])
        {
            //all enemies in are eliminated => start new wave
            mWaveCount++;

            if (mWaveCount >= mEnemies.Length)
            {
                mIsSpawnerActive = false;
                if (mHasUpgrade)
                {
                    Debug.Log("ShowUpgrade");
                    GameController.Instance.PrepareUpgradeDialog(ActiveNextRound);
                }
                else
                    ActiveNextRound();
            }
        }
    }

    private void SpawnEnemies(Vector3 pos)
    {
        Vector2 RandomPos = UnityEngine.Random.insideUnitCircle * 3.0f;
        GameObject enemyObj = Instantiate(GameController.Instance.GetRandomEnemy(), pos + new Vector3(RandomPos.x, 0.0f, RandomPos.y), Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.Init(OnEnemiesDeath, mHealthModifier, mSpeedModifier);
        mActiveEnemies.Add(enemy);
    }

    private void ActiveNextRound()
    {
        //all of waves are eliminated => move to next way point
        if (OnClearObjects != null)
        {
            foreach (GameObject o in OnClearObjects)
                o.SetActive(true);
        }

        if (OnFinishedGroup != null)
            OnFinishedGroup.Invoke();
        gameObject.SetActive(false);
    }
}
