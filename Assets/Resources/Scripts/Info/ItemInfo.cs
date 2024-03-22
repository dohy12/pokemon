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
        info.Add(++itmID, new Item(itmID, "����", Type.IMPOTANT));
        info.Add(++itmID, new Item(itmID, "�������ô�", Type.IMPOTANT));

        info.Add(++itmID, new Item(itmID, "��ó��", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "������ó��", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "��޻�ó��", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "����ġ����", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "�ص���", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "����¾�", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "����ġ����", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "������ġ��", Type.HEAL));
        info.Add(++itmID, new Item(itmID, "���������", Type.HEAL));

        info.Add(++itmID, new Item(itmID, "���ͺ�", Type.BALL));
        info.Add(++itmID, new Item(itmID, "���ۺ�", Type.BALL));
        info.Add(++itmID, new Item(itmID, "�����ۺ�", Type.BALL));

        
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
