using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class City_EndDayUI : MonoBehaviour
{

    void Start()
    {

    }

    void EndDayAndUpdateToFile()
    {
        DataController.Instance.playData.cur_day_ID++;
        DataController.Instance.ToPlayJson(DataController.Instance.gameData.cur_save_file_ID);
        SceneManager.LoadScene("CityMap");
    }


}
