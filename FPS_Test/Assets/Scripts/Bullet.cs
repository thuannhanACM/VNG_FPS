using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float mSpeed = 0.0f;
    private Vector3 mTargetPos;
    private bool mIsInitialize = false;

    public void Init(float speed, Vector3 targetPos)
    {
        mSpeed = speed;
        mTargetPos = targetPos;

        transform.LookAt(targetPos);
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * mSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning(other.transform.name);
        Destroy(this.gameObject);

        if (other.transform.tag == "Target")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.ApplyDamage(GameController.Instance.GetPlayerDamage());
        }
    }
}
