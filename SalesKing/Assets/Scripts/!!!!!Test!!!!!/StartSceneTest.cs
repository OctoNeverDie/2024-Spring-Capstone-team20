using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneTest : MonoBehaviour
{
    public void LoadCityScene()
    {
        SceneManager.LoadScene("CityMap");
    }

    public void LoadOfficeScene()
    {
        SceneManager.LoadScene("OfficeMap");
    }
}
