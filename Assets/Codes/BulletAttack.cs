using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : IAttackBehavior
{

    private GameObject bulletPrefab;
    private Transform firePoint;

    public BulletAttack(GameObject bulletPrefab, Transform firePoint)
    {
        this.bulletPrefab = bulletPrefab;
        this.firePoint = firePoint;
    }

    public void Attack(Transform target)
    {
        GameObject bulletGo = GameObject.Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGo.GetComponent<Bullet>();
        if(bullet != null) bullet.Seek(target);
    }
}
