using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileChange : MonoBehaviour
{
    [SerializeField] private ProfileSO npcProfileData; // ScriptableObject 참조
    [SerializeField] private List<Button> profileButtons; // 버튼 리스트

    private Image accza;

    private void Start()
    {
        accza = GetComponent<Image>();

        // 각 버튼에 대해 이벤트 리스너 추가
        for (int i = 0; i < profileButtons.Count; i++)
        {
            int index = i;
            profileButtons[i].onClick.AddListener(() => ChangeProfile(index));
        }
    }

    // 프로필을 변경하는 메서드
    private void ChangeProfile(int index)
    {
        if (index >= 0 && index < npcProfileData.npcProfileImg.Count)
        {
            accza.sprite = npcProfileData.npcProfileImg[index];
            accza.SetNativeSize();
        }
    }
}
