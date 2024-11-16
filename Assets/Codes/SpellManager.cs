using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellManager : MonoBehaviour
{
    public bool spellToUse;

    public float range = 5f;               // 감전 범위
    public float freezeDuration = 1f;     // 정지 지속 시간
    public LayerMask enemyLayer;          // 적을 감지할 레이어
    public GameObject rangeIndicatorPrefab; // 원 범위 표시 프리팹 (선택)

    public float damage = 5;

    public void SellectShockSpell()
    {
        spellToUse = true;
        Debug.Log("Sellect Spell");
    }

    private void Update()
    {

        if(EventSystem.current.IsPointerOverGameObject())
        return;

        if (spellToUse && Input.GetMouseButtonUp(0))
        {
            CastFreezeSpell();

        }
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
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.Freeze(freezeDuration,damage);
                }
            }
        }
    }

    void ShowRangeIndicator(Vector3 position)
    {

        if (rangeIndicatorPrefab != null)
        {
            Vector3 rotation =  new Vector3(90f,0,0);
            GameObject indicator =  Instantiate(rangeIndicatorPrefab,position,Quaternion.Euler(rotation));
            indicator.transform.localScale = new Vector3(range * 2,range* 2,1f);
            Destroy(indicator,freezeDuration);
        }
    }

}
