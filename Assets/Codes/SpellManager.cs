using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellManager : MonoBehaviour
{
    public static SpellManager instance;

    void Awake() {
        if(instance != null) {
            Debug.LogError("More than one SpellManager in scene!");
        }
        instance = this;
    }
    public SpellCaster spellCaster;

    public void SellectSpell(SpellCaster spellCaster)
    {
        BuildManager.instance.ClearSelection();
        this.spellCaster = spellCaster;
    }

    private void Update()
    {

        if(EventSystem.current.IsPointerOverGameObject())
            return;

        if (BuildManager.instance.GetTurretToBuild() != null)
            return;
        
        if (spellCaster != null && Input.GetMouseButtonUp(0))
        {     
            spellCaster.UseSpell();
        }
    }
}