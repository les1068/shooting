using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScroller : Scroller
{
    SpriteRenderer[] spritrenderers;

    protected override void Awake()
    {
        base.Awake();
         
        spritrenderers = GetComponentsInChildren<SpriteRenderer>();  // �ڽ����� �ִ� SpriteRenderer ���
    }
    protected override void MoveRightEnd(int index)
    {
        base.MoveRightEnd(index);

        int rand = Random.Range(0, 4);  // 0(0b_00),1(0b_01),2(0b_10),3(0b_11) 

        spritrenderers[index].flipX = ((rand & 0b_01) != 0);  // ù��° ��Ʈ�� 1�̸� true, �ƴϸ� false
        spritrenderers[index].flipY = ((rand & 0b_10) != 0);  // �ι�° ��Ʈ�� 1�̸� true, �ƴϸ� false
    }
}
