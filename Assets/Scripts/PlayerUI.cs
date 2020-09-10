using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerUI : MonoBehaviour
{
    public Stack<SubPlayerUI> m_uiStack;
    public QuickActionBarUI actionBar;

    public Player player;

    public float inputDelay = 0.2f;
    bool shouldHandleInput = true;

    private void Start()
    {
        m_uiStack = new Stack<SubPlayerUI>();
        m_uiStack.Push(actionBar);
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldHandleInput)
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        if(Input.GetButton("Confirm"))
        {
            Confirm();
        }
        if(Input.GetButton("Cancel"))
            Cancel();
        if(Input.GetButton("ViewTower"))
            OpenTowerMenu();
        if(Input.GetButton("ViewCharacter"))
            OpenCharaterMenu();
        if(Input.GetAxis("DPadX") != 0 || Input.GetAxis("ScrollMenu") != 0)
        {
            float value = Input.GetAxis("DPadX") + Input.GetAxis("ScrollMenu");
            value = Mathf.Clamp(value, -1, 1);
            NavigateMenu(new Vector2(value, 0));
        }
        if(Input.GetButton("NextButton"))
            NextSubMenu();
        if(Input.GetButton("PreviousButton"))
            PreviousSubMenu();

        if(!shouldHandleInput)
        {
            Invoke("EnableHandleInput", inputDelay);
        }
    }

    void EnableHandleInput()
    {
        shouldHandleInput = true;
    }

    public void Confirm()
    {
        m_uiStack.Peek().Confirm();
        shouldHandleInput = false;
    }
    public void Cancel()
    {
        m_uiStack.Peek().Cancel();
        shouldHandleInput = false;
    }
    public void OpenTowerMenu()
    {
        m_uiStack.Peek().OpenTowerMenu();
        shouldHandleInput = false;
    }
    public void OpenCharaterMenu()
    {
        m_uiStack.Peek().OpenCharaterMenu();
        shouldHandleInput = false;
    }
    public void NavigateMenu(Vector2 input)
    {
        m_uiStack.Peek().NavigateMenu(input);
        shouldHandleInput = false;
    }
    public void NextSubMenu()
    {
        m_uiStack.Peek().NextSubMenu();
        shouldHandleInput = false;
    }
    public void PreviousSubMenu()
    {
        m_uiStack.Peek().PreviousSubMenu();
        shouldHandleInput = false;
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