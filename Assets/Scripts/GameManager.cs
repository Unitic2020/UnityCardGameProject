using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] public CardDisplay cardPrefab;
    public List<int> sampleDeck = new List<int>() { 1, 1, 1, 1, 1, 1, 1 };
    [SerializeField] Transform playerHand;
    public static GameManager instance;

    /*private void Awake() {
        if (instance == null) { 
            instance = this;
        }
    }*/

    void Start() {
       GiveOutCard(sampleDeck);
    }
    
    public void GiveOutCard(List<int> Deck) {
        int cardId = Deck[0];  // sampleDeckと混同しないように変数名を変更
        CardDisplay card = Instantiate(cardPrefab, playerHand, false);
        card.Initialize(cardId); //これに関係なく出る
    }
}