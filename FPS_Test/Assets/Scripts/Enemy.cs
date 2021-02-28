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
    private float mFindingPlayerThreshold = 0.5f;

    [SerializeField]
    private Renderer mRenderer;

    private float mFindingPlayerTimer = 0.0f;
    private float mHP = 0.0f;
    private float mMaxHP = 0.0f;

    private Action<Enemy> OnDead = null;

    public void Init(Action<Enemy> ondead, float healthModifier, float speedModifier)
    {
        mMaxHP = mHP = mBaseHP * healthModifier;
        OnDead = ondead;
        mNavMeshAgent.speed = mMoveSpeed * speedModifier;
    }

    private void FixedUpdate()
    {
        if (mFindingPlayerTimer <= 0.0f)
        {
            mNavMeshAgent.destination = GameController.Instance.GetPlayerPos();
        }
    }

    private void Update()
    {
        float dist = mNavMeshAgent.remainingDistance;
        if (dist != Mathf.Infinity && mNavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && mNavMeshAgent.remainingDistance == 0)
        {
            GameController.Instance.PlayDeadAnimation();
        }
    }

    public void ApplyDamage(float damage)
    {
        mHP -= damage;
        mRenderer.material.color = Color.Lerp(Color.white, Color.red, (1 - (mHP / mMaxHP)));
        if (mHP <= 0)
        {
            if (OnDead != null)
            {
                OnDead.Invoke(this);
                OnDead = null;
            }
        }
    }
}
