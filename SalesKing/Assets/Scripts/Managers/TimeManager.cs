using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeManager : MonoBehaviour
{
    public float worldTimer = 0f;
    public int Hour = 0;
    public int Minute = 0;
    private float TimePerGameMinute = 0.1f;

    private Material Day;
    private Material Morning;
    private Material Night;
    private Material Sunset;

    private float lerpDuration = 10f; // Lerp 전환 시간
    private float lerpTimer = 0f;

    private List<int> skyChanges = new List<int> { 0, 6, 12, 18, 24 };

    private static readonly string basePath = "Skyboxes";
    public Dictionary<Define.Skybox, List<Material>> SkyboxDictionary = new Dictionary<Define.Skybox, List<Material>>();

    private Light myLight;

    private void Awake()
    {
        LoadSkyboxes();
        AssignRandomSkybox();
        RenderSettings.skybox = Night;
        myLight = GameObject.Find("Directional Light").GetComponent<Light>();
    }

    private void Update()
    {
        worldTimer += Time.deltaTime; // 매 프레임마다 실제 시간 누적
        int totalMinute = (int)(worldTimer / TimePerGameMinute);
        Hour = totalMinute / 60;
        Minute = totalMinute % 60;

        // Lerp 진행 시간 계산
        lerpTimer += Time.deltaTime;
        float lerpFactor = lerpTimer / lerpDuration;

        // 자정부터 새벽 6시
        if (Hour >= skyChanges[0] && Hour < skyChanges[1])
        {
            // Lerp Night -> Morning
            LerpSkybox(Night, Morning, 0.5f);
        }
        // 새벽 6시부터 정오
        else if (Hour >= skyChanges[1] && Hour < skyChanges[2])
        {
            // Lerp Morning -> Day
            LerpSkybox(Morning, Day, lerpFactor);
        }
        // 정오부터 오후 6시
        else if (Hour >= skyChanges[2] && Hour < skyChanges[3])
        {
            // Lerp Day -> Sunset 
            LerpSkybox(Day, Sunset, lerpFactor);
        }
        // 오후 6시부터 자정
        else if (Hour >= skyChanges[3] && Hour < skyChanges[4])
        {
            // Lerp Sunset -> Night
            LerpSkybox(Sunset, Night, lerpFactor);
        }

        // Lerp가 끝나면 타이머 초기화
        if (lerpFactor >= 1.0f)
        {
            lerpTimer = 0f;
        }
    }

    private void LerpSkybox(Material fromSkybox, Material toSkybox, float lerpFactor)
    {
        // Skybox의 속성들(Lerp 가능한 값들)을 보간
        Material tempSkybox = new Material(fromSkybox); // 임시 Skybox Material 생성

        // 1. 노출 (Exposure) 보간
        if (fromSkybox.HasProperty("_Exposure") && toSkybox.HasProperty("_Exposure"))
        {
            float fromExposure = fromSkybox.GetFloat("_Exposure");
            float toExposure = toSkybox.GetFloat("_Exposure");
            float lerpedExposure = Mathf.Lerp(fromExposure, toExposure, lerpFactor);
            tempSkybox.SetFloat("_Exposure", lerpedExposure);
        }

        // 2. 색상 (Tint) 보간
        if (fromSkybox.HasProperty("_Tint") && toSkybox.HasProperty("_Tint"))
        {
            Color fromColor = fromSkybox.GetColor("_Tint");
            Color toColor = toSkybox.GetColor("_Tint");
            Color lerpedColor = Color.Lerp(fromColor, toColor, lerpFactor);
            tempSkybox.SetColor("_Tint", lerpedColor);
        }

        // 다른 속성들도 필요에 따라 추가 가능

        // 보간된 값을 현재 Skybox에 적용
        RenderSettings.skybox = tempSkybox;
    }

    public void StopAndRestartTime(bool isStop)
    {
        if(isStop) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

    private void LoadSkyboxes()
    {
        // set every-mesh dictionary
        foreach (Define.Skybox category in System.Enum.GetValues(typeof(Define.Skybox)))
        {
            string folderPath = $"{basePath}/{category.ToString()}";
            Material[] mats = Resources.LoadAll<Material>(folderPath);

            if (mats.Length > 0)
            {
                if (!SkyboxDictionary.ContainsKey(category))
                {
                    SkyboxDictionary[category] = new List<Material>();
                }

                SkyboxDictionary[category].AddRange(mats);
                Debug.Log($"Loaded {mats.Length} mats for category '{category}'.");
            }
        }        

    }

    private void AssignRandomSkybox()
    {
        int morning_cnt = SkyboxDictionary[Define.Skybox.Morning].Count;
        Morning = SkyboxDictionary[Define.Skybox.Morning][Random.Range(0, morning_cnt)];

        int day_cnt = SkyboxDictionary[Define.Skybox.Day].Count;
        Day = SkyboxDictionary[Define.Skybox.Day][Random.Range(0, day_cnt)];

        int sunset_cnt = SkyboxDictionary[Define.Skybox.Sunset].Count;
        Sunset = SkyboxDictionary[Define.Skybox.Sunset][Random.Range(0, sunset_cnt)];

        int night_cnt = SkyboxDictionary[Define.Skybox.Night].Count;
        Night = SkyboxDictionary[Define.Skybox.Night][Random.Range(0, night_cnt)];

    }

}
