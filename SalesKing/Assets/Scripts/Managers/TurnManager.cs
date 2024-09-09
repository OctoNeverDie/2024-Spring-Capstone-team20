using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public int curTurn;


    void Start()
    {
        
    }


    private void Update()
    {
        // test
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ConvoFinished();
        }
    }

    public void ConvoFinished()
    {
        StopAndRestartTime(false);
        Managers.Player.MyPlayer.GetComponent<Player>().BackToWalking();
    }

    public void StopAndRestartTime(bool isStop)
    {
        if(isStop) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

}
