using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimatorHandle : MonoBehaviour
{
    public Tower tower;

    //Event function called from the animator
    public void FireProjectile()
    {
        tower.FireProjectile();
    }
}
