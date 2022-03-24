using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private Transform target;
    private bool isLaunched = false;

    public void LaunchProjectile(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
        isLaunched = true;
    }

    private void Update()
    {
        if (!isLaunched)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, 12.5f * Time.deltaTime);
        transform.LookAt(target);

        if (Vector3.Distance(transform.position, target.position) < 1)
        {
            OnArrival();
        }
    }

    private void OnArrival()
    {
        target.GetComponent<Enemy>().TakeDamage(damage);
        Destroy(gameObject);
    }
}