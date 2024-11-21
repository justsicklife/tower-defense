using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    [HideInInspector]
    public float speed;

    // public bool isFrozen = false;

    public float startHealth = 100;
    private float health;
    /// <summary>
    /// enemy 를 죽이면 획득하는 재화
    /// </summary>
    public int worth = 50;
    public GameObject deathEffect;

    [Header("Unity Stuff")]
    public Image healthBar;

    void Start() {
        speed = startSpeed;
        health = startHealth;
    }

    public void TakeDamage(float amount) {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if(health <= 0) 
        {
            Die();
        }
    }

    public void Slow(float pct) {
        speed = startSpeed * (1f - pct);
    }

    void Die() 
    {
        GameObject effect = Instantiate(deathEffect,transform.position,Quaternion.identity);
        Destroy(effect,5f);

        PlayerStats.Money += worth;

        WaveSpawner.EnemiesAlive--;

        Destroy(gameObject);
        EnemyGoldDisplay.instance.showGoldText(this.transform.position,worth);
    }

}
