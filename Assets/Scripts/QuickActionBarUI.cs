using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickActionBarUI : SubPlayerUI
{
    bool m_displayTowerInfo;

    void Start()
    {
        List<Tower> towers = GameState.instance.towers;
        //Spawn towers buttons
    }

    public override void Cancel()
    {
        m_displayTowerInfo = false;
    }

    public override void Confirm()
    {
        if(m_displayTowerInfo)
        {
            //Place tower
            m_displayTowerInfo = false;
        }
        else
        {
            m_displayTowerInfo = true;
        }
    }

    public override void NavigateMenu(Vector2 input)
    {
        //Navigates the bar
    }

    public override void NextSubMenu()
    {
        //Unused
    }

    public override void OpenCharaterMenu()
    {
        //Unused may be used again eventually
    }

    public override void OpenTowerMenu()
    {
        //Using player's hitbox, find the nearest tower that's being overlapped
        Tower tower = null;
        if(tower)
        {
            //Set tower UI's tower to the selected tower
        }

    }

    public override void PreviousSubMenu()
    {
        //Unused
    }
}
