using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    
    public GameObject ui;
    public TextMeshProUGUI upgradeCost;
    public Button upgradeButton;
    public TextMeshProUGUI sellAmount; 
    private Node target;

    /// <summary>
    /// 노드를 변경함과 동시에 NodeUI 텍스트도 선택한 노드에 따라 정보 변경
    /// </summary>
    /// <param name="_target">변경할 노드</param>
    public void setTarget(Node _target) {
        target = _target;
    
        transform.position = target.GetBuildPosition();
        
        if(!target.isUpgraded)
        {
           upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
           upgradeButton.interactable = true;
        } else {
            upgradeCost.text = "DONE";
           upgradeButton.interactable = false;
        }

        sellAmount.text = "$" + target.turretBlueprint.GetSellAmount();

        ui.SetActive(true);
    }

    public void Hide() {
        ui.SetActive(false);
    }

    public void Upgrade() {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Sell() {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }

}
