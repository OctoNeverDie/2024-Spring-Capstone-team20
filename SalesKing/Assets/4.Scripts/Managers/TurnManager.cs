using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : Singleton<TurnManager>, ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => true;

    protected override void Awake()
    {
        base.Awake();
    }

    void EndDayAndUpdateToFile()
    {
        DataController.Instance.playData.cur_day_ID++;
        DataController.Instance.ToPlayJson(DataController.Instance.gameData.cur_save_file_ID);
        SceneManager.LoadScene("CityMap");
    }

}
