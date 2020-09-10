using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class TowerInfoBuyUI : MonoBehaviour
{
    public Tower tower;

    public TextMeshProUGUI towerName;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI costText;

    public void SetTower(Tower tower)
    {
        this.tower = tower;
        if(!tower)
        {
            return;
        }
        towerName.text = tower.name;
        damageText.text = tower.damage.ToString();
        rangeText.text = tower.range.ToString();
        attackSpeedText.text = tower.attackSpeed.ToString();
        costText.text = tower.cost.ToString();
    }

}
