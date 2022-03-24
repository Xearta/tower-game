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

    // Healthbar fields
    public GameObject healthBar;
    public Text healthText;
    public Color32 hitpointHealthy;
    public Color32 hitpointDying;
    private Image healthImage;
    private RectTransform healthBarTransform;

    private void Start()
    {
        Instance = this;
        NavigateTo(0);
        UpdateCurrenciesText();

        healthBarTransform = healthBar.GetComponent<RectTransform>();
        healthImage = healthBar.GetComponent<Image>();

        UpdateHealthBar();
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

    public void UpdateHealthBar()
    {
        float currentHP = TheTower.Instance.Hitpoint;
        float maxHP = StatsHelper.Instance.GetStatValue(Stat.Hitpoint);
        float ratio = currentHP / maxHP;

        if (currentHP > 0)
        {
            healthImage.color = Color.Lerp(hitpointDying, hitpointHealthy, ratio);
            healthText.text = currentHP.ToString() + " / " + maxHP.ToString();
            healthBarTransform.localScale = new Vector3(ratio, 1, 1);
        }
        else
        {
            healthText.text = "DEAD";
            healthBarTransform.localScale = new Vector3(0, 1, 1);
        }

    }
}
