using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickActionBarUI : SubPlayerUI
{
    bool m_displayTowerInfo;
    public TowerInfoUI towerInfoUI;
    public TowerInfoBuyUI towerBuyUI;
    int currentTowerIndex = 0;

    public RectTransform content;
    public TowerIcon towerIconPrefab;

    List<TowerIcon> towerIcons;

    public float spacing = 0.0f;

    void Start()
    {
        towerIcons = new List<TowerIcon>();
        List<Tower> towers = GameState.instance.towers;

        foreach(Tower tower in towers)
        {
            TowerIcon icon = Instantiate(towerIconPrefab, content);
            icon.transform.localPosition = new Vector3(((icon.transform as RectTransform).rect.width + spacing) * towerIcons.Count, 0);

            icon.SetTower(tower);
            towerIcons.Add(icon);
        }
        //Spawn towers buttons
    }

    public override void Cancel()
    {
        m_displayTowerInfo = false;
        towerBuyUI.gameObject.SetActive(false);
        playerUI.player.playerTowerDetector.Disable();
    }

    public override void Confirm()
    {
        if(m_displayTowerInfo && playerUI.player.playerTowerDetector.isTowerPlaceable())
        {
            if(!GameState.instance.SpendCurrency(GameState.instance.towers[currentTowerIndex].cost))
                return;
            //Place tower
            Tower tower = Instantiate(GameState.instance.towers[currentTowerIndex], GameState.instance.objectStretch.transform);
            tower.transform.position = playerUI.player.transform.position;
            m_displayTowerInfo = false;
            towerBuyUI.gameObject.SetActive(false);
            playerUI.player.playerTowerDetector.Disable();
        }
        else if(!m_displayTowerInfo)
        {
            m_displayTowerInfo = true;
            towerBuyUI.SetTower(GameState.instance.towers[currentTowerIndex]);
            towerBuyUI.gameObject.SetActive(true);
            playerUI.player.playerTowerDetector.Enable(GameState.instance.towers[currentTowerIndex].unitSize);
        }
    }

    public override void NavigateMenu(Vector2 input)
    {
        Debug.Log("Navigate");
        Debug.Log(input.x);
        //Navigates the bar
        if(input.x > 0)
        {
            Debug.Log("Scroll next");
            currentTowerIndex = (currentTowerIndex + 1) % GameState.instance.towers.Count;
        }
        if(input.x < 0)
        {
            Debug.Log("Scroll previous");
            currentTowerIndex -= 1;
            if(currentTowerIndex < 0)
                currentTowerIndex += GameState.instance.towers.Count;
        }
        if(m_displayTowerInfo)
        {
            towerBuyUI.SetTower(GameState.instance.towers[currentTowerIndex]);
            playerUI.player.playerTowerDetector.Enable(GameState.instance.towers[currentTowerIndex].unitSize);
        }

        content.localPosition = Vector3.zero - new Vector3(((towerIconPrefab.transform as RectTransform).rect.width + spacing) * currentTowerIndex, 0);
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
        Tower tower = playerUI.player.nearestTower;
        if(tower)
        {
            m_displayTowerInfo = false;
            towerBuyUI.gameObject.SetActive(false);

            //Set tower UI's tower to the selected tower
            towerInfoUI.SetTower(tower);
            towerInfoUI.gameObject.SetActive(true);
            playerUI.m_uiStack.Push(towerInfoUI);
        }

    }

    public override void PreviousSubMenu()
    {
        //Unused
    }
}
