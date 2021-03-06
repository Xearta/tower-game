using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Tiny = 1,
    Fast = 2,
    Tough = 3,
}

public class Enemy : MonoBehaviour
{
    public EnemyType type;
    public float hitpoint = 1;
    public float attackPerSecond = 1.5f;
    public float damage = 1.0f;
    public float timeToTower = 7.5f;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private float transition;
    private float lastHit;
    private bool isAlive = false;

    public void LaunchEnemy()
    {
        startPosition = transform.position;
        endPosition = startPosition.normalized * TheTower.Instance.GetTowerWidth();
        gameObject.SetActive(true);
        isAlive = true;
    }

    public void RemoveEnemy()
    {
        gameObject.SetActive(false);
        isAlive = false;
    }

    public void TakeDamage(float amount)
    {
        hitpoint -= amount;
        if (hitpoint <= 0)
            RemoveEnemy();
    }

    private void Update()
    {
        if (isAlive)
        {
            transition += Time.deltaTime * 1/timeToTower;
            transform.position = Vector3.Lerp(startPosition, endPosition, transition);

            if (transition > 1)
            {
                if(Time.time - lastHit > 1 / attackPerSecond)
                {
                    lastHit = Time.time;
                    TheTower.Instance.TakeDamage(damage);
                }
            }
        }
    }
}
