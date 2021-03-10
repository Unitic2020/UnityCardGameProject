using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBattleState : MonoBehaviour
{

    GameObject gameManager;
    GameManager gameManagerScript;

    GameObject playerTurn;
    PlayerTurn playerTurnScript;

    GameObject enemyTurn;
    EnemyTurn enemyTurnScript;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();

        // PlayerTurnのGameObjectに属するScriptを読み込む
        playerTurn = GameObject.Find("PlayerTurn");
        playerTurnScript = playerTurn.GetComponent<PlayerTurn>();

        // EnemyTurnのGameObjectに属するScript読み込む
        enemyTurn = GameObject.Find("EnemyTurn");
        enemyTurnScript = enemyTurn.GetComponent<EnemyTurn>();
    }
    public void CheckPlayerAlive()
    {
        if (gameManagerScript.playerHp <= 0)
        {
            gameManagerScript.losePanel.SetActive(true);
            Debug.Log("Player Lose");
            StopAllCoroutines();
            playerTurnScript.StopPlayerTurnCoroutine();
            enemyTurnScript.StopEnemyTurnCoroutine();
        }
        else if (gameManagerScript.enemyHp <= 0)
        {
            gameManagerScript.winPanel.SetActive(true);
            Debug.Log("Player Win!");
            StopAllCoroutines();
            playerTurnScript.StopPlayerTurnCoroutine();
            enemyTurnScript.StopEnemyTurnCoroutine();
        }
    }

}
