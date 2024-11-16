using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 이거 쓰면 자동으로 컴포넌트 추가 됨
[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int wavepointIndex = 0;

    private Enemy enemy;

    void Start() {
        enemy = GetComponent<Enemy>();

        target = Waypoints.points[0];
    }

    void Update() {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized *enemy.speed *Time.deltaTime,Space.World);

        // 현재 위치와 타겟의 거리가 .4f 보다 작다면 
        if(Vector3.Distance(transform.position,target.position) <= .4f) {
            // 다음 point 로 타겟변경
            GetNextWaypoint();
        }

        // enemy.speed = enemy.startSpeed;

    }


    void GetNextWaypoint() {

        if(wavepointIndex >= Waypoints.points.Length-1){
            EndPath();
            return;
        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }

    void EndPath() {
        PlayerStats.Lives--;
        WaveSpawner.EnemiesAlive--;
        Destroy(this.gameObject);
    }
}
