using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
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

    void Start() {
        InvokeRepeating("UpdateTarget",0f,0.5f);
    }

    void UpdateTarget() {
        // 태그로 적 오브젝트 목록을 가져옴
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        // 가장 짧은 적과의 거리 
        float shortestDistance = Mathf.Infinity;
        // 가장 짧은 적의 게임 오브젝트를 담을 변수
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            // 적 오브젝트 하나 하나 거리를 구함
            float distanceToEnemy =  Vector3.Distance(transform.position,enemy.transform.position);
            // 가장 짧은 거리보다 작다면 
            if(distanceToEnemy < shortestDistance) {
                // 거리 최솟값에 가장 짧은 거리 대입
                shortestDistance = distanceToEnemy;
                // 가장 짧은 적 게임 오브젝트를 대입
                nearestEnemy = enemy;
            }
        }

        // 가장 가까운 적 오브젝트가 있고
        // 가장 짧은 거리가 range 보다 크거나 같다면 
        if(nearestEnemy != null && shortestDistance <= range) {
            // 가장 짧은 적의 transform 을 대입
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        } else {
            target = null;
        }
    }

    void Update() {
        // 타겟이 없고
        if(target == null) 
        {
            // 레이저 터렛 이라면
            if(useLaser) 
            {
                // 라인 렌더러가 활성화되있다면
                if(lineRenderer.enabled)
                {
                    // 다 비활성화
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }

            return;
        }
            
        LockOnTarget();

        if(useLaser) 
        {
            Laser();
        } else 
        {
            // 발사 시간이 0보다 작거나 같으면
            if(fireCountdown <= 0f)  
            {
                Shoot();
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

    void Laser() {

        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        if(!lineRenderer.enabled) {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
        lineRenderer.SetPosition(0,firePoint.position);
        lineRenderer.SetPosition(1,target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized * .5f;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);

    }

    void Shoot()
    {
        // 총알 생성
        GameObject bulletGo = Instantiate(bulletPrefab,firePoint.position,firePoint.rotation);    
        Bullet bullet = bulletGo.GetComponent<Bullet>();

        if(bullet != null) 
            bullet.Seek(target);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,range);
    }
}
