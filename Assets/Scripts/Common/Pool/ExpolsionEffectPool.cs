using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpolsionEffectPool : ObjectPool<Effect>
{
    private void Start()
    {
        Initialize();
    }
}
