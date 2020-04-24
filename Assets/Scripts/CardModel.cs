using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel {
    public string name;
    public int hp;
    public int at;
    public int cost;
    public Sprite icon;

    public CardModel(int cardId) {

        CardEntity cardEntity = Resources.Load<CardEntity>("CardList/Card" + cardId); // ここに問題がないことを確認しました 確認方法 => pathを変更してエラーがどうなるかの挙動を確認する
        name = cardEntity.name;
        hp = cardEntity.hp;
        at = cardEntity.at;
        cost = cardEntity.cost;
        icon = cardEntity.icon;
    }
}

