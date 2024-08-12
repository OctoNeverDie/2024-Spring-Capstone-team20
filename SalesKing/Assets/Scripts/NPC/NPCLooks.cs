using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLooks : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer; // ������ SkinnedMeshRenderer ������Ʈ
    public Mesh newMesh; // ���Ӱ� ������ �޽�

    void Start()
    {
        // SkinnedMeshRenderer�� �Ҵ���� �ʾ��� ���, ���� ���� ������Ʈ���� ã���ϴ�.
        if (skinnedMeshRenderer == null)
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        }

        // SkinnedMeshRenderer�� �����ϰ�, ���ο� �޽��� �Ҵ�Ǿ� ���� ���� �����մϴ�.
        if (skinnedMeshRenderer != null && newMesh != null)
        {
            skinnedMeshRenderer.sharedMesh = newMesh;
        }
        else
        {
            Debug.LogWarning("SkinnedMeshRenderer �Ǵ� �� �޽��� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }


}
