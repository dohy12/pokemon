using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Poke
{
    private static int idGenerator = 0; 
    public int pokeID;//���ϸ� ���� ���̵�
    public int id;//���ϸ� ���� ���̵�
    public int[] stat; //ü,��,��,Ư��,Ư��,���ǵ�
    public int level;
    public int exp;
    public int maxExp;
    public int[] skills;
    public int[] skillsPP = new int[4];
    public int hp;

    public Poke(int id, int level)
    {
        pokeID = idGenerator++;
        this.id = id;
        this.level = level;
        this.exp = 0;
        this.maxExp = GetMaxExp(level);
        this.skills = PokemonSkillInfo.Instance.GetPokemonSkillByLevel(id, level);
        this.stat = new int[6];

        var poke = PokemonInfo.Instance.pokemons[id];
        SetStats(poke);
        SetSkillsPP();

        this.hp = stat[0];
    }

    private void SetSkillsPP()
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
        return "(���� ���̵� : " + pokeID + ", ���ϸ� ���� : " + GetInfo().name + " , " + "[" + stat.ToString() + "]" + " , lv:" + level + " ,  hp:" + hp + ")";
    }

    public PokemonInfo.Pokemon GetInfo()
    {
        return PokemonInfo.Instance.pokemons[id];
    }

    public void LevelUP()
    {
        var prevMaxHp = stat[0];

        level += 1;
        var pokeInfo = PokemonInfo.Instance.pokemons[id];
        SetStats(pokeInfo);

        var nextMaxHp = stat[0];
        var plusHp = nextMaxHp - prevMaxHp;
        hp += plusHp;
    }
    
}
