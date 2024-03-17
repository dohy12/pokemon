using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EventManager : MonoBehaviour
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

    public GameObject eventObj = null;

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
    }

    private void SetEventProgress()
    {
        eventProgress.Add("mainEvent", 0);
        eventProgress.Add("FirstPokemon", -1);
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
                AddEventMove(0, Unit.Direc.DOWN, 1);
                AddEventMove(0, Unit.Direc.RIGHT, 2);

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
            if (eventProgress["mainEvent"] == 2)//
            {
                var pokeballNum = args[0];
                AddEventQuest(2005 + pokeballNum, 100 + pokeballNum, 103);
                ActiveNextEvent();
            }
        }
        else if(eventID>=100 && eventID <= 102)
        {
            GetItem();
            eventProgress["mainEvent"] = 3;
            eventProgress["FirstPokemon"] = eventID - 100;

            AddEventMove(3, Unit.Direc.DOWN, 2);
            var __left = eventProgress["FirstPokemon"] + 1;
            if (__left == 3)
            {
                __left = 0;
            }
            AddEventMove(3, Unit.Direc.RIGHT, 2 + __left);
            AddEventMove(3, Unit.Direc.UP, 1);
            AddEventDialog(3002);
        }
        else if (eventID == 103)
        {
            AddEventDialog(2004);
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
                {
                    AddEventDialog(1002);
                    ActiveNextEvent();
                }
                break;

            case 2://오박사
                if (eventProgress["mainEvent"] == 2)
                {
                    AddEventDialog(2004);
                    ActiveNextEvent();
                }
                break;

            case 3://크리스
                if (eventProgress["mainEvent"] == 2)
                {
                    AddEventDialog(3001);
                    ActiveNextEvent();
                }
                else if (eventProgress["mainEvent"] == 3)
                {
                    AddEventDialog(3002);
                    ActiveNextEvent();
                }
                break;

            case 18://마을여자
                AddEventDialog(18001);
                ActiveNextEvent();
                break;

            case 19://마을뚱보
                AddEventDialog(19001);
                ActiveNextEvent();
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
            DialogManager.instance.Active(tmpEvent.dialogID);
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
            DialogManager.instance.Active(tmpEvent.dialogID, 0, tmpEvent.questYes, tmpEvent.questNo);
        }

    }


    private void AddEventMove(int unitID, Unit.Direc direc, int count)
    {
        for (var i=0; i < count; i++)
        {
            events.Enqueue(new Event(EventKind.MOVE, unitID, (int)direc));
        }
    }

    private void AddEventDirec(int unitID, Unit.Direc direc, float poseTime)
    {
        events.Enqueue(new Event(EventKind.DIREC, unitID, (int)direc, poseTime));
    }

    private void AddEventExMark(int unitID, float poseTime)
    {
        events.Enqueue(new Event(EventKind.EXMARK, unitID, poseTime));
    }

    private void AddEventDialog(int dialogID)
    {
        events.Enqueue(new Event(EventKind.DIALOG, dialogID));
    }

    private void AddEventQuest(int dialogID, int questYes, int questNo)
    {
        events.Enqueue(new Event(EventKind.QUEST, dialogID, questYes, questNo));
    }

    class Event
    {
        public EventKind eventKind;//[0]케릭터 움직임, [1]대화창, [2]트레이너 배틀, [3]야생 포켓몬 배틀, [4]포켓몬 사진 출력, [5]케릭터 방향전환. [6]케릭터 느낌표
        public int unitID;
        public int direc;
        public int dialogID;
        public float poseTime;
        public int questYes;
        public int questNo;

        public Event(EventKind eventKind, int unitID, int direc)//움직임
        {
            this.eventKind = eventKind;
            this.unitID = unitID;
            this.direc = direc;
        }
       
        public Event(EventKind eventKind, int dialogID)//대화창
        {
            this.eventKind = eventKind;
            this.dialogID = dialogID;
        }

        public Event(EventKind eventKind, int unitID, int direc, float poseTime)//방향전환
        {
            this.eventKind = eventKind;
            this.unitID = unitID;
            this.direc = direc;
            this.poseTime = poseTime;
        }

        public Event(EventKind eventKind, int unitID, float poseTime)//느낌표
        {
            this.eventKind = eventKind;
            this.unitID = unitID;
            this.poseTime = poseTime;
        }

        public Event(EventKind eventKind, int dialogID, int questYes, int questNo)//퀘스트창
        {
            this.eventKind = eventKind;
            this.dialogID = dialogID;
            this.questYes = questYes;
            this.questNo = questNo;
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
        QUEST
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

}
