using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public static NpcManager npcManager;
    public Dictionary<int, Npc> npcs;

    private void Awake()
    {
        npcManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        npcs = new Dictionary<int, Npc>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
