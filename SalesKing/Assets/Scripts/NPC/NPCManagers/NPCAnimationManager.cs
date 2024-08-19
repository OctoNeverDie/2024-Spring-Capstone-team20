using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationManager : MonoBehaviour
{
    private static readonly string basePath = "Animations/NPC";
    public Dictionary<NPCDefine.AnimType, List<Animation>> NPCAnimDictionary = new Dictionary<NPCDefine.AnimType, List<Animation>>();

    private void Awake()
    {
        LoadAnimations();
        
    }

    private void LoadAnimations()
    {
        
        foreach (NPCDefine.AnimType category in System.Enum.GetValues(typeof(NPCDefine.AnimType)))
        {
            string folderPath = $"{basePath}/{category.ToString()}";
            Animation[] animations = Resources.LoadAll<Animation>(folderPath);

            if (animations.Length > 0)
            {
                if (!NPCAnimDictionary.ContainsKey(category))
                {
                    NPCAnimDictionary[category] = new List<Animation>();
                }

                NPCAnimDictionary[category].AddRange(animations);
                Debug.Log($"Loaded {animations.Length} animations for category '{category}'.");
            }
        }
        
    }

    
    public List<Animation> GetAnimByCategory(NPCDefine.AnimType category)
    {
        if (NPCAnimDictionary.TryGetValue(category, out List<Animation> animations))
        {
            return animations;
        }
        else
        {
            Debug.LogWarning($"No meshes found for category '{category}'.");
            return new List<Animation>();
        }
    }
    
}
