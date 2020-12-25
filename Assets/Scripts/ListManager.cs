using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor.Experimental.U2D;
using Unity.Collections;
using System;

public class ListManager : MonoBehaviour{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private CardDisplay cardDisplayPrefab;
    [SerializeField] private Image iconImage;
    [SerializeField] private Text attackText;
    [SerializeField] private Text hitPointText;
    [SerializeField] private Text costText;
    [SerializeField] private Text nameText;
    [SerializeField] private Text abilityText;
    [SerializeField] Transform listPanel;
    [SerializeField] GameObject detailPanel;

    CardModel cardModel;
   
    private bool isDetail = false;
    private List<int> CList = new List<int>() { 5,2,4,3,1};
    private List<GameObject> saveCard = new List<GameObject>();

    void Start() {
       
        CardList(CList);

       // Debug.Log(CList.Count);
        
        for (int i = 0; i < CList.Count; i++) {
       //     Debug.Log(CList[i]);
        }
    }

    public void CardList(List<int> cardId) {

        for (int i = 0; i < cardId.Count; i++) {

            cardDisplayPrefab.Initialize(cardId[i]);

            GameObject card = Instantiate(cardPrefab, listPanel, false);

            card.transform.localScale = new Vector3(1, 2, 0);

            AddEventTrigger(card, cardId[i]);

            saveCard.Add(card);

        }

    }

    public void AddEventTrigger(GameObject card, int id) {
        card.AddComponent<EventTrigger>();
        EventTrigger trigger = card.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => IsDetailPanel(id));
        trigger.triggers.Add(entry);
    }

    public void IsDetailPanel(int id) {
       //画像とテキストの読み込み
        cardModel = new CardModel(id);//test

        abilityText.text = "カード効果:\n\n  " + cardModel.ability;
        attackText.text = "攻撃力：" + cardModel.at.ToString();
        hitPointText.text = "体力：" + cardModel.hp.ToString();
        costText.text = "コスト：" + cardModel.cost.ToString();
        nameText.text = cardModel.name;
        iconImage.sprite = cardModel.icon;


        if (isDetail == false) {
            detailPanel.SetActive(true);
        }

        isDetail = true;
    }

    public void offDetailPanel() {
        if (isDetail == true) {
            detailPanel.SetActive(false);
        }

        isDetail = false;
    }

    public void sortId() {//配列の中のidを並べ替える
        CardModel[] inList = new CardModel[CList.Count];
        
        for(int i = 0;i < CList.Count; i++) {
            inList[i] = new CardModel(CList[i]);    
        }

        Array.Sort(inList, (a, b) => a.id - b.id);

        for (int i = 0; i < saveCard.Count; i++) {//現在表示されているカードの削除
            Destroy(saveCard[i]);
        }

        for (int i = 0; i < CList.Count; i++) {//inListが動的に生成されるのでCountが使えない。idがかぶることはないのでClistと同じ数だしええやろ

            cardDisplayPrefab.Initialize(inList[i].id);

            GameObject card = Instantiate(cardPrefab, listPanel, false);

            card.transform.localScale = new Vector3(1, 2, 0);

            AddEventTrigger(card, inList[i].id);

            saveCard.Add(card);

        }

        for (int i = 0; i < CList.Count; i++) {
            Debug.Log(inList[i].id);//consoleでソートできているのかの確認用
        }

    }

    public void sortHP() {//配列の中のhpを並べ替える
        CardModel[] inList = new CardModel[CList.Count];

        for (int i = 0; i < CList.Count; i++) {
            inList[i] = new CardModel(CList[i]);
        }

        Array.Sort(inList, (a, b) => a.hp - b.hp);

        for (int i = 0; i < saveCard.Count; i++) {//現在表示されているカードの削除
            Destroy(saveCard[i]);
        }

        for (int i = 0; i < CList.Count; i++) {//inListが動的に生成されるのでCountが使えない。idがかぶることはないのでClistと同じ数だしええやろ

            cardDisplayPrefab.Initialize(inList[i].id);

            GameObject card = Instantiate(cardPrefab, listPanel, false);

            card.transform.localScale = new Vector3(1, 2, 0);

            AddEventTrigger(card, inList[i].id);

            saveCard.Add(card);

        }

        for (int i = 0; i < CList.Count; i++) {
            Debug.Log(inList[i].hp);//consoleでソートできているのかの確認用
        }

    }

    public void sortAttack() {//配列の中のidを並べ替える
        CardModel[] inList = new CardModel[CList.Count];

        for (int i = 0; i < CList.Count; i++) {
            inList[i] = new CardModel(CList[i]);
        }

        Array.Sort(inList, (a, b) => a.at - b.at);

        for (int i = 0; i < saveCard.Count; i++) {//現在表示されているカードの削除
            Destroy(saveCard[i]);
        }

        for (int i = 0; i < CList.Count; i++) {//inListが動的に生成されるのでCountが使えない。idがかぶることはないのでClistと同じ数だしええやろ

            cardDisplayPrefab.Initialize(inList[i].id);

            GameObject card = Instantiate(cardPrefab, listPanel, false);

            card.transform.localScale = new Vector3(1, 2, 0);

            AddEventTrigger(card, inList[i].id);

            saveCard.Add(card);

        }

        for (int i = 0; i < CList.Count; i++) {
            Debug.Log(inList[i].at);//consoleでソートできているのかの確認用
        }

    }


}
