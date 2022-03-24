using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    public static TheTower Instance{ set; get; }
    public int[] TowerStats{ set; get; }
    public Material towerMat;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateTowerMesh();
        LoadLocal();
        RescaleTower();

        SceneManager.LoadScene("Menu");

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

    private void RescaleTower()
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
    
    private void OnApplicationQuit()
    {
        SaveLocal();
    }
}
