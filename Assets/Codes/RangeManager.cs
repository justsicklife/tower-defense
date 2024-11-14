using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeManager : MonoBehaviour
{
    public static RangeManager instance;

    public GameObject TurretRange;

    public Vector3 positionOffset;

    void Awake() {
        if(instance != null) {
            Debug.LogError("More than one RangeManager in scene!");
        }
        instance = this;
    }

    public void ShowRange(Vector3 targetPosition,float targetRadius) {
        TurretRange.SetActive(true);
        TurretRange.transform.position = targetPosition + positionOffset;
        TurretRange.transform.localScale = new Vector3(targetRadius * 2,targetRadius*2,1);
    }

    public void hideRange() {
        TurretRange.SetActive(false);
    }
}
