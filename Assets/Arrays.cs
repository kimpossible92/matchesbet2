using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Arrays : MonoBehaviour {
    [SerializeField] OpenAppLevel levelsManager2;
    public static Arrays THIS;
    public int SizeX = 9;
    public int SizeY = 11;
    public Gem[,] gems;
    Gem GetGem1;
    Gem GetGem2;
    protected List<HitCandy> currentHits;
    public Gem this[int r, int c]
    {
        get { return gems[r, c]; }
        set { gems[r, c] = value; }
    }
    public HitCandy[] ingredientsGems;
    public HitCandy[] ingredientsGems2;
    public HitCandy IngredientCurrent;
    public Gem currentGem;
    protected bool rightMouseButtonDown = false;
    // Use this for initialization
    void Start() {
    }
    private void Awake()
    {
        THIS = this;
        gems = new Gem[SizeY, SizeX];
        for (int row = 0; row < SizeY; row++)
        {
            for (int c = 0; c < SizeX; c++)
            {
                gems[row, c] = new Gem(row, c);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rightMouseButtonDown = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            rightMouseButtonDown = false;
        }
    }
    public void OnSwapping(Gem gem1, Gem gem2)
    {
        GetGem1 = gem1;
        GetGem2 = gem2;

        HitCandy hitGem = gem1.hitGem;
        gem1.OnInit(gem2.hitGem);
        gem2.OnInit(hitGem);
    }
    public void Lastsp()
    {
        //OnSwapping(GetGem1, GetGem2);
    }
    /// <summary>
    /// Ienumerable совпадений
    /// </summary>
    /// <param name="gemsIe"></param>
    /// <returns></returns>
    public IEnumerable<Block> GetNeighbours(IEnumerable<Block> gemsIe)
    {
        List<Block> gemslist = new List<Block>();
        foreach (var g in gemsIe)
        {
            gemslist.AddRange(GetProp(g,g.row,g.col).GetGems);
        }
        return gemslist.Distinct();
    }
    public NeighbourProp GetProp2(Block gem, int rw, int cl)
    {
        NeighbourProp neighbour = new NeighbourProp();
        IEnumerable<Block> nhMatch = MatchesHorrizontally(gem, rw, cl);
        IEnumerable<Block> nvMatch = MatchesVertically(gem, rw, cl);
        if (GetBonusType1(nhMatch))
        {
            if (neighbour.bt == 0) { neighbour.MatchType = 1; neighbour.bt = 1; }
        }
        if (GetBonusType1(nvMatch))
        {
            if (neighbour.bt == 0) { neighbour.MatchType = 2; neighbour.bt = 1; }
        }
        //if (GetBonusType2(nhMatch))
        //{
        //    nhMatch = GetEntireCol(gem);
        //}
        //if (GetBonusType2(nvMatch))
        //{
        //    nvMatch = GetEntireRow(gem);
        //}
        //if (GetSwirlType1(nhMatch))
        //{
        //    nhMatch = GetEntire5Match(gem);
        //}
        //if (GetSwirlType1(nvMatch))
        //{
        //    nvMatch = GetEntire5Match(gem);
        //}
        foreach (var g in nhMatch)
        {
            neighbour.gemms.Add(g);
            OpenAppLevel.THIS.printScores += g.addScore;
        }
        foreach (var g in nvMatch)
        {
            neighbour.gemms.Add(g);
            OpenAppLevel.THIS.printScores += g.addScore;
        }
        return neighbour;
    }
    /// <summary>
    /// совпадения по вертикали и горизонтали
    /// </summary>
    /// <param name="gem"></param>
    /// <returns></returns>
    public NeighbourProp GetProp(Block gem,int rw,int cl)
    {
        NeighbourProp neighbour = new NeighbourProp();
        IEnumerable<Block> nhMatch = MatchesHorrizontally(gem,rw,cl);
        IEnumerable<Block> nvMatch = MatchesVertically(gem,rw,cl);
        if (GetBonusType1(nhMatch))
        {
            if (neighbour.bt == 0) { neighbour.MatchType = 1; neighbour.bt = 1; }
        }
        if (GetBonusType1(nvMatch))
        {
            if (neighbour.bt == 0) { neighbour.MatchType = 2; neighbour.bt = 1; }
        }
        //if (GetBonusType2(nhMatch))
        //{
        //    nhMatch = GetEntireCol(gem);
        //}
        //if(GetBonusType2(nvMatch))
        //{
        //    nvMatch = GetEntireRow(gem);
        //}
        //if(GetSwirlType1(nhMatch))
        //{
        //    nhMatch = GetEntire5Match(gem);
        //}
        //if (GetSwirlType1(nvMatch))
        //{
        //    nvMatch = GetEntire5Match(gem);
        //}
        foreach (var g in nhMatch)
        {
            neighbour.gemms.Add(g);
            OpenAppLevel.THIS.printScores += g.addScore;
        }
        foreach (var g in nvMatch)
        {
            neighbour.gemms.Add(g); //if (neighbour.gemms.Contains(g)) neighbour.gemms.Add(g);
            OpenAppLevel.THIS.printScores += g.addScore;
        }        
        return neighbour;
    }
    /// <summary>
    /// добавить весь ряд
    /// </summary>
    /// <param name="gem"></param>
    /// <returns></returns>
    public IEnumerable<Block> GetEntireRow(Block gem)
    {
        List<Block> vermatch = new List<Block> { gem };
        int rower1 = gem.row;
        int col1 = gem.col;
        for (int row = 0; row < SizeY; row++)
        {
            if (IsNulls(row, col1) == false&&OpenAppLevel.THIS.blocksp[row*OpenAppLevel.THIS.MaxX+col1].candy.type!= "ingredient"+0&& OpenAppLevel.THIS.blocksp[row*OpenAppLevel.THIS.MaxX+col1].candy.type != "ingredient"+1) vermatch.Add(OpenAppLevel.THIS.blocksp[row*OpenAppLevel.THIS.MaxX+col1]);
        }
        return vermatch;
    }
    /// <summary>
    /// добавить всю строку
    /// </summary>
    /// <param name="gem"></param>
    /// <returns></returns>
    public IEnumerable<Block> GetEntireCol(Block gem)
    {
        List<Block> hormatch = new List<Block> { gem };
        int row1 = gem.row;
        for (int col = 0; col < SizeX; col++)
        {
            if (IsNulls(row1, col) == false&&OpenAppLevel.THIS.blocksp[row1*OpenAppLevel.THIS.MaxX+col].candy.type != "ingredient"+0&& OpenAppLevel.THIS.blocksp[row1*OpenAppLevel.THIS.MaxX+col].candy.type != "ingredient" + 1) hormatch.Add(OpenAppLevel.THIS.blocksp[row1*OpenAppLevel.THIS.MaxX+col]);
        }
        return hormatch;
    }
    public IEnumerable<Block> GetEntireAlls(Block gem)
    {
        List<Block> matchesall = new List<Block> { gem};
        int row1 = gem.row;
        int col1 = gem.col;
        for (int row = gem.row - 1; row < gem.row + 2; row++)
        {
            for (int col = gem.col - 1; col < gem.col + 2; col++)
            {
                if (gems[row, col] != null)
                {
                    if(IsNulls(row1, col) == false) matchesall.Add(OpenAppLevel.THIS.blocksp[row * OpenAppLevel.THIS.MaxX + col]);//
                }
            }
        }
        return matchesall;
    }
    public IEnumerable<Block> GetEntire5Match(Block gem)
    {
        List<Block> matchesall = new List<Block> { gem };
        for(int row=0;row<SizeY;row++)
        {
            for(int col=0;col<SizeX;col++)
            {
                if((OpenAppLevel.THIS.blocksp[row * OpenAppLevel.THIS.MaxX + col]).match3(gem))
                {
                    if (IsNulls(row, col) == false) matchesall.Add(OpenAppLevel.THIS.blocksp[row * OpenAppLevel.THIS.MaxX + col]);
                }
            }
        }
        return matchesall;
    }
    /// <summary>
    /// MatchesHorrizontally
    /// </summary>
    /// <param name="Block"></param>
    /// <returns></returns>
    private IEnumerable<Block> MatchesHorrizontally(Block gem, int rw, int cl)
    {
        List<Block> hormatch = new List<Block>();
        hormatch.Add(gem);

        for (int col = cl - 1; col >= 0; col--)
        {
            if (gem != null && gem.col != 0)
            {
                Block l = OpenAppLevel.THIS.blocksp[gem.row * OpenAppLevel.THIS.MaxX + col];
                if (l.match3(gem))
                {
                    //if (l.candy != null && rightMouseButtonDown) { MoveLayer.THIS.addCandies(l.candy); }
                    l.savecol = l.col - gem.col;
                    l.saverow = l.row - gem.row;
                    hormatch.Add(l);
                }
                else
                {
                    break;
                }
            }
        }
        for (int col = cl + 1; col < SizeX; col++)
        {
            if (gem != null && gem.col != SizeX - 1)
            {
                Block r = OpenAppLevel.THIS.blocksp[gem.row * OpenAppLevel.THIS.MaxX + col];
                if (r.match3(gem))
                {
                    //if (r.candy != null && rightMouseButtonDown) { MoveLayer.THIS.addCandies(r.candy); }
                    r.savecol = r.col - gem.col;
                    r.saverow = r.row - gem.row;
                    hormatch.Add(r);
                }
                else
                {
                    break;
                }
            }
        }
        if (hormatch.Count() >= 1 && GetSwirlType1(hormatch))
        {

        }
        else if (hormatch.Count() < 2)
        {
            hormatch.Clear();
        }

        return hormatch.Distinct();
    }
    Block GetBlockload(int row,int col)
    {
       return OpenAppLevel.THIS.blocksp[row * OpenAppLevel.THIS.MaxX + col];
    }
    /// <summary>
    /// MatchesVertically
    /// </summary>
    /// <param name="Block"></param>
    /// <returns></returns>
    private IEnumerable<Block> MatchesVertically(Block gem, int rw, int cl)
    {
        List<Block> vermatch = new List<Block>();

        vermatch.Add(gem);

        for (int row = rw - 1; row >= 0; row--)
        {
            if (gem != null && gem.row != 0)
            {
                Block down = OpenAppLevel.THIS.blocksp[row * OpenAppLevel.THIS.MaxX + gem.col];
                if (down.match3(gem))
                {
                    //if (down.candy!=null&& rightMouseButtonDown) { MoveLayer.THIS.addCandies(down.candy); }
                    down.savecol = down.col - gem.col;
                    down.saverow = down.row - gem.row;
                    vermatch.Add(down);
                }
                else
                {
                    break;
                }
            }
        }

        for (int row = rw + 1; row < SizeY; row++)
        {
            if (gem != null && gem.row != SizeY - 1)
            {
                Block up = OpenAppLevel.THIS.blocksp[row * OpenAppLevel.THIS.MaxX + gem.col];
                if (up.match3(gem))
                {
                    //if (up.candy != null && rightMouseButtonDown) { MoveLayer.THIS.addCandies(up.candy); }
                    up.savecol = up.col - gem.col;
                    up.saverow = up.row - gem.row;
                    vermatch.Add(up);
                }
                else
                {
                    break;
                }
            }
        }

        if (vermatch.Count() >= 1 && GetSwirlType1(vermatch))
        {

        }
        else if (vermatch.Count() < 2)
        {
            vermatch.Clear();
        }
        return vermatch.Distinct();
    }

    /// <summary>
    /// Collapse
    /// </summary>
    /// <param name="colum"></param>
    /// <returns></returns>
    public UpdateAfterMatch UpdateAfter(IEnumerable<int> colum)
    {
        UpdateAfterMatch afterMatch = new UpdateAfterMatch();
        foreach (var c in colum)
        {
            for (int row = 0; row < SizeY - 1; row++)
            {
                if (GetBlockload(row, c).candy == null&&IsNulls(row, c) == false)
                {
                    for (int row2 = row + 1; row2 < SizeY; row2++)
                    {
                        if (!GetBlockload(row2, c).INull&& IsNulls(row2, c) == false)
                        {                            
                            Block gem1 = GetBlockload(row, c);
                            Block gem2 = GetBlockload(row2, c);
                            gem1.OnInit(gem2.candy);
                            gem2.Nil();
                            afterMatch.MaxDistance = Mathf.Max(row2 - row, afterMatch.MaxDistance);
                            afterMatch.AddGemms(gem1);
                            if (IsNulls(row, c) == true|| levelsManager2.blocksp[row * levelsManager2.MaxX + c].types == 0) Destroy(GetBlockload(row, c).candy.gameObject); 
                            if (IsNulls(row2, c) == true|| levelsManager2.blocksp[row2 * levelsManager2.MaxX + c].types == 0) Destroy(GetBlockload(row2, c).candy.gameObject); 
                            break;
                        }
                    }
                }
            }
        }

        return afterMatch;
    }
    public bool IsNulls(int row,int col)
    {
        if (levelsManager2.blocksp[row*levelsManager2.MaxX+col].types==0)
        {
            return true;
        }
        return false;
    }
    public IEnumerable<Gem> GetEmptyColumnsInfo(int column)
    {
        List<Gem> emptyMatches = new List<Gem>();
        for (int row = 0; row < SizeY; row++)
        {
            if (gems[row, column] == null) emptyMatches.Add(new Gem(row, column) { y = row, x = column });
        }
        return emptyMatches;
    }
    public IEnumerable<Block> NullGemsonc(int collum)
    {
        List<Block> gemsnull = new List<Block>();
        for (int i = 0; i < SizeY; i++)
        {
            if (gems[i, collum].INull||IsNulls(i,collum)==true||levelsManager2.blocksp[i*levelsManager2.MaxX+collum].types==0)
            {
                gemsnull.Add(GetBlockload(i, collum));
            }
        }
        return gemsnull;
    }
    public bool GetBonusType1(IEnumerable<Block> gemses)
    {
        if(gemses.Count()>=4)
        {
            foreach(var g in gemses)
            {
                if (g.bonus == 0) return true;
            }
        }
        return false;
    }
    /// <summary>
    /// определяем бонус
    /// </summary>
    /// <param name="gemses"></param>
    /// <returns></returns>
    public bool GetBonusType2(IEnumerable<Block> gemses)
    {
        if(gemses.Count()>=3)
        {
            foreach(var g in gemses)
            {
                if (g.candy.isBonus==true) { return true; }
            }
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gemses"></param>
    /// <returns></returns>
    public bool GetSwirlType1(IEnumerable<Block> gemses)
    {
        if(gemses.Count()>=2)
        {
            foreach(var g in gemses)
            {
                if (g.candy.isSwirl == true) { return true; }
            }
        }
        return false;
    }
}