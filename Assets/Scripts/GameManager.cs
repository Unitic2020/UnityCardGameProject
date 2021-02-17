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
    [SerializeField] public GameObject yourTurnPanel;
    [SerializeField] public GameObject enemyTurnPanel;
    [SerializeField] public Text displayTimeCount;
    [SerializeField] public Text displayPlayerHp;
    [SerializeField] public Text displayEnemyHp;
    [SerializeField] public Text displayPlayerManaCost;
    [SerializeField] public Text displayEnemyManaCost;
    public Text displayNumberOfPlayerHandCard;
    [SerializeField] Text displayNumberOfPlayerCardInGraveyard;
    [SerializeField] public Text displayNumberOfEnemyHandCard;
    [SerializeField] Text displayNumberOfEnemyCardInGraveyard;
    [SerializeField] public GameObject winPanel;
    [SerializeField] public GameObject losePanel;

    public CardDisplay[] playerHandCardList;
    public CardDisplay[] enemyHandCardList;
    public CardDisplay[] playerFieldCardList;

    private bool IsPressed = false;


    // HP初期値
    public int playerHp = 10;
    public int enemyHp = 10;

    public static GameManager instance;
    public bool turn; //true = player, false = enemy 
    public List<int> playerSampleDeck = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    public List<int> enemySampleDeck = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

    public int defaultPlayerManaCost = 0;
    public int defaultEnemyManaCost = 0;
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

    GameObject checkBattleState;
    CheckBattleState checkBattleStateScript;

    GameObject manaManager;
    ManaManager manaManagerScript;

    GameObject cardManager;
    CardManager cardManagerScript;

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

        // CheckBattleStateのGameObjectに属するScriptを読み込む
        checkBattleState = GameObject.Find("CheckBattleState");
        checkBattleStateScript = checkBattleState.GetComponent<CheckBattleState>();

        // ManaManagerのGameObjectに属するScriptを読み込む
        manaManager = GameObject.Find("ManaManager");
        manaManagerScript = manaManager.GetComponent<ManaManager>();

        // プレイヤーと敵のそれぞれの手札にあるカードの枚数を取得する
        playerHandCardList = playerHand.GetComponentsInChildren<CardDisplay>();
        enemyHandCardList = enemyHand.GetComponentsInChildren<CardDisplay>();

        cardManager = GameObject.Find("CardManager");
        cardManagerScript = cardManager.GetComponent<CardManager>();


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
            StartCoroutine(cardManagerScript.GiveOutCard(playerSampleDeck, playerHand));
            StartCoroutine(cardManagerScript.GiveOutCard(enemySampleDeck, enemyHand));
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
    
}