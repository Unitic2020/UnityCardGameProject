using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Create CardEntity")]

public class CardEntity : ScriptableObject {

    public new string name;
    public int id;
    public int hp;
    public int at;
    public int cost;
    public Sprite icon;
    public new string ability;
}

