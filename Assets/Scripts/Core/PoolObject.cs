using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 오브젝트 풀이 사용할 게임 오브젝트
public class PoolObject : MonoBehaviour
{
    public Action onDisable;  // 이 게임 오브젝트가 비활성화 될 때 실행되는 델리게이트

    protected virtual void OnDisable()  // 게임 오브젝트가 비활성화 될 때 실행
    {
        onDisable?.Invoke();    // 이 델리게이트에 등록된 함수들 실행
    }
    
    //특정시간 후에 비활성화 시키기 위한 델리게이트
    protected IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay); // delay만큼 대기하고
        this.gameObject.SetActive(false);       // 비활성화 시키기        
    }
}
