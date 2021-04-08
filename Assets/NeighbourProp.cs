using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NeighbourProp : MonoBehaviour
{
    public int MaxDistance { get; set; }
    public List<Block> gemms = new List<Block>();
    public int bt { get; set; }
    public int MatchType;
    public IEnumerable<Block> GetGems
    {
        get { return gemms.Distinct(); }
    }
    public void AddGemms(Block gem)
    {
        if (!gemms.Contains(gem)) gemms.Add(gem);
    }
    public bool BonusTypeWhite(int bt1)
    {
        return bt1 == 1;
    }
}
