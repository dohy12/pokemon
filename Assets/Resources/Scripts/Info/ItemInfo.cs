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
        info.Add(++itmID, new Item(itmID, "����", Type.IMPOTANT, 0));         //0
        info.Add(++itmID, new Item(itmID, "�������ô�", Type.IMPOTANT, 0));   //1

        info.Add(++itmID, new Item(itmID, "��ó��", Type.HEAL, 200));//2
        info.Add(++itmID, new Item(itmID, "������ó��", Type.HEAL, 400));//3
        info.Add(++itmID, new Item(itmID, "��޻�ó��", Type.HEAL, 1000));//4
        info.Add(++itmID, new Item(itmID, "����ġ����", Type.HEAL, 100));//5
        info.Add(++itmID, new Item(itmID, "�ص���", Type.HEAL, 100));//6
        info.Add(++itmID, new Item(itmID, "����¾�", Type.HEAL, 100));//7
        info.Add(++itmID, new Item(itmID, "����ġ����", Type.HEAL, 100));//8
        info.Add(++itmID, new Item(itmID, "������ġ��", Type.HEAL, 500));//9
        info.Add(++itmID, new Item(itmID, "���������", Type.HEAL, 1500));//10

        info.Add(++itmID, new Item(itmID, "���ͺ�", Type.BALL, 200));//11
        info.Add(++itmID, new Item(itmID, "���ۺ�", Type.BALL, 600));//12
        info.Add(++itmID, new Item(itmID, "�����ۺ�", Type.BALL, 1200));//13

        info.Add(++itmID, new Item(itmID, "�ݱ���", Type.NONEEFF, 5000));//14
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
