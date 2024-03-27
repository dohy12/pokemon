using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poke
{
    public int id;
    public int[] stat; //체,공,방,특공,특방,스피드
    public int level;
    public int exp;
    public int[] skills;
    public int hp;

    public Poke(int id, int[] stat, int level, int[] skills)
    {
        this.id = id;
        this.stat = stat;
        this.level = level;
        this.exp = 0;
        this.skills = skills;
        this.hp = stat[0] * Random.Range(1, 10)/10;
    }


    public string ToString()
    {
        return "(" + id + " , " + "[" + stat.ToString() + "]" + " , lv:" + level + " ,  hp:" + hp + ")";
    }

}
