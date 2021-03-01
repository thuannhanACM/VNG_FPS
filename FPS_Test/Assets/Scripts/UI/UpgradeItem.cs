using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    [SerializeField]
    private Text mItemDescription;

    private Button mButton;

    private GameController.UPGRADE mUpgrade;
    public void Init(GameController.UPGRADE upgrade)
    {
        mUpgrade = upgrade;

        switch (upgrade)
        {
            case GameController.UPGRADE.DAMAGE:
                mItemDescription.text = GameController.DAMAGE_DESC;
                break;

            case GameController.UPGRADE.FIRE_RATE:
                mItemDescription.text = GameController.FIRE_RATE_DESC;
                break;

            case GameController.UPGRADE.CRIT_RATE:
                mItemDescription.text = GameController.CRIT_RATE_DESC;
                break;

            case GameController.UPGRADE.DOUBLE_BULLET:
                mItemDescription.text = GameController.DOUBLE_BULLET_DESC;
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(Onclick);
    }

    private void Onclick()
    {
        GameController.Instance.SelectUpgrade(mUpgrade);
        UIManager.Instance.CloseUpgradeDialog();
    }
}
