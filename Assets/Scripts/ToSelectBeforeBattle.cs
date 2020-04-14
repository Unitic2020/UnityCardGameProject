using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToSelectBeforeBattle : MonoBehaviour {
    private bool IsPressed = false;

    public void PressBattleButton() {

        if (!IsPressed) {
            SceneManager.LoadScene("SelectDeckBeforeBattle");
            IsPressed = true;
        }

    }
}
