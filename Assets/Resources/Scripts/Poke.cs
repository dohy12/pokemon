using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poke
{
    private static int idGenerator = 0; 
    public int pokeID;//포켓몬 고유 아이디
    public int id;
    public int[] stat; //체,공,방,특공,특방,스피드
    public int level;
    public int exp;
    public int maxExp;
    public int[] skills;
    public int[] skillsPP = new int[4];
    public int hp;

    public Poke(int id, int level, int[] skills)
    {
        pokeID = idGenerator++;
        this.id = id;
        this.level = level;
        this.exp = 0;
        this.maxExp = GetMaxExp(level);
        this.skills = skills;
        this.stat = new int[6];

        var poke = PokemonInfo.Instance.pokemons[id];
        SetStats(poke);
        SetSkills();

        this.hp = stat[0] * Random.Range(1, 10) / 10;
    }

    private void SetSkills()
    {
        var skillInfo = PokemonSkillInfo.Instance.skills;
        for (var i = 0; i < 4; i++)
        {
            skillsPP[i] = 0;
            if (skills[i] != 0)
            {
                skillsPP[i] = skillInfo[skills[i]].ppMax;
            }
        }
    }

    private void SetStats(PokemonInfo.Pokemon poke)
    {
        stat[0] = ((poke.stat[0] * 2 + 15 + 100) * level) / 100 + 10;

        for (int i = 1; i< 6; i++)
        {
            stat[i] = ((poke.stat[i] * 2 + 15) * level) / 100 + 5;
        }
    }

    private int GetMaxExp(int level)
    {
        return (level * level * level) - ((level - 1) * (level - 1) * (level - 1));
    }

    public string ToString()
    {
        return "(고유 아이디 : " + pokeID + ", 포켓몬 종류 : " + id + " , " + "[" + stat.ToString() + "]" + " , lv:" + level + " ,  hp:" + hp + ")";
    }


    
}
