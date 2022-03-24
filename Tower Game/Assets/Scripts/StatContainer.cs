using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatContainer : MonoBehaviour
{
    public Stat stat;

    private Text[] texts;

    private void Start()
    {
        texts = GetComponentsInChildren<Text>();
        GetComponentInChildren<Button>().onClick.AddListener(LevelUp);
        UpdateText();
    }

    private void UpdateText()
    {
        int statLevel = TheTower.Instance.TowerStats[(int)stat];

        texts[0].text = stat.ToString(); // Title
        texts[1].text = StatsHelper.Instance.GetPrice(statLevel) + " gold\nLevel Up"; 
        texts[2].text = "Lv." + statLevel;
        texts[3].text = StatsHelper.Instance.GetStatValue(stat).ToString();
    }

    private void LevelUp()
    {
        // if enough currency
        int price = StatsHelper.Instance.GetPrice(TheTower.Instance.TowerStats[(int)stat]);
        if (TheTower.Instance.Currencies[(int)Currency.Gold] >= price)
        {
            TheTower.Instance.Currencies[(int)Currency.Gold] -= price;
            GameUI.Instance.UpdateCurrenciesText();

            TheTower.Instance.TowerStats[(int)stat]++;
            UpdateText();

            if (stat == Stat.Hitpoint || stat == Stat.Range)
            {
                GameUI.Instance.UpdateHealthBar();
                TheTower.Instance.RescaleTower();
            }
        }
        else
        {
            Debug.Log("Not enough money :(");
        }

      

    }
}
