using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float mMoveSpeed = 1.0f;

    [SerializeField]
    private float mBaseHP = 10.0f;

    [SerializeField]
    private NavMeshAgent mNavMeshAgent;

    [SerializeField]
    private Renderer mRenderer;

    private float mHP = 0.0f;
    private float mMaxHP = 0.0f;

    private Action<Enemy> OnDead = null;
    private bool isInit = false;

    private EnemyLocator mLocator;

    public void Init(Action<Enemy> ondead, float healthModifier, float speedModifier)
    {
        mMaxHP = mHP = mBaseHP * healthModifier;
        OnDead = ondead;
        mNavMeshAgent.speed = mMoveSpeed * speedModifier;
        mNavMeshAgent.destination = GameController.Instance.GetPlayerPos();
        isInit = true;

        mLocator = UIManager.Instance.GenerateEnemyLocator().GetComponent<EnemyLocator>();
    }

    private void Update()
    {
        if (!isInit)
            return;

        float dist = mNavMeshAgent.remainingDistance;
        if (dist != Mathf.Infinity && mNavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && mNavMeshAgent.remainingDistance <= 1.5f)
        {
            GameController.Instance.PlayDeadAnimation();
        }
        mLocator.UpdateLocator(transform.position);
    }

    public void ApplyDamage(float damage)
    {
        mHP -= damage;
        mRenderer.material.color = Color.Lerp(Color.white, Color.red, (1 - (mHP / mMaxHP)));
        if (mHP <= 0)
        {
            Destroy(mLocator.gameObject);

            if (OnDead != null)
            {
                OnDead.Invoke(this);
                OnDead = null;
            }
        }
    }
}
