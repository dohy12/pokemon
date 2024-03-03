using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : Unit
{
    public bool isMovable;

    // Start is called before the first frame update
    void Start()
    {
        UnitStart();
    }

    // Update is called once per frame
    void Update()
    {
        SetSprite();

        if (isMovable)
        {
            MoveUpdate();
        }        
    }
}
