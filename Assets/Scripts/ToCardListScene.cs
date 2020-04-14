using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToCardListScene : MonoBehaviour {
    private bool IsPressed = false;

    public void PressCardListButton() {

        if (!IsPressed) {
            SceneManager.LoadScene("CardList");
            IsPressed = true;
        }

    }
}
