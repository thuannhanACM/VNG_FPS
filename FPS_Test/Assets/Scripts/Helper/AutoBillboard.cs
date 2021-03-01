using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBillboard : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.forward = (transform.position - GameController.Instance.GetPlayerPos()).normalized;
    }
}
