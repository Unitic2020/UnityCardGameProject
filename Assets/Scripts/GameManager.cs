using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField] public CardDisplay cardPrefab;
    [SerializeField] Transform playerHand;
    [SerializeField] Transform enemyHand;
    [SerializeField] GameObject yourTurnPanel;
    [SerializeField] GameObject enemyTurnPanel;
    [SerializeField] Text displayTimeCount;
    [SerializeField] Text displayPlayerHp;
    [SerializeField] Text displayEnemyHp;
    [SerializeField] Text displayPlayerManaCost;
    [SerializeField] Text displayEnemyManaCost;
    [SerializeField] Text displayNumberOfPlayerHandCard;
    [SerializeField] Text displayNumberOfPlayerCardInGraveyard;
    [SerializeField] Text displayNumberOfEnemyHandCard;
    [SerializeField] Text displayNumberOfEnemyCardInGraveyard;

    CardDisplay[] playerHandCardList;
    CardDisplay[] enemyHandCardList;

    public static GameManager instance;
    public bool turn; //true = player, false = enemy 
    public List<int> playerSampleDeck = new List<int>() { 1, 1, 1, 1, 1, 1, 1 ,1,1,1,1,1,1,1,1,1,1};
    public List<int> enemySampleDeck = new List<int>() { 1, 1, 1, 1, 1, 1, 1 ,1,1,1,1,1,1,1,1,1,1};

    int defaultPlayerManaCost = 0;
    int defaultEnemyManaCost = 0;
    void Start() {

        // プレイヤーと敵のそれぞれの手札にあるカードの枚数を取得する
        playerHandCardList = playerHand.GetComponentsInChildren<CardDisplay>();
        enemyHandCardList = enemyHand.GetComponentsInChildren<CardDisplay>();


        displayPlayerHp.text = 20.ToString();
        displayEnemyHp.text = 20.ToString();
        displayPlayerManaCost.text = defaultPlayerManaCost.ToString();
        displayEnemyManaCost.text = defaultEnemyManaCost.ToString();
        displayNumberOfPlayerHandCard.text = "x" + playerHandCardList.Length.ToString();
        displayNumberOfEnemyHandCard.text = "x" + enemyHandCardList.Length.ToString();
        displayNumberOfPlayerCardInGraveyard.text = "x" + 0.ToString();
        displayNumberOfEnemyCardInGraveyard.text = "x" + 0.ToString();

        for (int i = 0; i < 4; i++) {
            GiveOutCard(playerSampleDeck, playerHand);
            GiveOutCard(enemySampleDeck, enemyHand);
        }

        turn = true;
        
        if (turn) {
            defaultPlayerManaCost += 1;
            displayPlayerManaCost.text = defaultPlayerManaCost.ToString();
            StartCoroutine(PlayerTurn());
        } else {
            defaultEnemyManaCost += 1;
            displayEnemyManaCost.text = defaultEnemyManaCost.ToString();
            StartCoroutine(EnemyTurn());
        }
    }

    /*プレイヤーのターンだよ*/
    IEnumerator PlayerTurn() {

        Debug.Log("プレイヤーターン");
        yourTurnPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        yourTurnPanel.SetActive(false);
        GiveOutCard(playerSampleDeck, playerHand);
        StartCoroutine(TimeSetting());
    }

    /*敵のターンだよ*/
    IEnumerator EnemyTurn() {

        Debug.Log("エネミーターン");
        enemyTurnPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        enemyTurnPanel.SetActive(false);
        GiveOutCard(enemySampleDeck, enemyHand);
        StartCoroutine(TimeSetting());
    }

    /*ターンごとに時間制限設けるよ*/
    IEnumerator TimeSetting() {
        
        int time = 5;
        displayTimeCount.text = time.ToString();

        while(time >= 0) {
            yield return new WaitForSeconds(1);
            time -= 1;
            displayTimeCount.text = time.ToString();
        }
      
        SwitchTurn(turn);
    }

    public void SwitchTurn(bool isTurn) {

        turn = !isTurn;
        if (turn) {
            StartCoroutine(PlayerTurn());
        } else {
            StartCoroutine(EnemyTurn());
        }
        IncreaseManaCost(turn);
    
    }

    /*カード配るよ*/
    public void GiveOutCard(List<int> deck, Transform hand) {
        int cardId = deck[0]; 
        CardDisplay card = Instantiate(cardPrefab, hand, false);
        card.Initialize(cardId);
        deck.RemoveAt(0);

        playerHandCardList = playerHand.GetComponentsInChildren<CardDisplay>();
        enemyHandCardList = enemyHand.GetComponentsInChildren<CardDisplay>();

        displayNumberOfPlayerHandCard.text = "x" + playerHandCardList.Length.ToString();
        displayNumberOfEnemyHandCard.text = "x" + enemyHandCardList.Length.ToString();

    }

    public void IncreaseManaCost(bool isPlayerTurn){
        if (isPlayerTurn){
            defaultPlayerManaCost++;
        }else{
            defaultEnemyManaCost++;
        }

        displayPlayerManaCost.text = defaultPlayerManaCost.ToString();
        displayEnemyManaCost.text = defaultEnemyManaCost.ToString();
    }
}
