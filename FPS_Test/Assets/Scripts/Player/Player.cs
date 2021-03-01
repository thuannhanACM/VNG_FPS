using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const string ANIM_FIRE_TRIGGER = "fire";

    [SerializeField]
    private float mMoveSpeed = 10.0f;

    [SerializeField]
    private float mAimingSpeed = 10.0f;

    [SerializeField]
    private float mFireRate = 0.5f;

    [SerializeField]
    private int nNumBullets = 10;

    [SerializeField]
    private float mReloadTime = 0.5f;

    [SerializeField]
    private Transform mFirePos;

    [SerializeField]
    private GameObject mBulletPrefab;

    [SerializeField]
    private float mBulletSpeed;

    [SerializeField]
    private AudioSource mShootingSource;

    [SerializeField]
    private GameObject mGunFlashFX;

    [SerializeField]
    private float mBaseDamage;
    public float BASEDAMAGE { get { return mBaseDamage; } }

    private float mFireTimer = 0.0f;


    private Animator animator;

    private bool mIsMoving = false;

    public Action OnMovingFinished = null;
    private Vector3 mTargetMovePos;

    private int mLevel = 1;
    public int LEVEL { get { return mLevel; } }



    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        GameController.Instance.RegisterPlayer(this);
    }

    public void Fire(Vector3 targetPos)
    {
        if (GameController.Instance.IsGameOver)
            return;

        if (mFireTimer <= 0.0f)
        {
            animator.SetTrigger(ANIM_FIRE_TRIGGER);

            GameObject flash = Instantiate(mGunFlashFX);
            flash.transform.parent = mFirePos;
            flash.transform.localPosition = Vector3.zero;
            flash.transform.localEulerAngles = Vector3.zero;

            if (GameController.Instance.IS_DOUBLE_BULLET)
            {
                for (int i = 0; i < 2; i++)
                {   
                    GameObject bulletObj = Instantiate(mBulletPrefab, mFirePos.position + mFirePos.right * ((i == 1 )? 0.25f: -0.25f), Quaternion.identity);
                    Bullet bullet = bulletObj.GetComponent<Bullet>();
                    if (bullet != null)
                        bullet.Init(mBulletSpeed, targetPos);
                }
            }
            else
            {
                GameObject bulletObj = Instantiate(mBulletPrefab, mFirePos.position, Quaternion.identity);
                Bullet bullet = bulletObj.GetComponent<Bullet>();
                if (bullet != null)
                    bullet.Init(mBulletSpeed, targetPos);
            }

            if(GameController.Instance.SFX)
                mShootingSource.PlayOneShot(mShootingSource.clip);
            mFireTimer = GetFireRate();
        }
    }

    private void Update()
    {
        //update FireRate
        if (mFireTimer > 0.0f)
            mFireTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (mIsMoving)
        {
            Vector3 moveDir = (mTargetMovePos - transform.position);
            if(moveDir.magnitude * Time.fixedDeltaTime <= mMoveSpeed * Time.fixedDeltaTime)
                transform.position += moveDir * Time.fixedDeltaTime;
            else
                transform.position += moveDir.normalized * mMoveSpeed * Time.fixedDeltaTime;
        }
    }

    private float GetFireRate()
    {
        float rate = mFireRate * (1.0f - GameController.Instance.BOOST_FIRERATE);
        Debug.Log("fireRate: " + rate);
        return rate;
    }

    public float GetAimingSpeed()
    {
        return mAimingSpeed;
    }

    public bool IsMoving()
    {
        return mIsMoving;
    }

    public void MoveToPosition(Vector3 target)
    {
        mTargetMovePos = target;
        mTargetMovePos.y = 0.0f;
        mIsMoving = true;
    }

    public void StopMove()
    {
        mIsMoving = false;
    }
}
