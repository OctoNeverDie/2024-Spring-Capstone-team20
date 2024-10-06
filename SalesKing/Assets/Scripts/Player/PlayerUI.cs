using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractableEntry
{
    public Define.Interactables interactable;
    public GameObject gameObject;
}

public class PlayerUI : MonoBehaviour
{
    public GameObject RaycastHitObj;
    public List<InteractableEntry> interactableEntries = new List<InteractableEntry>();
    public Dictionary<Define.Interactables, GameObject> InteractableIcons = new Dictionary<Define.Interactables, GameObject>();
    public Define.Interactables curInteractable;

    void Start()
    {
        foreach (var entry in interactableEntries)
        {
            InteractableIcons[entry.interactable] = entry.gameObject;
        }
    }

    public void ShowCurInteractableIcon(Define.Interactables index)
    {
        foreach (var kvp in InteractableIcons)
        {
            if (kvp.Key == index)
                kvp.Value.SetActive(true);
            else
                kvp.Value.SetActive(false);
        }

        curInteractable = index;
    }

    public void CrosshairTriggersButton(bool isShow)
    {
        RaycastHitObj.SetActive(isShow);
    }
}
