using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMenuScene : MonoBehaviour
{
    private bool IsPressed = false;
    
    public void PressToMenuButton () {
        
        if(!IsPressed){
            SceneManager.LoadScene("Menu");
            IsPressed = true;
        }
    } 


}
