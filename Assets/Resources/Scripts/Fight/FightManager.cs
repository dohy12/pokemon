
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

    private int hpTarget = -1;
    private float prevHp;
    private float hpDelta;
    private RectTransform targetHpBar;
    private TMP_Text targetHpText;
    private float hpbarMaxWidth = 0;
    private float hpbarHeight = 0;

    private bool isActive;
    public bool isWaitTime = true;
    private float waitTimeCh = 0f;

    private int statusHitTarget = 0;
    private bool isStatushit = false;
    private float statusHitCh = 0f;

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
        if(isActive)
        {
            HpUpdate();
            WaitTimeUpdate();
            StatusHitUpdate();
        }
        
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

        isActive = true;
    }

    void UnActive()
    {
        isActive = false;
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

    private void HpUpdate()
    {
        if (hpTarget != -1)
        {
            if (prevHp > pokes[hpTarget].hp)
            {
                prevHp -= Time.deltaTime * hpDelta;                 
            }
            else
            {
                prevHp = pokes[hpTarget].hp;
            }

            var hpbarWidth = hpbarMaxWidth * prevHp/ pokes[hpTarget].stat[0];

            targetHpBar.sizeDelta = new Vector2 (hpbarWidth, hpbarHeight);
            targetHpText.text = (int)prevHp + " / " + pokes[hpTarget].stat[0];

            if (prevHp == pokes[hpTarget].hp)
            {
                hpTarget = -1;
            }
        }
    }

    private void WaitTimeUpdate()
    {
        if (isWaitTime)
        {
            waitTimeCh += Time.deltaTime * 10f;

            var yy = Mathf.Sin(waitTimeCh) * 3f;

            if (waitTimeCh > 2* Mathf.PI)
            {
                waitTimeCh = 0;
            }

            var tmpRect = (RectTransform)pokeSpritess[0].transform;
            tmpRect.anchoredPosition = new Vector2(imgStartPos[0].x, imgStartPos[0].y + yy - 3f);

        }
    }

    

    public void HpEvent(int target, int damage)
    {
        var tmp = pokes[target].hp;
        pokes[target].hp -= damage;
        if (pokes[target].hp < 0) { pokes[target].hp = 0; }

        prevHp = tmp;
        hpDelta = prevHp - pokes[target].hp;
        hpTarget = target;

        if (hpTarget == 0)
        {
            hpbarMaxWidth = 185.54f;
            hpbarHeight = 11.597f;
            targetHpBar = (RectTransform)transform.Find("Status").Find("Hpbar");
            targetHpText = transform.Find("Status").Find("HpText").GetComponent<TMP_Text>();
        }
        if (hpTarget == 1)
        {
            hpbarMaxWidth = 188.088f;
            hpbarHeight = 11.8f;
            targetHpBar = (RectTransform)transform.Find("Status2").Find("Hpbar");
            targetHpText = transform.Find("Status2").Find("HpText").GetComponent<TMP_Text>();
        }

    }

    private void StatusHitUpdate()
    {
        if (isStatushit)
        {
            statusHitCh += Time.deltaTime * 4* Mathf.PI * 3;
            var yy = Mathf.Sin(statusHitCh) * 10;

            if (statusHitCh >= 4 * Mathf.PI)
            {
                statusHitCh = 0;
                isStatushit = false;
                yy = 0f;
            }           

            RectTransform rectT;
            if (statusHitTarget == 0)
            {
                rectT = (RectTransform)statuses[0].obj;
                rectT.anchoredPosition = new Vector2(-240f, -213f + yy);
            }
            else
            {
                rectT = (RectTransform)statuses[1].obj;
                rectT.anchoredPosition = new Vector2(-161f, 174f + yy);
            }

            var alphaCh = Mathf.Cos(statusHitCh * 2);
            float alpha;
            if (alphaCh > 0) { alpha = 1.0f; }
            else { alpha = 0f; }

            pokeSpritess[statusHitTarget].color = new Color(1, 1, 1, alpha);
        }
    }

    public void StatusHitActive(int target)
    {
        isStatushit = true;
        statusHitTarget = target;
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
        public Transform obj;
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
