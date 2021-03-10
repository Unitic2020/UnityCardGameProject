using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedCard : MonoBehaviour, IDropHandler
{

    GameObject attack;
    Attack attackScript;

    void Start()
    {
        attack = GameObject.Find("Attack");
        attackScript = attack.GetComponent<Attack>();
    }

    public void OnDrop(PointerEventData eventData){
        CardDisplay attacker = eventData.pointerDrag.GetComponent<CardDisplay>();
        CardDisplay defender = GetComponent<CardDisplay>();

        attackScript.FightCard(attacker, defender);
    }
}

