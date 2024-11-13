using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public int damage= 50;
    public float explosionRadius = 0f;

    public GameObject impactEffect; 

    public void Seek(Transform _target) {
        target = _target;
    }

    void Update()
    {
        if(target == null) 
        {
            Destroy(gameObject);
            return;
        }

        // 방향을 구함
        Vector3 dir = target.position - transform.position;

        // 이번 프레임에서 이동할 거리 계산
        float distanceThisFrame = speed * Time.deltaTime;

        // magnitude : 현재 발사체와 타겟간의 거리 
        if(dir.magnitude <= distanceThisFrame) {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame,Space.World);
        
        transform.LookAt(target);
    
    }

    void HitTarget() {
        GameObject effectIns = Instantiate(impactEffect,transform.position,transform.rotation);
        Destroy(effectIns,5f);

        if(explosionRadius > 0f) {
            Explode();
        } else {
            Damage(target);
        }

        Destroy(gameObject);
    
    }

    void Explode() 
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,explosionRadius);
    
        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Enemy") 
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if(e != null) 
        {
            e.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;   
        Gizmos.DrawWireSphere(transform.position,explosionRadius);
    }
}
