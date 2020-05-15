using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;


public class ListManager : MonoBehaviour
{
    [SerializeField] public GameObject cardPrefab;
    [SerializeField] Transform Listpanel;
    public List<int> CardId = new List<int>() { 1, 1, 1, 1, 1 };

    void Start()
    {
        CardList(CardId);
        AddEventTrigger(cardPrefab);
    }

    public void CardList(List<int> cardId) {

        for (int i = 0; i < 12; i++) {

          GameObject card = Instantiate(cardPrefab, Listpanel, false);

          card.transform.localScale = new Vector3(1,2,0);

       //   card.Initialize(cardId[1]);

        }
      
    }

    public void AddEventTrigger(GameObject card) {
        card.AddComponent<EventTrigger>();
        EventTrigger trigger = card.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { Debug.Log("PointerDown"); });
        //entry.callback.AddListener((data) =>  IsDetailPanel() );
        //Debug.Log(entry);
        trigger.triggers.Add(entry);
    }

    public void IsDetailPanel() {
        Debug.Log("add trigger.");
    }


}
