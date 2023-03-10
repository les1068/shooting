using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLines = null;   // UI에서 표시하는 랭킹 한줄들을 모아둔 배열 (0번째가 1등, 4번째가 5등)
    int[] highScores = null;       // 랭킹별 최고점 (0번째가 1등, 4번째가 5등)
    string[] rankerNames = null;   // 랭킹에 들어간 사람 이름  (0번째가 1등, 4번째가 5등)

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();  // 모든 랭킹 라인 다 가져오기

        int size = rankLines.Length;         
        highScores = new int[size];     // 배열 확보
        rankerNames = new string[size];

        if (!LoadRankingData())         // 파일에서 읽기 시도
        {
            for (int i = 0; i < size; i++)    // 파일에서 못 읽었으면 디폴트 값 주기
            {
                int result = 1;
                for(int j = size-i; j >0; j--)
                {
                    result *= 10;
                }
                highScores[i] = result;  // 10만, 만, 천, 백, 십

                char temp = 'A';
                temp = (char)((byte)temp + i);
                rankerNames[i] = $"{temp}{temp}{temp}";    // AAA,BBB,CCC,DDD,EEE

            }
        }
        RefreshRankLines();

        SaveRankingData();
    }
    void SaveRankingData()
    {
        //PlayerPrefs.SetInt("Score", 10);    // 컴퓨터에 Score라는 이름으로 10을 저장

        //SaveData saveData = new SaveData();            
        SaveData saveData = new(); // 윗줄과 같은 코드(타입을 알 수 있기 때문에 생략한것)

        saveData.rankerNames = rankerNames;  // 생성한 인스턴스에 데이터 기록
        saveData.highScores = highScores;    //
        
        string json = JsonUtility.ToJson(saveData);
        //Debug.Log(json);

        string path = $"{Application.dataPath}/Save/";
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string fullPath = $"{path}Save.json";
        File.WriteAllText(fullPath, json);
    }
    bool LoadRankingData()
    {
        return false;
    }
    void RefreshRankLines()
    {
        for (int i = 0; i < rankLines.Length; i++)
        {
            rankLines[i].SetData(rankerNames[i], highScores[i]);
        }
    }
}
