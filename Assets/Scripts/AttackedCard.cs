using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData){
        CardDisplay attacker = eventData.pointerDrag.GetComponent<CardDisplay>();
        CardDisplay defender = GetComponent<CardDisplay>();

        GameManager.gameManagerObject.FightCard(attacker, defender);
    }
}

