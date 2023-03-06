using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Asteroid : Test_Base
{
    protected override void Test1(InputAction.CallbackContext _)
    {
        Factory.Inst.GetAsteroidSmall();
    }
    protected override void Test2(InputAction.CallbackContext _)
    {
        float criticalChance = 0.1f;
        int testNum = 10000000;

        int counter = 0;
        for (int i = 0; i < testNum; i++)
        {
            float random = Random.Range(0.0f, 1.0f);
            if (random < criticalChance)
            {
                counter++;
            }
        }
        Debug.Log($"���� ��� : {testNum * criticalChance}");
        Debug.Log($"���� ��� : {counter}");

    }
}