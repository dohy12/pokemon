
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    private RectTransform rectT;
    public static FightManager instance;
    private Queue<Poke> enemyPokeQueue;
    public Poke[] pokes;

    private Image[] pokeSpritess;
    private Status[] statuses;

    private Vector2[] imgStartPos;

    public bool isTrainerBattle;

    private void Awake()
    {
        instance = this;
        enemyPokeQueue = new Queue<Poke>();
        pokes = new Poke[2];

        statuses = new Status[2];
        statuses[0] = new Status(transform.Find("Status"));
        statuses[1] = new Status(transform.Find("Status2"));

        pokeSpritess = new Image[2];
        pokeSpritess[0] = transform.Find("Image").GetChild(0).GetComponent<Image>();
        pokeSpritess[1] = transform.Find("Image2").GetChild(0).GetComponent<Image>();

        rectT = (RectTransform)transform;

        imgStartPos = new Vector2[2];
        imgStartPos[0] = new Vector2(5.6f, 25.58f);
        imgStartPos[1] = new Vector2(-3.906f, 9.5f);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Active(Poke myPoke, PokeANDLevel[] enemyPokes, bool isTrainerBattle)
    {       
        rectT.anchoredPosition = Vector2.zero;

        for (var i=0; i<enemyPokes.Length; i++)
        {
            enemyPokeQueue.Enqueue(enemyPokes[i].GetPoke());
        }

        pokes[0] = myPoke;
        pokes[1] = enemyPokeQueue.Peek();

        this.isTrainerBattle = isTrainerBattle;

        SetScreen();

        BattleMenu1.instance.Active();
    }

    void UnActive()
    {
        
    }

    void SetScreen()
    {
        for (var i = 0; i < 2; i++)
        {
            statuses[i].SetStatus(pokes[i]);
        }

        SetImage(pokeSpritess[0], PokeSpr.instance.backSprites[pokes[0].id], imgStartPos[0]);
        SetImage(pokeSpritess[1], PokeSpr.instance.sprites[pokes[1].id + 1], imgStartPos[1]);
    }


    public void Fight(int fightID, params int[] args)
    {
        Debug.Log("Fight! : " + fightID);
        //EventManager.instance.ActiveNextEvent();

        var myPoke = GameDataManager.instance.pokeList[0];
        PokeANDLevel[] enemyPokes;
        if (fightID != -1)
        {
            enemyPokes = FightInfo.instance.infos[fightID].npcPokes;
        }
        else
        {
            enemyPokes = new PokeANDLevel[1] {new PokeANDLevel(args[0], args[1]) };
        }
        Active(myPoke, enemyPokes, (fightID != -1));

    }

    public class PokeANDLevel
    {
        public int level;
        public int pokeID;

        public PokeANDLevel(int pokeID, int level)
        {
            this.level = level;
            this.pokeID = pokeID;
        }

        public Poke GetPoke()
        {
            return new Poke(pokeID, level);
        }
    }

    public class Status
    {
        private Transform obj;
        private TMP_Text name;
        private TMP_Text level;
        private TMP_Text hpText;
        private RectTransform hpBar;
        private RectTransform expBar;

        private Vector2 hpBarSize;
        private Vector2 expBarSize;

        public Status(Transform obj)
        {
            this.obj = obj;

            name = obj.Find("Name").GetComponent<TMP_Text>();
            level = obj.Find("Level").GetComponent<TMP_Text>();

            hpBar = (RectTransform)obj.Find("Hpbar");
            expBar = (RectTransform)obj.Find("Expbar");

            hpBarSize = hpBar.sizeDelta;
            expBarSize = expBar.sizeDelta;

            hpText = obj.Find("HpText").GetComponent<TMP_Text>();
        }
    
        public void SetStatus(Poke poke)
        {
            name.text = poke.GetInfo().name;
            level.text = "Lv." + poke.level;

            hpBar.sizeDelta = new Vector2(hpBarSize.x * poke.hp/ poke.stat[0], hpBarSize.y);
            expBar.sizeDelta = new Vector2(expBarSize.x * poke.exp/ poke.maxExp, expBarSize.y);

            hpText.text = poke.hp + " / " + poke.stat[0]; 
        }
    }

    private void SetImage(Image img, Sprite sprite, Vector2 zeroPos) 
    {
        var tmpT = (RectTransform)img.transform;
        tmpT.pivot = sprite.pivot/64f;
        tmpT.anchoredPosition = zeroPos;

        img.sprite = sprite;
    }
}
