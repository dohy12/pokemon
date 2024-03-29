using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using static Unit;

public class EventManager : MonoBehaviour, SelectUIRedirec
{
    public static EventManager instance;
    private Queue<Event> events;
    public static bool isEvent = false;
    private float poseTime = 0f;
    private bool isPose = false;

    public Dictionary<string, int> eventProgress;

    private GameObject exclamationMark;
    private bool isExMark = false;
    private float exMarkCheck = 0f;

    private int healMachineID = -1;
    private int healerID = -1;

    public GameObject eventObj = null;

    private GameDataManager data;

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
        if (isPose)
        {
            poseTime -= Time.deltaTime;
            if (poseTime < 0)
            {
                isPose = false;
                ActiveNextEvent();
            }
        }

        if (isExMark)
        {
            exMarkCheck -= Time.deltaTime;
            if (exMarkCheck < 0)
            {
                isExMark = false;
                UnActiveExMark();
            }
        }
    }

    private void SetExMark()
    {
        exclamationMark = transform.Find("ExclamationMark").gameObject;
    }

    private void ActiveExMark(Vector2 position, float poseTime)
    {
        isExMark = true;
        exMarkCheck = poseTime;
        exclamationMark.transform.position = new Vector3(position.x, position.y+1f, 0);
        var spriteRenderer = exclamationMark.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
    }

    private void UnActiveExMark()
    {
        var spriteRenderer = exclamationMark.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }


    private void Init()
    {
        events = new Queue<Event>();
        eventProgress = new Dictionary<string, int>();

        SetEventProgress();
        SetExMark();

        data = GameDataManager.instance;
    }

    private void SetEventProgress()
    {
        eventProgress.Add("mainEvent", 0);
        eventProgress.Add("FirstPokemon", -1); 
        eventProgress.Add("Npc13Battle", 0);
        eventProgress.Add("Npc9Battle", 0);
        eventProgress.Add("Npc10Battle", 0);
    }

    public void StartEvent(int eventID, params int[] args)
    {        
        if (eventID == 1)//엄마가 말거는 이벤트
        {
            if (eventProgress["mainEvent"] == 0)
            {
                AddEventMove(1, Unit.Direc.RIGHT, 2);
                AddEventMove(1, Unit.Direc.UP, 2);
                AddEventDialog(1001);
                AddEventMove(1, Unit.Direc.DOWN, 2);
                AddEventMove(1, Unit.Direc.LEFT, 2);

                ActiveNextEvent();
                eventProgress["mainEvent"] = 1;

            }
        }
        else if(eventID == 2)
        {
            if (eventProgress["mainEvent"] == 1)
            {
                AddEventMove(0, Unit.Direc.UP, 8);
                AddEventDialog(2001);
                AddEventDirec(2, Unit.Direc.RIGHT, 0.5f);
                AddEventDialog(2002);
                AddEventDirec(2, Unit.Direc.DOWN, 0.1f);

                ActiveNextEvent();
                eventProgress["mainEvent"] = 2;
            }
        }
        else if(eventID == 3)//포켓몬 안 고르고 연구소 나갈려고 할때
        {
            if (eventProgress["mainEvent"] == 2)
            {
                AddEventDirec(2, Unit.Direc.DOWN, 0.1f);
                AddEventDialog(2003);
                AddEventMove(0, Unit.Direc.UP, 2);

                ActiveNextEvent();
            }
            if (eventProgress["mainEvent"] == 3)//포켓몬 고르고 연구소 밖으로 나가려고 할때 배틀
            {
                var playerX = (int)NpcManager.npcManager.npcs[0].transform.position.x;
                var destinationX = -48;
                if (playerX == -48)
                    destinationX = -47;

                var distance = (int)(NpcManager.npcManager.npcs[3].transform.position.x - destinationX);
                AddEventDirec(3, Unit.Direc.DOWN, 0.1f);
                AddEventExMark(3, 0.5f);
                AddEventMove(3, Unit.Direc.LEFT, distance);
                AddEventMove(3, Unit.Direc.DOWN, 3);
                if (playerX == -47)
                {
                    AddEventDirec(3, Unit.Direc.RIGHT, 0.1f);
                    AddEventDirec(0, Unit.Direc.LEFT, 0.1f);
                }
                else
                {
                    AddEventDirec(3, Unit.Direc.LEFT, 0.1f);
                    AddEventDirec(0, Unit.Direc.RIGHT, 0.1f);
                }

                AddEventDialog(3003);
                AddEventFight(0);
                AddEventDialog(3004);
                AddEventMove(3, Unit.Direc.DOWN, 6);
                AddEventDelete(true, 3);


                eventProgress["mainEvent"] = 4;

                ActiveNextEvent();
            }
        }
        else if(eventID == 4)//포켓몬 안 고르고 마을 밖으로 나갈려고 할때
        {
            if (eventProgress["mainEvent"] == 1)
            {
                AddEventDirec(18, Unit.Direc.LEFT, 0.1f);
                AddEventExMark(18, 0.5f);
                AddEventDirec(0, Unit.Direc.RIGHT, 0.1f);
                AddEventDialog(18002);
                AddEventMove(0, Unit.Direc.RIGHT, 2);

                ActiveNextEvent();
            }
        }
        else if(eventID == 5)
        {
            if (eventProgress["Npc13Battle"] == 0)
            {
                NPCBattleFind(13);
            }
        }
        else if (eventID == 6)
        {
            if (eventProgress["Npc10Battle"] == 0)
            {
                NPCBattleFind(10);
            }
        }
        else if (eventID == 7)
        {
            if (eventProgress["Npc9Battle"] == 0)
            {
                NPCBattleFind(9);
            }
        }
        else if (eventID == 8)
        {
            if (eventProgress["mainEvent"] == 4)
            {
                Npc npc = (Npc)NpcManager.npcManager.npcs[5];

                var direc = npc.GetDirecToPlayer();
                var distance = npc.GetDistanceFromPlayer() - 1;

                AddEventDirec(5, (Unit.Direc)direc, 0.1f);
                AddEventExMark(5, 0.5f);
                if (distance > 0)
                {
                    AddEventMove(5, (Unit.Direc)direc, distance);
                }
                AddEventDialog(5002);

                ActiveNextEvent();
                eventProgress["mainEvent"] = 5;
            }
        }
        else if(eventID == 100)
        {
            if (eventProgress["mainEvent"] == 2)
            {
                var pokeballNum = args[0];
                AddEventPicture(pokeballNum * 3 + 1);
                AddEventQuest(2005 + pokeballNum, 101 + pokeballNum, 104);
                ActiveNextEvent();
            }
        }
        else if(eventID>=101 && eventID <= 103)
        {
            GetItem();
            eventProgress["mainEvent"] = 3;
            eventProgress["FirstPokemon"] = eventID - 101;
            AddEventAddPoke(eventProgress["FirstPokemon"] * 3, 5);

            AddEventMove(3, Unit.Direc.DOWN, 2);
            var __left = eventProgress["FirstPokemon"] + 1;
            if (__left == 3)
            {
                __left = 0;
            }
            AddEventMove(3, Unit.Direc.RIGHT, 2 + __left);
            AddEventMove(3, Unit.Direc.UP, 1);
            AddEventDialog(3002);
            AddEventDelete(false, __left);
            ActiveNextEvent();
        }
        else if (eventID == 104)
        {
            AddEventDialog(2004);
            ActiveNextEvent();
        }
        else if (eventID == 200)//연구소 힐
        {
            healMachineID = args[0];
            AddEventQuest(99026, 204, -1);
            ActiveNextEvent();
        }
        else if (eventID == 201)//간호순 힐
        {
            healMachineID = args[0];
            healerID = args[0];
            AddEventQuest(23001, 205, 202);
            ActiveNextEvent();
        }
        else if (eventID == 202)//간호순 인사
        {
            AddEventDirec(healerID, Unit.Direc.UP, 0.5f);
            AddEventDirec(healerID, Unit.Direc.DOWN, 0.1f);
            AddEventDialog(23002);
            ActiveNextEvent();
        }
        else if (eventID == 203)//간호순, 포켓몬이 없을 경우
        {
            healerID = args[0];
            AddEventDialog(23004);
            AddNextEvent(202);
            ActiveNextEvent();
        }
        else if (eventID == 204)
        {
            AddEventHeal(healMachineID);
            ActiveNextEvent();
        }
        else if (eventID == 205)
        {
            AddEventDirec(healerID, Unit.Direc.LEFT, 0.5f);
            AddEventHeal(healMachineID);
            AddEventDirec(healerID, Unit.Direc.DOWN, 0.2f);
            AddEventDialog(23003);
            AddNextEvent(202);
            ActiveNextEvent();
        }
        else if (eventID == 999)// 다이알로그 실행
        {
            AddEventDialog(args[0]);
            ActiveNextEvent();
        }
        else
        {
            isEvent = false;
        }     

    }

    public void NpcEventStart(int npcID)//npc한테 말 걸었을 때
    {       


        switch (npcID)
        {
            case 1://엄마
                if (eventProgress["mainEvent"] == 1)
                    StartEvent(999, 1002);
                else
                    StartEvent(999, 1003);
                break;

            case 2://오박사
                if (eventProgress["mainEvent"] == 2)
                    StartEvent(999, 2004);
                else
                    StartEvent(999, 2008);
                break;

            case 3://크리스
                if (eventProgress["mainEvent"] == 2)
                    StartEvent(999, 3001);
                else if (eventProgress["mainEvent"] == 3)
                    StartEvent(999, 3002);
                break;

            default:
                Npc npc = (Npc)NpcManager.npcManager.npcs[npcID];

                switch (npc.npcType)
                {
                    case Npc.Type.NORMAL: //일반 npc
                        StartEvent(999, npcID * 1000 + 001);
                        break;

                    case Npc.Type.BATTLE:
                        if (eventProgress.GetValueOrDefault("Npc" + npcID + "Battle", 1) == 0)
                        {
                            NPCBattleFind(npcID);
                        }
                        break;

                    case Npc.Type.NURSE:
                        if (eventProgress["mainEvent"] > 3)
                        {
                            StartEvent(201, npcID);
                        }
                        else
                        {
                            StartEvent(203, npcID);
                        }
                        break;

                    case Npc.Type.SHOP:
                        StartEvent(999, 25001);
                        Shop.instance.Active();
                        
                        break;
                }
                
                break;
        }
    }

    public void ActiveNextEvent(float poseTime)
    {
        isEvent = true;
        Player.player.inputStun = 0.05f;
        this.poseTime = poseTime;
        isPose = true;
    }

    public void ActiveNextEvent()
    {
        if (events.Count > 0)
        {
            isEvent = true;
            Player.player.inputStun = 0.05f;
            var tmpEvent = events.Dequeue();
            ActiveEvent(tmpEvent);
        }
        else
        {
            isEvent = false;
        }
    }

    private void ActiveEvent(Event tmpEvent)
    {
        if (tmpEvent.eventKind == EventKind.MOVE)
        {
            Unit unit = NpcManager.npcManager.npcs[tmpEvent.unitID];
            unit.MoveOrder(tmpEvent.direc);
            unit.isEventMove = true;
        }
        else if(tmpEvent.eventKind == EventKind.DIALOG)
        {
            DialogManager.instance.Active(tmpEvent.dialogID, null, DialogManager.Type.NORMAL);
        }
        else if(tmpEvent.eventKind == EventKind.DIREC)
        {
            Unit unit = NpcManager.npcManager.npcs[tmpEvent.unitID];
            unit.direc = tmpEvent.direc;
            unit.eventPose = tmpEvent.poseTime;
            unit.isEventPose = true;
        }
        else if(tmpEvent.eventKind == EventKind.EXMARK)
        {
            Unit unit = NpcManager.npcManager.npcs[tmpEvent.unitID];
            unit.eventPose = tmpEvent.poseTime;
            unit.isEventPose = true;

            ActiveExMark(unit.transform.position, tmpEvent.poseTime);
        }
        else if(tmpEvent.eventKind == EventKind.QUEST)
        {
            DialogManager.instance.Active(tmpEvent.dialogID, this, DialogManager.Type.QUEST, tmpEvent.arg1, tmpEvent.arg2);
        }
        else if(tmpEvent.eventKind == EventKind.DELETE)
        {
            if (tmpEvent.deleteKind)
            {
                NpcManager.npcManager.DeleteNpc(tmpEvent.unitID, true);
            }
            else
            {
                ItemManager.Instance.DeleteItem(tmpEvent.unitID, true);
            }
        }
        else if(tmpEvent.eventKind == EventKind.FIGHT)
        {
            FightManager.instance.Fight(tmpEvent.dialogID);
        }
        else if(tmpEvent.eventKind == EventKind.HEAL)
        {
            var machine = NpcManager.npcManager.healMachines[tmpEvent.unitID];
            machine.Active(data.pokeList.Count);
        }
        else if(tmpEvent.eventKind == EventKind.EVENT)
        {
            StartEvent(tmpEvent.unitID);
        }
        else if (tmpEvent.eventKind == EventKind.PICTURE)
        {
            PokemonPicture.instance.Active(tmpEvent.arg1);
        }
        else if (tmpEvent.eventKind == EventKind.ADDPOKE)
        {
            GameDataManager.instance.AddPokemon(tmpEvent.arg1, tmpEvent.arg2);     
        }

    }


    private void AddEventMove(int unitID, Unit.Direc direc, int count)
    {
        for (var i=0; i < count; i++)
        {
            Event ev = new Event();
            ev.eventKind = EventKind.MOVE;
            ev.unitID = unitID;
            ev.direc = (int)direc;
            events.Enqueue(ev);
        }
    }

    private void AddEventDirec(int unitID, Unit.Direc direc, float poseTime)
    {
        Event ev = new Event();
        ev.eventKind = EventKind.DIREC;
        ev.unitID = unitID;
        ev.direc = (int)direc;
        ev.poseTime = poseTime;
        events.Enqueue(ev);
    }

    private void AddEventExMark(int unitID, float poseTime)
    {
        Event ev = new Event();
        ev.eventKind = EventKind.EXMARK;
        ev.unitID = unitID;
        ev.poseTime= poseTime;
        events.Enqueue(ev);
    }

    private void AddEventDialog(int dialogID)
    {
        Event ev = new Event();
        ev.eventKind = EventKind.DIALOG;
        ev.dialogID = dialogID;
        events.Enqueue(ev);
    }

    private void AddEventQuest(int dialogID, int questYes, int questNo)
    {
        Event ev = new Event();
        ev.eventKind = EventKind.QUEST;
        ev.dialogID = dialogID;
        ev.arg1 = questYes;
        ev.arg2 = questNo;
        events.Enqueue(ev);
    }

    private void AddEventDelete(bool deleteKind, int unitID)
    {
        Event ev = new Event();
        ev.eventKind = EventKind.DELETE;
        ev.unitID = unitID;
        ev.deleteKind = deleteKind;
        events.Enqueue(ev);
    }

    private void AddEventFight(int fightID)
    {
        Event ev = new Event();
        ev.eventKind = EventKind.FIGHT;
        ev.dialogID = fightID;
        events.Enqueue(ev);
    }

    private void AddEventHeal(int machineID)
    {
        Event ev = new Event();
        ev.eventKind = EventKind.HEAL;
        ev.unitID = machineID;
        events.Enqueue(ev);
    }

    private void AddNextEvent(int eventID)
    {
        Event ev = new Event();
        ev.eventKind = EventKind.EVENT;
        ev.unitID = eventID;
        events.Enqueue(ev);
    }

    private void AddEventPicture(int pokeID)
    {
        Event ev = new Event();
        ev.eventKind = EventKind.PICTURE;
        ev.arg1 = pokeID;
        events.Enqueue(ev);
    }

    private void AddEventAddPoke(int pokeID, int level)
    {
        Event ev = new Event();
        ev.eventKind = EventKind.ADDPOKE;
        ev.arg1 = pokeID;
        ev.arg2 = level;
        events.Enqueue(ev);
    }

    class Event
    {
        public EventKind eventKind;
        public int unitID;
        public int direc;
        public int dialogID;
        public float poseTime;
        public bool deleteKind;
        public int arg1;
        public int arg2;
        public int arg3;

        public Event()
        {

        }
    }
    
    enum EventKind
    {
        MOVE,
        DIALOG,
        BATTLE_TRAINER,
        BATTLE_POKEMON,
        SHOW_POKEMON,
        DIREC,
        EXMARK,
        QUEST,
        DELETE,
        FIGHT,
        HEAL,
        EVENT,
        PICTURE,
        ADDPOKE
    }



    private void GetItem()
    {
        Debug.Log("test");
        DialogObject dialogObject = eventObj.GetComponent<DialogObject>();
        if (dialogObject.objKind == 1) //포켓몬
        {
            AddEventDialog(99023);
        }
        else if(dialogObject.objKind == 2) //아이템
        {
            AddEventDialog(99024);
        }

        Destroy(eventObj);
    }


    private void NPCBattleFind(int npcID)
    {
        Npc npc = (Npc)NpcManager.npcManager.npcs[npcID];

        var direc = npc.GetDirecToPlayer();
        var distance = npc.GetDistanceFromPlayer() - 1;

        AddEventDirec(npcID, (Unit.Direc)direc, 0.1f);
        AddEventExMark(npcID, 0.5f);
        if (distance > 0)
        {
            AddEventMove(npcID, (Unit.Direc)direc, distance);
        }
        AddEventDialog(npcID * 1000 + 001);
        AddEventFight(npc.fightID);
        AddEventDialog(npcID * 1000 + 002);
        ActiveNextEvent();
    }

    public void OnSelectRedirec(int num, params int[] args)
    {
        Debug.Log(num +" " + args[0] + " " + args[1]);
        StartEvent(args[num]);
    }

}
