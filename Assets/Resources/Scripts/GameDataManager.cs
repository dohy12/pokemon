using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;
    public List<Poke> pokeList;
    public int money;
    public Dictionary<int, int> items;


    private void Awake()
    {
        instance = this;
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("TestStart", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        pokeList = new List<Poke>();
        
        money = 10000;

        items = new Dictionary<int, int>();
        items[0] = 1;//Áöµµ
        items[1] = 1;//³¬½Ë´ë
    }

    public void AddPokemon(int pokeID, int pokeLevel)
    {
        pokeList.Add(new Poke(pokeID, pokeLevel));

        PokeDexManager.instance.CatchPoke(pokeID);        
    }

    public bool HasPokemon()
    {
        return (pokeList.Count > 0);
    }

    public void TestStart()
    {
        AddPokemon(0, 5);
    }

}
