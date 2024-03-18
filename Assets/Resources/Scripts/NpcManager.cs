using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public static NpcManager npcManager;
    public Dictionary<int, Unit> npcs;
    public Dictionary<int, HealMachine> healMachines;

    private void Awake()
    {
        npcManager = this;
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
        npcs = new Dictionary<int, Unit>();

        npcs.Add(0, Player.player);

        for (var i=0; i< transform.childCount; i++)
        {
            var unit = transform.GetChild(i).GetComponent<Unit>();
            npcs.Add(unit.npcId, unit);
        }
        
        healMachines = new Dictionary<int, HealMachine>();
        
        var heals = transform.parent.Find("HealMachines");
        for (var i=0; i< heals.childCount; i++)
        {
            var heal = heals.GetChild(i).GetComponent<HealMachine>();
            healMachines.Add(heal.machineID, heal);
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
