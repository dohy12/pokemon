using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeSpr : MonoBehaviour
{
    public static PokeSpr instance;

    public Sprite[] sprites;
    public Sprite[] backSprites;
    public Sprite[] icons;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
