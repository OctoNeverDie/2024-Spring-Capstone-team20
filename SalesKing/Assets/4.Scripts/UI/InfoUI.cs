using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class InfoUI : MonoBehaviour
{
    [SerializeField] GameObject info_panel;
    [SerializeField] GameObject info_button;

    public void openInfo()
    {
        info_panel.transform.DOLocalMove(new Vector3(-400, 418, 0), 0.5f).SetEase(Ease.OutQuad); 
        info_button.SetActive(false);
    }

    public void closeInfo()
    {
        info_panel.transform.DOLocalMove(new Vector3(-400, 700, 0), 0.5f).SetEase(Ease.OutQuad);
        info_button.SetActive(true);
    }
}
