using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PokeDexManager : MonoBehaviour
{
    public static PokeDexManager instance;

    private UIManager uiManager;
    private UIManager.TYPE uiType1;
    private UIManager.TYPE uiType2;

    private float posY = 0;
    private float posTime = 0f;
    private bool isActive = false;
    private RectTransform rectTransform;
    private GlobalInput input;

    Dictionary<int, int> pokeDex; //[0]미발견, [1]발견, [2]잡음 
    private string[] pokeDexStrings;
    private Transform[] pokeDexStringObjs;

    private int pokeDexPage = 0;
    private int selectNum = 0;
    private RectTransform selectBox;
    private float selectBoxPos;
    private float selectBoxPosYY = 284f;

    private float inputStun = 0f;

    private RectTransform scroll;
    private float scrollPos;    

    private void Awake()
    {
        instance = this;

        uiType1 = UIManager.TYPE.POKEDEX1;
        uiType2 = UIManager.TYPE.POKEDEX2;

        rectTransform = (RectTransform)transform;

        PokeDexInit();
    }

    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.instance;
        input = GlobalInput.globalInput;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUi();
        InputCheck();
        SelectBoxUpdate();
    }

    private void PokeDexInit()
    {
        pokeDex = new Dictionary<int, int>();
        
        for (var i = 0; i < 23; i++)
        {
            pokeDex.Add(i, 2);
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

        selectBox = (RectTransform)transform.Find("SelectBox");
        scroll = (RectTransform)transform.Find("Scroll");
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

        var catch_text = transform.Find("Catch").GetComponent<TMP_Text>();
        catch_text.text = "잡은 수   " + catch_poke.ToString("D3");

        selectNum = 0;
        selectBox.anchoredPosition = new Vector2(449f, 912f);

        scrollPos = 0f;
        scroll.anchoredPosition = new Vector2(-126.7f, -132.3f);
    }


    private void SetPokeString()
    {
        for (var i = 0; i < 7; i++)
        {
            if (pokeDex[i] > 0) { pokeDexStrings[i] = PokemonInfo.Instance.pokemons[i + pokeDexPage].name; }
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
        posTime = 0f;
        isActive = true;
        uiManager.ActiveUI(uiType1);

        SetPokeDex();
    }

    private void UnActivePokedex()
    {
        posTime = 0.5f;
        isActive = false;
        uiManager.UnActiveUI();
    }

    private void InputCheck()
    {

        if (uiManager.CheckUITYPE(uiType1))
        {
            if (input.bButtonDown)
            {
                UnActivePokedex();
            }

            inputStun -= Time.deltaTime;
            if (input.verticalRaw != 0 && inputStun<0)
            {
                inputStun = 0.2f;
                selectNum -= (int)input.verticalRaw;
                if (selectNum < 0)
                {
                    selectNum = 0;
                    GoPage(-1);
                }
                if (selectNum > 6)
                {
                    selectNum = 6;
                    GoPage(1);
                }
            }
        }
    }

    private void SelectBoxUpdate()
    {
        if (uiManager.CheckUITYPE(uiType1))
        {
            var tmpPos = 912 - selectBoxPosYY * selectNum;
            
            if (tmpPos != selectBoxPos)
            {
                selectBoxPos = tmpPos / 10f + selectBoxPos * 9 / 10f;
                if (Mathf.Abs(tmpPos - selectBoxPos) < 5f)
                {
                    selectBoxPos = tmpPos;
                }

                selectBox.anchoredPosition = new Vector2(449f, selectBoxPos);
            }

            var scrollNum = selectNum + pokeDexPage;
            tmpPos = 89.1091f * scrollNum;

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

    void UpdateUi()
    {
        if (isActive)
        {
            if (posY < 560f)
            {
                posTime += Time.deltaTime * 2;
                posY = -2240 * posTime * posTime + 2240 * posTime;
                if (posY > 550f)
                {
                    posY = 560f;
                }
                SetUIPos();
            }
        }
        else
        {
            if (posY > 0)
            {
                posTime += Time.deltaTime * 2;
                posY = -2240 * posTime * posTime + 2240 * posTime;
                if (posY < 10f)
                {
                    posY = 0f;
                }
                SetUIPos();
            }
        }
    }

    void SetUIPos()
    {
        rectTransform.anchoredPosition = new Vector2(0f, 560 - posY);
    }
}
