using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {

    CardModel initializeCardModel;
    public int cardId;
    [SerializeField] Text nameText;
    [SerializeField] Text hpText;
    [SerializeField] Text atText;
    [SerializeField] Text costText;
    [SerializeField] Image iconImage;
    [SerializeField] Text abilityText;

    public void Display(CardModel cardModel) {

        nameText.text = cardModel.name;
        hpText.text = cardModel.hp.ToString();
        atText.text = cardModel.at.ToString();
        costText.text = cardModel.cost.ToString();
        iconImage.sprite = cardModel.icon;
        abilityText.text = cardModel.ability;
    }

    public void Initialize(int cardId) {

        initializeCardModel = new CardModel(cardId);
        Display(initializeCardModel);
    }
}
