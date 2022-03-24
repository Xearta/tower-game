using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { set; get; }

    public GameObject[] panels;
    public Button[] buttons;
    public Text[] currenciesText;

    private void Start()
    {
        Instance = this;
        NavigateTo(0);
        UpdateCurrenciesText();
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

    public void UpdateCurrenciesText()
    {
        currenciesText[0].text = TheTower.Instance.Currencies[(int)Currency.Gold].ToString();
        currenciesText[1].text = TheTower.Instance.Currencies[(int)Currency.MagicBrick].ToString();
        currenciesText[2].text = TheTower.Instance.Currencies[(int)Currency.Diamond].ToString();
    }
}
