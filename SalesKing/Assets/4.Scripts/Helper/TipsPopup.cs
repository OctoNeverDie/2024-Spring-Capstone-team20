using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;

/// <summary>
/// 자식 오브젝트들을 순차적으로 활성화하여 팝업을 표시하는 스크립트
/// </summary>
public class TipsPopup : MonoBehaviour
{
    [SerializeField] TipSO tipso;
    [SerializeField] float popDelay = 0.1f;
    [SerializeField] int npcID = 0;

    private GameObject[] children;
    private string[] tips;
    void Start()
    {
        if(npcID == 0) npcID = ChatManager.Instance.ThisNpc.NpcID;
        tips = thisNpcTips(npcID);

        children = GetDirectChildObjects();

        StartCoroutine(PopEachChild(children));
    }

    private string[]? thisNpcTips(int npcID)
    {
        return tipso.npcTips
                    .Where(n => n.npcId == npcID)
                    .Select(n => n.tips) // 필터링된 항목에서 npcProfileImg 속성 선택
                    .SingleOrDefault(); // 첫 번째 항목 반환, 없으면 null
    }

    private GameObject[] GetDirectChildObjects()
    {
        int childCount = transform.childCount;
        GameObject[] childObjects = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (tips[i] != null)
                child.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = tips[i];
            child.SetActive(false);
            childObjects[i] = child;
        }

        return childObjects;
    }

    IEnumerator PopEachChild(GameObject[] childObjects)
    {
        foreach (var child in childObjects)
        {
            child.SetActive(true);
            yield return new WaitForSeconds(popDelay);
        }
    }
}
