﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{
    public enum TYPE
    {
        HAND,
        FIELD
    }
    public TYPE type;
    public void OnDrop(PointerEventData eventData)
    {


        if (type == TYPE.HAND)
        {
            return;
        }
        CardDisplay card = eventData.pointerDrag.GetComponent<CardDisplay>();

        if (!card.playerCard)
        {
            return;
        }

        //落としてくるカードがないときは処理中断する
        // 落としてきたカードの親を、このスクリプトがアタッチされている
        // GameObjectにしてやる。
        if (card.canDrag)
        {
            Debug.Log("親が変われ！");
            card.defaultParent = this.transform;

            GameManager.gameManagerObject.ReduceManaCost(card);
        }
    }
}
