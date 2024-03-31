using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PokemonInfo;

public class FightQueueManager : MonoBehaviour
{
    public static FightQueueManager instance;
    public List<BattleEvent> battleEvents;
    private FightManager fight;

    private float eventCh = 0;
    private GlobalInput input;
    private DialogManager dialog;

    private bool isEvent = false;
    private int nextDialogID;

    private void Awake()
    {
        instance = this;
        battleEvents = new List<BattleEvent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fight = FightManager.instance;
        input = GlobalInput.globalInput;
        dialog = DialogManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        EventUpdate();
    }

    private void EventUpdate()
    {
        if (eventCh > 0)
            eventCh -= Time.deltaTime;
        
        if (eventCh <= 0)
        {
            if (battleEvents.Count == 0)
            {
                if (isEvent)
                {
                    dialog.UnActive();
                    isEvent = false;
                    BattleMenu1.instance.Active();
                }

                return;
            }
            var ev = battleEvents[0];
            eventCh = 1f;
            battleEvents.RemoveAt(0);
            input.InputStun(1.1f);

            if (ev.evType == BTEventType.USESKILL)
            {
                var moveID = ev.args[0];
                var pokemonID = fight.pokes[ev.target].id;                
                dialog.Active(99101, pokemonID, moveID);
                int other = Mathf.Abs(ev.target - 1);
                var nextEv = new BattleEvent(BTEventType.HITSKILL, other, moveID);

                battleEvents.Insert(0, nextEv);
            }
            else if (ev.evType == BTEventType.HITSKILL)
            {
                Debug.Log("hit");
                var moveID = ev.args[0];
                var move = PokemonSkillInfo.Instance.skills[moveID];
                int other = Mathf.Abs(ev.target - 1);
                var poke = fight.pokes[other]; //0체,1공,2방,3특공,4특방,5스피
                var otherPoke = fight.pokes[ev.target];

                int damage = -1;
                var sameType = 1f;
                if (move.type == poke.GetInfo().type1) sameType = 1.5f;
                if (move.type == poke.GetInfo().type2) sameType = 1.5f;

                var weekType1 = PokemonInfo.GetTypeBattle(move.type, otherPoke.GetInfo().type1);
                var weekType2 = PokemonInfo.GetTypeBattle(move.type, otherPoke.GetInfo().type2);
                if (move.kind == PokemonSkillInfo.SkillType.PHYSICAL)
                {         
                    damage = (int)(((((((poke.level * 2 / 5) + 2) * move.damge * poke.stat[1] / 50) / otherPoke.stat[2])) + 2) * Random.Range(90, 100) / 100 * sameType * weekType1 * weekType2);
                }
                else if (move.kind == PokemonSkillInfo.SkillType.SPECIAL)
                {
                    damage = (int)(((((((poke.level * 2 / 5) + 2) * move.damge * poke.stat[3] / 50) / otherPoke.stat[4])) + 2) * Random.Range(90, 100) / 100 * sameType * weekType1 * weekType2);
                }
                Debug.Log("dmg : " + damage);
                Debug.Log(weekType1 * weekType2);
                Debug.Log(PokemonInfo.TypeToString(move.type) + "," + PokemonInfo.TypeToString(otherPoke.GetInfo().type1));
                Debug.Log(PokemonInfo.TypeToString(move.type) + "," + PokemonInfo.TypeToString(otherPoke.GetInfo().type2));
                if (weekType1 * weekType2 != 1f)
                {
                    if (weekType1 * weekType2 > 1f)
                    {
                        //효과가 굉장했다.
                        var dialogID = 99102;
                        var nextEv = new BattleEvent(BTEventType.DIALOG, -1, dialogID);
                        battleEvents.Insert(0, nextEv);
                    }
                    if (weekType1 * weekType2 < 1f)
                    {
                        //효과가 좋지 못했다.
                        var dialogID = 99103;
                        var nextEv = new BattleEvent(BTEventType.DIALOG, -1, dialogID);
                        battleEvents.Insert(0, nextEv);
                    }
                    if (weekType1 * weekType2 == 0f)
                    {
                        //그러나 효과가 없었다.......
                        var dialogID = 99104;
                        var nextEv = new BattleEvent(BTEventType.DIALOG, -1, dialogID);
                        battleEvents.Insert(0, nextEv);
                    }
                }
            }
            else if (ev.evType == BTEventType.DIALOG)
            {
                var dialogID = ev.args[0];
                dialog.Active(dialogID);
            }
        }
    }

    public class BattleEvent
    {
        public BTEventType evType;
        public int target;
        public int[] args;

        public BattleEvent(BTEventType evType, int target, params int[] args)
        {
            this.evType = evType;
            this.target = target;
            this.args = args;
        }
    }

    public void Active(BTEventType type, params int[] args)
    {
        var playerPoke = fight.pokes[0];
        var enemyPoke = fight.pokes[1];
        
        float playerPriority;
        float enemyPriority = enemyPoke.stat[5] / 1000f;

        if (type == BTEventType.USESKILL)
        {
            playerPriority = playerPoke.stat[5]/1000f;            
        }
        else
        {
            playerPriority = 8f;
        }
        battleEvents.Add(new BattleEvent(type, 0, args));


        var eneEvent = new BattleEvent(BTEventType.USESKILL, 1, GetEnemyMove());
        if (playerPriority > enemyPriority)
        {
            battleEvents.Add(eneEvent);
        }
        else
        {
            battleEvents.Insert(0, eneEvent);
        }

        isEvent = true;
    }




    private int GetEnemyMove()
    {
        var poke = fight.pokes[1];
        var moveNum = 0;
        while (true)
        {
            moveNum = poke.skills[Random.Range(0, 4)];

            if (moveNum != 0)
                break;
        }
        return moveNum;
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
        RUN,
        DIALOG
    }


}
