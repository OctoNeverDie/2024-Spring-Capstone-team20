using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class ScriptableObjectManager<T> : MonoBehaviour
{
    public virtual void Init()
    {
        MakeSO();
    }

    protected string basePath = "Assets/Resources/Data/";
    protected void MakeDirectory(string folderName = "folder")
    {
#if UNITY_EDITOR
        if (!folderName.StartsWith(basePath))
            folderName = Path.Combine(basePath, folderName);

        if (!Directory.Exists(folderName))
        {
            Directory.CreateDirectory(folderName);
            //can seemed not deleted Due to the delay of editor
            //Directory.Delete(folderName, true);
        }
#endif
    }

    protected abstract void MakeSO();
    protected abstract void MakeSOInstance(T DataFromList);
}
