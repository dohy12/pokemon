using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;
    public List<Poke> pokeList;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        pokeList = new List<Poke>();
        //pokeList.Add(new Poke(0, 5, new int[] { 1, 8, 4, 0 }));
    }

    public void AddPokemon(int pokeID, int pokeLevel)
    {
        pokeList.Add(new Poke(pokeID, pokeLevel, new int[] { 1, 8, 4, 0 }));
    }
}
