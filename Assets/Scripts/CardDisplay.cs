using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    public CardModel initializeCardModel;
    public int cardId;
    [SerializeField] Text nameText;
    [SerializeField] Text hpText;
    [SerializeField] Text atText;
    [SerializeField] Text costText;
    [SerializeField] Image iconImage;
    public Transform previousParent;    // ドラッグ前の親Transformを記録するメンバ
    public bool playerCard;
    public bool canMoveToField;
    public bool canAttack;

    public bool canDrag;


    public void Display(CardModel cardModel)
    {

        nameText.text = cardModel.name;
        hpText.text = cardModel.hp.ToString();
        atText.text = cardModel.at.ToString();
        costText.text = cardModel.cost.ToString();
        iconImage.sprite = cardModel.icon;
    }

    public void Initialize(int cardId)
    {

        initializeCardModel = new CardModel(cardId);
        Display(initializeCardModel);
    }


    // 以下の記述は、カードのドラッグアンドドロップに関する部分。
    public Transform defaultParent; // 親になる（カードが移動する先の）ゲームオブジェクト（FieldやHandが入る）

    // ドラッグ開始時の動作
    public void OnBeginDrag(PointerEventData eventData)
    {
        CardDisplay card = GetComponent<CardDisplay>();

        canDrag = true;

        if (!card.playerCard)
        {
            canDrag = false;
        }

        if ((card.initializeCardModel.cost > GameManager.gameManagerObject.playerManaCost) && transform.parent != GameManager.gameManagerObject.playerField)
        {
            canDrag = false;
        }

        if (!GameManager.gameManagerObject.turn)
        {
            canDrag = false;
        }

        if(!canAttack && transform.parent == GameManager.gameManagerObject.playerField)
        {
            canDrag = false;
        }

        if (!canDrag)
        {
            return;
        }
        previousParent = transform.parent;
        defaultParent = transform.parent;
        transform.SetParent(defaultParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // ドラッグ中の動作
    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }
        CardDisplay card = GetComponent<CardDisplay>();
        if (!card.playerCard)
        {
            return;
        }
        transform.position = eventData.position;
    }

    // ドラッグ終了時の動作
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }

        CardDisplay card = GetComponent<CardDisplay>();
        if (!card.playerCard)
        {
            return;
        }
        transform.SetParent(defaultParent, false);
        Debug.Log("value of defaultParent:" + defaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;


        // 手札のカード枚数を更新
        GameManager.gameManagerObject.playerHandCardList = GameManager.gameManagerObject.playerHand.GetComponentsInChildren<CardDisplay>();
        GameManager.gameManagerObject.displayNumberOfPlayerHandCard.text = "x" + GameManager.gameManagerObject.playerHandCardList.Length.ToString();

    }


    // カードが生きているか確認して、死んでる場合はカードを破壊
    public void CheckAlive()
    {
        if (initializeCardModel.isAlive){
            hpText.text = initializeCardModel.hp.ToString();
        }else{
            Destroy(this.gameObject);
        }
    }



    void Start()
    {
        defaultParent = transform.parent;
    }

}