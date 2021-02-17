using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{

    GameObject manaManager;
    ManaManager manaManagerScript;

    public enum TYPE
    {
        HAND,
        FIELD
    }
    public TYPE type;

    void Start()
    {
        manaManager = GameObject.Find("ManaManager");
        manaManagerScript = manaManager.GetComponent<ManaManager>();
    }

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
            GameManager.gameManagerObject.playerFieldCardList = GameManager.gameManagerObject.playerField.GetComponentsInChildren<CardDisplay>();
            if (GameManager.gameManagerObject.playerFieldCardList.Length <5)
            {
                Debug.Log("親が変われ！");
                card.defaultParent = this.transform;

                manaManagerScript.ReduceManaCost(card);
            }
          
        }
    }
}
