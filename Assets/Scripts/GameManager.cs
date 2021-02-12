using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] public Transform enemyField;
    [SerializeField] public Transform playerField;
    [SerializeField] public CardDisplay cardPrefab;
    public Transform playerHand;
    [SerializeField] public Transform enemyHand;
    [SerializeField] GameObject yourTurnPanel;
    [SerializeField] public GameObject enemyTurnPanel;
    [SerializeField] Text displayTimeCount;
    [SerializeField] Text displayPlayerHp;
    [SerializeField] public Text displayEnemyHp;
    [SerializeField] Text displayPlayerManaCost;
    [SerializeField] public Text displayEnemyManaCost;
    public Text displayNumberOfPlayerHandCard;
    [SerializeField] Text displayNumberOfPlayerCardInGraveyard;
    [SerializeField] public Text displayNumberOfEnemyHandCard;
    [SerializeField] Text displayNumberOfEnemyCardInGraveyard;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;

    public CardDisplay[] playerHandCardList;
    CardDisplay[] enemyHandCardList;
    public CardDisplay[] playerFieldCardList;

    private bool IsPressed = false;


    // HP初期値
    int playerHp = 10;
    public int enemyHp = 10;

    public static GameManager instance;
    public bool turn; //true = player, false = enemy 
    public List<int> playerSampleDeck = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    public List<int> enemySampleDeck = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

    int defaultPlayerManaCost = 0;
    int defaultEnemyManaCost = 0;
    // 実際のマナコスト
    public int playerManaCost;
    public int enemyManaCost;

    // 他クラスでもGameManagerのオブジェクトを参照できるように、GameManagerのオブジェクトを、staticで宣言。
    public static GameManager gameManagerObject;

    // プレイヤーターンを制御するクラス
    GameObject playerTurn;
    PlayerTurn playerTurnScript;

    GameObject enemyTurn;
    EnemyTurn enemyTurnScript;

    void Awake()
    {
        if (gameManagerObject == null)
        {
            gameManagerObject = this;
        }
    }

    void Start()
    {
        // PlayerTurnのGameObjectに属するScriptを読み込む
        playerTurn = GameObject.Find("PlayerTurn");
        playerTurnScript = playerTurn.GetComponent<PlayerTurn>();

        // EnemyTurnのGameObjectに属するScript読み込む
        enemyTurn = GameObject.Find("EnemyTurn");
        enemyTurnScript = enemyTurn.GetComponent<EnemyTurn>();

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
            StartCoroutine(GiveOutCard(playerSampleDeck, playerHand));
            StartCoroutine(GiveOutCard(enemySampleDeck, enemyHand));
        }

        turn = true;

        playerTurnScript.CreateCoroutineMethod(yourTurnPanel, playerFieldCardList, playerField, playerSampleDeck, playerHand);
        enemyTurnScript.CreateCoroutineMethod();

        if (turn)
        {
            defaultPlayerManaCost += 1;
            playerManaCost = defaultPlayerManaCost;
            displayPlayerManaCost.text = playerManaCost.ToString();
            playerTurnScript.RunPlayerTurnCoroutine();
        }
        else
        {
            defaultEnemyManaCost += 1;
            enemyManaCost = defaultEnemyManaCost;
            displayEnemyManaCost.text = enemyManaCost.ToString();
            enemyTurnScript.RunEnemyTurnCoroutine();
        }
    }

/*ターンごとに時間制限設けるよ*/
public IEnumerator TimeSetting()
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
            enemyTurnScript.StopEnemyTurnCoroutine();
            playerTurnScript.CreateCoroutineMethod(yourTurnPanel, playerFieldCardList, playerField, playerSampleDeck, playerHand);
            playerTurnScript.RunPlayerTurnCoroutine();
        }
        else
        {
            StopAllCoroutines();
            playerTurnScript.StopPlayerTurnCoroutine();
            enemyTurnScript.CreateCoroutineMethod();
            enemyTurnScript.RunEnemyTurnCoroutine();
        }
        IncreaseManaCost(turn);

    }

    /*カード配るよ*/
    public IEnumerator GiveOutCard(List<int> deck, Transform hand)
    {
        Debug.Log("deckの要素数" + deck.Count);
        if(deck.Count <= 0){
            yield return new WaitForSeconds(1);
            if(turn = true)
            {
                losePanel.SetActive(true);
                Debug.Log("Player Lose");
                StopAllCoroutines();
            }
            else
            {
                winPanel.SetActive(true);
                Debug.Log("Player Win!");
                StopAllCoroutines();
            }
            
        }
        int cardId = deck[0];
        CardDisplay card = Instantiate(cardPrefab, hand, false);
        card.Initialize(cardId);
        if (hand == playerHand)
        {
            card.playerCard = true;
            playerHandCardList = playerHand.GetComponentsInChildren<CardDisplay>();
            if(playerHandCardList.Length > 7)
            {
                yield return new WaitForSeconds(1);
                Destroy(card.gameObject);
            }
        }
        else
        {
            card.playerCard = false;
            enemyHandCardList = enemyHand.GetComponentsInChildren<CardDisplay>();
            if (enemyHandCardList.Length > 7)
            {
                yield return new WaitForSeconds(1);
                Destroy(card.gameObject);
            }        
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
            playerTurnScript.StopPlayerTurnCoroutine();
            enemyTurnScript.StopEnemyTurnCoroutine();
        }else if(this.enemyHp <=0) {
            winPanel.SetActive(true);
            Debug.Log("Player Win!");
            StopAllCoroutines();
            playerTurnScript.StopPlayerTurnCoroutine();
            enemyTurnScript.StopEnemyTurnCoroutine();
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