using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public Dictionary<int, Item> items;

    private void Awake()
    {
        Instance = this;
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
        items = new Dictionary<int, Item>();
        
        for(var i=0; i<transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var item = child.GetComponent<Item>();
            if (item != null)
            {
                items.Add(i, item);
            }
        }
    }

    public void DeleteItem(int id, bool isEvent)
    {
        Debug.Log("아이템 제거");
        Destroy(items[id].gameObject);

        if (isEvent)
        {
            EventManager.instance.ActiveNextEvent();
        }
    }
}
