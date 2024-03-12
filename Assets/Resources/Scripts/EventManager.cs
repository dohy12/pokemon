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

    private Dictionary<string, int> eventProgress;

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
    }

    private void Init()
    {
        events = new Queue<Event>();
        eventProgress = new Dictionary<string, int>();

        SetEventProgress();
    }

    private void SetEventProgress()
    {
        eventProgress.Add("mainEvent", 0);
    }

    public void StartEvent(int eventID)
    {        
        switch (eventID)
        {
            case 1://엄마가 말거는 이벤트
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
                break;

            case 2://연구소 입장
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
                break;

            case 3:
                if (eventProgress["mainEvent"] == 2)//포켓몬 안 고르고 나갈려고 할때
                {
                    AddEventDialog(2003);
                    AddEventMove(0, Unit.Direc.UP, 2);

                    ActiveNextEvent();
                }
                break;


            default:
                isEvent = false;
                break;
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

    private void AddEventDialog(int dialogID)
    {
        events.Enqueue(new Event(EventKind.DIALOG, dialogID));
    }

    class Event
    {
        public EventKind eventKind;//[0]케릭터 움직임, [1]대화창, [2]트레이너 배틀, [3]야생 포켓몬 배틀, [4]포켓몬 사진 출력, [5]케릭터 방향전환
        public int unitID;
        public int direc;
        public int dialogID;
        public float poseTime;

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
    }
    
    enum EventKind
    {
        MOVE,
        DIALOG,
        BATTLE_TRAINER,
        BATTLE_POKEMON,
        SHOW_POKEMON,
        DIREC
    }
    
}
