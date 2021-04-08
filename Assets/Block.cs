using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    public HitCandy candy;
    public int row;
    public int col;
    public int types;
    public int saverow = 0;
    public int savecol = 0;
    public bool dontDest = false;
    public Block goParent;
    public int bonus { get; set; }
    public List<GameObject> block = new List<GameObject>();
    private int ccc;
    public bool emptyes=false;
    [SerializeField] Sprite GetSprite1;
    [HideInInspector] public int modelvlsquare;
    public int addScore = 10;
    public bool INull { get { return candy == null; } }
	// Use this for initialization
	void Start () {
		
	}
	public bool match3(Block g)
    {
        return !INull && !g.INull && candy.isequal(g.candy);
    }
	// Update is called once per frame
	void Update () {
		
	}
    public void OnInit(HitCandy hitI)
    {
        candy = hitI;
        candy.isBlock(this);//
    }
    public static void DestroyBlocks()
    {

    }
    public void Nil()
    {
        candy = null;
    }
}
public class SquareBlocks
{
    public int blck;
    public void Changeblck(int bl) { blck = bl; }
    public int block() { return blck; }
    public int obstacle;
    public int addScore = 10;
}
