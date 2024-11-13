using UnityEngine;

public class BuildManager : MonoBehaviour
{

    public static BuildManager instance;

    void Awake() {
        if(instance != null) {
            Debug.LogError("More than one BuildManager in scene!");
        }
        instance = this;
    }

    public GameObject buildEffect;
    public GameObject sellEffect;

    /// <summary>
    /// 선택된 터렛
    /// </summary>
    private TurretBlueprint turretToBuild;
    private Node selectedNode;

    public NodeUI nodeUI;

    // 터렛이 존재하는데 null 이라면 true 
    public bool CanBuild{ get {return turretToBuild != null;}}
    public bool HasMoney{ get {return PlayerStats.Money >= turretToBuild.cost ;}}

    /// <summary>
    /// 노드를 선택하면 이 메서드가 실행
    /// </summary>
    /// <param name="node">클릭한 노드</param>
    public void SelectNode(Node node) {

        // 선택된 노드를 다시 누르면
        if(selectedNode == node) {
            DeselectNode();
            return;
        }

        selectedNode = node;    
        turretToBuild = null;

        nodeUI.setTarget(node);
    }

    /// <summary>
    /// 선택된 노드 선택 해제
    /// </summary>
    public void DeselectNode() {
        selectedNode = null;
        nodeUI.Hide();
    }

    /// <summary>
    /// 상점에서 선택한 터렛이 들어간다
    /// </summary>
    public void SelectTurretToBuild(TurretBlueprint turret) {
        turretToBuild = turret; 
        DeselectNode();
    }

    public TurretBlueprint GetTurretToBuild() {
        return turretToBuild;
    }

}