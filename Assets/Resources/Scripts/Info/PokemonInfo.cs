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
        pokemons.Add(0, new Pokemon(0, "ÀÌ»óÇØ¾¾", new int[] { 45, 49, 49, 65, 65, 45 }, Type.GRASS, Type.POISION));
        pokemons.Add(1, new Pokemon(1, "ÀÌ»óÇØÇ®", new int[] { 60, 62, 63, 80, 80, 60 }, Type.GRASS, Type.POISION));
        pokemons.Add(2, new Pokemon(2, "ÀÌ»óÇØ²É", new int[] { 80, 82, 83, 100, 100, 80 }, Type.GRASS, Type.POISION));
        pokemons.Add(3, new Pokemon(3, "ÆÄÀÌ¸®", new int[] { 39, 52, 43, 60, 50, 65 }, Type.FIRE, Type.NONE));
        pokemons.Add(4, new Pokemon(4, "¸®ÀÚµå", new int[] { 58, 64, 58, 80, 65, 80 }, Type.FIRE, Type.NONE));
        pokemons.Add(5, new Pokemon(5, "¸®ÀÚ¸ù", new int[] { 78, 84, 78, 109, 85, 100 }, Type.FIRE, Type.FLY));
        pokemons.Add(6, new Pokemon(6, "²¿ºÎ±â", new int[] { 44, 48, 65, 50, 64, 43 }, Type.WATER, Type.NONE));
        pokemons.Add(7, new Pokemon(7, "¾î´ÏºÎ±â", new int[] { 59, 63, 80, 65, 80, 58 }, Type.WATER, Type.NONE));
        pokemons.Add(8, new Pokemon(8, "°ÅºÏ¿Õ", new int[] { 79, 83, 100, 85, 105, 78 }, Type.WATER, Type.NONE));
        pokemons.Add(9, new Pokemon(9, "Ä³ÅÍÇÇ", new int[] { 45, 30, 35, 20, 20, 45 }, Type.BUG, Type.NONE));
        pokemons.Add(10, new Pokemon(10, "´Üµ¥±â", new int[] { 50, 20, 55, 25, 25, 30 }, Type.BUG, Type.NONE));
        pokemons.Add(11, new Pokemon(11, "¹öÅÍÇÃ", new int[] { 60, 45, 50, 90, 80, 70 }, Type.BUG, Type.FLY));
        pokemons.Add(12, new Pokemon(12, "»ÔÃæÀÌ", new int[] { 40, 35, 30, 20, 20, 50 }, Type.BUG, Type.POISION));
        pokemons.Add(13, new Pokemon(13, "µüÃæÀÌ", new int[] { 45, 25, 50, 25, 25, 35 }, Type.BUG, Type.POISION));
        pokemons.Add(14, new Pokemon(14, "µ¶Ä§ºØ", new int[] { 65, 90, 40, 45, 80, 75 }, Type.BUG, Type.POISION));
        pokemons.Add(15, new Pokemon(15, "±¸±¸", new int[] { 40, 45, 40, 35, 35, 56 }, Type.NORMAL, Type.FLY));
        pokemons.Add(16, new Pokemon(16, "ÇÇÁÔ", new int[] { 63, 60, 55, 50, 50, 71 }, Type.NORMAL, Type.FLY));
        pokemons.Add(17, new Pokemon(17, "ÇÇÁÔÅõ", new int[] { 83, 80, 75, 70, 70, 101 }, Type.NORMAL, Type.FLY));
        pokemons.Add(18, new Pokemon(18, "²¿·¿", new int[] { 30, 56, 35, 25, 35, 72 }, Type.NORMAL, Type.NONE));
        pokemons.Add(19, new Pokemon(19, "·¹Æ®¶ó", new int[] { 55, 81, 60, 50, 70, 97 }, Type.NORMAL, Type.NONE));
        pokemons.Add(20, new Pokemon(20, "±úºñÂü", new int[] { 40, 60, 30, 31, 31, 70 }, Type.NORMAL, Type.FLY));
        pokemons.Add(21, new Pokemon(21, "±úºñµå¸±Á¶", new int[] { 65, 90, 65, 61, 61, 100 }, Type.NORMAL, Type.FLY));
        pokemons.Add(22, new Pokemon(22, "ÇÇÄ«Ãò", new int[] { 45, 80, 50, 75, 60, 120 }, Type.ELEC, Type.NONE));
        pokemons.Add(23, new Pokemon(23, "À×¾îÅ·", new int[] { 20, 10, 55, 15, 20, 80 }, Type.WATER, Type.NONE));
        pokemons.Add(24, new Pokemon(24, "°¼¶óµµ½º", new int[] { 95, 125, 79, 60, 100, 81 }, Type.WATER, Type.FLY));
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

        if (type == Type.FLY)
            return new Color(0.411764f, 0.9607f, 0.784f);

        return Color.white;
    }

    public static Color GetColorFromType2(Type type)
    {
        if (type == Type.NORMAL)
            return new Color(148 / 255f, 148 / 255f, 148 / 255f);

        if (type == Type.GRASS)
            return new Color(102 / 255f, 169 / 255f, 069 / 255f);

        if (type == Type.FIRE)
            return new Color(229 / 255f, 108 / 255f, 062 / 255f);

        if (type == Type.WATER)
            return new Color(081 / 255f, 133 / 255f, 197 / 255f);

        if (type == Type.BUG)
            return new Color(159 / 255f, 162 / 255f, 068 / 255f);

        if (type == Type.ELEC)
            return new Color(246 / 255f, 216 / 255f, 081 / 255f);

        if (type == Type.GROUND)
            return new Color(156 / 255f, 119 / 255f, 067 / 255f);

        if (type == Type.PSY)
            return new Color(221 / 255f, 107 / 255f, 123 / 255f);

        if (type == Type.POISION)
            return new Color(115 / 255f, 081 / 255f, 152 / 255f);

        if (type == Type.FLY)
            return new Color(162 / 255f, 195 / 255f, 231 / 255f);

        return Color.white;
    }

    public static string TypeToString(Type type)
    {
        if (type == Type.NORMAL)
            return "³ë¸»";

        if (type == Type.GRASS)
            return "Ç®";

        if (type == Type.FIRE)
            return "ºÒ²É";

        if (type == Type.WATER)
            return "¹°";

        if (type == Type.BUG)
            return "¹ú·¹";

        if (type == Type.ELEC)
            return "¹ø°³";

        if (type == Type.GROUND)
            return "¶¥";

        if (type == Type.PSY)
            return "¿¡½ºÆÛ";

        if (type == Type.POISION)
            return "µ¶";

        if (type == Type.FLY)
            return "ºñÇà";

        return "";
    }
}
