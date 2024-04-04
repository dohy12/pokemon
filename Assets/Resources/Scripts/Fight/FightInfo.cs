using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightInfo : MonoBehaviour
{
    public static FightInfo instance;
    public Dictionary<int, Info> infos;

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
        infos = new Dictionary<int, Info>();
        infos.Add(1, new Info(1, 15, 2, 9, 3)); //[0]이상해씨 5레벨
    }


    public class Info
    {
        public int fightID;
        public FightManager.PokeANDLevel[] npcPokes;


        public Info(int fightID, params int[] args)
        {
            this.fightID = fightID;

            npcPokes = new FightManager.PokeANDLevel[args.Length/2];
            for (int i = 0; i < args.Length; i+=2) 
            {
                var pokeID = args[i];
                var level = args[i+1];

                npcPokes[i / 2] = new FightManager.PokeANDLevel(pokeID, level);
            }
        }
    }

}
