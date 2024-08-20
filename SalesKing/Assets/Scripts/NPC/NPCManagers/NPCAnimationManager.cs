using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationManager : MonoBehaviour
{
    private static readonly string basePath = "Animations/NPC";
    public Dictionary<NPCDefine.AnimType, List<AnimationClip>> NPCAnimDictionary = new Dictionary<NPCDefine.AnimType, List<AnimationClip>>();

    void Awake()
    {
        LoadAnimations();
    }

    private void LoadAnimations()
    {
        
        foreach (NPCDefine.AnimType category in System.Enum.GetValues(typeof(NPCDefine.AnimType)))
        {
            string folderPath = $"{basePath}/{category.ToString()}";
            AnimationClip[] animations = Resources.LoadAll<AnimationClip>(folderPath);

            if (animations.Length > 0)
            {
                if (!NPCAnimDictionary.ContainsKey(category))
                {
                    NPCAnimDictionary[category] = new List<AnimationClip>();
                }

                NPCAnimDictionary[category].AddRange(animations);
                //Debug.Log($"Loaded {animations.Length} animations for category '{category}'.");
            }
        }
        
    }

    
    public List<AnimationClip> GetAnimByCategory(NPCDefine.AnimType category)
    {
        if (NPCAnimDictionary.TryGetValue(category, out List<AnimationClip> animations))
        {
            return animations;
        }
        else
        {
            Debug.LogWarning($"No meshes found for category '{category}'.");
            return new List<AnimationClip>();
        }
    }
    
}
