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
        if (eventID == 1)//������ ���Ŵ� �̺�Ʈ
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
        else if(eventID == 3)//���ϸ� �� ���� ������ �������� �Ҷ�
        {
            if (eventProgress["mainEvent"] == 2)
            {
                AddEventDirec(2, Unit.Direc.DOWN, 0.1f);
                AddEventDialog(2003);
                AddEventMove(0, Unit.Direc.UP, 2);

                ActiveNextEvent();
            }
            if (eventProgress["mainEvent"] == 3)//���ϸ� ���� ������ ������ �������� �Ҷ� ��Ʋ
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
        else if(eventID == 4)//���ϸ� �� ���� ���� ������ �������� �Ҷ�
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
            if (eventProgress["mainEvent"] == 2)
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
            AddEventDelete(false, __left);
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

    public void NpcEventStart(int npcID)//npc���� �� �ɾ��� ��
    {
        switch (npcID)
        {
            case 1://����
                if (eventProgress["mainEvent"] == 1)
                {
                    AddEventDialog(1002);
                    ActiveNextEvent();
                }
                break;

            case 2://���ڻ�
                if (eventProgress["mainEvent"] == 2)
                {
                    AddEventDialog(2004);
                    ActiveNextEvent();
                }
                else
                {
                    AddEventDialog(2008);
                    ActiveNextEvent();
                }
                break;

            case 3://ũ����
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

            default:
                AddEventDialog(npcID*1000 + 001);
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
        ev.questYes = questYes;
        ev.questNo = questNo;
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

    class Event
    {
        public EventKind eventKind;//[0]�ɸ��� ������, [1]��ȭâ, [2]Ʈ���̳� ��Ʋ, [3]�߻� ���ϸ� ��Ʋ, [4]���ϸ� ���� ���, [5]�ɸ��� ������ȯ. [6]�ɸ��� ����ǥ
        public int unitID;
        public int direc;
        public int dialogID;
        public float poseTime;
        public int questYes;
        public int questNo;
        public bool deleteKind;

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
        FIGHT
    }



    private void GetItem()
    {
        Debug.Log("test");
        DialogObject dialogObject = eventObj.GetComponent<DialogObject>();
        if (dialogObject.objKind == 1) //���ϸ�
        {
            AddEventDialog(99023);
        }
        else if(dialogObject.objKind == 2) //������
        {
            AddEventDialog(99024);
        }

        Destroy(eventObj);
    }

}
