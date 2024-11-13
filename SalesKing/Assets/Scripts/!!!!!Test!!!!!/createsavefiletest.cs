using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createsavefiletest : MonoBehaviour
{
    int count = 0;
    public void onclickcreate()
    {
        DataController.Instance.LoadPlayData("playData"+count);
        count++;
        SaveFileManager.Instance.UpdateSaveFilePanels();
    }
}
