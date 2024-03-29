using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokeDetail : MonoBehaviour
{
    public static PokeDetail instance;
    private GlobalInput input;
    private GameObject uiID;

    private Image pokeSpr;
    private TMP_Text pokeName;
    private TMP_Text pokeLevel;
    private TMP_Text[] pokeStats;

    private Image pokeHpBarImg;
    private TMP_Text pokeHpTxt;

    private Image pokeExpBarImg;

    private Transform[] pokeSkills;

    private PokemonList pokeListUI;

    private float inputStun = 0;
    private GameObject canvasObj;

    private RectTransform rectT;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        InputCheck();
    }

    public void Active()
    {
        UIManager.instance.ActiveUI(uiID);

        canvasObj.SetActive(false);
        Invoke("ShowDetail", 0.2f);
        input.InputStun(0.2f);

        rectT.anchoredPosition = Vector2.zero;
    }

    public void UnActive()
    {
        input.InputStun();
        UIManager.instance.UnActiveUI(uiID);

        rectT.anchoredPosition = new Vector2(-551f, 569f);
    }

    private void Init()
    {
        uiID = gameObject;
        input = GlobalInput.globalInput;
        pokeListUI = PokemonList.instance;
        canvasObj = transform.GetChild(0).gameObject;

        var canvas = transform.GetChild(0);

        pokeSpr = canvas.Find("PokeSpr").GetComponent<Image>();
        pokeName = canvas.Find("Name").GetComponent<TMP_Text>();
        pokeLevel = canvas.Find("Level").GetComponent<TMP_Text>();

        pokeStats = new TMP_Text[5];
        var pokeStatsObj = canvas.Find("Stats");
        for (var i = 0; i < 5; i++)
        {
            pokeStats[i] = pokeStatsObj.GetChild(i)
                .GetChild(0).GetComponent<TMP_Text>();
        }

        var pokeHpbar = canvas.Find("HPBar");
        pokeHpTxt = pokeHpbar.Find("Text").GetComponent<TMP_Text>();
        pokeHpBarImg = pokeHpbar.Find("Bar").GetComponent<Image>();

        pokeExpBarImg = canvas.Find("ExpBar").GetChild(0).GetComponent<Image>();

        pokeSkills = new Transform[4];
        var pokeSkillsObj = canvas.Find("Skills");
        for (var i = 0; i < 4; i++)
        {
            pokeSkills[i] = pokeSkillsObj.GetChild(i);
        }

        rectT = (RectTransform)transform;
    }

    private void InputCheck()
    {
        if (GetActive())
        {
            if (input.bButtonDown)
            {
                UnActive();
            }
        
            if (inputStun < 0)
            {
                if (input.verticalRaw != 0)
                {
                    inputStun = 0.4f;

                    pokeListUI.cursor.CursorMove(-(int)input.verticalRaw);
                    canvasObj.SetActive(false);
                    Invoke("ShowDetail", 0.2f);
                    input.InputStun(0.2f);
                }
            }
            else
            {
                inputStun -= Time.deltaTime;
            }
        }
    }

    public bool GetActive() { return UIManager.instance.CheckUITYPE(uiID); }


    private void ShowDetail()
    {
        canvasObj.SetActive(true);

        var pokeList = GameDataManager.instance.pokeList;
        var cursorNum = pokeListUI.GetCursorNum();
        var poke = pokeList[cursorNum];
        var pokeInfo = PokemonInfo.Instance.pokemons[poke.id];

        pokeSpr.sprite = PokeSpr.instance.sprites[poke.id + 1];
        pokeName.text = pokeInfo.name;
        pokeLevel.text = "LV." + poke.level;
        
        for ( var i = 0; i < 5; i++)
        {
            pokeStats[i].text = poke.stat[i+1].ToString();
        }
        pokeHpTxt.text = poke.hp + "/" + poke.stat[0];
        var tmp = (RectTransform)pokeHpBarImg.transform;
        tmp.sizeDelta = new Vector2(266.2f * poke.hp / poke.stat[0], 18.5f);

        tmp = (RectTransform)pokeExpBarImg.transform;
        tmp.sizeDelta = new Vector2(138.3f * poke.exp / poke.maxExp, 15.7f);


        for ( var i = 0; i < 4; i++)
        {
            var pokeSKill = PokemonSkillInfo.Instance.skills[poke.skills[i]];
            SetMove(pokeSkills[i], pokeSKill, poke.skillsPP[i]);
        }
    }

    private void SetMove(Transform moveObj, PokemonSkillInfo.PokemonSkill skill, int pp)
    {
        if (skill == null)
        {
            moveObj.GetChild(0).gameObject.SetActive(false);
            moveObj.GetChild(1).gameObject.SetActive(false);

            moveObj.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            moveObj.GetChild(0).gameObject.SetActive(true);
            moveObj.GetChild(1).gameObject.SetActive(true);

            moveObj.GetComponent<Image>().color = PokemonInfo.GetColorFromType(skill.type);
            moveObj.GetChild(0).GetComponent<TMP_Text>().text = skill.name;
            moveObj.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = pp + "/" + skill.ppMax;
        }
        
    }
}