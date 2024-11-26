using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<PlayerManager>, ISingletonSettings
{
    [SerializeField] private List<GameObject> tutorial_list = new List<GameObject>();

    public bool ShouldNotDestroyOnLoad => true;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        int day_index = DataController.Instance.playData.cur_day_ID;
        tutorial_list[day_index].gameObject.SetActive(true);
    }

}
