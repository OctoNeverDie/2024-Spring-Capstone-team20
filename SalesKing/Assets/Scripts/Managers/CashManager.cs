using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    // 총 현금량
    public float TotalCash { get; private set; }

    private void Start()
    {
        // PlayerPrefs에서 현금 로드 (현재는 주석 처리)
        //LoadCash();
        TotalCash = 0f;
        UpdateCashUI(); 
    }

    // 현금을 추가하는 메서드
    public void AddCash(float amount)
    {
        TotalCash += amount;
        SaveCash();
        UpdateCashUI();
    }

    // 현금을 제거하는 메서드
    public bool RemoveCash(float amount)
    {
        if (TotalCash >= amount)
        {
            TotalCash -= amount;
            SaveCash();
            UpdateCashUI();
            return true; // 성공적으로 제거됨
        }
        return false; // 현금 부족 -> 게임 오버
    }

    // 현금 저장
    private void SaveCash()
    {
       // PlayerPrefs.SetInt("TotalCash", TotalCash);
        //PlayerPrefs.Save();
    }

    // 현금 로드
    private void LoadCash()
    {

        TotalCash = PlayerPrefs.GetInt("TotalCash", 300); // 기본값 300
    }

    // UI 업데이트 (예: TextMeshPro 또는 UI Text 업데이트)
    private void UpdateCashUI()
    {
        Managers.UI.SetCashText(TotalCash.ToString());
    }
}
