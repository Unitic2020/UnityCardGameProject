using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToBack : MonoBehaviour {
    public GameObject Panel;
    private bool IsOn = true;


    public void DeletePanel() {
        if (IsOn) {
            Panel.SetActive(false);
            IsOn = false;
        }
    }

}
