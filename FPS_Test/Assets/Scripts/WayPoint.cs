using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public int ID;

    public int[] mEnemies;
    public float[] mSpawnRate;
    public Transform[] mSpawnPoints;
    public GameObject[] mEnemiesPrefab;
    public float mHealthModifier = 1.0f;
    public float mSpeedModifier = 1.0f;

    public GameObject[] OnClearObjects;

    private List<Enemy> mActiveEnemies = new List<Enemy>();

    private float mSpawnTimer = 0.0f;
    private int mWaveCount = 0;
    private int mEnemyCount = 0;
    private bool mIsSpawnerActive = false;

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;

        mIsSpawnerActive = true;
        mWaveCount = 0;
        mEnemyCount = 0;
        mSpawnTimer = mSpawnRate[mWaveCount];

        GameController.Instance.OnWayPointActive();
    }

    private void Update()
    {
        if (!mIsSpawnerActive)
            return;
        if (mSpawnTimer <= 0.0f && mEnemyCount < mEnemies[mWaveCount])
        {
            Vector3 pos = mSpawnPoints[Random.Range(0, mSpawnPoints.Length)].position;
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

        if (mActiveEnemies.Count == 0 && mEnemyCount >= mEnemies[mWaveCount])
        {
            //all enemies in are eliminated => start new wave
            mWaveCount++;

            if (mWaveCount >= mEnemies.Length)
            {
                mIsSpawnerActive = false;
                //all of waves are eliminated => move to next way point
                if (OnClearObjects != null)
                {
                    foreach(GameObject o in OnClearObjects)
                        o.SetActive(true);
                }
                gameObject.SetActive(false);
            }
        }
    }

    private void SpawnEnemies(Vector3 pos)
    {
        GameObject enemyObj = Instantiate(mEnemiesPrefab[Random.Range(0, mEnemiesPrefab.Length)], pos, Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.Init(OnEnemiesDeath, mHealthModifier, mSpeedModifier);
        mActiveEnemies.Add(enemy);
    }
}
