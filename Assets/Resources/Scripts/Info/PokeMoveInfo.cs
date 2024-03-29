using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeMoveInfo : MonoBehaviour
{
    public static PokeMoveInfo Instance;
    public Dictionary<int, Tuple<int,int>> info;


    private void Awake()
    {
        Instance = this;
    }


}
