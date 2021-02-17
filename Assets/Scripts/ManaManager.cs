using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaManager : MonoBehaviour
{

    GameObject gameManager;
    GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    public void IncreaseManaCost(bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            gameManagerScript.defaultPlayerManaCost++;
            gameManagerScript.playerManaCost = gameManagerScript.defaultPlayerManaCost;
        }
        else
        {
            gameManagerScript.defaultEnemyManaCost++;
            gameManagerScript.enemyManaCost = gameManagerScript.defaultEnemyManaCost;
        }

        gameManagerScript.displayPlayerManaCost.text = gameManagerScript.playerManaCost.ToString();
        gameManagerScript.displayEnemyManaCost.text = gameManagerScript.enemyManaCost.ToString();
    }

    public void ReduceManaCost(CardDisplay card)
    {

        if (card.playerCard)
        {
            gameManagerScript.playerManaCost -= card.initializeCardModel.cost;
            gameManagerScript.displayPlayerManaCost.text = gameManagerScript.playerManaCost.ToString();
        }
        else
        {
            gameManagerScript.enemyManaCost -= card.initializeCardModel.cost;
            gameManagerScript.displayEnemyManaCost.text = gameManagerScript.enemyManaCost.ToString();

        }

    }
}
