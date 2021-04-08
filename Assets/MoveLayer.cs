using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
enum MoveType
{
    Horrizontally,Vertically,HorVer
}
public class MoveLayer : MonoBehaviour {
    public static MoveLayer THIS;
    public string Name;
    public string urlOnTournament;
    [HideInInspector] public int movecount;
    [HideInInspector] public int limitMove;
    public UnityEngine.UI.Text GetTextMove;
    public int state = 0;
    Vector2[] visibleSize;
    private int SizeX;
    private int SizeY;
    public GameObject[] bug;
    [SerializeField] List<Vector2> ylist2 = new List<Vector2>();
    protected HitCandy currentHits;
    protected IEnumerable<Block> currentHitList;
    protected int MoveCount;
    [SerializeField] Arrays GetArrays;
    [SerializeField] HitCandy[] GetCandyPrefab;
    [SerializeField] HitCandy[] GetCandieSecons;
    [SerializeField] OpenAppLevel levelsApps;
    [SerializeField] OpenAppLevel AppLevel;
    [SerializeField] GameObject blockpref;
    HitCandy GetHitGem;
    HitCandy GetHitNext;
    [SerializeField] Sprite sprite12;
    public HitCandy bubble, bubble2;
    [SerializeField] HitCandy[] GetBonusPrefab;
    [SerializeField] HitCandy swirlPrefab;
    [SerializeField] Sprite[] GetIngredientSprite;
    [SerializeField] GameObject plus5Seconds;
    [SerializeField] 
    UnityEngine.UI.Image BubleCursor;
    [SerializeField]
    UnityEngine.UI.Image BubleCursor2;//
    List<HitCandy> DestroyList = new List<HitCandy>();
    List<HitCandy> CurrenthitCandies;
    protected bool rightMouseButtonDown = false;
    //[SerializeField] Sprite IconItem;
    bool load;
    int Ending=0;
    private Block[] blocksquare;
    protected Block CurrentBlk;
    protected int currentrow,currentcol;
    protected int bubbleplus=0;
    protected List<Block> MoveParents = new List<Block>();
    protected List<Block> BlockParents = new List<Block>();
    protected Block GetBlockMove;
    float[] rowlist = { 4.08f, 4.08f, 12.31f, -4.63f };
    float[] collist = { -4.37f, 12.25f, 4.1f,4.1f };
    public void addCandies(HitCandy candy)
    {
        CurrenthitCandies.Add(candy);
    }
    /*delegates*/
    MotionDetectionDelegate motionDelegate;
    public delegate bool MotionDetectionDelegate(Vector3 position);

    CollisionDetectionDelegate collisionDelegate;
    public delegate void CollisionDetectionDelegate(GameObject bubble);

