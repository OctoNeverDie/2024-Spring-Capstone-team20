using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDdoing : MonoBehaviour
{
    [SerializeField] List<(float, float)> tweenFactors = new List<(float, float)>
    {
        (1.2f, 0.75f),
        (1.0f, 0.3f)
    };
    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        if(btn!=null)
            btn.onClick.AddListener(DdoingAction);
    }

    private void DdoingAction()
    {
        Util.PopDotween(transform, tweenFactors);
    }
}