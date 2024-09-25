using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public static CashManager Instance;

    // 총 현금량
    public int TotalCash { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 필요에 따라 추가
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 방지
        }

        // PlayerPrefs에서 현금 로드 일단은 지금은 안씀.
        //LoadCash();

        TotalCash = 300;
    }

    // 현금을 추가하는 메서드
    public void AddCash(int amount)
    {
        TotalCash += amount;
        //SaveCash();
        UpdateCashUI();
    }

    // 현금을 제거하는 메서드
    public bool RemoveCash(int amount)
    {
        if (TotalCash >= amount)
        {
            TotalCash -= amount;
            //SaveCash();
            UpdateCashUI();
            return true; // 성공적으로 제거됨
        }
        return false; // 현금 부족 -> 게임 오버
    }

    // 현금 저장
    private void SaveCash()
    {
        PlayerPrefs.SetInt("TotalCash", TotalCash);
        PlayerPrefs.Save();
    }

    // 현금 로드
    private void LoadCash()
    {
        TotalCash = PlayerPrefs.GetInt("TotalCash", 300); // 기본값 300
    }

    // UI 업데이트 (예: TextMeshPro 또는 UI Text 업데이트)
    private void UpdateCashUI()
    {
        // UI 업데이트 로직 추가 이거는 추후 추가
        Debug.Log("Total Cash: " + TotalCash);
    }
}
