using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] public CardDisplay cardPrefab;
    [SerializeField] Transform playerHand;
    [SerializeField] Transform enemyHand;
    [SerializeField] GameObject yourTurnPanel;
    [SerializeField] GameObject enemyTurnPanel;
    public static GameManager instance;
    public bool turn; //true = player, false = enemy 
    public List<int> playerSampleDeck = new List<int>() { 1, 1, 1, 1, 1, 1, 1 ,1,1,1,1,1,1,1,1,1,1};
    public List<int> enemySampleDeck = new List<int>() { 1, 1, 1, 1, 1, 1, 1 ,1,1,1,1,1,1,1,1,1,1};

    void Start() {
        for (int i = 0; i < 4; i++) {
            GiveOutCard(playerSampleDeck, playerHand);
            GiveOutCard(enemySampleDeck, enemyHand);
        }
        
        turn = true;
        
        if (turn) {
            StartCoroutine(PlayerTurn());
        } else {
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
        yield return new WaitForSeconds(3);
        SwitchTurn(turn);
    }

    /*ターンごとに時間制限設けるよ*/
    IEnumerator TimeSetting() {
        
        int time = 5;

        while(time >= 0) {
            yield return new WaitForSeconds(1);
            time -= 1;
        }
      
        SwitchTurn(turn);
    }

    public void SwitchTurn(bool isTurn) {

        isTurn = !isTurn;
        turn = isTurn;
        if (isTurn) {
            StartCoroutine(PlayerTurn());
        } else {
            StartCoroutine(EnemyTurn());
        }
    
    }

    /*カード配るよ*/
    public void GiveOutCard(List<int> deck, Transform hand) {
        int cardId = deck[0]; 
        CardDisplay card = Instantiate(cardPrefab, hand, false);
        card.Initialize(cardId);
        deck.RemoveAt(0);
    }
}
