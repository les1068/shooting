using System.Collections;
using System.Collections.Generic;
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
    }
    void SaveRankingData()
    {

    }
    bool LoadRankingData()
    {
        return false;
    }
}
