using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class LightManager : MonoBehaviour
{
    /// <summary>
    /// 자식들이 각각 조명을 담당,
    /// directional light는 따로 넣어주기
    /// </summary>
    
    [Header("Light Settings")]
    [SerializeField] LightSO lightSO;

    [SerializeField] Light LeftLight;
    [SerializeField] Light RightLight;
    [SerializeField] Light SideLight;
    [SerializeField] Light DownLight;

    Light DirectionalLight;

    private List<LightSO.LightSet> lightSet = new List<LightSO.LightSet>();
    Define.Emotion currentEmotion;

    private Dictionary<Define.LightType, Light> lightDictionary;

    private void Awake()
    {
        City_ChattingUI.OnEmotionSetup -= SetLight;
        City_ChattingUI.OnEmotionSetup += SetLight;

        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        DirectionalLight = rootObjects
            .Select(go => go.GetComponent<Light>())
            .FirstOrDefault(l => l != null && l.type == LightType.Directional);

        // LightType과 Light 컴포넌트를 매핑
        lightDictionary = new Dictionary<Define.LightType, Light>
        {
            { Define.LightType.Left, LeftLight },
            { Define.LightType.Right, RightLight },
            { Define.LightType.Side, SideLight },
            { Define.LightType.Bottom, DownLight }
        };
    }

    private void OnDestroy()
    {
        City_ChattingUI.OnEmotionSetup -= SetLight;
    }

    /// <summary>
    /// 감정에 따라 조명을 설정합니다.
    /// </summary>
    /// <param name="thisEmotion">설정할 감정</param>
    public void SetLight(Define.Emotion thisEmotion)
    {
        currentEmotion = thisEmotion;

        Vector3 directionalRotation = DirectionalLight.transform.eulerAngles;
        directionalRotation.x = (thisEmotion == Define.Emotion.worst) ? -50f : 50f;
        DirectionalLight.transform.eulerAngles = directionalRotation;

        lightSet = lightSO.GetLightSets(thisEmotion);

        foreach (var kvp in lightDictionary)
        {
            Define.LightType lightType = kvp.Key;
            Light light = kvp.Value;

            SetLightProperties(lightType, light);
        }
    }

    private void SetLightProperties(Define.LightType lightType, Light light)
    {
        var lightData = lightSet.FirstOrDefault(n => n.lightType == lightType);
        if (lightData != null)
        {
            light.intensity = lightData.intensity;
            light.color = ColorParsing(lightData.Color);
        }
        else
        {
            // 기본값 설정 또는 경고 메시지 출력
            Debug.LogWarning($"LightType {lightType}에 해당하는 LightSet이 존재하지 않습니다.");
            light.intensity = 1f; // 기본 강도
            light.color = Color.white; // 기본 색상
        }
    }

    private Color ColorParsing(string strHexColor)
    {
        strHexColor = "#" + strHexColor;

        Color parsedColor;
        bool isValidColor = ColorUtility.TryParseHtmlString(strHexColor, out parsedColor);
        if (isValidColor)
        {
            return parsedColor;
        }
        else
        {
            Debug.Log("변환이 안 됨");
            return Color.white; // 기본 색상
        }
    }

}
