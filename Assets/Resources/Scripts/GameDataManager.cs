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
        pokeList.Add(new Poke(0, new int[] { 50, 2, 3 }, 5, new int[] { 1 }));
        pokeList.Add(new Poke(3, new int[] { 40, 2, 3 }, 25, new int[] { 1 }));
        pokeList.Add(new Poke(6, new int[] { 20, 2, 3 }, 10, new int[] { 1 }));
        pokeList.Add(new Poke(9, new int[] { 123, 2, 3 }, 99, new int[] { 1 }));
        pokeList.Add(new Poke(12, new int[] { 123, 2, 3 }, 100, new int[] { 1 }));
        pokeList.Add(new Poke(15, new int[] { 123, 2, 3 }, 50, new int[] { 1 }));
    }
}
