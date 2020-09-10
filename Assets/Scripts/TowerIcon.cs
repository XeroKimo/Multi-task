using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerIcon : MonoBehaviour
{
    public Tower tower { get; private set; }
    public RawImage image;

    public void SetTower(Tower tower)
    {
        this.tower = tower;

        image.texture = tower.GetComponentInChildren<SpriteRenderer>().sprite.texture;
    }
}
