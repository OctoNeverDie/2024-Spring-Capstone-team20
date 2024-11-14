using DG.Tweening;
using UnityEngine;
/// <summary>
/// tab 누르면 나왔다 사라졌다 함
/// </summary>
public class City_TabletAction : MonoBehaviour
{
    public GameObject Tablet;

    public bool isTablet = false;
    void Start()
    {
        isTablet = false;
        InitTablet();
    }
    void Update()
    {
        if (Input.GetButtonDown("Tab"))
        {
            if (isTablet)
            {
                OnClickHideTablet();
                isTablet = false;
            }
            else
            {
                OnClickShowTablet();
                isTablet = true;
            }
            
        }
    }

    public void InitTablet()
    {
        Tablet.transform.localPosition = new Vector3(-2000, 0, 0);
    }

    public void OnClickShowTablet()
    {
        // Tablet을 -2000,0,0에서 0,0,0으로 이동
        Tablet.transform.localPosition = new Vector3(-2000, 0, 0);
        Tablet.transform.DOLocalMove(Vector3.zero, 1f); // 1초 동안 이동
    }

    public void OnClickHideTablet()
    {
        // Tablet을 0,0,0에서 -2000,0,0으로 이동
        Tablet.transform.localPosition = new Vector3(0, 0, 0);
        Tablet.transform.DOLocalMove(new Vector3(-2000, 0, 0), 1f); // 1초 동안 이동
    }
}
