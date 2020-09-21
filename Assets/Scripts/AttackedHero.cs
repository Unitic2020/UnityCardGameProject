using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedHero : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("AttackedHero called");
        CardDisplay attacker = eventData.pointerDrag.GetComponent<CardDisplay>();
        if (attacker.canAttack){
            GameManager.gameManagerObject.AttackToHero(attacker, true);
        }
    }
}

