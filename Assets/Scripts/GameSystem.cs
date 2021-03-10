using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{

    GameObject gameManager;
    GameManager gameManagerScript;

    GameObject playerTurn;
    PlayerTurn playerTurnScript;

    GameObject enemyTurn;
    EnemyTurn enemyTurnScript;

    GameObject manaManager;
    ManaManager manaManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();

        playerTurn = GameObject.Find("PlayerTurn");
        playerTurnScript = playerTurn.GetComponent<PlayerTurn>();

        enemyTurn = GameObject.Find("EnemyTurn");
        enemyTurnScript = enemyTurn.GetComponent<EnemyTurn>();

        manaManager = GameObject.Find("ManaManager");
        manaManagerScript = manaManager.GetComponent<ManaManager>();
    }

    public void UpdateHpText()
    {
        gameManagerScript.displayPlayerHp.text = "HP: " + gameManagerScript.playerHp.ToString();
        gameManagerScript.displayEnemyHp.text = "HP: " + gameManagerScript.enemyHp.ToString();
    }

    /*ターンごとに時間制限設けるよ*/
    public IEnumerator TimeSetting()
    {

        int time = 120;
        gameManagerScript.displayTimeCount.text = time.ToString();

        while (time >= 0)
        {
            yield return new WaitForSeconds(1);
            time -= 1;
            gameManagerScript.displayTimeCount.text = time.ToString();
        }

        SwitchTurn(gameManagerScript.turn);
    }

    public void SwitchTurn(bool isTurn)
    {

        gameManagerScript.turn = !isTurn;
        if (gameManagerScript.turn)
        {
            StopAllCoroutines();
            enemyTurnScript.StopEnemyTurnCoroutine();
            playerTurnScript.CreateCoroutineMethod(gameManagerScript.yourTurnPanel, gameManagerScript.playerFieldCardList, gameManagerScript.playerField, gameManagerScript.playerSampleDeck, gameManagerScript.playerHand);
            playerTurnScript.RunPlayerTurnCoroutine();
        }
        else
        {
            StopAllCoroutines();
            playerTurnScript.StopPlayerTurnCoroutine();
            enemyTurnScript.CreateCoroutineMethod();
            enemyTurnScript.RunEnemyTurnCoroutine();
        }
        manaManagerScript.IncreaseManaCost(gameManagerScript.turn);

    }

    public void OnClickTurnEndButton()
    {
        if (!gameManagerScript.turn)
        {
            return;
        }
        SwitchTurn(gameManagerScript.turn);
    }
}