    /* Setters and getters */
    public CollisionDetectionDelegate CollisionDelegate
    {
        set
        {
            collisionDelegate = value;
        }
    }
    public MotionDetectionDelegate MotionDelegate
    {
        set
        {
            motionDelegate = value;
        }
    }
    public int End() {return Ending; }
    public bool loaldo() { return load; }
    public void loadMove(bool load1)
    {
        load = load1;
    }
    private void Awake()
    {
        THIS = this;
        GetHitNext = GetCandyPrefab[Random.Range(0, GetCandyPrefab.Length)];
        GetHitGem = GetHitNext;
        BubleCursor.sprite = GetHitNext.GetComponent<SpriteRenderer>().sprite;
        //BubleCursor.GetComponent<HitCandy>() = GetHitGem;
        bubble2 = GetHitGem;
        GetHitNext = GetCandyPrefab[Random.Range(0, GetCandyPrefab.Length)];
    }
    // Use this for initialization
    void Start()
    {
        state = 0;
    }
    /// <summary>
    /// restart
    /// </summary>
    public void restarting()
    {
        SizeX = AppLevel.MaxX;
        SizeY = AppLevel.MaxY;
        Gems();
    }
    // Update is called once per frame
    void Update()
    {
        BubleCursor.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        GetTextMove.text = string.Format("{0}", movecount);
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (bubbleplus >= 4) { bubbleplus = 0; }
            else bubbleplus += 1;
            
        }
        //BubleCursor.sprite = GetHitGem.GetComponent<SpriteRenderer>().sprite;
        if (Input.GetMouseButtonDown(1))
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
            print(state);
            if (hit.collider != null&&state==1)
            {
                //StopCoroutine(TryMatch(hit));
                CurrentBlk = hit.collider.GetComponent<Block>();
                //GetHitGem = GetCandyPrefab[Random.Range(0, GetCandyPrefab.Length)];
                //BubleCursor.sprite = GetHitGem.GetComponent<SpriteRenderer>().sprite;
                ////BubleCursor.GetComponent<HitCandy>() = GetHitGem;
                //bubble2 = GetHitGem;
                if (CurrentBlk != null&& GetBlockMove!=null)
                {
                    if (CurrentBlk.types == 2)
                    {
                        GetBlockMove.transform.position = CurrentBlk.transform.position;
                        //var createload = Instantiate(GetBlockMove.gameObject, CurrentBlk.transform.position, Quaternion.identity);
                        //GetHitGem = GetCandyPrefab[Random.Range(0, GetCandyPrefab.Length)];
                        //BubleCursor.sprite = GetHitGem.GetComponent<SpriteRenderer>().sprite;
                        ////BubleCursor.GetComponent<HitCandy>() = GetHitGem;
                        //bubble2 = GetHitGem;
                        MoveParents.Clear();

                        state = 0;
                    }
                }
               
               
            }
        }
        switch (bubbleplus)
        {
            case 0:
                BubleCursor2.rectTransform.anchoredPosition = new Vector2(4.08f, -4.37f);
                break;
            case 1:
                BubleCursor2.rectTransform.anchoredPosition = new Vector2(4.08f, 12.25f);
                break;
            case 2:
                BubleCursor2.rectTransform.anchoredPosition = new Vector2(-4.63f, 4.1f);
                break;
            case 3:
                BubleCursor2.rectTransform.anchoredPosition = new Vector2(12.31f, 4.1f);
                break;
            case 4:
                BubleCursor2.rectTransform.anchoredPosition = new Vector2(4.01f, 4.01f);
                break;
        }
        
        BubleCursor2.sprite = BubleCursor.sprite;
        if (movecount != limitMove)
        {
            GetTextMove.gameObject.SetActive(true);//
            switch (state)
            {
                case 0:
                    if(levelsApps.modeLvl==2) StartCoroutine(IngredientScale());
                    if (Input.GetMouseButtonDown(1))
                    {
                        rightMouseButtonDown = true;
                        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
                        if (hit.collider != null)
                        {
                            //CurrentBlk = hit.collider.GetComponent<Block>();
                            var cndy = hit.collider.GetComponent<HitCandy>();
                            GetHitGem = cndy;bubble2 = cndy;
                            BubleCursor.sprite = GetHitNext.GetComponent<SpriteRenderer>().sprite;
                            currentHits = hit.collider.GetComponent<HitCandy>();
                            if (cndy != null&& cndy.GetBlock.types==1)
                            {
                                GameObject vblck = ((GameObject)Instantiate(blockpref, cndy.GetBlock.transform.position, Quaternion.identity));
                                vblck.GetComponent<Block>().row = cndy.GetBlock.row;
                                vblck.GetComponent<Block>().col = cndy.GetBlock.col;
                                vblck.GetComponent<Block>().types = 1;
                                GetBlockMove = vblck.GetComponent<Block>();
                                GetArrays[GetBlockMove.row, GetBlockMove.col] = new Gem(GetBlockMove.row, GetBlockMove.col);
                                GetArrays[GetBlockMove.row, GetBlockMove.col].Nil();
                                var lisp = GetArrays.GetProp2(cndy.GetBlock, cndy.GetBlock.row, cndy.GetBlock.col);
                                MoveCount = lisp.GetGems.Count();
                                currentHitList = lisp.GetGems;
                                //GameObject vblck = ((GameObject)Instantiate(blockpref, levelsApps.vector2position + new Vector2(cndy.GetBlock.col * levelsApps.blckWH(), cndy.GetBlock.row * levelsApps.blckWH()), Quaternion.identity));
                                CurrentBlk = cndy.GetBlock;
                                CurrentBlk.dontDest = true;
                                StartCoroutine(TryMatch(hit));
                                GetHitGem = cndy;
                                //foreach (var bb in MoveParents)
                                //{
                                //    if(bb.GetComponent<Block>().row==cndy.GetBlock.row
                                //        && bb.GetComponent<Block>().col == cndy.GetBlock.col)
                                //    {
                                //        GetBlockMove = bb;
                                //    }
                                //}
                                //state = 1;
                                //print(state);
                            }
                        }
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        rightMouseButtonDown = false;
                        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
                        if (hit.collider != null)
                        {
                            print(state);
                            CurrentBlk = hit.collider.GetComponent<Block>();
                            var cndy = hit.collider.GetComponent<HitCandy>();
                            currentHits = hit.collider.GetComponent<HitCandy>();
                            MoveParents.Add(CurrentBlk);

                            hit.collider.GetComponent<HitCandy>();
                            if (CurrentBlk != null)
                            {
                                if (GetHitGem == null)
                                {
                                    GetHitGem = GetHitNext;
                                   
                                    BubleCursor.sprite = GetHitNext.GetComponent<SpriteRenderer>().sprite;
                                    GetHitNext = GetCandyPrefab[Random.Range(0, GetCandyPrefab.Length)];
                                    //BubleCursor.GetComponent<HitCandy>() = GetHitGem;
                                    bubble2 = GetHitGem;
                                }
                                CreateGem(CurrentBlk.row, CurrentBlk.col, GetHitGem);//
                                switch (bubbleplus)
                                {
                                    case 0:
                                        CreateGem(CurrentBlk.row - 1, CurrentBlk.col, bubble2);
                                        currentrow = CurrentBlk.row - 1;
                                        currentcol = CurrentBlk.col;
                                        break;
                                    case 1:
                                        CreateGem(CurrentBlk.row + 1, CurrentBlk.col, bubble2);
                                        currentrow = CurrentBlk.row + 1;
                                        currentcol = CurrentBlk.col;
                                        break;
                                    case 2:
                                        CreateGem(CurrentBlk.row, CurrentBlk.col - 1, bubble2);
                                        currentrow = CurrentBlk.row;
                                        currentcol = CurrentBlk.col - 1;
                                        break;
                                    case 3:
                                        CreateGem(CurrentBlk.row, CurrentBlk.col + 1, bubble2);
                                        currentrow = CurrentBlk.row;
                                        currentcol = CurrentBlk.col + 1;
                                        break;
                                    case 4:
                                        break;
                                }
                                StartCoroutine(TryMatch(hit));
                                GetHitGem = GetHitNext;
                                BubleCursor.sprite = GetHitNext.GetComponent<SpriteRenderer>().sprite;
                                GetHitNext = GetCandyPrefab[Random.Range(0, GetCandyPrefab.Length)];
                                //BubleCursor.GetComponent<HitCandy>() = GetHitGem;
                                bubble2 = GetHitGem;
                            }
                            if ((uint)levelsApps.ltype == 1)
                            {
                                //levelsApps.GetPause();
                            }
                            //GetHitGem.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,0);
                            state = 0;
                        }
                    }
                    break;
                case 1:
                    
                    break;
            }
        }

    }
    [SerializeField] List<List<Vector2>> xlist1 = new List<List<Vector2>>();
    public List<string> vslist;
    List<Gem> matchesOne(Gem[,] gemmall, int row, int col)
    {
        List<Gem> match = new List<Gem>();
        vslist.Clear();
        if (col <= GetArrays.SizeX - 2 && gemmall[row, col].match3(gemmall[row, col + 1]))//
        {
            if (row > 0 && col < GetArrays.SizeX - 2 && gemmall[row, col].match3(gemmall[row - 1, col + 2]))
            {
                match.Add(gemmall[row, col]);
                match.Add(gemmall[row, col + 1]);
                Scale2Match(gemmall[row - 1, col + 2].hitGem);
                Scale2Match(gemmall[row, col + 2].hitGem);
                Debug.Log("match:" + row + "," + col + ";" + (row) + "," + (col + 1) + ";" + (row - 1) + "," + (col + 2));
                vslist.Add("match:" + row + "," + col + ";" + (row) + "," + (col + 1) + ";" + (row - 1) + "," + (col + 2));
                List<Vector2> vsadd = new List<Vector2>();
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2(col * 0.6f, row * 0.6f)));
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2((col + 1) * 0.6f, row * 0.6f)));
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2((col + 2) * 0.6f, (row - 1) * 0.6f)));
                xlist1.Add(vsadd);
            }
        }
        return match;
    }
    List<Gem> matchesTwo(Gem[,] gemmall, int row, int col)//
    {
        List<Gem> match = new List<Gem>();
        if (col <= GetArrays.SizeX - 2 && gemmall[row, col].match3(gemmall[row, col + 1]))//
        {
            if (row <= GetArrays.SizeY - 2 && col < GetArrays.SizeX - 2 && gemmall[row, col].match3(gemmall[row + 1, col + 2]))
            {
                match.Add(gemmall[row, col]);
                match.Add(gemmall[row, col + 1]);
                //match.Add(gemmall[row + 1, col + 2]);
                Scale2Match(gemmall[row + 1, col + 2].hitGem);
                Scale2Match(gemmall[row, col + 2].hitGem);//

                Debug.Log("match:" + row + "," + col + ";" + (row) + "," + (col + 1) + ";" + (row + 1) + "," + (col + 2));
                vslist.Add("match:" + row + "," + col + ";" + (row) + "," + (col + 1) + ";" + (row + 1) + "," + (col + 2));
                List<Vector2> vsadd = new List<Vector2>();
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2(col * 0.6f, row * 0.6f)));
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2((col + 1) * 0.6f, row * 0.6f)));
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2((col + 2) * 0.6f, (row + 1) * 0.6f)));
                xlist1.Add(vsadd);
            }
        }
        return match;//
    }
    List<Gem> matchesThree(Gem[,] gemmall, int row, int col)//
    {
        List<Gem> match = new List<Gem>();
        if (col <= 2 && gemmall[row, col].match3(gemmall[row, col + 1]))//
        {
            //if (row >=2 && col<GetArrayMatch.SizeX - 2 && gemmall[row, col].match3(gemmall[row - 1, col + 2]) ||
            //row >= GetArrayMatch.SizeY - 2 && gemmall[row, col].match3(gemmall[row + 1, col + 2]))
            //{
            if (row > 0 && col >= 2 && gemmall[row, col].match3(gemmall[row - 1, col + 2]))
            {
                match.Add(gemmall[row, col]);
                match.Add(gemmall[row, col + 1]);
                Scale2Match(gemmall[row - 1, col + 2].hitGem);
                Scale2Match(gemmall[row, col + 2].hitGem);
                //match.Add(match1);
                Debug.Log("match:" + row + "," + col + ";" + (row) + "," + (col + 1) + ";" + (row - 1) + "," + (col + 2));
                vslist.Add("match:" + row + "," + col + ";" + (row) + "," + (col + 1) + ";" + (row - 1) + "," + (col + 2));
                List<Vector2> vsadd = new List<Vector2>();
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2(col * 0.6f, row * 0.6f)));
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2((col + 1) * 0.6f, row * 0.6f)));
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2((col + 2) * 0.6f, (row - 1) * 0.6f)));
                xlist1.Add(vsadd);
            }
        }
        return match;//
    }
    List<Gem> matchesFour(Gem[,] gemmall, int row, int col)//
    {
        List<Gem> match = new List<Gem>();
        if (col <= 2 && gemmall[row, col].match3(gemmall[row, col - 1]))
        {
            if (row < GetArrays.SizeY - 2 && col >= 2 && gemmall[row, col].match3(gemmall[row + 1, col - 2]))
            {
                match.Add(gemmall[row, col]);
                match.Add(gemmall[row, col - 1]);
                match.Add(gemmall[row + 1, col - 2]);
                Destroy(gemmall[row, col - 2].hitGem.gameObject);
                print("gem:" + gemmall[row, col].x + gemmall[row, col].y);
                Debug.Log("match:" + row + "," + col + ";" + (row) + "," + (col - 1) + ";" + (row + 1) + "," + (col - 2));
                vslist.Add("match:" + row + "," + col + ";" + (row) + "," + (col - 1) + ";" + (row + 1) + "," + (col - 2));
                List<Vector2> vsadd = new List<Vector2>();
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2(col * 0.6f, row * 0.6f)));
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2((col - 1) * 0.6f, row * 0.6f)));
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2((col - 2) * 0.6f, (row + 1) * 0.6f)));
            }
        }
        return match;
    }
    List<Gem> matchesFive(Gem[,] gemmall, int row, int col)
    {
        List<Gem> match = new List<Gem>();
        if (row < GetArrays.SizeY - 2 && gemmall[row, col].match3(gemmall[row + 1, col]))
        {
            if (row < GetArrays.SizeY - 2 && col > 0 && gemmall[row, col].match3(gemmall[row + 2, col - 1]))
            {
                match.Add(gemmall[row, col]);
                match.Add(gemmall[row + 1, col]);
                Scale2Match(gemmall[row + 2, col - 1].hitGem);
                Scale2Match(gemmall[row + 2, col].hitGem);
                vslist.Add("match:" + row + "," + col + ";" + (row + 1) + "," + col + ";" + (row + 2) + "," + (col - 1) + ";");
                List<Vector2> vsadd = new List<Vector2>();
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2(col * 0.6f, row * 0.6f)));
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2((col) * 0.6f, (row + 1) * 0.6f)));
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2((col - 1) * 0.6f, (row + 2) * 0.6f)));
                xlist1.Add(vsadd);
            }
        }
        return match;
    }
    List<Gem> matchesSix(Gem[,] gemmall, int row, int col)
    {
        List<Gem> match = new List<Gem>();
        if (row < GetArrays.SizeY - 2 && gemmall[row, col].match3(gemmall[row + 1, col]))
        {
            if (row < GetArrays.SizeY - 2 && col < GetArrays.SizeX - 2 && gemmall[row, col].match3(gemmall[row + 2, col + 1]))
            {
                match.Add(gemmall[row, col]);
                match.Add(gemmall[row + 1, col]);
                Scale2Match(gemmall[row + 2, col + 1].hitGem);
                Scale2Match(gemmall[row + 2, col].hitGem);
                vslist.Add("match:" + row + "," + col + ";" + (row + 1) + "," + col + ";" + (row + 2) + "," + (col + 1) + ";");
                List<Vector2> vsadd = new List<Vector2>();
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2(col * 0.6f, row * 0.6f)));
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2((col) * 0.6f, (row + 1) * 0.6f)));
                vsadd.Add(new Vector2(-2.37f, -4.27f) + (new Vector2((col + 1) * 0.6f, (row + 2) * 0.6f)));
                xlist1.Add(vsadd);
            }
        }
        return match;
    }
    List<Gem> matches1(Gem[,] gemmall)
    {
        List<List<Gem>> match = new List<List<Gem>>();
        for (int row = 0; row < GetArrays.SizeY; row++)
        {
            for (int col = 0; col < GetArrays.SizeX; col++)
            {
                match.Add(matchesOne(gemmall, row, col));
                match.Add(matchesTwo(gemmall, row, col));
                //match.Add(matchesThree(gemmall, row, col));
                //match.Add(matchesFour(gemmall, row, col));
                match.Add(matchesFive(gemmall, row, col));
                match.Add(matchesSix(gemmall, row, col));
            }
        }
        ylist2 = xlist1[Random.Range(0, xlist1.Count)];//
        return match[Random.Range(0, match.Count)];
    }
    public void LoadHelp()
    {
        xlist1.Clear();
        foreach (var gem123 in matches1(GetArrays.gems))
        {

        }//
        for (int i = 0; i < bug.Length; i++)
        {
            bug[i].transform.position = ylist2[i];
        }
    }
    public IEnumerator IngredientScale()
    {
        IEnumerable<Gem> matchs = null;
        List<Gem> ScaleIngr = new List<Gem>();
        IEnumerable<Gem> matchs2 = null;
        for (int i = 0; i < SizeX; i++)
        {
            if (GetArrays.gems[0, i].hitGem.type == "ingredient" + 0&& levelsApps.ingCtar[0]!=0)//
            { ScaleIngr.Add(GetArrays.gems[0, i]); levelsApps.ingCtar[0] -= 1; }
            if (GetArrays.gems[0, i].hitGem.type == "ingredient" + 1&& levelsApps.ingCtar[1] != 0) {
                ScaleIngr.Add(GetArrays.gems[0, i]); levelsApps.ingCtar[1] -= 1; }
        }
        matchs = ScaleIngr;
        foreach (var k in ScaleIngr)
        {
            ScaleMatch(k.hitGem);
        }
        var bottom = matchs.Select(gem => gem.x).Distinct();
        var updateafterMatch = GetArrays.UpdateAfter(bottom);
        var newSpawn = GetNeighbourProp(bottom);
        int maxDistance = Mathf.Max(updateafterMatch.MaxDistance, newSpawn.MaxDistance);
        Moved(maxDistance, newSpawn.GetGems);
        Moved(maxDistance, updateafterMatch.GetGems);
        yield return new WaitForSeconds(0.05f * Mathf.Max(updateafterMatch.MaxDistance, newSpawn.MaxDistance));
        //matchs2 = GetArrays.GetNeighbours(newSpawn.GetGems).Union(GetArrays.GetNeighbours(updateafterMatch.GetGems)).Distinct();
        while (matchs2.Count() >= 3)
        {
            foreach (var k2 in matchs2)
            {
                ScaleMatch(k2.hitGem);
            }
            var bottom2 = matchs2.Select(gem => gem.x).Distinct();
            var updateafterMatch2 = GetArrays.UpdateAfter(bottom);
            var newSpawn2 = GetNeighbourProp(bottom2);
            int maxDistance2 = Mathf.Max(updateafterMatch.MaxDistance, newSpawn2.MaxDistance);
            Moved(maxDistance, newSpawn2.GetGems);
            Moved(maxDistance, updateafterMatch2.GetGems);
            yield return new WaitForSeconds(0.05f * maxDistance2);
            //matchs2 = GetArrays.GetNeighbours(newSpawn2.GetGems).Union(GetArrays.GetNeighbours(updateafterMatch2.GetGems)).Distinct();
        }
    }
    public Gem IngredientPosition(int ingr1,int ingr2)
    {
        int[] IngredientXPosition = { SizeX / 2, (SizeX / 2) + 1, SizeX / 2 - 1 };
        Gem[] genIngr0 = new Gem[ingr1];
        Gem[] gemIngr1 = new Gem[ingr2];
        int randpos;
        for (int i = 0; i < ingr1; i++)
        {
            randpos = IngredientXPosition[Random.Range(0, IngredientXPosition.Length)];
            //if (IsNulls((SizeY - 1) - i, randpos) == true)
            //{
                randpos = IngredientXPosition[Random.Range(0, IngredientXPosition.Length)];
            //}
            genIngr0[i] = GetArrays.gems[(SizeY - 1) - i, randpos];
            genIngr0[i].hitGem.type = "ingredient" + 0;
            genIngr0[i].hitGem.GetComponent<SpriteRenderer>().sprite = GetIngredientSprite[0];
        }
        for (int i = 0; i < ingr2; i++)
        {
            randpos = IngredientXPosition[Random.Range(0, IngredientXPosition.Length)];
            if (GetArrays.gems[(SizeY / 2) - i, randpos].hitGem.type == "ingredient" + 0) randpos = IngredientXPosition[Random.Range(0, IngredientXPosition.Length)];
            if (IsNulls((SizeY / 2) - i, randpos) == true)
            {
                randpos = IngredientXPosition[Random.Range(0, IngredientXPosition.Length)];
                if (GetArrays.gems[(SizeY / 2) - i, randpos].hitGem.type == "ingredient" + 0) randpos = IngredientXPosition[Random.Range(0, IngredientXPosition.Length)];
            }
            gemIngr1[i] = GetArrays.gems[(SizeY / 2) - i, IngredientXPosition[Random.Range(0, IngredientXPosition.Length)]];
            gemIngr1[i].hitGem.type = "ingredient" + 1;
            gemIngr1[i].hitGem.GetComponent<SpriteRenderer>().sprite = GetIngredientSprite[1];
        }
        GetArrays.ingredientsGems = new HitCandy[genIngr0.Length];
        GetArrays.ingredientsGems2 = new HitCandy[gemIngr1.Length];//
        for (int k = 0; k < genIngr0.Length; k++) { GetArrays.ingredientsGems[k] = genIngr0[k].hitGem; }
        for (int k = 0; k < gemIngr1.Length; k++) { GetArrays.ingredientsGems2[k] = gemIngr1[k].hitGem; }
        //GetArrays.ingredientsGems2 = gemIngr1[0].hitGem;//
        return gemIngr1[0];
    }
    public void GetDestroyAlls()
    {
        for (int row = 0; row < GetArrays.SizeY; row++)
        {
            for (int col = 0; col < GetArrays.SizeX; col++)
            {
                if (GetArrays[row, col].INull == false)
                {
                    Destroy(obj: GetArrays[row, col].hitGem.gameObject);
                    Destroy(OpenAppLevel.THIS.blocksp[row * OpenAppLevel.THIS.MaxX + col].gameObject);
                }
            }
        }
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                Destroy(OpenAppLevel.THIS.bblocks[row * 9 + col].gameObject);
            }
        }
        foreach(var pp in BlockParents)
        {
            Destroy(pp.gameObject);
        }
    }
    /// <summary>
    /// начало
    /// </summary>
    private void Gems()
    {
        GetHitGem = GetHitNext;
        GetHitNext = GetCandyPrefab[Random.Range(0, GetCandyPrefab.Length)];
        BubleCursor.sprite = GetHitGem.GetComponent<SpriteRenderer>().sprite;
        //BubleCursor.gameObject.GetComponent<HitCandy>() = GetHitGem;
        bubble = GetHitGem;
        bubble2 = GetHitGem;
        float xc = levelsApps.blckWH();//
        float yr = levelsApps.blckWH();//
        blocksquare = new Block[SizeX* SizeY];
        for (int row = 0; row < GetArrays.SizeY; row++)
        {
            for (int col = 0; col < GetArrays.SizeX; col++)
            {
                if (GetArrays[row, col].INull == false)
                {
                    Destroy(obj: GetArrays[row, col].hitGem.gameObject);
                }
            }
        }
        for (int rw = 0; rw < OpenAppLevel.THIS.MaxY; rw++)
        {
            for (int cl = 0; cl < OpenAppLevel.THIS.MaxX; cl++)
            {
                
            }
        }
        for (int row = 0; row < OpenAppLevel.THIS.MaxY; row++)
        {
            for (int col = 0; col < OpenAppLevel.THIS.MaxX; col++)
            {
                GetArrays.gems[row, col] = new Gem(row, col);
                GetArrays.gems[row, col].Nil();

            }
        }
        int g = SizeX;
        visibleSize = new Vector2[g];
        
        for (int row = 0; row < SizeY; row++)
        {
            for (int col = 0; col < SizeX; col++)
            {

                HitCandy hitCandy = GetCandyPrefab[Random.Range(0, GetCandyPrefab.Length)];
                //while (col >= 2 && GetArrays[row, col - 1].hitGem.isequal(hitCandy) && GetArrays[row, col - 2].hitGem.isequal(hitCandy))//&& levelsManager.squaresArray[(row) * levelsManager.maxCols + (col-1)].types != SquareTypes.NONE&& levelsManager.squaresArray[(row) * levelsManager.maxCols + (col-1)].types != SquareTypes.NONE)
                //{
                    hitCandy = GetCandyPrefab[Random.Range(0, GetCandyPrefab.Length)];
                //}
                //while (row >= 2 && GetArrays[row - 1, col].hitGem.isequal(hitCandy) && GetArrays[row - 2, col].hitGem.isequal(hitCandy))//&& levelsManager.squaresArray[(row - 1) * levelsManager.maxCols + col].types != SquareTypes.NONE && levelsManager.squaresArray[(row-2)*levelsManager.maxCols+col].types!=SquareTypes.NONE)
                //{
                //hitCandy = GetCandyPrefab[Random.Range(0, GetCandyPrefab.Length)];
                //}
                //CreateSquaresBlocks(col, row);
                CreateGem(row, col, hitCandy);
            }
        }

        for (int i = 0; i < SizeX; i++)
        {
            visibleSize[i] = levelsApps.vector2position + new Vector2(i * xc, SizeY * yr); //new Vector2(-2.37f, -4.27f) + new Vector2(i * 0.7f, SizeY * 0.7f);
        }
        if(levelsApps.modeLvl!=2)LoadHelp();
    }
    private void NewOldBlock(int i, int j)
    {
        GameObject vblck = ((GameObject)Instantiate(blockpref, levelsApps.vector2position + new Vector2(i * levelsApps.blckWH(), j * levelsApps.blckWH()), Quaternion.identity));
        vblck.transform.SetParent(GetArrays.transform);
        blocksquare[j * SizeX + i] = vblck.GetComponent<Block>();
        vblck.GetComponent<Block>().row = j;
        vblck.GetComponent<Block>().col = i;
        vblck.GetComponent<Block>().types = 1;
    }
    private void CreateSquaresBlocks(int i, int j, Block vblck2)
    {
        GameObject vblck = ((GameObject)Instantiate(blockpref, levelsApps.vector2position + new Vector2(i * levelsApps.blckWH(), j * levelsApps.blckWH()), Quaternion.identity));
        vblck.transform.SetParent(GetArrays.transform);
        blocksquare[j * SizeX + i] = vblck.GetComponent<Block>();
        BlockParents.Add(vblck.GetComponent<Block>());
        BlockParents.Add(vblck2);
        vblck.GetComponent<Block>().row = j;
        vblck.GetComponent<Block>().col = i;
        vblck.GetComponent<Block>().types = 1;
        vblck.transform.SetParent(vblck2.gameObject.transform);
        vblck.GetComponent<Block>().goParent = vblck2;
        
    }
    public bool IsNulls(int row, int col)
    {
        if (levelsApps.blocksp[row* levelsApps.MaxX+col].types==0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="row"></param>
    /// <param name="c"></param>
    /// <param name="hgem"></param>
    public void CreateGem(int row, int c, HitCandy hgem)
    {
        if (GetArrays[row, c] == null) { return; }
        int[] randomplus5sec = {0,1,2,3,4,5,6,7,8};
        int plus5sec = randomplus5sec[Random.Range(0, randomplus5sec.Length)];
        if ((int)levelsApps.ltype==1&&plus5sec == c) { hgem = GetCandieSecons[Random.Range(0, GetCandieSecons.Length)]; }
        float xc = levelsApps.blckWH();
        float yr = levelsApps.blckWH();
        Vector2 vectorgem = levelsApps.vector2position + (new Vector2(c * xc, row * yr));
        HitCandy gemit = ((GameObject)Instantiate(hgem.gameObject, new Vector3(vectorgem.x, vectorgem.y, -0.1f), Quaternion.identity)).GetComponent<HitCandy>();
        gemit.transform.SetParent(GetArrays.transform);
        GetArrays[row, c].OnInit(gemit);
        OpenAppLevel.THIS.blocksp[row * OpenAppLevel.THIS.MaxX + c].candy = gemit;
        OpenAppLevel.THIS.blocksp[row * OpenAppLevel.THIS.MaxX + c].OnInit(gemit);
        if (IsNulls(row, c) == true) { Destroy(gemit.gameObject); }
    }
    public void MovedGem(int row, int c, HitCandy hgem)
    {

        int[] randomplus5sec = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        int plus5sec = randomplus5sec[Random.Range(0, randomplus5sec.Length)];
        if ((int)levelsApps.ltype == 1 && plus5sec == c) { hgem = GetCandieSecons[Random.Range(0, GetCandieSecons.Length)]; }
        float xc = levelsApps.blckWH();
        float yr = levelsApps.blckWH();
        Vector2 vectorgem = levelsApps.vector2position + (new Vector2(c * xc, row * yr));
        hgem.gameObject.transform.position = new Vector3(vectorgem.x, vectorgem.y, -0.1f);
        hgem.transform.SetParent(GetArrays.transform);
        GetArrays[row, c].OnInit(hgem);
        OpenAppLevel.THIS.blocksp[row * OpenAppLevel.THIS.MaxX + c].candy = hgem;
        OpenAppLevel.THIS.blocksp[row * OpenAppLevel.THIS.MaxX + c].OnInit(hgem);
        if (IsNulls(row, c) == true) { Destroy(hgem.gameObject); }
    }
    /// <summary>
    /// новые Конфеты
    /// </summary>
    /// <param name="colls"></param>
    /// <returns></returns>
    private UpdateAfterMatch GetNeighbourProp(IEnumerable<int> colls)
    {
        UpdateAfterMatch afterMatch = new UpdateAfterMatch();
        foreach (int coll in colls)
        {
            var emptyGems = GetArrays.NullGemsonc(coll);
            foreach (var g in emptyGems)
            {
                if (IsNulls(g.row, coll)==false)//
                {
                    var pref = GetCandyPrefab[Random.Range(0, GetCandyPrefab.Length)];
                    if ((int)levelsApps.ltype == 1)
                    {
                        int[] randomplus5sec = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                        int plus5sec = randomplus5sec[Random.Range(0, randomplus5sec.Length)];
                        if ((int)levelsApps.ltype == 1 && plus5sec == coll) { pref = GetCandieSecons[Random.Range(0, GetCandieSecons.Length)]; }
                    }
                    var initgem = ((GameObject)Instantiate(pref.gameObject, new Vector3(visibleSize[coll].x, visibleSize[coll].y, -0.1f), Quaternion.identity)).GetComponent<HitCandy>();
                    initgem.transform.SetParent(GetArrays.transform);
                    g.OnInit(initgem);
                    if (GetArrays.SizeY - g.row > afterMatch.MaxDistance)
                    {
                        afterMatch.MaxDistance = GetArrays.SizeY - g.row;
                    }
                    afterMatch.AddGemms(g);
                }
                if (IsNulls(g.row, coll) == true)//
                {
                    if (GetArrays.gems[g.row, coll].hitGem.type == "ingredient" + 0)//
                    { levelsApps.ingCtar[0] -= 1; }
                    if (GetArrays.gems[g.row, coll].hitGem.type == "ingredient"+1)//
                    { levelsApps.ingCtar[0] -= 1; }
                    //Destroy(GetArrays.gems[g.y, coll].hitGem.gameObject);
                }
            }
        }
        return afterMatch;
    }
    protected void MoveToSquare()
    {
        ///OpenAppLevel.THIS.sq
    }
    /// <summary>
    /// Начинаем собирать совпадения
    /// </summary>
    /// <param name="raycast2"></param>
    /// <returns></returns>
    IEnumerator TryMatch(RaycastHit2D raycast2)
    {
        HitCandy GetHitGem2 = bubble2;
        if(GetHitGem.type=="swirl"&&GetHitGem2.type!= "ingredient"+0&&GetHitGem2.type != "ingredient" + 1)
        {
            GetHitGem.type = GetHitGem2.type;
        }
        if (GetHitGem2.type == "swirl"&& GetHitGem.type != "ingredient" + 0&&GetHitGem.type != "ingredient"+1)
        {
            GetHitGem2.type = GetHitGem.type;
        }
        Block blockpr = null;
        if (CurrentBlk != null)
        {
            blockpr = CurrentBlk;
        }
        else { blockpr = GetHitGem.GetBlock; }
        var GetHitGemNeigbours = GetArrays.GetProp(blockpr, blockpr.row, blockpr.col);
        var GetHitGemNeigbours2 = GetArrays.GetProp(OpenAppLevel.THIS.blocksp[currentrow*OpenAppLevel.THIS.MaxX + currentcol],currentrow,currentcol);        
        var matchs = GetHitGemNeigbours.GetGems.Union(GetHitGemNeigbours2.GetGems).Distinct();        
        if (matchs.Count() < 3)
        {
            //GetHitGem.transform.TweenPosition(0.2f, GetHitGem2.transform.position);
            //GetHitGem2.transform.TweenPosition(0.2f, GetHitGem.transform.position);
            yield return new WaitForSeconds(0.2f);
            GetArrays.Lastsp();
            movecount += 1;
            if (Ending==1)
            {
                Ending = 0;
            }
        }
        string typebonus="";
        Block bonusGem = null;
        bool addsquareBonus=false;
        bool addswirlBonus = matchs.Count() == 5 && GetHitGemNeigbours.bt == 1 || GetHitGemNeigbours2.bt == 1;
        bool addBonus = matchs.Count() >= 4 && GetHitGemNeigbours.bt == 1 || GetHitGemNeigbours2.bt == 1;
        if(addBonus)
        {
            var sameTypeGem = GetHitGemNeigbours.gemms.Count() > 0 ? GetHitGem : GetHitGem2;
            bonusGem = sameTypeGem.GetBlock;
            if(matchs.Count()==5)
            {
                //sameTypeGem.type = "swirl";
            }
            typebonus = sameTypeGem.type;
            OpenAppLevel.THIS.StripeGameCount += 1;
        }
        while (matchs.Count()>=2)
        {
            NeighbourProp neighbour=new NeighbourProp();
            foreach(var i in matchs)
            {
                if (rightMouseButtonDown) 
                {
                    OpenAppLevel.THIS.printScores += 10;
                    //GetArrays.gems[i.row, i.col] = new Gem(i.row, i.col);
                    //GetArrays.gems[i.row, i.col].Nil();
                    if (i.candy != null &&i.dontDest==false) { Destroy(i.candy.gameObject); }
                    neighbour = GetArrays.GetProp(i, i.row, i.col);
                    //GetHitGem.transform.SetParent(GetHitGem.GetBlock.transform);
                    CreateSquaresBlocks(i.col, i.row,GetHitGem.GetBlock);
                    GetBlockMove = GetHitGem.GetBlock;
                    matchs = neighbour.GetGems;
                }
                else
                {
                    neighbour = GetArrays.GetProp(i, i.row, i.col);
                    CreateSquaresBlocks(i.col, i.row, CurrentBlk);

                    matchs = neighbour.GetGems;
                }
            }
            //ScaleMatch()
            if (addBonus)
            {
                CreateBonus(GetHitGem.GetBlock,currentrow,currentcol,typebonus);
            }
            addBonus = false;

            //var bottom = matchs.Select(gem => gem.col).Distinct();
            //var updateafterMatch = GetArrays.UpdateAfter(bottom);
            //var newSpawn = GetNeighbourProp(bottom);
            //int maxDistance = Mathf.Max(updateafterMatch.MaxDistance, newSpawn.MaxDistance);
            //Moved(maxDistance, newSpawn.GetGems);
            //Moved(updateafterMatch.MaxDistance, updateafterMatch.GetGems);
            if (neighbour.GetGems!=null)
            {
                neighbour.MaxDistance = 2;
                yield return new WaitForSeconds(0.05f * neighbour.MaxDistance);
                matchs = neighbour.GetGems;
                if (rightMouseButtonDown) { currentHitList = matchs; }
            }
            else { yield return new WaitForSeconds(0.05f); }
            // * Mathf.Max(updateafterMatch.MaxDistance, newSpawn.MaxDistance));            
            //matchs = GetArrays.GetNeighbours(newSpawn.GetGems).Union(GetArrays.GetNeighbours(updateafterMatch.GetGems)).Distinct();
            //matchs = GetArrays.GetNeighbours(updateafterMatch.GetGems).Distinct();
            
        }
        //GetHitGem = GetCandyPrefab[Random.Range(0, GetCandyPrefab.Length)];
        //bubble2 = GetHitGem;
        //BubleCursor.sprite = GetHitGem.GetComponent<SpriteRenderer>().sprite;
        if (Ending==1)
        {
            levelsApps.LoadLB();
            Ending = 2;
        }
        for (int i = 0; i < bug.Length; i++)
        {
            bug[i].transform.position = new Vector3(0, -123, 0);
        }
        if (rightMouseButtonDown) { state = 1; }
        else
        {
            state = 0;
        }
        //levelsApps.GetPause();
    }
    public void ender()
    {
        Ending = 0;
    }
    void Moved(int dist,IEnumerable<Block> gemsie)
    {
        float xc = levelsApps.blckWH();
        float yr = levelsApps.blckWH();
        foreach (var j in gemsie)
        {
                j.candy.transform.TweenPosition(0.05f * dist, (new Vector3(levelsApps.vector2position.x,levelsApps.vector2position.y)) + (new Vector3(j.col * xc, j.row * yr,-0.1f)));// new Vector2(-2.37f, -4.27f) + new Vector2(j.x * 0.7f, j.y * 0.7f));//
        }
    }
    /// <summary>
    /// уночтожить совпадения
    /// </summary>
    /// <param name="hitmatchs"></param>
    public void ScaleMatch(HitCandy hitmatchs)
    {
        if ((int)levelsApps.ltype == 1 && hitmatchs.seconds != 0) levelsApps.Limit += hitmatchs.seconds;
        if (levelsApps.modeLvl == 1)
        {
            string[] CollectItemsfromcolor = { "", "red", "purple", "blue1", "blue2" };
            for (int i = 0; i < CollectItemsfromcolor.Count(); i++)
            {
                if (hitmatchs.type == CollectItemsfromcolor[i])
                {
                    if (i == (int)levelsApps.collectItems[0]) { if(levelsApps.ingCtar[0]> 0) levelsApps.ingCtar[0] -= 1; }
                    if (i == (int)levelsApps.collectItems[1]) { if (levelsApps.ingCtar[1]> 0) levelsApps.ingCtar[1] -= 1; }
                }
            }
        }
        
        levelsApps.printScores += 10;
        
        if (levelsApps.blocksp[hitmatchs.GetGem.y * levelsApps.MaxX + hitmatchs.GetGem.x].modelvlsquare == 3)
        {
            levelsApps.NotColorSquare(hitmatchs.GetGem.x, hitmatchs.GetGem.y, sprite12);
            levelsApps.blockscount -= 1;
        }
        if (levelsApps.blocksp[hitmatchs.GetGem.y * levelsApps.MaxX + hitmatchs.GetGem.x].types != 0)
        {
            hitmatchs.GetGem.Nil();
            Destroy(hitmatchs.gameObject);
        }
    }
    public void Scale2Match(HitCandy hitmatchs)
    {
        if ((int)levelsApps.ltype == 1 && hitmatchs.seconds != 0) levelsApps.Limit += hitmatchs.seconds;
        if (levelsApps.modeLvl == 1)
        {
            string[] CollectItemsfromcolor = { "", "red", "purple", "blue1", "blue2" };
            for (int i = 0; i < CollectItemsfromcolor.Count(); i++)
            {
                if (hitmatchs.type == CollectItemsfromcolor[i])
                {
                    if (i == (int)levelsApps.collectItems[0]) { if (levelsApps.ingCtar[0] > 0) levelsApps.ingCtar[0] -= 1; }
                    if (i == (int)levelsApps.collectItems[1]) { if (levelsApps.ingCtar[1] > 0) levelsApps.ingCtar[1] -= 1; }
                }
            }
        }

        levelsApps.printScores += 10;
        //if (hitmatchs.GetBlock.candy != null)
        //{
        //    hitmatchs.GetBlock.Nil();
        //}
        hitmatchs.GetBlock.Nil();
        Destroy(hitmatchs.gameObject);
    }
    /// <summary>
    /// создать бонус
    /// </summary>
    /// <param name="bgem"></param>
    /// <param name="type"></param>
    public void CreateBonus(Block bgem,int rw,int cl,string type)
    {
        float xc = levelsApps.blckWH();
        float yr = levelsApps.blckWH();
        HitCandy hitCandynew = null;
        if (type == "swirl")
        {
            hitCandynew = swirlPrefab;
        }
        else
        {
            for (int i = 0; i < GetBonusPrefab.Length; i++)
            {
                if (GetBonusPrefab[i].type == type)
                {
                    hitCandynew = GetBonusPrefab[i];
                }
            }
        }        
        Vector2 vectorgem = levelsApps.vector2position + new Vector2(cl * xc, rw * yr);
        HitCandy bonuses = ((GameObject)Instantiate(hitCandynew.gameObject, new Vector3(vectorgem.x, vectorgem.y, -0.1f), Quaternion.identity)).GetComponent<HitCandy>();
        bonuses.transform.SetParent(GetArrays.transform);
        //GetArrays[bgem.row, bgem.col].OnInit(bonuses);
        //GetArrays.gems[bgem.row, bgem.col] = bonuses.GetGem;
        OpenAppLevel.THIS.blocksp[rw * OpenAppLevel.THIS.MaxX + cl].OnInit(bonuses);
        OpenAppLevel.THIS.blocksp[rw * OpenAppLevel.THIS.MaxX + cl] = bonuses.GetBlock;
        bonuses.GetBlock.bonus = 1;
        bonuses.isBonus = true;
        if (type == "swirl") { bonuses.type = type; bonuses.isSwirl = true;bonuses.isBonus = false; }
    }

}
