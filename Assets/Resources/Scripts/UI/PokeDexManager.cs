using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PokeDexManager : SlideUI, CursorUI
{
    public static PokeDexManager instance;

    private UIManager uiManager;
    private GameObject uiID1; //도감
    private GameObject uiID2; //도감상세

    private GlobalInput input;

    Dictionary<int, int> pokeDex; //[0]미발견, [1]발견, [2]잡음 
    private string[] pokeDexStrings;
    private Transform[] pokeDexStringObjs;

    private int pokeDexPage = 0;

    private float inputStun = 0f;

    private RectTransform scroll;
    private float scrollPos;

    private GameObject pokedexDetail;

    private Image pokeImage1;
    private Image pokeImage2;
    private Cursor cursor;

    private bool isOnlyDetail = false;

    private int findPokeNum = 0;

    private void Awake()
    {        
        instance = this;

        rectTransform = (RectTransform)transform;

        PokeDexInit();
    }

    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.instance;
        input = GlobalInput.globalInput;
        SlideUiInit();
    }

    // Update is called once per frame
    void Update()
    {
        SlideUiUpdate();
        InputCheck();
        SelectBoxUpdate();
    }

    private void PokeDexInit()
    {
        pokeDex = new Dictionary<int, int>();
        
        for (var i = 0; i < 23; i++)
        {
            pokeDex.Add(i, 0);
        }        

        pokeDexStrings = new string[7];

        for (var i=0; i < 7; i++)
        {
            pokeDexStrings[i] = "- - - - - ";
        }

        pokeDexStringObjs = new Transform[7];
        var list = transform.Find("List");
        for (var i = 0; i < 7; i++)
        {
            pokeDexStringObjs[i] = list.GetChild(i);
        }

        scroll = (RectTransform)transform.Find("Scroll");
        pokeImage1 = transform.Find("Image").GetComponent<Image>();  
        pokedexDetail = transform.Find("Detail").gameObject;
        pokedexDetail.SetActive(false);
        pokeImage2 = pokedexDetail.transform.Find("Image").GetComponent<Image>();

        uiID1 = gameObject;
        uiID2 = pokedexDetail;
    }

    private void SetPokeDex()
    {
        pokeDexPage = 0;
        SetPokeString();        

        var find_poke = 0;
        var catch_poke = 0;
        for (var i = 0; i < 23; i++)
        {
            if (pokeDex[i] > 0) { find_poke++; }
            if (pokeDex[i] > 1) { catch_poke++; }
        }

        var find_text = transform.Find("Find").GetComponent<TMP_Text>();
        find_text.text = "발견한 수 " + find_poke.ToString("D3");
        findPokeNum = find_poke;

        var catch_text = transform.Find("Catch").GetComponent<TMP_Text>();
        catch_text.text = "잡은 수   " + catch_poke.ToString("D3");

        scrollPos = 0f;
        scroll.anchoredPosition = new Vector2(-126.7f, -132.3f);

        pokeImage1.sprite = GetSprite(cursor.cursorNum + pokeDexPage);
    }


    private void SetPokeString()
    {
        for (var i = 0; i < 7; i++)
        {
            if (pokeDex[i + pokeDexPage] > 0) { pokeDexStrings[i] = PokemonInfo.Instance.pokemons[i + pokeDexPage].name; }
            else { pokeDexStrings[i] = "- - - - - "; }            
        }

        for (var i = 0; i < 7; i++)
        {
            var text = pokeDexStringObjs[i].GetChild(1).GetComponent<TMP_Text>();
            text.text = pokeDexStrings[i];

            var img = pokeDexStringObjs[i].GetChild(0).gameObject;
            if (pokeDex[i + pokeDexPage] == 2) { img.SetActive(true); }
            else { img.SetActive(false); }
        }

        pokeImage1.sprite = GetSprite(cursor.cursorNum + pokeDexPage);
    }

    private void GoPage(int pageTmp)
    {
        pokeDexPage += pageTmp;
        if (pokeDexPage < 0)
        {
            pokeDexPage = 0;
        }
        if (pokeDexPage > 16)
        {
            pokeDexPage = 16;
        }

        SetPokeString();
    }


    public void ActivePokedex()
    {
        pokedexDetail.SetActive(false);

        isOnlyDetail = false;
        SlideUiActive();
        uiManager.ActiveUI(uiID1);
        cursor.Active();

        SetPokeDex();
    }

    private void ActiveDetail()
    {
        uiManager.ActiveUI(uiID2);
        pokedexDetail.SetActive(true);

        SetDetail();
    }

    public void ActiveDetail(int pokeMonID)
    {        
        SlideUiActive();
        isOnlyDetail = true;
        uiManager.ActiveUI(uiID2);
        pokedexDetail.SetActive(true);
        cursor.cursorNum = 0;
        pokeDexPage = pokeMonID;

        SetDetail();
    }

    private void SetDetail()
    {
        var num = cursor.cursorNum + pokeDexPage;
        var detailObj = transform.Find("Detail");
        var info = PokeDexInfo.instance.info[num];

        detailObj.Find("Number").GetComponent<TMP_Text>().text = "No." + (num+1).ToString("D3");
        detailObj.Find("Name").GetComponent<TMP_Text>().text = info.name;
        detailObj.Find("Kind").GetComponent<TMP_Text>().text = info.kind;

        if (pokeDex[num] == 1)
        {
            detailObj.Find("Height").GetComponent<TMP_Text>().text = "???m";
            detailObj.Find("Weight").GetComponent<TMP_Text>().text = "???kg";
            detailObj.Find("DetailText").GetComponent<TMP_Text>().text = " ";
        }
        else
        {
            detailObj.Find("Height").GetComponent<TMP_Text>().text = info.height;
            detailObj.Find("Weight").GetComponent<TMP_Text>().text = info.weight;
            detailObj.Find("DetailText").GetComponent<TMP_Text>().text = info.detail;
        }

        pokeImage2.sprite = GetSprite(num);
    }

    private void UnActivePokedex()
    {
        SlideUiUnActive();
        uiManager.UnActiveUI(uiID1);
        input.InputStun();
    }

    private void UnActivePokedex2()
    {
        inputStun = 0.1f;
        uiManager.UnActiveUI(uiID2);
        input.InputStun();
        

        if (!isOnlyDetail)
        {
            pokedexDetail.SetActive(false);
            cursor.SetCursor(cursor.cursorNum);
        }            
        else
        {
            SlideUiUnActive();
            EventManager.instance.ActiveNextEvent(); 
        }
            
    }

    private void InputCheck()
    {

        if (uiManager.CheckUITYPE(uiID1))
        {
            inputStun -= Time.deltaTime;
            if (input.bButtonDown && inputStun < 0)
            {
                UnActivePokedex();
            }
        }

        if (uiManager.CheckUITYPE(uiID2))
        {
            inputStun -= Time.deltaTime;
            

            if (!isOnlyDetail)
            {
                if (input.bButtonDown)
                {
                    UnActivePokedex2();
                }

                if (input.verticalRaw != 0 && inputStun < 0)
                {
                    inputStun = 0.3f;
                    if (input.verticalRaw == 1) { if (GoPreviousPokemon()) { SetDetail(); } }
                    else { if (GoNextPokemon()) { SetDetail(); } }
                }
            }
            else
            {
                if (input.aButtonDown || input.bButtonDown)
                {
                    UnActivePokedex2();
                }
            }
            
        }
    }

    private bool GoNextPokemon()
    {
        if (findPokeNum<=1){ return false; }

        var num = cursor.cursorNum + pokeDexPage;

        while (num < 23)
        {
            num++;
            cursor.cursorNum += 1;
            if (cursor.cursorNum > 6)
            {
                cursor.cursorNum = 6;
                pokeDexPage += 1;
                if (pokeDexPage > 16)
                {
                    pokeDexPage = 16;
                }
            }

            if (num == 23) { return false; }
            if (pokeDex[num] > 0) { SetPokeString(); return true; }
        }
        return false;
    }

    private bool GoPreviousPokemon()
    {
        if (findPokeNum <= 1) { return false; }

        var num = cursor.cursorNum + pokeDexPage;

        while (num >= 0)
        {
            num--;
            cursor.cursorNum -= 1;
            if (cursor.cursorNum < 0)
            {
                cursor.cursorNum = 0;
                pokeDexPage -= 1;
                if (pokeDexPage < 0)
                {
                    pokeDexPage = 0;
                }
            }

            if (pokeDex[num] > 0) { SetPokeString();     return true; }                
        }
        return false;
    }

    private void SelectBoxUpdate()
    {
        if (uiManager.CheckUITYPE(uiID1))
        {
            var scrollNum = cursor.cursorNum + pokeDexPage;
            var tmpPos = 89.1091f * scrollNum;

            if (tmpPos != scrollPos)
            {
                scrollPos = tmpPos / 10f + scrollPos * 9 / 10f;
                if (Mathf.Abs(tmpPos - scrollPos) < 5f)
                {
                    scrollPos = tmpPos;
                }

                scroll.anchoredPosition = new Vector2(-126.7f, -132.3f - scrollPos);
            }
        }
    }


    Sprite GetSprite(int num)
    {
        if (pokeDex[num] == 0)
            return PokeSpr.instance.sprites[0];

        return PokeSpr.instance.sprites[num + 1];
    }


    public bool GetActive() { return UIManager.instance.CheckUITYPE(uiID1); }

    public void CursorChange(int pageTmp) 
    {
        if (pageTmp != 0)
        {
            GoPage(pageTmp);
        }
        
        pokeImage1.sprite = GetSprite(cursor.cursorNum + pokeDexPage);
    }

    public void CursorChoose(int num)
    {
        if (pokeDex[num + pokeDexPage] > 0)
        {
            ActiveDetail();
        }
    }

    public void CursorInit(Cursor cursor)
    {
        this.cursor = cursor;
        int cursorMaxNum = 7 - 1;
        float yDist = 284f;
        cursor.Init(cursorMaxNum, yDist, false);
    }

    public void FindPoke(int pokeID)
    {
        if (pokeDex[pokeID] == 0)
        {
            pokeDex[pokeID] = 1;
        }
            
    }

    public void CatchPoke(int pokeID)
    {
        if (pokeDex[pokeID] < 2)
        {
            pokeDex[pokeID] = 2;
        }            
    }
}
