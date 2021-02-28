using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private Player mPlayer;

    public List<WayPoint> mWayPoints = new List<WayPoint>();

    
    public void MovePlayerToPosition(Vector3 targetPos)
    {
        mPlayer.MoveToPosition(targetPos);
    }

    public Vector3 GetPlayerPos()
    {
        return mPlayer.transform.position;
    }

    public void RegisterPlayer(Player p)
    {
        mPlayer = p;
    }

    public int GetPlayerLevel()
    {
        return mPlayer.LEVEL;
    }

    public float GetPlayerDamage()
    {
        return mPlayer.BASEDAMAGE;
    }

    public void OnWayPointActive()
    {
        mPlayer.StopMove();
    }

    public void PlayDeadAnimation()
    {
        Debug.LogError("Player Dead");
    }
}
