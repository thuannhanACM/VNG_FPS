using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    private float mLifeTime = 1.0f;

    private float mTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        mTimer = mLifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        mTimer -= Time.deltaTime;
        if (mTimer <= 0.0f)
            Destroy(gameObject);
    }
}
