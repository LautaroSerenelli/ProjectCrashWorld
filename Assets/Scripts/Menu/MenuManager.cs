using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuUI;

    public void Start()
    {
        if (menuUI != null && menuUI.activeSelf)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    public void Update()
    {
        if (menuUI != null && menuUI.activeSelf)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ToggleMenu()
    {
        if (menuUI != null)
        {
            bool isMenuActive = menuUI.activeSelf;
            menuUI.SetActive(!isMenuActive); 
        }
    }
}