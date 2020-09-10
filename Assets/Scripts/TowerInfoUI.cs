using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TowerInfoUI : SubPlayerUI
{
    public Tower tower;

    public TextMeshProUGUI towerName;
    public TextMeshProUGUI targetStateText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI attackSpeedText;

    public override void Cancel()
    {
        //Pop this UI

        playerUI.m_uiStack.Pop();
        gameObject.SetActive(false);
    }

    public override void Confirm()
    {
        //Confirm the selected button
        //EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
    }

    public override void NavigateMenu(Vector2 input)
    {
        //Switch the selected button
    }

    public override void NextSubMenu()
    {
        tower.NextTargetSceme();
        targetStateText.text = tower.targetScheme.ToString();
    }

    public override void OpenCharaterMenu()
    {
        //Do nothing
    }

    public override void OpenTowerMenu()
    {
        //Do nothing
    }

    public override void PreviousSubMenu()
    {
        tower.PreviousTargetScheme();
        targetStateText.text = tower.targetScheme.ToString();
    }

    public void SetTower(Tower tower)
    {
        this.tower = tower;
        towerName.text = tower.name;
        targetStateText.text = tower.targetScheme.ToString();
        damageText.text = tower.damage.ToString();
        rangeText.text = tower.range.ToString();
        attackSpeedText.text = tower.attackSpeed.ToString();
    }

    private void FixedUpdate()
    {
        //Check if player is still in range, drop this UI if it's gone
        Collider2D[] cast = Physics2D.OverlapCircleAll(playerUI.player.transform.position, playerUI.player.unitSize, 1 << LayerMask.NameToLayer("Unit Collider") | 1 << LayerMask.NameToLayer("Path Collider"));

        bool collidingPlayer = false;
        foreach(Collider2D collider in cast)
        {
            if(collider.transform.parent.gameObject == tower.gameObject)
                collidingPlayer = true;
        }

        if(!collidingPlayer)
        {
            Cancel();
        }
    }
}
