using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : Pool
{
    public static ProjectilePool instance;

    public override void Awake()
    {
        if (instance) return;
        instance = this;

        base.Awake();
    }
}
