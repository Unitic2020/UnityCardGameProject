using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RuleSceneScript : MonoBehaviour
{
    // ルール説明を行うテキストは、スクリプトから取得する。
    [SerializeField] Text ruleText;
    // データを読み込んでTextAssetのオブジェクトに格納する。
    private TextAsset textAsset;

    // Start is called before the first frame update
    void Start()
    {
        LoadRuleText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Assets内のテキストファイルから、ルール説明のテキストを読み出す機能
    void LoadRuleText()
    {
        textAsset = Resources.Load<TextAsset>("Text/rule");
        string allText = textAsset.text;
        ruleText.text = allText;
    }

    // 戻るボタンが押されたときにメイン画面に戻るシーンチェンジを実装
    public void OnClickBackButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
