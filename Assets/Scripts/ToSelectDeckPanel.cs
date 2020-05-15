using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToSelectDeckPanel : MonoBehaviour
{
    public GameObject Panel;
    private bool IsOn;

    public void start() {
        Panel.SetActive(false);
        IsOn = false;
    }

    public void PressDeck() {
        if (!IsOn) {
            Panel.SetActive(true);
            
        }
    }
       public void DeletePanel() {
        if (!IsOn) {
            Panel.SetActive(false);
            
        }
    }
}
