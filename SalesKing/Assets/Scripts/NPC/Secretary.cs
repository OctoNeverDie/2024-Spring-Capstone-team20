using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secretary : MonoBehaviour
{
    public GameObject Neck;


    void Update()
    {
        Neck.transform.LookAt(Managers.Player.MyPlayer.transform.position);
    }
}
