using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    Damage = 0,
    Range,
    Speed,
    Hitpoint
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

        TowerStats = new[] { 1, 1, 1, 1 };

        CreateTowerMesh();
        RescaleTower();

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
}
