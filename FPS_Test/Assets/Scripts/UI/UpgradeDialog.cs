using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeDialog : BaseDialog
{
    [SerializeField]
    private Ease mEaseType;

    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private GameObject mUpgradeItemPrefab;

    [SerializeField]
    private RectTransform mLayoutRtf;

    public override void Init()
    {
        base.Init();
        Alpha = 1;
    }

    public override void Refresh()
    {
        //
    }

    public override void Open(float sp = 1)
    {
        BlocksRaycasts = true;
        Interactable = true;
        rectTransform
            .DOScale(1.0f, sp)
            .SetEase(mEaseType)
            .Play();
        Refresh();
    }

    public override void Close(float sp = 1, Action callback = null)
    {
        BlocksRaycasts = false;
        Interactable = false;
        rectTransform
            .DOScale(0.0f, sp)
            .SetEase(mEaseType)
            .OnComplete(()=> ClearUpgrades())
            .Play();
        Refresh();
    }

    public void SetUpgrade(GameController.UPGRADE[] upgrades)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject item = Instantiate(mUpgradeItemPrefab);
            item.transform.SetParent(mLayoutRtf);
            item.transform.localScale = Vector3.one;
            item.GetComponent<UpgradeItem>().Init(upgrades[i]);
        }
        Open(1.0f);
    }

    public void ClearUpgrades()
    {
        for (int i = mLayoutRtf.childCount - 1; i >= 0; i--)
        {
            Destroy(mLayoutRtf.GetChild(i).gameObject);
        }
    }
}
