using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfoUI : SubPlayerUI
{
    public Tower tower;

    public override void Cancel()
    {
        //Pop this UI
    }

    public override void Confirm()
    {
        //Confirm the selected button
    }

    public override void NavigateMenu(Vector2 input)
    {
        //Switch the selected button
    }

    public override void NextSubMenu()
    {
        tower.NextTargetSceme();
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
    }
}
