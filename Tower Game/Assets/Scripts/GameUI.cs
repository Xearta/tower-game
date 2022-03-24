using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject[] panels;
    public Button[] buttons;

    private void Start()
    {
        NavigateTo(0);
    }

    public void NavigateTo(int menuIndex)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == menuIndex)
            {
                panels[i].SetActive(true);
                buttons[i].Select();
            }
            else
            {
                panels[i].SetActive(false);
            }
        }
    }
}
