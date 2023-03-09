using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onDie += ShowPanel;      // �÷��̾��� onDie ��������Ʈ�� �Լ� ���
        //gameObject.SetActive(false);    // ���� ������Ʈ ��Ȱ��ȭ
    }

    private void ShowPanel()
    {
        //transform.GetChild(0).gameObject.SetActive(true);   // ���ڰ� ��Ȱ��ȭ �Ǿ��ִ� ���� Ȱ��ȭ
        //gameObject.SetActive(true);     // ���� ���� �г� ��ü ���̱�

        anim.SetTrigger("GameOverStart");
    }
}