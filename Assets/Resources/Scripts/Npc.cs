using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : Unit
{
    public int npcKind;//0)�ȿ�����,  1)���⸸ �ٲ�, 2)��ȸ��, 3)���ϸ�
    public int npcId;

    private float alarm = 0;

    private float originX = 0;
    private float originY = 0;

    private float pokemonSpriteStep = 1f;

    // Start is called before the first frame update
    void Start()
    {
        UnitStart();
        originX = moveX;
        originY = moveY;
        alarm = Random.Range(3f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckDistanceFromPlayer())
        {
            NpcUpdate();

            SetSprite();
            MoveUpdate();

            if (npcKind != 3)
            {
                SpriteUpdate();
            }
            else
            {
                PokemonSpriteUpdate();
            }
        }
            
    }


    bool CheckDistanceFromPlayer()
    {
        Player player = Player.player;
        var heading = player.transform.position - transform.position;
        var distance = heading.magnitude;

        if (distance < 8f)
        {
            return true;
        }


        return false;
    }

    void NpcUpdate()
    {
        if (npcKind == 1 || npcKind == 2)
        {
            alarm -= Time.deltaTime;

            if (alarm < 0)
            {
                alarm = Random.Range(3f, 5f);

                NpcAlarm(npcKind);
            }
        }
    }

    void NpcAlarm(int npcKind)
    {
        direc = Random.Range(0, 4);

        if (npcKind == 2)
        {
            if (Random.Range(0, 100f) < 80)
            {
                if (MoveCheck())
                {
                    MoveOrder(direc);
                }
            }            
        }
    }

    bool MoveCheck()
    {
        Vector2 direcVector = GetVector2fromDirec(direc);
        Vector3 nextPos = transform.position + (Vector3)direcVector;

        if (nextPos.x >= originX + 2) return false;
        if (nextPos.x <= originX - 2) return false;
        if (nextPos.y >= originY + 2) return false;
        if (nextPos.y <= originY - 2) return false;

        RaycastHit2D[] hits = Physics2D.RaycastAll(nextPos, direcVector, 0.01f);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("Block") || hits[i].transform.CompareTag("Player"))
            {
                return false;
            }
        }

        return true;
    }

    private void SpriteUpdate()
    {
        if (movingStep < 0 || movingStep > 0.5f / movingSpeed)//���� ���� ���
        {
            isSpriteMov = false;
        }
        else
        {
            isSpriteMov = true;
        }
    }

    private void PokemonSpriteUpdate()
    {
        pokemonSpriteStep -= Time.deltaTime * 1.5f;

        if (pokemonSpriteStep < 0.5f)
        {
            isSpriteMov = false;
        }
        else
        {
            isSpriteMov = true;
        }


        if (pokemonSpriteStep <0)
        {
            pokemonSpriteStep = 1f;
        }
    }

}