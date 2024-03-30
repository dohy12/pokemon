using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightQueueManager : MonoBehaviour
{
    public static FightQueueManager instance;
    public Queue<BattleEvent> battleEvents;


    private void Awake()
    {
        instance = this;
        battleEvents = new Queue<BattleEvent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class BattleEvent
    {
        public BTEventType evType;
        public int[] args;

        public BattleEvent(BTEventType evType, params int[] args)
        {
            this.evType = evType;
            this.args = args;
        }
    }


    public enum BTEventType
    {
        TRAINER,
        SUMMON,
        USESKILL,
        HITSKILL,
        BUFF,
        CHANGE,
        USEITEM,
        RUN
    }
}
