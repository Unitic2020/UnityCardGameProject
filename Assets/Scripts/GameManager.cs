using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] Transform enemyField;
    [SerializeField] Transform playerField;
    [SerializeField] public CardDisplay cardPrefab;
    public Transform playerHand;
    [SerializeField] Transform enemyHand;
    [SerializeField] GameObject yourTurnPanel;
    [SerializeField] GameObject enemyTurnPanel;
    [SerializeField] Text displayTimeCount;
    [SerializeField] Text displayPlayerHp;
    [SerializeField] Text displayEnemyHp;
    [SerializeField] Text displayPlayerManaCost;
    [SerializeField] Text displayEnemyManaCost;
    public Text displayNumberOfPlayerHandCard;
    [SerializeField] Text displayNumberOfPlayerCardInGraveyard;
    [SerializeField] Text displayNumberOfEnemyHandCard;
    [SerializeField] Text displayNumberOfEnemyCardInGraveyard;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;

    public CardDisplay[] playerHandCardList;
    CardDisplay[] enemyHandCardList;

    private bool IsPressed = false;


    // HP初期値
    int playerHp = 1;
    int enemyHp = 1;

    public static GameManager instance;
    public bool turn; //true = player, false = enemy 
    public List<int> playerSampleDeck = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    public List<int> enemySampleDeck = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

    int defaultPlayerManaCost = 0;
    int defaultEnemyManaCost = 0;
    // 実際のマナコスト
    public int playerManaCost;
    int enemyManaCost;

    // 他クラスでもGameManagerのオブジェクトを参照できるように、GameManagerのオブジェクトを、staticで宣言。
    public static GameManager gameManagerObject;

    void Awake()
    {
        if (gameManagerObject == null)
        {
            gameManagerObject = this;
        }
    }

    void Start()
    {

        // プレイヤーと敵のそれぞれの手札にあるカードの枚数を取得する
        playerHandCardList = playerHand.GetComponentsInChildren<CardDisplay>();
        enemyHandCardList = enemyHand.GetComponentsInChildren<CardDisplay>();


        displayPlayerHp.text = "HP: " + this.playerHp.ToString();
        displayEnemyHp.text = "HP: " + this.enemyHp.ToString();
        displayPlayerManaCost.text = defaultPlayerManaCost.ToString();
        displayEnemyManaCost.text = defaultEnemyManaCost.ToString();
        displayNumberOfPlayerHandCard.text = "x" + playerHandCardList.Length.ToString();
        displayNumberOfEnemyHandCard.text = "x" + enemyHandCardList.Length.ToString();
        displayNumberOfPlayerCardInGraveyard.text = "x" + 0.ToString();
        displayNumberOfEnemyCardInGraveyard.text = "x" + 0.ToString();

        for (int i = 0; i < 4; i++)
        {
            GiveOutCard(playerSampleDeck, playerHand);
            GiveOutCard(enemySampleDeck, enemyHand);
        }

        turn = true;

        if (turn)
        {
            defaultPlayerManaCost += 1;
            playerManaCost = defaultPlayerManaCost;
            displayPlayerManaCost.text = playerManaCost.ToString();
            StartCoroutine(PlayerTurn());
        }
        else
        {
            defaultEnemyManaCost += 1;
            enemyManaCost = defaultEnemyManaCost;
            displayEnemyManaCost.text = enemyManaCost.ToString();
            StartCoroutine(EnemyTurn());
        }
    }



    /*プレイヤーのターンだよ*/
    IEnumerator PlayerTurn()
    {

        Debug.Log("プレイヤーターン");
        yourTurnPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        yourTurnPanel.SetActive(false);
        CardDisplay[] playerFieldCardList = playerField.GetComponentsInChildren<CardDisplay>();
        for (int i =0; i < playerFieldCardList.Length; i++){
            playerFieldCardList[i].canAttack = true;
        }

        GiveOutCard(playerSampleDeck, playerHand);
        StartCoroutine(TimeSetting());
    }

    /*敵のターンだよ*/
    IEnumerator EnemyTurn() {

        Debug.Log("エネミーターン");
        enemyTurnPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        enemyTurnPanel.SetActive(false);
        StartCoroutine(TimeSetting());
        yield return new WaitForSeconds(2);
        CardDisplay[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardDisplay>();
        for(int i = 0;i < enemyFieldCardList.Length;i++) {
            enemyFieldCardList[i].canAttack = true;
        }
        yield return new WaitForSeconds(2);
        GiveOutCard(enemySampleDeck,enemyHand);


        // この辺に、敵がカードを場に出す処理を記述する
        CardDisplay[] enemyHandCardList = enemyHand.GetComponentsInChildren<CardDisplay>();

        // コスト以下のカードであれば、カードをフィールドに出し続ける
        while(Array.Exists(enemyHandCardList,card => card.initializeCardModel.cost <= enemyManaCost)) {
            // 条件に合うカードすべてを選択し、配列にぶっこむ
            CardDisplay[] selectableHandCardList = Array.FindAll(enemyHandCardList, card => card.initializeCardModel.cost <= enemyManaCost);

            // 今は、選択可能カードの配列先頭から順番に抽出することにする
            if(selectableHandCardList.Length > 0) {
                CardDisplay enemyCard = selectableHandCardList[0];
                enemyCard.transform.SetParent(enemyField);
                ReduceManaCost(enemyCard);
                enemyHandCardList = enemyHand.GetComponentsInChildren<CardDisplay>();
                this.displayNumberOfEnemyHandCard.text = "x" + enemyHandCardList.Length.ToString();
            } else {
                break;
            }
        }

        // 敵カードがplayerのカードに攻撃する
        enemyFieldCardList = enemyField.GetComponentsInChildren<CardDisplay>();


        // 攻撃可能カードの配列を取得
        CardDisplay[] enemyCanAttackCardList = Array.FindAll(enemyFieldCardList, card => card.canAttack);
        // 攻撃先（プレイヤーのフィールド）のカードを配列で取得
        CardDisplay[] playerFieldCardList = playerField.GetComponentsInChildren<CardDisplay>();
        Debug.Log("while前");
        yield return new WaitForSeconds(5);
        Debug.Log(enemyCanAttackCardList.Length);
        while(enemyCanAttackCardList.Length > 0 && playerFieldCardList.Length > 0) {
            Debug.Log("while中");
            CardDisplay attacker = enemyCanAttackCardList[0];
            // 攻撃先（プレイヤーのフィールド）にカードがいる場合は、攻撃する。
                Debug.Log("while中if中");
                // defenderカード（攻撃対象のカード）を選択
                CardDisplay defender = playerFieldCardList[0];
                FightCard(attacker,defender);
                yield return new WaitForSeconds(1);
            playerFieldCardList = playerField.GetComponentsInChildren<CardDisplay>();
            enemyFieldCardList = enemyField.GetComponentsInChildren<CardDisplay>();
            enemyCanAttackCardList = Array.FindAll(enemyFieldCardList,card => card.canAttack);
        }
        while(enemyCanAttackCardList.Length > 0){
            yield return new WaitForSeconds(1);
            this.AttackToHero(enemyCanAttackCardList[0], false);
            this.UpdateHpText();
            enemyCanAttackCardList = Array.FindAll(enemyFieldCardList, card => card.canAttack);
        }
        yield return new WaitForSeconds(1);
        SwitchTurn(turn);
    }

/*ターンごとに時間制限設けるよ*/
IEnumerator TimeSetting()
    {

        int time = 120;
        displayTimeCount.text = time.ToString();

        while (time >= 0)
        {
            yield return new WaitForSeconds(1);
            time -= 1;
            displayTimeCount.text = time.ToString();
        }

        SwitchTurn(turn);
    }

    public void SwitchTurn(bool isTurn)
    {

        turn = !isTurn;
        if (turn)
        {
            StopAllCoroutines();
            StartCoroutine(PlayerTurn());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(EnemyTurn());
        }
        IncreaseManaCost(turn);

    }

    /*カード配るよ*/
    public void GiveOutCard(List<int> deck, Transform hand)
    {
        Debug.Log("deckの要素数" + deck.Count);
        if(deck.Count <= 0){
            return;
        }
        int cardId = deck[0];
        CardDisplay card = Instantiate(cardPrefab, hand, false);
        card.Initialize(cardId);
        if (hand == playerHand)
        {
            card.playerCard = true;
        }
        else
        {
            card.playerCard = false;
        }
        deck.RemoveAt(0);

        playerHandCardList = playerHand.GetComponentsInChildren<CardDisplay>();
        enemyHandCardList = enemyHand.GetComponentsInChildren<CardDisplay>();

        displayNumberOfPlayerHandCard.text = "x" + playerHandCardList.Length.ToString();
        displayNumberOfEnemyHandCard.text = "x" + enemyHandCardList.Length.ToString();

    }

    public void IncreaseManaCost(bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            defaultPlayerManaCost++;
            playerManaCost = defaultPlayerManaCost;
        }
        else
        {
            defaultEnemyManaCost++;
            enemyManaCost = defaultEnemyManaCost;
        }

        displayPlayerManaCost.text = playerManaCost.ToString();
        displayEnemyManaCost.text = enemyManaCost.ToString();
    }

    public void ReduceManaCost(CardDisplay card)
    {

        if (card.playerCard)
        {
            playerManaCost -= card.initializeCardModel.cost;
            displayPlayerManaCost.text = playerManaCost.ToString();
        }
        else
        {
            enemyManaCost -= card.initializeCardModel.cost;
            displayEnemyManaCost.text = enemyManaCost.ToString();

        }

    }


    public void OnClickTurnEndButton()
    {
        if (!this.turn)
        {
            return;
        }
        SwitchTurn(this.turn);
    }

    public void FightCard(CardDisplay attacker, CardDisplay defender){
        // attackerがdefenderに攻撃する
        if (attacker.playerCard != defender.playerCard && attacker.canAttack)
        {
            defender.initializeCardModel.hp -= attacker.initializeCardModel.at;
            // 反撃
            attacker.initializeCardModel.hp -= defender.initializeCardModel.at;
            attacker.canAttack = false;
        }

        // hp が0を下回ったらカードを殺す
        if (attacker.initializeCardModel.hp <= 0)
        {
            attacker.initializeCardModel.isAlive = false;
        }
        if (defender.initializeCardModel.hp <= 0)
        {
            defender.initializeCardModel.isAlive = false;
        }
        attacker.CheckAlive();
        defender.CheckAlive();
    }

    public void UpdateHpText(){
        displayPlayerHp.text = "HP: " + this.playerHp.ToString();
        displayEnemyHp.text = "HP: " + this.enemyHp.ToString();
        displayEnemyHp.text = "HP: " + this.enemyHp.ToString();
    }

    public void CheckPlayerAlive(){
        if (this.playerHp <= 0){
            losePanel.SetActive(true);
            Debug.Log("Player Lose");
            StopAllCoroutines();
        }else if(this.enemyHp <=0) {
            winPanel.SetActive(true);
            Debug.Log("Player Win!");
            StopAllCoroutines();
        }
    }

    // プレイヤーへの攻撃
    public void AttackToHero(CardDisplay attacker, bool isPlayerCard){
        if (isPlayerCard) {
            this.enemyHp -= attacker.initializeCardModel.at;
        } else {
            this.playerHp -= attacker.initializeCardModel.at;
        }
        attacker.canAttack = false;
        this.UpdateHpText();
        this.CheckPlayerAlive();
    }
}