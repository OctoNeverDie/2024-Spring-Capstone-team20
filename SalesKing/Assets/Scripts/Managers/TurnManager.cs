using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public int curTurn;


    void Start()
    {
        
    }

    public void StopAndRestartTime(bool isStop)
    {
        if(isStop) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

}
