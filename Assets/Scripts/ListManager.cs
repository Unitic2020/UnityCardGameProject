using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor.Experimental.U2D;
using Unity.Collections;

public class ListManager : MonoBehaviour {
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
    public List<int> cardId = new List<int>() { 0, 1, 2, 3, 4 ,5 ,6 ,7 ,8 ,9 ,10 ,11 ,12 , 13};
    private bool isDetail = false; 

    void Start() {
       
        CardList(cardId);//この中で配列が書き換わっている

    }

    public void CardList(List<int> cardId) {

        //int t = cardId.Count;
        //Debug.Log(cardId.Count);
        //cardidの「５」からエラーを吐く、
        //Debug.Log(cardId.Count);
        // Debug.Log(t);
        // Debug.Log(t - 1);
      

        for (int i = 0; i < cardId.Count; i++) {
            Debug.Log(cardId[i]);
        }
            // cardId.Add(3);

            //t = cardId.Count;

            //Debug.Log(t);

            for (int i = 0; i < 12; i++) {



            //  Debug.Log(cardId[i]);

            //if (i == 4) {

            //  break;

            //}

            cardDisplayPrefab.Initialize(cardId[1]);

            GameObject card = Instantiate(cardPrefab, listPanel, false);

            card.transform.localScale = new Vector3(1, 2, 0);

            AddEventTrigger(card);

            

        }

    }

    public void AddEventTrigger(GameObject card) {
        card.AddComponent<EventTrigger>();
        EventTrigger trigger = card.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => IsDetailPanel());
        trigger.triggers.Add(entry);
    }

    public void IsDetailPanel() {

        //画像とテキストの読み込み
        cardModel = new CardModel(1);//test
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

}
