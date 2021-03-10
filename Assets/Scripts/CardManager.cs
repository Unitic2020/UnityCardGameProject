using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    GameObject gameManager;
    GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    /*カード配るよ*/
    public IEnumerator GiveOutCard(List<int> deck, Transform hand)
    {
        Debug.Log("deckの要素数" + deck.Count);
        if (deck.Count <= 0)
        {
            yield return new WaitForSeconds(1);
            if (gameManagerScript.turn = true)
            {
                gameManagerScript.losePanel.SetActive(true);
                Debug.Log("Player Lose");
                StopAllCoroutines();
            }
            else
            {
                gameManagerScript.winPanel.SetActive(true);
                Debug.Log("Player Win!");
                StopAllCoroutines();
            }

        }
        int cardId = deck[0];
        CardDisplay card = Instantiate(gameManagerScript.cardPrefab, hand, false);
        card.Initialize(cardId);
        if (hand == gameManagerScript.playerHand)
        {
            card.playerCard = true;
            gameManagerScript.playerHandCardList = gameManagerScript.playerHand.GetComponentsInChildren<CardDisplay>();
            if (gameManagerScript.playerHandCardList.Length > 7)
            {
                yield return new WaitForSeconds(1);
                Destroy(card.gameObject);
            }
        }
        else
        {
            card.playerCard = false;
            gameManagerScript.enemyHandCardList = gameManagerScript.enemyHand.GetComponentsInChildren<CardDisplay>();
            if (gameManagerScript.enemyHandCardList.Length > 7)
            {
                yield return new WaitForSeconds(1);
                Destroy(card.gameObject);
            }
        }
        deck.RemoveAt(0);

        gameManagerScript.playerHandCardList = gameManagerScript.playerHand.GetComponentsInChildren<CardDisplay>();
        gameManagerScript.enemyHandCardList = gameManagerScript.enemyHand.GetComponentsInChildren<CardDisplay>();

        gameManagerScript.displayNumberOfPlayerHandCard.text = "x" + gameManagerScript.playerHandCardList.Length.ToString();
        gameManagerScript.displayNumberOfEnemyHandCard.text = "x" + gameManagerScript.enemyHandCardList.Length.ToString();

    }
}
