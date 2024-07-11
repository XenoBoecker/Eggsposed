using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] List<Transform> buttons = new List<Transform>();

    MainMenu mainMenu;

    int currentButtonIndex;

    private void Start()
    {
        mainMenu = FindObjectOfType<MainMenu>();

        mainMenu.OnInputConfirm += Confirm;
        mainMenu.OnInputLeft += Left;
        mainMenu.OnInputRight += Right;
        
    }

    private void Confirm()
    {
        throw new NotImplementedException();
    }

    private void Left()
    {
        throw new NotImplementedException();
    }

    private void Right()
    {
        throw new NotImplementedException();
    }
}