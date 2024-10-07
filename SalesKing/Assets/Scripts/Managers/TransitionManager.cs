using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] GameObject TransPrefab;
    GameObject TransGO;
    public TransitionUI ui;

    void Start()
    {
        SpawnTransitionUI();
    }

    private void SpawnTransitionUI()
    {
        TransPrefab = Resources.Load<GameObject>("Prefabs/UI/TransitionCanvas");

        if (TransPrefab != null)
        {
            // Instantiate the prefab
            TransGO = Instantiate(TransPrefab);
            DontDestroyOnLoad(TransGO);

            // Set ManagersGO as the parent
            //TransGO.transform.SetParent(Managers.Instance.ManagersGO.transform, false);

            ui = TransGO.GetComponent<TransitionUI>();
        }
    }
}
