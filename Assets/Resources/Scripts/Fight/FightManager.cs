
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class FightManager : MonoBehaviour
{
    private RectTransform rectT;
    public static FightManager instance;
    public List<Poke> enemyPokeList;
    public Poke[] pokes;

    public Image[] pokeSprites;
    public Image[] pokeEffs;
    public Image[] trainerSprites;
    public Image[] pokeballSprites;
    private Status[] statuses;

    public Vector2[] imgStartPos;

    
    public bool isTrainerBattle;

    private int hpTarget = -1;
    private float prevHp;
    private float hpDelta;
    private RectTransform targetHpBar;
    private TMP_Text targetHpText;
    private float hpbarMaxWidth = 0;
    private float hpbarHeight = 0;

    public bool isActive;
    public bool isWaitTime = true;
    private float waitTimeCh = 0f;

    private int statusHitTarget = 0;
    private bool isStatushit = false;
    private float statusHitCh = 0f;

    private bool isPokeHit = false;
    private float pokeHitCh = 0f;

    private bool isPokeDie = false;
    private float pokeDieCh = 0f;
    private int dieTarget = 0;

    public int enePokeIndex = 0;

    private GameObject battleScreenSwitch;
    private bool isScreenSwitch = false;
    private bool isScreenSwitchActiveCh = false;
    public float battleScreenCh = 0f;
    Transform[] screenSwitchTransform;
    int[] screenSwitchIdx;

    private bool isSummon = false;
    private float summonCh = 0f;
    private int summonTarget = 0;

    private bool isTrainerOut = false;
    private float trainerOutCh = 0f;

    private void Awake()
    {
        instance = this;
        enemyPokeList = new List<Poke>();
        pokes = new Poke[2];

        statuses = new Status[2];
        statuses[0] = new Status(transform.Find("Status"));
        statuses[1] = new Status(transform.Find("Status2"));

        pokeSprites = new Image[2];
        pokeSprites[0] = transform.Find("Image").Find("Pokemon").GetComponent<Image>();
        pokeSprites[1] = transform.Find("Image2").Find("Pokemon").GetComponent<Image>();

        pokeEffs = new Image[2];
        pokeEffs[0] = pokeSprites[0].transform.GetChild(0).GetComponent<Image>();
        pokeEffs[1] = pokeSprites[1].transform.GetChild(0).GetComponent<Image>();

        rectT = (RectTransform)transform;

        imgStartPos = new Vector2[2];
        imgStartPos[0] = new Vector2(5.6f, 25.58f);
        imgStartPos[1] = new Vector2(-3.906f, 9.5f);

        battleScreenSwitch = transform.parent.Find("BattleScreenSwitch").gameObject;
        screenSwitchTransform = new Transform[6];

        trainerSprites = new Image[2];
        trainerSprites[0] = transform.Find("Trainer").GetComponent<Image>();
        trainerSprites[1] = transform.Find("Trainer2").GetComponent<Image>();

        pokeballSprites = new Image[2];
        pokeballSprites[0] = transform.Find("Pokeball1").GetComponent<Image>();
        pokeballSprites[1] = transform.Find("Pokeball2").GetComponent<Image>();

        for (var i = 0; i < 6; i++)
        {
            screenSwitchTransform[i] = battleScreenSwitch.transform.GetChild(i);
        }

        screenSwitchIdx = new int[6] {0, 2, 4, 1, 3, 5 };
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
            PokeHitUpdate();
            DieUpdate();
            SummonUpdate();
            TrainerOutUpdate();
        }
        ScreenSwitchUpdate();
    }

    private void FightInit()
    {
        //포켓몬, 트레이너 모두 비활성화
        for (var i=0; i < 2; i++)
        {
            pokeSprites[i].enabled = false;
            trainerSprites[i].enabled = false;
            pokeballSprites[i].enabled = false;

            pokeEffs[i].enabled = false;

            statuses[i].obj.gameObject.SetActive(false);
        }

        //내 트레이너 활성화
        trainerSprites[0].enabled = true;
        trainerSprites[0].sprite = PokeSpr.instance.trainers[0];

        //isTrainer일경우 트레이너, 아닐경우 포켓몬 활성화
        if (isTrainerBattle)
        {
            trainerSprites[1].enabled = true;
        }
        else
        {
            pokeSprites[1].enabled = true;
        }

        FightQueueManager q = FightQueueManager.instance;
        q.battleEvents.Clear();

        q.BattleInit();
        
    }

    public void Summon(int target)
    {
        isSummon = true;
        summonCh = 0f;

        summonTarget = target;

        pokeballSprites[target].sprite = FightInfo.instance.pokeballs[0];
        pokeballSprites[target].enabled = true;
    }

    private void SummonUpdate()
    {
        if (isSummon)
        {
            summonCh += Time.deltaTime;

            if (summonCh < 0.3f)
            {
                var timeTmp = summonCh;
                RectTransform rt = (RectTransform)pokeballSprites[summonTarget].transform;
                rt.localScale = new Vector3(1+ timeTmp, 1 - timeTmp, 1);
            }
            else if (summonCh < 0.5f)
            {
                var timeTmp = summonCh - 0.3f;
                pokeballSprites[summonTarget].sprite = FightInfo.instance.pokeballs[1];

                RectTransform rt = (RectTransform)pokeballSprites[summonTarget].transform;
                rt.localScale = new Vector3(1, 1.2f - timeTmp, 1f);
            }
            else if (summonCh <0.7f)
            {
                var timeTmp = (summonCh - 0.5f)*5;

                pokeballSprites[summonTarget].enabled = false;
                pokeSprites[summonTarget].enabled = true;
                pokeEffs[summonTarget].enabled = true;

                RectTransform rt = (RectTransform)pokeSprites[summonTarget].transform;
                rt.localScale = new Vector3(timeTmp + 0.1f, timeTmp + 0.1f, 1f);
                statuses[summonTarget].obj.gameObject.SetActive(true);
            }
            else
            {
                isSummon = false;
                pokeEffs[summonTarget].enabled = false;

                RectTransform rt = (RectTransform)pokeSprites[summonTarget].transform;
                rt.localScale = new Vector3(1f, 1f , 1f);
            }
        }
    }

    public void Active()
    {       
        rectT.anchoredPosition = Vector2.zero;

        

        SetScreen();

        //BattleMenu1.instance.Active();

        isActive = true;
        enePokeIndex = 0;

        FightInit();
    }

    public void UnActive()
    {
        isActive = false;
        FightQueueManager q = FightQueueManager.instance ;
        q.battleEvents.Clear();

        BattleMenu1.instance.UnActive();
        rectT.anchoredPosition = new Vector2(943f, -613.8f);

        EventManager ev = EventManager.instance;
        ev.ActiveNextEvent();

        enemyPokeList.Clear();

    }

    void SetScreen()
    {
        for (var i = 0; i < 2; i++)
        {
            statuses[i].SetStatus(pokes[i]);
        }

        SetImage(pokeSprites[0], PokeSpr.instance.backSprites[pokes[0].id], imgStartPos[0]);
        SetImage(pokeSprites[1], PokeSpr.instance.sprites[pokes[1].id + 1], imgStartPos[1]);
    }


    public void ChangePoke(int target, Poke poke)
    {
        pokes[target] = poke;
        SetScreen();
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
        ScreenSwitchActive(myPoke, enemyPokes, (fightID != -1));

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

            var tmpRect = (RectTransform)pokeSprites[0].transform;
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

            pokeSprites[statusHitTarget].color = new Color(1, 1, 1, alpha);

            
        }
    }



    private void PokeHitUpdate()
    {
        if (isPokeHit)
        {
            if (pokeHitCh < 0.3f)
            {
                pokeHitCh += Time.deltaTime * 2;

                var rectT = (RectTransform)pokeSprites[statusHitTarget].transform;

                var xx = 0f;

                if (pokeHitCh < 0.1f)
                {
                    xx = (-4000 * pokeHitCh * pokeHitCh + 800 * pokeHitCh);
                }
                else
                {
                    xx = (-200 * pokeHitCh + 60);
                }

                if (pokeHitCh >= 0.3f) xx = 0f;

                if (statusHitTarget == 0) xx *= -1;
                rectT.anchoredPosition = new Vector2(imgStartPos[statusHitTarget].x + xx, imgStartPos[statusHitTarget].y);
            }
            else
            {
                isPokeHit = false;
                pokeHitCh = 0f;
            }               

        }
            
    }

    private void DieUpdate()
    {
        if (isPokeDie)
        {
            pokeDieCh += Time.deltaTime;

            var yy = -pokeDieCh * 1000f;
            var rectT = (RectTransform)pokeSprites[statusHitTarget].transform;
            rectT.anchoredPosition = new Vector2(imgStartPos[statusHitTarget].x, imgStartPos[statusHitTarget].y + yy);

            if (pokeDieCh > 1.0f)
            {
                pokeDieCh = 0f;
                isPokeDie = false;
            }
        }
    }

    public void Die(int target)
    {
        isPokeDie = true;
        pokeDieCh = 0f;
        dieTarget = target;

        pokeSprites[target].enabled = false;
        statuses[target].obj.gameObject.SetActive(false);
    }

    public void StatusHitActive(int target)
    {
        isStatushit = true;
        statusHitTarget = target;

        isPokeHit = true;
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

    public bool hasNextPokemon(int target)
    {
        if (target == 1)//적
        {
            for (int i = 0; i < enemyPokeList.Count; i++)
            {
                if (enemyPokeList[i].hp > 0) return true;
            }
        }      
        

        return false;
    }

    public void ScreenSwitchActive(Poke myPoke, PokeANDLevel[] enemyPokes, bool isTrainerBattle)
    {
        SetScreenSwitch();

        GlobalInput.globalInput.InputStun(1.6f);
        isScreenSwitch = true;
        battleScreenCh = 0f;

        for (var i = 0; i < enemyPokes.Length; i++)
        {
            enemyPokeList.Add(enemyPokes[i].GetPoke());
        }

        pokes[0] = myPoke;
        pokes[1] = enemyPokeList[0];

        this.isTrainerBattle = isTrainerBattle;
    }

    private void SetScreenSwitch()
    {
        for (var i = 0; i < 6; i++)
        {
            ((RectTransform)screenSwitchTransform[i].GetChild(1)).anchoredPosition = new Vector2(0, 0f);
            ((RectTransform)screenSwitchTransform[i].GetChild(0)).sizeDelta = new Vector2(48f, 96f);

            var idx = screenSwitchIdx[i];
            ((RectTransform)screenSwitchTransform[i]).anchoredPosition = new Vector2(0, - idx * 96);
        }

        isScreenSwitchActiveCh = true;
    }


    private void ScreenSwitchUpdate()
    {
        if (isScreenSwitch)
        {
            battleScreenCh += Time.deltaTime;           

            
            if (battleScreenCh < 1f)
            {
                var angle = battleScreenCh * 360f * 2;


                var xx = battleScreenCh * 960f;
                for (var i=0; i<3; i++)
                {
                    ((RectTransform)screenSwitchTransform[i].GetChild(1)).anchoredPosition = new Vector2(xx, 0f);
                    ((RectTransform)screenSwitchTransform[i].GetChild(0)).sizeDelta = new Vector2(48f + xx, 96f);

                    screenSwitchTransform[i].GetChild(1).rotation = Quaternion.Euler(0, 0, -angle);
                }

                for (var i=3; i<6;i++)
                {
                    ((RectTransform)screenSwitchTransform[i].GetChild(1)).anchoredPosition = new Vector2(-xx, 0f);
                    ((RectTransform)screenSwitchTransform[i].GetChild(0)).sizeDelta = new Vector2(48f + xx, 96f);

                    screenSwitchTransform[i].GetChild(1).rotation = Quaternion.Euler(0, 0, angle);

                }
            }
            else if(battleScreenCh < 1.5f)
            {
                var yy = (battleScreenCh - 1f) * 600f;
                
                for (var i=0; i < 6; i++)
                {
                    var idx = screenSwitchIdx[i];

                    if (idx < 3)
                    {
                        ((RectTransform)screenSwitchTransform[i]).anchoredPosition = new Vector2(0, yy - idx * 96);
                    }
                    else
                    {
                        ((RectTransform)screenSwitchTransform[i]).anchoredPosition = new Vector2(0, -yy - idx * 96);
                    }

                }

                if (isScreenSwitchActiveCh)
                {
                    isScreenSwitchActiveCh = false;
                    Active();
                }

            }
            else
            {
                isScreenSwitch = false;
                
            }
        }
    }


    public void TrainerOut()
    {
        isTrainerOut = true;
        trainerOutCh = 0f;
    }

    private void TrainerOutUpdate()
    {
        if (isTrainerOut)
        {
            trainerOutCh += Time.deltaTime *2;
            var xx = trainerOutCh * -600;
            int idx = (int)(trainerOutCh * 4 + 1);

            RectTransform rt = (RectTransform)trainerSprites[0].transform;
            rt.anchoredPosition = new Vector2(xx, 0);
            trainerSprites[0].sprite = PokeSpr.instance.trainers[idx];


            if (trainerOutCh >= 1.0f)
            {
                isTrainerOut = false;
            }

        }
    }
}
