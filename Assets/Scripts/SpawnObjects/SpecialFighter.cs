using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFighter : EnemyBase
{
    protected override void OnEnable()
    {
        base.OnEnable();  // 기존 초기화 작업 진행

        // 처음에 빠르게 움직이고 기다리다가 다시 빠르게 움직인다.  
    }
    protected override void OnCrush()
    {
        base.OnCrush(); // 기존 폭파처리
        GameObject obj =  Factory.Inst.GetObject(PoolObjectType.PowerUp); // 파워업 아이템 생성
        obj.transform.position = transform.position;    // 현재 내 위치로 옮기기
    }
}
