using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;

    public Color notEnoughMoneyColor;
    public Vector3 positionOffset;
    /// <summary>
    /// 배치된 터렛 정보
    /// </summary>
    [HideInInspector]
    public GameObject turret;
    /// <summary>
    /// 배치된 터렛의 청사진 
    /// </summary>
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded = false;
    private Renderer rend;
    private Color startColor;

    BuildManager buildManager;

    RangeManager rangeManager;

    void Start() {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    
        buildManager = BuildManager.instance;

        rangeManager = RangeManager.instance;
    }

    /// <summary>
    /// 터렛이 배치될 위치
    /// </summary>
    /// <returns>노드위치 + offset</returns>
    public Vector3 GetBuildPosition () {
        return transform.position + positionOffset;
    }

    void OnMouseDown() {
        
        // 포인터가 UI 에 있는 경우 true 반대는 false
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        // 선택된 터렛이 있다면
        if(turret != null) 
        {
            // buildManager 에서 node 선택
            buildManager.SelectNode(this);
            return;
        }

        // buildManager 에서 선택된 터렛이 없다면 true
        if(!buildManager.CanBuild)
            return;

        // BuildManager 에 있는 터렛 청사진을 가져옴
        BuildTurret(buildManager.GetTurretToBuild());
    }

    /// <summary>
    /// 터렛 생성 메서드 조건 (건설하려는 터렛을 값보다 내 돈이 많다면 조건 통과)
    /// </summary>
    void BuildTurret(TurretBlueprint blueprint) {

        if(PlayerStats.Money < blueprint.cost) {
            Debug.Log("Not enough money to build that!");
            return;
        }

        PlayerStats.Money -= blueprint.cost;

        GameObject _turret = Instantiate(blueprint.prefab,GetBuildPosition(),Quaternion.identity);
        turret = _turret;

        turretBlueprint = blueprint;


        GameObject effect = Instantiate(buildManager.buildEffect,GetBuildPosition() + positionOffset,Quaternion.identity);
        Destroy(effect,5f);


        Debug.Log("Turret build! Money left " + PlayerStats.Money);
    }

    public void UpgradeTurret() {
        if(PlayerStats.Money < turretBlueprint.upgradeCost) {
            Debug.Log("Not enough money to upgrade that!");
            return;
        }

        PlayerStats.Money -= turretBlueprint.upgradeCost;

        // Get rid of the old turret
        Destroy(turret);

        // Build a new one
        GameObject _turret = Instantiate(turretBlueprint.upgradedPrefab,GetBuildPosition(),Quaternion.identity);
        turret = _turret;

        GameObject effect = Instantiate(buildManager.buildEffect,GetBuildPosition() + positionOffset,Quaternion.identity);
        Destroy(effect,5f);

        isUpgraded = true;

        Debug.Log("Turret upgraded!");
    }

    public void SellTurret() {
        PlayerStats.Money += turretBlueprint.GetSellAmount();

        GameObject effect = Instantiate(buildManager.sellEffect,GetBuildPosition() + positionOffset,Quaternion.identity);
        Destroy(effect,5f);     

        Destroy(turret);
        turretBlueprint = null;
    }

    void OnMouseEnter() 
    {
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        if(turret != null) {
            rangeManager.ShowRange(this.transform.position,turret.GetComponent<Turret>().range);            
        }

        if(!buildManager.CanBuild)
            return;

        if(buildManager.HasMoney){
            rend.material.color = hoverColor;   
        } else 
        {
            rend.material.color = notEnoughMoneyColor;
        }
            
    }

    void OnMouseExit() {
        rend.material.color = Color.white;
        rangeManager.hideRange();
    }
}
