using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLocator : MonoBehaviour
{
    public static float mMaxLocatorUIPos = 500.0f;
    private RectTransform mRectf;
    private Image mImage;

    void Awake()
    {
        mRectf = GetComponent<RectTransform>();
        mImage = GetComponent<Image>();
    }

    // Update is called once per frame
    public void UpdateLocator(Vector3 position)
    {
        if (mRectf == null)
            return;

        Vector3 fromPlayerToEnemy = (position - GameController.Instance.GetPlayerPos());
        float distance = fromPlayerToEnemy.magnitude;
        if (distance >= 35.0f)
            mRectf.localScale = Vector3.one * 0.5f;
        else
        {
            float alpha = (1.0f - distance / 35.0f);
            mRectf.localScale = Vector3.one * alpha * 2.0f;
        }

        fromPlayerToEnemy.y = 0.0f;
        Vector3 playerLook = GameController.Instance.GetPlayerTransform().forward;
        playerLook.y = 0.0f;
        float dotForward = Vector3.Dot(playerLook.normalized, fromPlayerToEnemy.normalized);

        Vector3 playerRight = GameController.Instance.GetPlayerTransform().right;
        playerRight.y = 0.0f;
        float dotPlayerRight = Vector3.Dot(-fromPlayerToEnemy.normalized, playerRight.normalized);
        float targetPosX = 0;
        if (dotForward >= 0.0f)
        {
            targetPosX = -dotPlayerRight * mMaxLocatorUIPos;
        }
        else
        {
            if(dotPlayerRight > 0.0f)
                targetPosX = -mMaxLocatorUIPos;
            else
                targetPosX = mMaxLocatorUIPos;
        }
        mRectf.localPosition = new Vector2(targetPosX, 0);

    }
}
