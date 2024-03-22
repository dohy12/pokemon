using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public static ItemInfo instance;
    public Dictionary<int, Item> info;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        info = new Dictionary<int, Item>();
        var itmID = -1;
        info.Add(++itmID, new Item(itmID, "지도", Type.IMPOTANT));
        info.Add(++itmID, new Item(itmID, "낡은낚시대", Type.IMPOTANT));

        info.Add(++itmID, new Item(itmID, "상처약", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "좋은상처약", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "고급상처약", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "마비치료제", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "해독제", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "잠깨는약", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "동상치료제", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "만병통치약", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "기력의조각", Type.HEAL));

        info.Add(++itmID, new Item(itmID, "몬스터볼", Type.BALL));
        info.Add(++itmID, new Item(itmID, "슈퍼볼", Type.BALL));
        info.Add(++itmID, new Item(itmID, "하이퍼볼", Type.BALL));

        
    }

    public class Item
    {
        public int id;
        public string name;
        public Type type;

        public Item(int id, string name, Type type)
        {
            this.id = id;
            this.name = name;
            this.type = type;
        }
    }

    public enum Type
    {
        HEAL,
        BALL,
        IMPOTANT
    }
}
