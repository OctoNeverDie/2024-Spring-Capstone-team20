using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningManager : MonoBehaviour
{
    [SerializeField] TutorialController myOpening;

    void Update()
    {
        if (myOpening.isComplete) SceneManager.LoadScene("OfficeMap");
    }
}
