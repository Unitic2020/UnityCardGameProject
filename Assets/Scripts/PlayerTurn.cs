using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    GameObject gameManager;
    GameManager gameManagerScript;

    GameObject gameSystem;
    GameSystem gameSystemScript;

    GameObject cardManager;
    CardManager cardManagerScript;

    IEnumerator playerTurnCoroutine;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();

        gameSystem = GameObject.Find("GameSystem");
        gameSystemScript = gameSystem.GetComponent<GameSystem>();

        cardManager = GameObject.Find("CardManager");
        cardManagerScript = cardManager.GetComponent<CardManager>();
    }

    public void CreateCoroutineMethod(GameObject yourTurnPanel, CardDisplay[] playerFieldCardList, Transform playerField, List<int> playerSampleDeck, Transform playerHand)
    {
        this.playerTurnCoroutine = this.Turn(yourTurnPanel, playerFieldCardList, playerField, playerSampleDeck, playerHand);
    }


    /*プレイヤーのターンだよ*/
    public IEnumerator Turn(GameObject yourTurnPanel, CardDisplay[] playerFieldCardList, Transform playerField, List<int> playerSampleDeck, Transform playerHand)
    {

        Debug.Log("プレイヤーターン");
        yourTurnPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        yourTurnPanel.SetActive(false);
        playerFieldCardList = playerField.GetComponentsInChildren<CardDisplay>();
        for (int i = 0; i < playerFieldCardList.Length; i++)
        {
            playerFieldCardList[i].canAttack = true;
        }

        StartCoroutine(cardManagerScript.GiveOutCard(playerSampleDeck, playerHand));
        StartCoroutine(gameSystemScript.TimeSetting());
    }

    public void RunPlayerTurnCoroutine()
    {
        if(playerTurnCoroutine == null)
        {
            Debug.Log("コルーチンないんだけど");
        }
        StartCoroutine(playerTurnCoroutine);
    }

    public void StopPlayerTurnCoroutine()
    {
        if (playerTurnCoroutine == null)
        {
            Debug.Log("コルーチンないんだけど");
        }
        StopAllCoroutines();
    }

}
