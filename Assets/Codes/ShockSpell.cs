using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockSpell : MonoBehaviour, SpellCaster
{
    public float range = 5f;               // 감전 범위
    public float freezeDuration = 1f;     // 정지 지속 시간
    public LayerMask enemyLayer;          // 적을 감지할 레이어
    public GameObject rangeIndicatorPrefab; // 원 범위 표시 프리팹 (선택)
    public float damage = 5;

    public void SelectSpell()
    {
        SpellManager.instance.SellectSpell(this);
    }

    public void UseSpell()
    {
        CastFreezeSpell();
    }

    void CastFreezeSpell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 clickPosition = hit.point;

            ShowRangeIndicator(clickPosition);

            Collider[] enemies = Physics.OverlapSphere(clickPosition, range, enemyLayer);

            foreach (Collider enemy in enemies)
            {
                Freeze(enemy.GetComponent<Enemy>());
            }
        }
    }

    /// <summary>
    /// 범위를 시각 적으로 보여줌
    /// </summary>
    /// <param name="position"></param>
    void ShowRangeIndicator(Vector3 position)
    {

        if (rangeIndicatorPrefab != null)
        {
            position.y = 1.5f;
            Vector3 rotation =  new Vector3(90f,0,0);
            GameObject indicator =  Instantiate(rangeIndicatorPrefab,position,Quaternion.Euler(rotation));
            indicator.transform.localScale = new Vector3(range * 2,range* 2,1f);
            Destroy(indicator,freezeDuration);
        }
    }

    private void Freeze(Enemy enemy) {

        enemy.TakeDamage(damage);

        StartCoroutine(FreezeCoroutine(freezeDuration,enemy));

    }

    IEnumerator FreezeCoroutine(float duration,Enemy enemy) {
        enemy.speed = 0; 

        yield return new WaitForSeconds(duration);

        enemy.speed = enemy.startSpeed;
    }
}
