using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>, ISingletonSettings
{
    [SerializeField] private List<GameObject> tutorial_list = new List<GameObject>();
    public bool ShouldNotDestroyOnLoad => true;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        int day_index = DataController.Instance.gameData.cur_day_ID;
        Instantiate(tutorial_list[day_index], transform);
    }
}
