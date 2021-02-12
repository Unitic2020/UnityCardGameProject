using System.Collections;
using UnityEngine;
using System;
public class EnemyTurn : MonoBehaviour
{
    GameObject gameManager;

    GameManager gameManagerScript;

    IEnumerator enemyTurnCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    public void CreateCoroutineMethod()
    {
        this.enemyTurnCoroutine = this.Turn();
    }

    /*敵のターンだよ*/
    public IEnumerator Turn()
    {

        Debug.Log("エネミーターン");
        gameManagerScript.enemyTurnPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        gameManagerScript.enemyTurnPanel.SetActive(false);
        StartCoroutine(gameManagerScript.TimeSetting());
        yield return new WaitForSeconds(2);
        CardDisplay[] enemyFieldCardList = gameManagerScript.enemyField.GetComponentsInChildren<CardDisplay>();
        for (int i = 0; i < enemyFieldCardList.Length; i++)
        {
            enemyFieldCardList[i].canAttack = true;
        }
        yield return new WaitForSeconds(2);
        StartCoroutine(gameManagerScript.GiveOutCard(gameManagerScript.enemySampleDeck, gameManagerScript.enemyHand));


        // この辺に、敵がカードを場に出す処理を記述する
        CardDisplay[] enemyHandCardList = gameManagerScript.enemyHand.GetComponentsInChildren<CardDisplay>();

        // コスト以下のカードであれば、カードをフィールドに出し続ける
        while (Array.Exists(enemyHandCardList, card => card.initializeCardModel.cost <= gameManagerScript.enemyManaCost))
        {
            // 条件に合うカードすべてを選択し、配列にぶっこむ
            CardDisplay[] selectableHandCardList = Array.FindAll(enemyHandCardList, card => card.initializeCardModel.cost <= gameManagerScript.enemyManaCost);
            enemyFieldCardList = gameManagerScript.enemyField.GetComponentsInChildren<CardDisplay>();

            // 今は、選択可能カードの配列先頭から順番に抽出することにする
            if (selectableHandCardList.Length > 0)
            {
                CardDisplay enemyCard = selectableHandCardList[0];
                if (enemyFieldCardList.Length < 5)
                {
                    enemyCard.transform.SetParent(gameManagerScript.enemyField);
                    gameManagerScript.ReduceManaCost(enemyCard);
                    enemyHandCardList = gameManagerScript.enemyHand.GetComponentsInChildren<CardDisplay>();
                    gameManagerScript.displayNumberOfEnemyHandCard.text = "x" + enemyHandCardList.Length.ToString();
                }
                else
                {
                    break;
                }

            }
            else
            {
                break;
            }
        }

        // 敵カードがplayerのカードに攻撃する
        enemyFieldCardList = gameManagerScript.enemyField.GetComponentsInChildren<CardDisplay>();


        // 攻撃可能カードの配列を取得
        CardDisplay[] enemyCanAttackCardList = Array.FindAll(enemyFieldCardList, card => card.canAttack);
        // 攻撃先（プレイヤーのフィールド）のカードを配列で取得
        CardDisplay[] playerFieldCardList = gameManagerScript.playerField.GetComponentsInChildren<CardDisplay>();
        Debug.Log("while前");
        yield return new WaitForSeconds(5);
        Debug.Log(enemyCanAttackCardList.Length);
        while (enemyCanAttackCardList.Length > 0 && playerFieldCardList.Length > 0)
        {
            Debug.Log("while中");
            CardDisplay attacker = enemyCanAttackCardList[0];
            // 攻撃先（プレイヤーのフィールド）にカードがいる場合は、攻撃する。
            Debug.Log("while中if中");
            // defenderカード（攻撃対象のカード）を選択
            CardDisplay defender = playerFieldCardList[0];
            gameManagerScript.FightCard(attacker, defender);
            yield return new WaitForSeconds(1);
            playerFieldCardList = gameManagerScript.playerField.GetComponentsInChildren<CardDisplay>();
            enemyFieldCardList = gameManagerScript.enemyField.GetComponentsInChildren<CardDisplay>();
            enemyCanAttackCardList = Array.FindAll(enemyFieldCardList, card => card.canAttack);
        }
        while (enemyCanAttackCardList.Length > 0)
        {
            yield return new WaitForSeconds(1);
            gameManagerScript.AttackToHero(enemyCanAttackCardList[0], false);
            gameManagerScript.UpdateHpText();
            enemyCanAttackCardList = Array.FindAll(enemyFieldCardList, card => card.canAttack);
        }
        yield return new WaitForSeconds(1);
        gameManagerScript.SwitchTurn(gameManagerScript.turn);
    }

    public void RunEnemyTurnCoroutine()
    {
        if (enemyTurnCoroutine == null)
        {
            Debug.Log("コルーチンないんだけど");
            return;
        }
        StartCoroutine(enemyTurnCoroutine);
    }

    public void StopEnemyTurnCoroutine()
    {
        if (enemyTurnCoroutine == null)
        {
            Debug.Log("コルーチンないんだけど");
            return;
        }
        StopAllCoroutines();
    }
}
