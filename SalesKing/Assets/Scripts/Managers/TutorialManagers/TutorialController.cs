using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private List<TutorialBase> tutorials;

    private TutorialBase currentTutorial = null;
    private int currentIndex = -1;

    private void Start()
    {
        SetNextTutorial();
    }

    private void Update()
    {
        if (currentTutorial != null) { 
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        if(currentTutorial != null)
        {
            currentTutorial.Exit();
        }

        if (currentIndex >= tutorials.Count - 1) {
            CompletedAllTutorials();
            return;
        }

        currentIndex++;
        currentTutorial = tutorials[currentIndex];

        currentTutorial.Enter();
    }

    public void CompletedAllTutorials()
    {
        currentTutorial = null;

        //행동 끝났을때 작성
        Debug.Log("Complete All");
    }
}
