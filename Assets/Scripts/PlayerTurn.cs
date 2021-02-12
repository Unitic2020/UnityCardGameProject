using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    GameObject gameManager;

    GameManager gameManagerScript;

    IEnumerator playerTurnCoroutine;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
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

        StartCoroutine(gameManagerScript.GiveOutCard(playerSampleDeck, playerHand));
        StartCoroutine(gameManagerScript.TimeSetting());
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
