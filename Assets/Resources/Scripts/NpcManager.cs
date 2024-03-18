using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public static NpcManager npcManager;
    public Dictionary<int, Unit> npcs;

    private void Awake()
    {
        npcManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void init()
    {
        npcs = new Dictionary<int, Unit>();

        npcs.Add(0, Player.player);

        for (var i=0; i< transform.childCount; i++)
        {
            var unit = transform.GetChild(i).GetComponent<Unit>();
            npcs.Add(unit.npcId, unit);
        }
        
    }

    public void DeleteNpc(int ncpID, bool isEvent)
    {
        Destroy(npcs[ncpID].gameObject);

        if (isEvent)
        {
            EventManager.instance.ActiveNextEvent();
        }
    }
}
