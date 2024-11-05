using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialog : TutorialBase
{
    [SerializeField]
    private List<string> dialogLines;
    [SerializeField]
    private gameObject DialogPanel;
    private int currentLineIndex = 0;
    private bool isCompleted = false;

    public override void Enter()
    {
        DialogPanel.SetActive(true);
        DisplayNextLine();
    }

    private void DisplayNextLine()
    {
        if (currentLineIndex < dialogLines.Count)
        {
            ShowDialog(dialogLines[currentLineIndex]);
            currentLineIndex++;
        }
        else
        {
            isCompleted = true;
        }
    }

    private void ShowDialog(string line)
    {
        //UI 수정 해야함
    }

    public override void Execute(TutorialController controller)
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (!isCompleted)
            {
                DisplayNextLine();
            }
        }

        if (isCompleted)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        // 튜토리얼 종료 시 실행할 코드
    }
}