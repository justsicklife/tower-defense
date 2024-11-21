using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSelector : MonoBehaviour
{

    public GameObject shockSpell;

    public void SellectShockSpell() {
        shockSpell.GetComponent<SpellCaster>().SelectSpell();
    }
}
