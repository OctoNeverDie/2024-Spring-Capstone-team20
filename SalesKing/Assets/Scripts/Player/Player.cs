using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isPlayerLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        isPlayerLocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishConvo()
    {
        isPlayerLocked = false;
    }
}
