using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class DataController : MonoBehaviour
{
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }

    static DataController _instance;
    public static DataController Instance                                                   //해당 스크립트를 어디에서든 불러올 수 있게끔 하는 함수 
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataController";
                _instance = _container.AddComponent(typeof(DataController)) as DataController;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            if (_gameData == null)
            {
                LoadGameData();
                ToGameJson();
            }
            return _gameData;
        }
    }
    public void LoadGameData()
    {
        //string filePath = Application.persistentDataPath + "/GameData.json";
        string filePath = Application.dataPath + "/Scripts/JSON/GameData.json";
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            print(filePath);
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            // 새 파일이 없을 경우, 새로운 GameData 객체 생성
            _gameData = new GameData();  // 기본 GameData 객체 생성
            ToGameJson(); // 생성된 GameData를 JSON 파일로 저장
        }
    }


    System.IO.StreamWriter SW = null;

    [ContextMenu("To Game Json")]
    public void ToGameJson()
    {
        //File.WriteAllText(Application.persistentDataPath + "/GameData.json", JsonUtility.ToJson(gameData, true));
        File.WriteAllText(Application.dataPath + "/Scripts/JSON/GameData.json", JsonUtility.ToJson(gameData, true));
    }

    
    void OnApplicationQuit()
    {
        ToGameJson();//종료할때 있어야함
    }


    void Start()
    {
        /*
        if (PlayerPrefs.HasKey("key") == true)
        {
            if (gameObject.name == "DataControllerF")
                GameObject.Find("DataControllerF").SetActive(false);
        }

        if (PlayerPrefs.HasKey("key") == false) // 최초실행이면
        {
            PlayerPrefs.SetInt("key", PlayerPrefs.GetInt("key", 0)); //최초실행시 여기 저장...
            //Debug.Log("최초실행");

            ToGameJson(); //이걸 먼저해야됨. 그리고 다른 데이터컨트롤러 인스턴스가 실행되는 스크립트가 메인씬에 없어야함

            if (gameObject.name == "DataControllerF")
                GameObject.Find("DataControllerF").SetActive(false);
        }
        */

    }

    IEnumerator SaveGameData()
    {
        while (true)
        {
            //File.WriteAllText(Application.persistentDataPath + "/GameData.json", JsonUtility.ToJson(gameData, true));
            File.WriteAllText(Application.dataPath + "/Scripts/JSON/GameData.json", JsonUtility.ToJson(gameData, true));

            try
            {
                //SW = new System.IO.StreamWriter(Application.persistentDataPath + "/GameData.json");
                SW = new System.IO.StreamWriter(Application.dataPath + "/Scripts/JSON/GameData.json");
            }
            catch (Exception exp)
            {
                UnityEngine.Debug.Log(exp);
            }
            finally
            {
                if (SW != null)
                {
                    SW.Close();
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

    }

}