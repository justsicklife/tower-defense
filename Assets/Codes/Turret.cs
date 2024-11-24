using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    
    private TargetingSystem targetingSystem;

    private IAttackBehavior attackBehavior;
    
    private Coroutine updateTargetCorutine;
    
    private Transform target;
    private Enemy targetEnemy;
    [Header("General")]
    public float range = 15f;
    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Use Laser")]
    public bool useLaser = false;

    public int damageOverTime = 30;
    public float slowAmount = .5f;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;

    public Transform firePoint;

    void Start()
    {
        if (useLaser)
        {
            attackBehavior = new LaserAttack(damageOverTime, slowAmount, lineRenderer, impactEffect, impactLight);
        }
        else
        {
            attackBehavior = new BulletAttack(bulletPrefab, firePoint);
        }
        
        targetingSystem = GetComponent<TargetingSystem>();
        updateTargetCorutine = StartCoroutine(UpdateTargetCoroutine(0.5f));
    }

    bool UpdateTarget()
    {
        target = targetingSystem.GetTarget(transform, range, "Enemy");
        if (target != null)
        {
            targetEnemy = target.GetComponent<Enemy>();
            return true;
        }

        return false;
    }

    private void OnDisable()
    {
        if (updateTargetCorutine != null)
        {
            StopCoroutine(updateTargetCorutine);
        };
    }

    /// <summary>
    /// 타겟이 없으면 이펙트 정지
    /// </summary>
    /// <param name="interval"></param>
    /// <returns></returns>
    private IEnumerator UpdateTargetCoroutine(float interval)
    {
        while (true)
        {
            bool hasTarget = UpdateTarget();
            // 타겟이 없고 attackBehavior 의 타입이 IHasEffectContrtol 이라면 
            if (!hasTarget && attackBehavior is IHasEffectControl effectControl)
            {
                // 이펙트 정지
                effectControl.StopEffect();
            }
            yield return new WaitForSeconds(interval);
        }
    }

    void Update() {
        // 타겟이 없고
        if(target == null) 
        {
            return;
        }
            
        LockOnTarget();

        if(useLaser) 
        {
            attackBehavior.Attack(target);
        } else 
        {
            // 발사 시간이 0보다 작거나 같으면
            if(fireCountdown <= 0f)  
            {
                // 총알 생성
                attackBehavior.Attack(target);
                fireCountdown = 1f / fireRate;
            }

            // 발사 시간 감소
            fireCountdown -= Time.deltaTime;
        }
    }

    void LockOnTarget() {   
        Vector3 dir = target.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(dir);

        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,lookRotation,Time.deltaTime * turnSpeed).eulerAngles;

        partToRotate.rotation = Quaternion.Euler(0f,rotation.y,0f);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,range);
    }
}
