using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToCreditScene:MonoBehaviour {
    private bool IsPressed = false;

    public void PressCreditButton() {

        if(!IsPressed) {
            SceneManager.LoadScene("Credit");
            IsPressed = true;
        }

    }
}
