using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LaserAttack : IAttackBehavior,IHasEffectControl
{

    public int damageOverTime;
    public float slowAmount;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    public LaserAttack(int damageOverTime, float slowAmount, LineRenderer lineRenderer, ParticleSystem impactEffect, Light impactLight)
    {
        this.damageOverTime = damageOverTime;
        this.slowAmount = slowAmount;
        this.lineRenderer = lineRenderer;
        this.impactEffect = impactEffect;
        this.impactLight = impactLight;
    }

    public void Attack(Transform target)
    {
        Enemy targetEnemy = target.GetComponent<Enemy>();

        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        if(!lineRenderer.enabled) {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
        lineRenderer.SetPosition(0,lineRenderer.transform.position);
        lineRenderer.SetPosition(1,target.position);

        Vector3 dir = lineRenderer.transform.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized * .5f;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    public void StopEffect()
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.enabled = false;
            impactEffect.Stop();
            impactLight.enabled = false;
        }
    }
}
