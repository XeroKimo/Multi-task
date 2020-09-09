using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerUI : MonoBehaviour
{
    private Stack<SubPlayerUI> m_uiStack;



    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if(Input.GetAxis("DPadHorizontal") != 0)
        {
            m_uiStack.Peek().NavigateMenu(new Vector2(Input.GetAxis("DPadHorizontal"), 0));
        }
    }

    public void Confirm()
    {
        m_uiStack.Peek().Confirm();
    }
    public void Cancel()
    {
    }
    public void OpenTowerMenu()
    {
    }
    public void OpenCharaterMenu()
    {
    }
    public void NavigateMenu(Vector2 input)
    {
    }
    public void NextSubMenu()
    {
    }
    public void PreviousSubMenu()
    {
    }
}

public abstract class SubPlayerUI : MonoBehaviour
{
    public PlayerUI playerUI;

    public abstract void Confirm();
    public abstract void Cancel();
    public abstract void OpenTowerMenu();
    public abstract void OpenCharaterMenu();
    public abstract void NavigateMenu(Vector2 input);
    public abstract void NextSubMenu();
    public abstract void PreviousSubMenu();
}