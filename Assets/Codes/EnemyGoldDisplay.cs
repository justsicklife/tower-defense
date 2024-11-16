using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGoldDisplay : MonoBehaviour
{
    public static EnemyGoldDisplay instance;

    private void Awake() {
        if(instance != null) {
            Debug.LogError("More than one EnemyGoldDisplay in scene!");
        }
        instance = this;
    }

    public GameObject goldTextPrefab;
    public Canvas goldCanvas;
    public void showGoldText(Vector3 worldPosition,int goldAmount) {
        GameObject goldText = Instantiate(goldTextPrefab,goldCanvas.transform);
        goldText.GetComponent<TextMeshProUGUI>().text = "+ $" + goldAmount.ToString(); 

        goldText.transform.position = worldPosition;

        Destroy(goldText,1.5f);
    
    }

}
