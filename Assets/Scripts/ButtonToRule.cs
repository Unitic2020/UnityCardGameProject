using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonToRule : MonoBehaviour
{
    public void OnClickButtonToRule()
    {
        SceneManager.LoadScene("Rule");
    }
}
