using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMachine : MonoBehaviour
{
    private SpriteRenderer[] pokeballs;
    private SpriteRenderer screen;

    public int machineID;
    public Sprite[] sprites;

    public float pokeballSettingSpeed;
    public float healingSpeed;

    private int pokeballNum = 1;
    private float healTime;
    private float pokeballSettingTime;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (pokeballSettingTime < pokeballSettingSpeed * (pokeballNum + 1))
            {
                pokeballSettingTime += Time.deltaTime;

                for (int i = 0; i < pokeballNum; i++)
                {
                    if (pokeballSettingTime > pokeballSettingSpeed * (i+1))
                    {
                        pokeballs[i].enabled = true;
                    }
                }
                screen.sprite = sprites[4];
            }
            else
            {
                healTime += Time.deltaTime;

                if (healTime < healingSpeed)
                {
                    var spriteIndex = (int)(healTime / (healingSpeed / 6f)) + 1;
                    while (spriteIndex >= 3)
                    {
                        spriteIndex -= 3;
                    }

                    for (int i = 0; i < pokeballNum; i++)
                    {
                        pokeballs[i].sprite = sprites[spriteIndex];
                    }
                    screen.sprite = sprites[4 + spriteIndex];
                }
                else
                {
                    for (int i = 0; i < pokeballNum; i++)
                    {
                        pokeballs[i].sprite = sprites[0];
                    }
                    screen.sprite = sprites[3];

                    if (healTime > healingSpeed + 0.5f)
                    {
                        UnActive();
                    }
                }
                    
            }
        }
    }

    void Init()
    {
        pokeballs = new SpriteRenderer[6];
        var objPokeballs = transform.Find("Pokeballs");
        for (int i = 0; i < pokeballs.Length; i++)
        {
            var child = objPokeballs.GetChild(i);
            pokeballs[i] = child.GetComponent<SpriteRenderer>();
            pokeballs[i].enabled = false;
        }

        screen = transform.Find("Screen").GetComponent<SpriteRenderer>();
    }

    public void Active(int pokeballNum)
    {
        Debug.Log(pokeballNum);
        Debug.Log(machineID);
        this.pokeballNum = pokeballNum;

        isActive = true;
        healTime = 0f;
        pokeballSettingTime = 0f;
    }

    private void UnActive()
    {
        isActive = false;

        for (int i = 0;i < pokeballs.Length; i++)
        {
            pokeballs[i].enabled = false;
        }

        EventManager.instance.ActiveNextEvent();
    }
}
