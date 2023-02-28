using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScroller : Scroller
{
    SpriteRenderer[] spritrenderers;

    protected override void Awake()
    {
        base.Awake();
         
        spritrenderers = GetComponentsInChildren<SpriteRenderer>();  // 자식으로 있는 SpriteRenderer 모두
    }
    protected override void MoveRightEnd(int index)
    {
        base.MoveRightEnd(index);

        int rand = Random.Range(0, 4);  // 0(0b_00),1(0b_01),2(0b_10),3(0b_11) 

        spritrenderers[index].flipX = ((rand & 0b_01) != 0);  // 첫번째 비트가 1이면 true, 아니면 false
        spritrenderers[index].flipY = ((rand & 0b_10) != 0);  // 두번째 비트가 1이면 true, 아니면 false
    }
}
