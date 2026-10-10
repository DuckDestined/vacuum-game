using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuNavigator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> menus;

    [SerializeField]
    private GameObject _currentMenu;

    void Start()
    {
        
    }

    public void NavigateToMenu(int menuId)
    {
        _currentMenu.SetActive(false);
        _currentMenu = menus.ElementAt(menuId);

        if(_currentMenu == null)
        {
            throw new IndexOutOfRangeException("Now menu with this id.");
        }

        _currentMenu.SetActive(true);
    }
}
