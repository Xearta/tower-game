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
        //! if enough currency
        TheTower.Instance.TowerStats[(int)stat]++;
        UpdateText();

        if (stat == Stat.Hitpoint || stat == Stat.Range)
            TheTower.Instance.RescaleTower();

    }
}
