using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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

    [SerializeField] private InputField inputField;
    [SerializeField] private Text inputText;

    private bool isDetail = false;
    private List<int> CList = new List<int>() { 1 ,2 ,3 ,4 ,5 ,6 ,7 ,8 ,9 ,10 ,11 ,12 ,13 ,14 ,15};//中身を絶対に変更しない
    private List<CardModel> referenceCard = new List<CardModel>();//idとかを参照する用
    private List<GameObject> saveCard = new List<GameObject>();//ゲームに表示する用

    void Start() {
       
        inputField = inputField.GetComponent<InputField>();
        inputText = inputText.GetComponent<Text>();

        CardList(CList);

        for (int i = 0; i < CList.Count; i++) {
            saveCard[i].GetComponent<AttackedCard>().enabled = false;
        }
    }

    public void InputText() {//文字が更新(入力・削除)されるたびに呼び出される

        inputText.text = inputField.text;

    }
   
    public void EnterInputText() {

        List<int> searchList = new List<int>();//検索したカードのidを格納するリスト

        Debug.Log(saveCard.Count);

        for (int i = 0; i < referenceCard.Count; i++) {//検索に引っ掛かったidを格納

            if (inputText.text == referenceCard[i].name) {
                if (!searchList.Contains(referenceCard[i].id)) {
                    Debug.Log("dainyu");
                    searchList.Add(referenceCard[i].id);
                }            
             }
        }
        
        /*
         * 動きがよくわかってなくて怪しい。でも動く
         */
        for (int i = 0; i < saveCard.Count; i++) {//現在表示されているカードの削除
            
            Destroy(saveCard[i]);//見た目を消してる(Listの中身は残っている)           
        }
        saveCard.Clear();//Listの中身を消してる


        if (inputText.text != "") {//検索に引っ掛かったカードのidをもとに生成

            Debug.Log("空白じゃないよ");

            for (int i = 0; i < searchList.Count; i++) {
               
                cardDisplayPrefab.Initialize(searchList[i]);

                GameObject card = Instantiate(cardPrefab, listPanel, false);

                card.transform.localScale = new Vector3(1, 2, 0);

                AddEventTrigger(card, searchList[i]);

                saveCard.Add(card);
            }

        } else {//検索欄に入力がなくなったら元に戻す

            Debug.Log("空白daよ");

            for (int i = 0; i < CList.Count; i++) {//検索に引っ掛かったカードのidをもとに生成
                cardDisplayPrefab.Initialize(CList[i]);

                GameObject card = Instantiate(cardPrefab, listPanel, false);

                card.transform.localScale = new Vector3(1, 2, 0);

                AddEventTrigger(card, CList[i]);

                saveCard.Add(card);
            }
        }

        inputField.text = "";
        inputText.text = "";

    }

    public void CardList(List<int> cardId) {

        for (int i = 0; i < cardId.Count; i++) {//表示用のカードの生成

            cardDisplayPrefab.Initialize(cardId[i]);

            GameObject card = Instantiate(cardPrefab, listPanel, false);

            card.transform.localScale = new Vector3(1, 2, 0);

            AddEventTrigger(card, cardId[i]);

            saveCard.Add(card);

        }

        for (int i = 0; i < CList.Count; i++) {//参照用カードの生成
            
            referenceCard.Add(new CardModel(CList[i]));//Listは代入じゃなくてAddだね。

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

    public void IsDetailPanel(int id) {//画像とテキストの読み込み
        
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

    /*
     * 現在は検索を完全一致で行っているため、検索中のソートが必要ない。
     * だから、検索中はソートできないようにするための応急処置。
     */

    public void sortId() {//配列の中のidを並べ替える

        if (CList.Count == saveCard.Count) {//応急処置


            referenceCard.Sort((a, b) => a.id - b.id);//Listのソート

            for (int i = 0; i < saveCard.Count; i++) {//現在表示されているカードの削除
                Destroy(saveCard[i]);
            }
            saveCard.Clear();//危険なdestroy clear

            for (int i = 0; i < CList.Count; i++) {//inListが動的に生成されるのでCountが使えない。idがかぶることはないのでClistと同じ数だしええやろ

                cardDisplayPrefab.Initialize(referenceCard[i].id);

                GameObject card = Instantiate(cardPrefab, listPanel, false);

                card.transform.localScale = new Vector3(1, 2, 0);

                AddEventTrigger(card, referenceCard[i].id);

                saveCard.Add(card);

            }
        }
    }

    public void sortHP() {//配列の中のhpを並べ替える
        
        if (CList.Count == saveCard.Count) {//応急処置

            referenceCard.Sort((a, b) => a.hp - b.hp);//Listのソート

            for (int i = 0; i < saveCard.Count; i++) {//現在表示されているカードの削除
                Destroy(saveCard[i]);
            }
            saveCard.Clear();//危険なdestroy clear

            for (int i = 0; i < CList.Count; i++) {//inListが動的に生成されるのでCountが使えない。idがかぶることはないのでClistと同じ数だしええやろ

                cardDisplayPrefab.Initialize(referenceCard[i].id);

                GameObject card = Instantiate(cardPrefab, listPanel, false);

                card.transform.localScale = new Vector3(1, 2, 0);

                AddEventTrigger(card, referenceCard[i].id);

                saveCard.Add(card);

            }
        }
       
    }

    public void sortAttack() {//配列の中のatを並べ替える

        if (CList.Count == saveCard.Count) {//応急処置

            referenceCard.Sort((a, b) => a.at - b.at);//Listのソート

            for (int i = 0; i < saveCard.Count; i++) {//現在表示されているカードの削除
                Destroy(saveCard[i]);
            }
            saveCard.Clear();//危険なdestroy clear

            for (int i = 0; i < CList.Count; i++) {//inListが動的に生成されるのでCountが使えない。idがかぶることはないのでClistと同じ数だしええやろ

                cardDisplayPrefab.Initialize(referenceCard[i].id);

                GameObject card = Instantiate(cardPrefab, listPanel, false);

                card.transform.localScale = new Vector3(1, 2, 0);

                AddEventTrigger(card, referenceCard[i].id);

                saveCard.Add(card);

            }
        }
    }

}
