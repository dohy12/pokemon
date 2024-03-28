using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonInfo : MonoBehaviour
{
    public static PokemonInfo Instance;
    public Dictionary<int, Pokemon> pokemons;

    private void Awake()
    {
        Instance = this;
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        pokemons = new Dictionary<int, Pokemon>();
        var pokeID = -1;
        pokemons.Add(++pokeID, new Pokemon(pokeID, "ÀÌ»óÇØ¾¾", new int[] { 45, 49, 49, 65, 65, 45 }, Type.GRASS, Type.POISION));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "ÀÌ»óÇØÇ®", new int[] { 60, 62, 63, 80, 80, 60 }, Type.GRASS, Type.POISION));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "ÀÌ»óÇØ²É", new int[] { 80, 82, 83, 100, 100, 80 }, Type.GRASS, Type.POISION));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "ÆÄÀÌ¸®", new int[] { 39, 52, 43, 60, 50, 65 }, Type.FIRE, Type.NONE));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "¸®ÀÚµå", new int[] { 58, 64, 58, 80, 65, 80 }, Type.FIRE, Type.NONE));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "¸®ÀÚ¸ù", new int[] { 78, 84, 78, 109, 85, 100 }, Type.FIRE, Type.FLY));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "²¿ºÎ±â", new int[] { 44, 48, 65, 50, 64, 43 }, Type.WATER, Type.NONE));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "¾î´ÏºÎ±â", new int[] { 59, 63, 80, 65, 80, 58 }, Type.WATER, Type.NONE));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "°ÅºÏ¿Õ", new int[] { 79, 83, 100, 85, 105, 78 }, Type.WATER, Type.NONE));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "Ä³ÅÍÇÇ", new int[] { 45, 30, 35, 20, 20, 45 }, Type.BUG, Type.NONE));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "´Üµ¥±â", new int[] { 50, 20, 55, 25, 25, 30 }, Type.BUG, Type.NONE));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "¹öÅÍÇÃ", new int[] { 60, 45, 50, 90, 80, 70 }, Type.BUG, Type.FLY));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "»ÔÃæÀÌ", new int[] { 40, 35, 30, 20, 20, 50 }, Type.BUG, Type.POISION));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "µüÃæÀÌ", new int[] { 45, 25, 50, 25, 25, 35 }, Type.BUG, Type.POISION));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "µ¶Ä§ºØ", new int[] { 65, 90, 40, 45, 80, 75 }, Type.BUG, Type.POISION));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "±¸±¸", new int[] { 40, 45, 40, 35, 35, 56 }, Type.NORMAL, Type.FLY));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "ÇÇÁÔ", new int[] { 63, 60, 55, 50, 50, 71 }, Type.NORMAL, Type.FLY));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "ÇÇÁÔÅõ", new int[] { 83, 80, 75, 70, 70, 101 }, Type.NORMAL, Type.FLY));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "²¿·¿", new int[] { 30, 56, 35, 25, 35, 72 }, Type.NORMAL, Type.NONE));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "·¹Æ®¶ó", new int[] { 55, 81, 60, 50, 70, 97 }, Type.NORMAL, Type.NONE));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "±úºñÂü", new int[] { 40, 60, 30, 31, 31, 70 }, Type.NORMAL, Type.FLY));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "±úºñµå¸±Á¶", new int[] { 65, 90, 65, 61, 61, 100 }, Type.NORMAL, Type.FLY));
        pokemons.Add(++pokeID, new Pokemon(pokeID, "ÇÇÄ«Ãò", new int[] { 45, 80, 50, 75, 60, 120 }, Type.ELEC, Type.NONE));
    }


    public class Pokemon
    {
        public int id;
        public string name;
        public int[] stat;
        public Type type1;
        public Type type2;

        public Pokemon(int id, string name, int[] stat, Type type1, Type type2)
        {
            this.id = id;
            this.name = name;
            this.stat = stat;
            this.type1 = type1;
            this.type2 = type2;
        }
    }

    public enum Type
    {
        NONE,
        NORMAL,
        FIRE,
        WATER,
        ELEC,
        GRASS,
        ICE,
        FIGHT,
        POISION,
        GROUND,
        FLY,
        PSY,
        BUG,
        ROCK,
        GHOST,
        DRAGON,
        DARK,
        STEEL,
        FAIRY
    }

    public static Color GetColorFromType(Type type)
    {
        if (type == Type.NORMAL)
            return Color.white;

        if (type == Type.GRASS)
            return new Color(0, 0.86f, 0.309f);

        if (type == Type.FIRE)
            return new Color(1f, 0.5f, 0.5f);

        if (type == Type.WATER)
            return new Color(0.41f, 0.7f, 1f);

        if (type == Type.WATER)
            return new Color(0.41f, 0.7f, 1f);

        if (type == Type.BUG)
            return new Color(0.83f, 0.82f, 0f);

        if (type == Type.ELEC)
            return new Color(1f, 1f, 0.39f);

        if (type == Type.GROUND)
            return new Color(0.85f, 0.61f, 0.36f);

        if (type == Type.PSY)
            return new Color(1f, 0.168f, 0.882f);

        if (type == Type.POISION)
            return new Color(0.784f, 0.392f, 0.784f);

        return Color.white;
    }
}
