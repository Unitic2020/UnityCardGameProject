using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    GameObject gameManager;
    GameManager gameManagerScript;

    GameObject checkBattleState;
    CheckBattleState checkBattleStateScript;

    GameObject gameSystem;
    GameSystem gameSystemScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();

        checkBattleState = GameObject.Find("CheckBattleState");
        checkBattleStateScript = checkBattleState.GetComponent<CheckBattleState>();

        gameSystem = GameObject.Find("GameSystem");
        gameSystemScript = gameSystem.GetComponent<GameSystem>();
    }

    // プレイヤーへの攻撃
    public void AttackToHero(CardDisplay attacker, bool isPlayerCard)
    {
        if (isPlayerCard)
        {
            gameManagerScript.enemyHp -= attacker.initializeCardModel.at;
        }
        else
        {
            gameManagerScript.playerHp -= attacker.initializeCardModel.at;
        }
        attacker.canAttack = false;
        gameSystemScript.UpdateHpText();
        checkBattleStateScript.CheckPlayerAlive();
    }

    public void FightCard(CardDisplay attacker, CardDisplay defender)
    {
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
}
