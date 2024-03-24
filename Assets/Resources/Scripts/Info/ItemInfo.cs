using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public static ItemInfo instance;
    public Dictionary<int, Item> info;

    private void Awake()
    {
        instance = this;
    }

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
        info.Add(++itmID, new Item(itmID, "지도", Type.IMPOTANT, 0));         //0
        info.Add(++itmID, new Item(itmID, "낡은낚시대", Type.IMPOTANT, 0));   //1

        info.Add(++itmID, new Item(itmID, "상처약", Type.HEAL, 200));//2
        info.Add(++itmID, new Item(itmID, "좋은상처약", Type.HEAL, 400));//3
        info.Add(++itmID, new Item(itmID, "고급상처약", Type.HEAL, 1000));//4
        info.Add(++itmID, new Item(itmID, "마비치료제", Type.HEAL, 100));//5
        info.Add(++itmID, new Item(itmID, "해독제", Type.HEAL, 100));//6
        info.Add(++itmID, new Item(itmID, "잠깨는약", Type.HEAL, 100));//7
        info.Add(++itmID, new Item(itmID, "동상치료제", Type.HEAL, 100));//8
        info.Add(++itmID, new Item(itmID, "만병통치약", Type.HEAL, 500));//9
        info.Add(++itmID, new Item(itmID, "기력의조각", Type.HEAL, 1500));//10

        info.Add(++itmID, new Item(itmID, "몬스터볼", Type.BALL, 200));//11
        info.Add(++itmID, new Item(itmID, "슈퍼볼", Type.BALL, 600));//12
        info.Add(++itmID, new Item(itmID, "하이퍼볼", Type.BALL, 1200));//13

        info.Add(++itmID, new Item(itmID, "금구슬", Type.NONEEFF, 5000));//14
    }

    public class Item
    {
        public int id;
        public string name;
        public Type type;
        public int price;

        public Item(int id, string name, Type type, int price)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.price = price;
        }
    }

    public enum Type
    {
        HEAL,
        BALL,
        IMPOTANT,
        NONEEFF
    }
}
