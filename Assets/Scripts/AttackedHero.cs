using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedHero : MonoBehaviour, IDropHandler
{

    GameObject attack;
    Attack attackScript;

    void Start()
    {
        attack = GameObject.Find("Attack");
        attackScript = attack.GetComponent<Attack>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("AttackedHero called");
        CardDisplay attacker = eventData.pointerDrag.GetComponent<CardDisplay>();
        if (attacker.canAttack){
            attackScript.AttackToHero(attacker, true);
        }
    }
}

