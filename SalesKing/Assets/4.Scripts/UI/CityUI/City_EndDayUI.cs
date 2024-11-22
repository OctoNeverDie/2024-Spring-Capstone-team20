using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class City_EndDayUI : MonoBehaviour
{
    [SerializeField] Button end_day_button;
    [SerializeField] Button return_to_main_button;

    void Start()
    {
        end_day_button.onClick.AddListener(EndDayAndUpdateToFile);
        return_to_main_button.onClick.AddListener(ReturnToMain);
    }

    void EndDayAndUpdateToFile()
    {
        DataController.Instance.playData.cur_day_ID++;
        Debug.Log("the number that its going for is " + DataController.Instance.playData.cur_day_ID);
        DataController.Instance.ToPlayJson(DataController.Instance.gameData.cur_save_file_ID);
        SceneManager.LoadScene("CityMap");
    }

    void ReturnToMain()
    {
        SceneManager.LoadScene("Start");
    }
}
