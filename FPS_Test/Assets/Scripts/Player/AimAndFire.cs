using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAndFire : MonoBehaviour
{
    [SerializeField]
    private Player mPlayer;
    private Camera mCamera;
    private bool mIsAiming = false;
    Vector3 mousePos;

    
    void Awake()
    {
        mCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !mIsAiming)
        {
            mIsAiming = true;
            mousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mIsAiming = false;
        }

        if (mIsAiming)
        {
            Vector3 delta = Input.mousePosition - mousePos;
            float aimSpeed = mPlayer.GetAimingSpeed();
            transform.localEulerAngles += new Vector3(-delta.y * Time.deltaTime * aimSpeed, delta.x * Time.deltaTime * aimSpeed, 0.0f);
            mousePos = Input.mousePosition;
        }
    }

    private void FixedUpdate()
    {
        if (mPlayer.IsMoving())
            return;

        if (mIsAiming)
        {
            RaycastHit hit;
            if (Physics.Raycast(mCamera.transform.position, mCamera.transform.forward, out hit, 999))
            {
                //if (hit.transform.tag == "Target")
                mPlayer.Fire(hit.point);
            }
        }
    }
}
