using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCandy : MonoBehaviour {
    public string type;
    public int saverow = 0;
    public int savecol = 0;
    public Sprite GetSpriteSquare;
    public Gem GetGem { get; private set; }
    public Block GetBlock { get; private set; }
    public bool isBonus;
    public int BonusMatchType;
    public bool isSwirl=false;
    public int seconds = 0;
    public HitCandy(string tp)
    {
        this.type = tp;
    }
    public void isGem(Gem g)
    {
        GetGem = g;
    }
    public void isBlock(Block b)
    {
        GetBlock = b;
    }
    public bool isequal(HitCandy hitCandy)
    {
        return hitCandy != null && hitCandy.type == type && hitCandy.type != "ingredient" + 0 && hitCandy.type != "ingredient" + 1;
    }
}
