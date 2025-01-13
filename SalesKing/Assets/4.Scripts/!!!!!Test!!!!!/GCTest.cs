using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GCTest : MonoBehaviour
{
    [SerializeField] bool isGCCollect = false;

    void Start()
    {
        long curMemory = GC.GetTotalMemory(false);
        Debug.Log($"Current Memory: {curMemory / (1024 * 1024)} MB");
    }

    public void onClickEndingScene()
    {
        if (isGCCollect)
        {
            StartCoroutine(LoadWithGC());
        }
        else
        {
            LoadWithoutGC();
        }
    }

    private void LoadWithoutGC()
    {
        long memoryBeforeScene = GC.GetTotalMemory(false);
        Debug.Log($"Memory before, without GC: {memoryBeforeScene / (1024 * 1024)} MB");

        SceneManager.LoadScene("Last");

        long memoryAfterScene = GC.GetTotalMemory(false);
        Debug.Log($"Memory after, without GC: {memoryAfterScene / (1024 * 1024)} MB");
    }

    private IEnumerator LoadWithGC()
    {
        Debug.Log("Unloading current scene...");

        // 현재 씬 언로드
        string currentScene = SceneManager.GetActiveScene().name;
        yield return SceneManager.UnloadSceneAsync(currentScene);

        // 메모리 회수 전 메모리 사용량 출력
        long memoryBefore = GC.GetTotalMemory(false);
        Debug.Log($"Memory before GC: {memoryBefore / (1024 * 1024)} MB");

        // 명시적으로 GC.Collect 호출
        Debug.Log("Calling GC.Collect...");
        GC.Collect();
        GC.WaitForPendingFinalizers();

        // 메모리 회수 후 메모리 사용량 출력
        long memoryAfter = GC.GetTotalMemory(false);
        Debug.Log($"Memory after GC: {memoryAfter / (1024 * 1024)} MB");

        Debug.Log("Loading new scene...");

        // 새 씬 로드
        yield return SceneManager.LoadSceneAsync("Last");
    }
}
