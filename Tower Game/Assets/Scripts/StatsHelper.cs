using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    Damage = 0,
    Range,
    Speed,
    Hitpoint,
    Regen,
    CritChance,
    CritDamage,
    Luck
}

public class StatsHelper : MonoBehaviour
{
    #region Stats Const

    // Damage
    private const float BASE_DAMAGE = 1.0f; // 1 damage on every projectile

    // Range
    private const float BASE_RANGE = 7.5f; // 7.5m in radius
    private const float GAIN_RANGE = 0.25f;

    // Speed
    private const float BASE_SPEED = 1.5f; // 1.5 attacks per second
    private const float GAIN_SPEED = 0.025f;

    // Hitpoint
    private const float BASE_HITPOINT = 1.0f; // MaxHitpoint

    // Regen
    private const float BASE_REGEN = 1.0f; // 1 hitpoint every 5 seconds

    // Crit Chance
    private const float BASE_CRIT_CHANCE = 0.01f; // 1/100 crit
    private const float GAIN_CRIT_CHANCE = 0.01f;

    // Crit Damage
    private const float BASE_CRIT_DAMAGE = 50.0f; // +50% of damage
    private const float GAIN_CRIT_DAMAGE = 5.0f;

    // Luck
    private const float BASE_LUCK = 0.10f; // 10% chance for drop
    private const float GAIN_LUCK = 0.15f;

    #endregion

    public static StatsHelper Instance{ set; get; }

    private const int MAX_SKILL_LEVEL = 200;
    private const float LEVEL_VALUES_SCALE = 20.0f;
    private const float PRICE_VALUES_SCALE = 15.0f;
    private int[] priceValuesArray = new int[MAX_SKILL_LEVEL];
    private int[] levelValuesArray = new int[MAX_SKILL_LEVEL];

    public void Awake()
    {
        Instance = this;
        GenerateLevelValuesArray();
        GeneratePriceValuesArray();
    }

    private void GenerateLevelValuesArray()
    {
        float points = 0;
        for (int i = 0; i < MAX_SKILL_LEVEL; i++)
        {
            points += Mathf.Floor(i+1 * Mathf.Pow(2, i / LEVEL_VALUES_SCALE));
            levelValuesArray[i] = (int)Mathf.Floor(points / 4);
            Debug.Log("Level " + i.ToString() + " : " + levelValuesArray[i]);
        }
    }

     private void GeneratePriceValuesArray()
    {
        float points = 0;
        for (int i = 0; i < MAX_SKILL_LEVEL; i++)
        {
            points += Mathf.Floor(i+300 * Mathf.Pow(2, i / PRICE_VALUES_SCALE));
            priceValuesArray[i] = (int)Mathf.Floor(points / 4);
            Debug.Log("Level " + i.ToString() + " : " + priceValuesArray[i]);
        }
    }

    public int GetPrice(int levelIndex)
    {
        return priceValuesArray[levelIndex];
    }

    public float GetStatValue(Stat stat)
    {
        int level = TheTower.Instance.TowerStats[(int)stat];

        switch (stat)
        {
            case Stat.Damage:
                return BASE_DAMAGE + levelValuesArray[level];
            case Stat.Hitpoint:
                return BASE_HITPOINT + levelValuesArray[level];
            case Stat.Regen:
                return BASE_REGEN + levelValuesArray[level];
            case Stat.Range:
                return BASE_RANGE + level * GAIN_RANGE;
            case Stat.Speed:
                return BASE_SPEED + level * -GAIN_SPEED;
            case Stat.CritDamage:
                return BASE_CRIT_DAMAGE + level * GAIN_CRIT_DAMAGE;
            case Stat.CritChance:
                return BASE_CRIT_CHANCE + level * GAIN_CRIT_CHANCE;
            case Stat.Luck:
                return BASE_LUCK + level * GAIN_LUCK;
        }

        Debug.Log("Stat has no return value, code it in the GetStatValue function");
        return -1f;
    }

}
