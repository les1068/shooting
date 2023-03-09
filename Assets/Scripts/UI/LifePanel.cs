using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifePanel : MonoBehaviour
{
    TextMeshProUGUI lifeText;

    private void Awake()
    {
        Transform textTransform = transform.GetChild(2);
        lifeText = textTransform.GetComponent<TextMeshProUGUI>();   // �ؽ�Ʈ�Ž� ���� ã��
    }
    private void Start()
    {
        //Player player =GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Player player = FindObjectOfType<Player>();         // �÷��̾� ã�Ƽ�
        player.OnLifeChange += Refresh;                     // ��������Ʈ�� �Լ� ���
    }

    private void Refresh(int life)
    {
        lifeText.text = life.ToString();                   // �Ķ���ʹ�� ���� ���
    }
}

