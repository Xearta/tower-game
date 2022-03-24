using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheTower : MonoBehaviour
{
    // Constants
    // Globals
    private const float BASE_TOWER_HEIGHT = 1.5f;
    private const float BASE_TOWER_WIDTH = 1.0f;

    // Range
    private const float RANGE_HEIGHT_GAIN = 0.05f;

    // Hitpoint
    private const float HITPOINT_WIDTH_GAIN = 0.025f;

    // Regen
    private const float REGEN_TICK = 0.5f;


    public static TheTower Instance{ set; get; }
    public int[] TowerStats{ set; get; }
    public int[] Currencies { set; get; }
    public float Hitpoint{ set; get; }
    public Material towerMat;
    public GameObject projectilePrefab;

    private bool isInGame = false;
    private float lastAttack;
    private float lastRegen;
    private GameObject recapMenu;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateTowerMesh();
        LoadLocal();
        RescaleTower();

        Hitpoint = StatsHelper.Instance.GetStatValue(Stat.Hitpoint);

        SceneManager.LoadScene("Menu");
    }

    private void Update()
    {
        if (isInGame)
        {
            RegenerateHitpoint();

            // look for enemies
            if (Time.time - lastAttack > StatsHelper.Instance.GetStatValue(Stat.Speed))
            {
                Collider[] col = Physics.OverlapSphere(transform.position, StatsHelper.Instance.GetStatValue(Stat.Range), LayerMask.GetMask("Enemy"));
                if (col.Length != 0)
                {
                    // find closest one
                    int closestIndex = 0;
                    float dist = Vector3.SqrMagnitude(col[closestIndex].transform.position - transform.position);
                    for (int i = 1; i < col.Length; i++)
                    {
                        float newDistance = Vector3.SqrMagnitude(col[i].transform.position - transform.position);
                        if (newDistance < dist)
                        {
                            dist = newDistance;
                            closestIndex = 1;
                        }
                    }

                    // shoot the closest enemy
                    ShootEnemy(col[closestIndex].transform);
                    lastAttack = Time.time;

                }
            }
        }
    }

    private void ShootEnemy(Transform target)
    {
        Vector3 projectileSpawnPoint = target.position.normalized;
        projectileSpawnPoint.y = GetTowerHeight();

        GameObject go = Instantiate(projectilePrefab, projectileSpawnPoint, Quaternion.identity) as GameObject;
        Projectile p = go.GetComponent<Projectile>();

        //! Calculate crit damage

        p.LaunchProjectile(target, StatsHelper.Instance.GetStatValue(Stat.Damage));
    }

    public void TakeDamage(float amount)
    {
        Hitpoint -= amount;
        GameUI.Instance.UpdateHealthBar();

        if (Hitpoint < 0)
        {
            recapMenu.SetActive(true);
        }
    }

    private void RegenerateHitpoint()
    {
        if (Time.time - lastRegen > REGEN_TICK)
        {
            Hitpoint += (StatsHelper.Instance.GetStatValue(Stat.Regen) / 5) * REGEN_TICK;
            if (Hitpoint > StatsHelper.Instance.GetStatValue(Stat.Hitpoint))
                Hitpoint = StatsHelper.Instance.GetStatValue(Stat.Hitpoint);

            GameUI.Instance.UpdateHealthBar();
            lastRegen = Time.time;

        }
    }

    private void OnLevelWasLoaded(int levelIndex)
    {
        isInGame = SceneManager.GetActiveScene().name == "Game";
        if (isInGame)
        {
            recapMenu = GameObject.Find("RecapMenu");
            recapMenu.SetActive(false);
        }
    }

    private void CreateTowerMesh()
    {
        float w = 0.5f;
        Vector3[] verts = new Vector3[]
        {
            new Vector3(-w, 0, -w),
            new Vector3(w, 0, -w),
            new Vector3(w, 0, w),
            new Vector3(-w, 0, w),

            new Vector3(-w, w * 2, -w),
            new Vector3(w, w * 2, -w),
            new Vector3(w, w * 2, w),
            new Vector3(-w, w * 2, w),
        };

        int[] tris = new int[]
        {
            0, 4, 1,  // Front
            4, 5, 1, 
            1, 5, 2,  // Right
            5, 6, 2, 
            2, 6, 3,  // Back
            6, 7, 3, 
            3, 7, 0,  // Left
            7, 4, 0, 
            4, 7, 5,  // Top
            7, 6, 5
        };

        Mesh m = new Mesh();
        m.vertices = verts;
        m.triangles = tris;
        m.RecalculateBounds();
        m.RecalculateNormals();

        gameObject.AddComponent<MeshFilter>().mesh = m;
        gameObject.AddComponent<MeshRenderer>().material = towerMat;
    }

    public void RescaleTower()
    {
        Vector3 newScale = Vector3.zero;
        newScale.x = BASE_TOWER_WIDTH + TowerStats[(int)Stat.Hitpoint] * HITPOINT_WIDTH_GAIN;
        newScale.z = newScale.x;
        newScale.y = BASE_TOWER_HEIGHT + TowerStats[(int)Stat.Range] * RANGE_HEIGHT_GAIN;
        transform.localScale = newScale;
    }

    private void LoadLocal()
    {
        TowerStats = new int[Enum.GetNames(typeof(Stat)).Length];

        TowerStats[(int)Stat.Damage] = PlayerPrefs.GetInt("StatDamage");
        TowerStats[(int)Stat.Hitpoint] = PlayerPrefs.GetInt("StatHitpoint");
        TowerStats[(int)Stat.Range] = PlayerPrefs.GetInt("StatRange");
        TowerStats[(int)Stat.Speed] = PlayerPrefs.GetInt("StatSpeed");
        TowerStats[(int)Stat.Regen] = PlayerPrefs.GetInt("StatRegen");
        TowerStats[(int)Stat.CritChance] = PlayerPrefs.GetInt("StatCritChance");
        TowerStats[(int)Stat.CritDamage] = PlayerPrefs.GetInt("StatCritDamage");
        TowerStats[(int)Stat.Luck] = PlayerPrefs.GetInt("Luck");

        // if it is the first time, set all null values on level 1
        for (int i = 0; i < TowerStats.Length; i++)
            if (TowerStats[i] == 0)
                TowerStats[i] = 1;
        
        Currencies = new int[Enum.GetNames(typeof(Currency)).Length];

        Currencies[(int)Currency.Gold] = PlayerPrefs.GetInt("CurrencyGold");
        Currencies[(int)Currency.MagicBrick] = PlayerPrefs.GetInt("CurrencyMagicBrick");
        Currencies[(int)Currency.Diamond] = PlayerPrefs.GetInt("CurrencyDiamond");
    }

    private void LoadCloud()
    {

    }

    private void SaveLocal()
    {
        PlayerPrefs.SetInt("StatDamage", TowerStats[(int)Stat.Damage]);
        PlayerPrefs.SetInt("StatHitpoint", TowerStats[(int)Stat.Hitpoint]);
        PlayerPrefs.SetInt("StatRange", TowerStats[(int)Stat.Range]);
        PlayerPrefs.SetInt("StatSpeed", TowerStats[(int)Stat.Speed]);
        PlayerPrefs.SetInt("StatRegen", TowerStats[(int)Stat.Regen]);
        PlayerPrefs.SetInt("StatCritChance", TowerStats[(int)Stat.CritChance]);
        PlayerPrefs.SetInt("StatCritDamage", TowerStats[(int)Stat.CritDamage]);
        PlayerPrefs.SetInt("StatLuck", TowerStats[(int)Stat.Luck]);

        PlayerPrefs.SetInt("CurrencyGold", Currencies[(int)Currency.Gold]);
        PlayerPrefs.SetInt("CurrencyMagicBrick", Currencies[(int)Currency.MagicBrick]);
        PlayerPrefs.SetInt("CurrencyDiamond", Currencies[(int)Currency.Diamond]);
    }

    private void SaveCloud()
    {

    }
    
    public float GetTowerHeight()
    {
        return BASE_TOWER_HEIGHT + TowerStats[(int)Stat.Range] * RANGE_HEIGHT_GAIN;
    }

    public float GetTowerWidth()
    {
        return BASE_TOWER_WIDTH + TowerStats[(int)Stat.Hitpoint] * HITPOINT_WIDTH_GAIN;
    }
    
    public void ReloadSaveData()
    {
        LoadLocal();
    }

    private void OnApplicationQuit()
    {
        SaveLocal();
    }
}
