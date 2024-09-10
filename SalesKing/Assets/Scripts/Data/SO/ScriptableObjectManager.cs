using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class ScriptableObjectManager<T>
{
    public void Init()
    {
        MakeSOs();
    }

    protected string basePath = "Assets/Resources/Data/";
    protected void MakeDirectory(string folderName = "folder")
    {
#if UNITY_EDITOR
        if (!folderName.StartsWith(basePath))
            folderName = Path.Combine(basePath, folderName);

        if (Directory.Exists(folderName))
        {
            //can seemed not deleted Due to the delay of editor
            Directory.Delete(folderName, true);
            
        }
       
        Directory.CreateDirectory(folderName);
#endif
    }

    protected abstract void MakeSOs();
    protected abstract void MakeSOInstance(T DataFromList);
}
