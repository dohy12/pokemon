using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PotalInfo
{
    public Dictionary<int, Potal> potalinfo = new Dictionary<int, Potal>();

    public class Potal
    {
        public int id;
        public Vector2 pos;
        public int direc;

        public Potal(int id, Vector2 pos, int direc)
        {
            this.id = id;
            this.pos = pos;
            this.direc = direc;
        }
    }
        
    public void AddPotal(int id, Vector2 pos, int direc)
    {
        Potal potal = new Potal(id, pos, direc);
        potalinfo.Add(id, potal);
    }

    public Potal GetPotal(int id)
    {
        return potalinfo[id];
    }
}
