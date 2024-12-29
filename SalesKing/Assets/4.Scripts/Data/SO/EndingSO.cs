using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EndingSO", menuName = "SO/EndingSO")]
public class EndingSO : ScriptableObject
{    
    [System.Serializable]
    public class Ending
    {
        public string EndingName;
        public List<Sprite> EndingScene;
        public List<DialoguePart> DialogueParts;
    }

    [System.Serializable]
    public class DialoguePart
    {
        public List<string> Dialogue;
    }

    public List<Ending> endings = new List<Ending>();
}
