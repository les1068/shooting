using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLines = null;   // UI���� ǥ���ϴ� ��ŷ ���ٵ��� ��Ƶ� �迭 (0��°�� 1��, 4��°�� 5��)
    int[] highScores = null;       // ��ŷ�� �ְ��� (0��°�� 1��, 4��°�� 5��)
    string[] rankerNames = null;   // ��ŷ�� �� ��� �̸�  (0��°�� 1��, 4��°�� 5��)

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();  // ��� ��ŷ ���� �� ��������

        int size = rankLines.Length;         
        highScores = new int[size];     // �迭 Ȯ��
        rankerNames = new string[size];

        if (!LoadRankingData())         // ���Ͽ��� �б� �õ�
        {
            for (int i = 0; i < size; i++)    // ���Ͽ��� �� �о����� ����Ʈ �� �ֱ�
            {
                int result = 1;
                for(int j = size-i; j >0; j--)
                {
                    result *= 10;
                }
                highScores[i] = result;  // 10��, ��, õ, ��, ��

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
        //PlayerPrefs.SetInt("Score", 10);    // ��ǻ�Ϳ� Score��� �̸����� 10�� ����

        //SaveData saveData = new SaveData();            
        SaveData saveData = new(); // ���ٰ� ���� �ڵ�(Ÿ���� �� �� �ֱ� ������ �����Ѱ�)

        saveData.rankerNames = rankerNames;  // ������ �ν��Ͻ��� ������ ���
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
