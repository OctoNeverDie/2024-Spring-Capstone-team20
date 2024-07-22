using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isPlayerLocked = false;

    void Start()
    {
        isPlayerLocked = false;
    }

    public void FinishConvo()
    {
        isPlayerLocked = false;
    }
}
