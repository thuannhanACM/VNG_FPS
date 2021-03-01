using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialBlurAnimationControl : MonoBehaviour
{
    public int mRadius = 1;
    public string mRadiusFieldName = "";

    private Material mMaterial;
    // Start is called before the first frame update
    void Start()
    {
        Image img = GetComponent<Image>();

        mMaterial = Instantiate(img.material);
        img.material = mMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        mMaterial.SetInt(mRadiusFieldName, mRadius);
    }
}
