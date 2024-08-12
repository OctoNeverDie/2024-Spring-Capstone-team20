using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLooks : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer; // 변경할 SkinnedMeshRenderer 컴포넌트
    public Mesh newMesh; // 새롭게 적용할 메쉬

    void Start()
    {
        // SkinnedMeshRenderer가 할당되지 않았을 경우, 현재 게임 오브젝트에서 찾습니다.
        if (skinnedMeshRenderer == null)
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        }

        // SkinnedMeshRenderer가 존재하고, 새로운 메쉬가 할당되어 있을 때만 변경합니다.
        if (skinnedMeshRenderer != null && newMesh != null)
        {
            skinnedMeshRenderer.sharedMesh = newMesh;
        }
        else
        {
            Debug.LogWarning("SkinnedMeshRenderer 또는 새 메쉬가 할당되지 않았습니다.");
        }
    }


}
