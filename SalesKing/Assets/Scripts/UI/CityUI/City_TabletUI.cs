using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DoTween 네임스페이스 추가

public class City_TabletUI : MonoBehaviour
{
    public GameObject Tablet;

    void OnClickShowTablet()
    {
        // Tablet을 -2000,0,0에서 0,0,0으로 이동
        Tablet.transform.localPosition = new Vector3(-2000, 0, 0);
        Tablet.transform.DOLocalMove(Vector3.zero, 1f); // 1초 동안 이동
    }

    void OnClickHideTablet()
    {
        // Tablet을 0,0,0에서 -2000,0,0으로 이동
        Tablet.transform.DOLocalMove(new Vector3(-2000, 0, 0), 1f); // 1초 동안 이동
    }

    void Update()
    {
        if (Input.GetButtonDown("Tab"))
        {
            OnClickShowTablet();
        }
    }
}
