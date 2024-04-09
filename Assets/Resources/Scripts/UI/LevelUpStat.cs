using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class LevelUpStat : SlideUI
{
    public static LevelUpStat instance;
    private TMP_Text txt;
    private FightManager fight;
    private int[] prevStat;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SlideUiInit();
        txt = transform.GetChild(0).GetComponent<TMP_Text>();
        fight = FightManager.instance;
        prevStat = new int[6] { 0, 0, 0, 0, 0, 0 };
    }


    // Update is called once per frame
    void Update()
    {
        SlideUiUpdate();
    }

    public void Active()
    {
        SlideUiActive();
        SetScreen();
    }

    public void UnActive()
    {
        SlideUiUnActive();
    }

    public void SetPrevStat()
    {
        Poke poke = fight.pokes[0];

        for(var i=0; i < 6; i++)
        {
            prevStat[i] = poke.stat[i];
        }
    }


    private void SetScreen()
    {
        Poke poke = fight.pokes[0];

        string str = "";

        str += "HP  : " + poke.stat[0] + "(" + (poke.stat[0] - prevStat[0]) + ")\n";
        str += "ATK : " + poke.stat[1] + "(" + (poke.stat[1] - prevStat[1]) + ")\n";
        str += "DEF : " + poke.stat[2] + "(" + (poke.stat[2] - prevStat[2]) + ")\n";
        str += "SAT : " + poke.stat[3] + "(" + (poke.stat[3] - prevStat[3]) + ")\n";
        str += "SDF : " + poke.stat[4] + "(" + (poke.stat[4] - prevStat[4]) + ")\n";
        str += "SPD : " + poke.stat[5] + "(" + (poke.stat[5] - prevStat[5]) + ")\n";

        txt.text = str;
    }
}
