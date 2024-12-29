using UnityEngine;

/// <summary>
/// Image && Ddaesa
/// </summary>

public class EndingController : MonoBehaviour
{
    public bool isTest;
    public bool isSuccess = true;

    [SerializeField] GameObject Happy;
    [SerializeField] GameObject Bad;

    void Start() {
        if (DataController.Instance == null) {
            if (!isTest) {
                Debug.LogError("DataController 싱글톤이 씬에 존재하지 않습니다.");
                return;
            }
        }
        else {
            if (!isTest) {
                isSuccess = DataController.Instance.gameData.cleared_npc_count > 6 ? true : false;
            }
            
        }
        
        ShowHappyEnding(isSuccess);
    }

    private void ShowHappyEnding(bool isHappy) {
        Happy.SetActive(isHappy);
        Bad.SetActive(!isHappy);
    }
}
