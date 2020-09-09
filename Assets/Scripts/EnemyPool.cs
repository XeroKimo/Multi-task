using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : Pool
{
    static public EnemyPool instance;

    public override void Awake()
    {
        if (instance) return;
        instance = this;

        base.Awake();
    }
}
