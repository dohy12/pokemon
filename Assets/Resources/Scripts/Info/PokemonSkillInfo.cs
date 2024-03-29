using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonSkillInfo : MonoBehaviour
{
    public static PokemonSkillInfo Instance;
    public Dictionary<int, PokemonSkill> skills;
    public Dictionary<int, Tuple<int, int>[]> pokeLevelMove;

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

    void Init()
    {
        skills = new Dictionary<int, PokemonSkill>();
        skills.Add(0, null);
        skills.Add(1, new PokemonSkill(1, "몸통박치기", SkillType.PHYSICAL, 40, 35, PokemonInfo.Type.NORMAL));
        skills.Add(2, new PokemonSkill(2, "할퀴기", SkillType.PHYSICAL, 40, 35, PokemonInfo.Type.NORMAL));
        skills.Add(3, new PokemonSkill(3, "쪼기", SkillType.PHYSICAL, 35, 35, PokemonInfo.Type.FLY));
        skills.Add(4, new PokemonSkill(4, "잎날가르기", SkillType.PHYSICAL, 40, 25, PokemonInfo.Type.GRASS));
        skills.Add(5, new PokemonSkill(5, "불꽃세례", SkillType.SPECIAL, 40, 25, PokemonInfo.Type.FIRE));
        skills.Add(6, new PokemonSkill(6, "물대포", SkillType.SPECIAL, 40, 25, PokemonInfo.Type.WATER));
        skills.Add(7, new PokemonSkill(7, "꼬리흔들기", SkillType.STATUS, 0, 30, PokemonInfo.Type.NORMAL));
        skills.Add(8, new PokemonSkill(8, "울음소리", SkillType.STATUS, 0, 40, PokemonInfo.Type.NORMAL));
        skills.Add(9, new PokemonSkill(9, "연막", SkillType.STATUS, 0, 20, PokemonInfo.Type.NORMAL));
        skills.Add(10, new PokemonSkill(10, "모래뿌리기", SkillType.STATUS, 0, 15, PokemonInfo.Type.GROUND));
        skills.Add(11, new PokemonSkill(11, "바람일으키기", SkillType.SPECIAL, 40, 35, PokemonInfo.Type.FLY));
        skills.Add(12, new PokemonSkill(12, "날개치기", SkillType.PHYSICAL, 60, 35, PokemonInfo.Type.FLY));
        skills.Add(13, new PokemonSkill(13, "독가루", SkillType.STATUS, 0, 35, PokemonInfo.Type.POISION));
        skills.Add(14, new PokemonSkill(14, "저리가루", SkillType.STATUS, 0, 30, PokemonInfo.Type.GRASS));
        skills.Add(15, new PokemonSkill(15, "메가드레인", SkillType.SPECIAL, 40, 15, PokemonInfo.Type.GRASS));
        skills.Add(16, new PokemonSkill(16, "화염방사", SkillType.SPECIAL, 60, 25, PokemonInfo.Type.FIRE));
        skills.Add(17, new PokemonSkill(17, "베어가르기", SkillType.PHYSICAL, 70, 20, PokemonInfo.Type.NORMAL));
        skills.Add(18, new PokemonSkill(18, "거품광선", SkillType.SPECIAL, 65, 20, PokemonInfo.Type.WATER));
        skills.Add(19, new PokemonSkill(19, "껍질에 숨기", SkillType.STATUS, 0, 40, PokemonInfo.Type.WATER));
        skills.Add(20, new PokemonSkill(20, "실뿜기", SkillType.STATUS, 0, 40, PokemonInfo.Type.BUG));
        skills.Add(21, new PokemonSkill(21, "단단해지기", SkillType.STATUS, 0, 30, PokemonInfo.Type.NORMAL));
        skills.Add(22, new PokemonSkill(22, "염동력", SkillType.SPECIAL, 50, 25, PokemonInfo.Type.PSY));
        skills.Add(23, new PokemonSkill(23, "독침", SkillType.PHYSICAL, 15, 35, PokemonInfo.Type.POISION));
        skills.Add(24, new PokemonSkill(24, "독찌르기", SkillType.PHYSICAL, 80, 20, PokemonInfo.Type.POISION));
        skills.Add(25, new PokemonSkill(25, "전광석화", SkillType.PHYSICAL, 40, 30, PokemonInfo.Type.NORMAL));
        skills.Add(26, new PokemonSkill(26, "필살앞니", SkillType.PHYSICAL, 80, 15, PokemonInfo.Type.NORMAL));
        skills.Add(27, new PokemonSkill(27, "회전부리", SkillType.PHYSICAL, 80, 20, PokemonInfo.Type.FLY));
        skills.Add(28, new PokemonSkill(28, "전기쇼크", SkillType.SPECIAL, 40, 30, PokemonInfo.Type.ELEC));
        skills.Add(29, new PokemonSkill(29, "10만볼트", SkillType.SPECIAL, 90, 15, PokemonInfo.Type.ELEC));
        skills.Add(30, new PokemonSkill(30, "튀어오르기", SkillType.STATUS, 0, 40, PokemonInfo.Type.NORMAL));
        skills.Add(31, new PokemonSkill(31, "난동부리기", SkillType.PHYSICAL, 120, 10, PokemonInfo.Type.NORMAL));
        skills.Add(32, new PokemonSkill(32, "파괴광선", SkillType.SPECIAL, 150, 5, PokemonInfo.Type.NORMAL));

        pokeLevelMove = new Dictionary<int, Tuple<int, int>[]>();
        //이상해씨 라인
        pokeLevelMove.Add(0, new Tuple<int, int>[] { GetT(0, 1), GetT(0, 8), GetT(7, 4), GetT(10, 13), GetT(15, 15) });
        pokeLevelMove.Add(1, new Tuple<int, int>[] { GetT(0, 1), GetT(0, 8), GetT(7, 4), GetT(10, 13), GetT(15, 15) });
        pokeLevelMove.Add(2, new Tuple<int, int>[] { GetT(0, 4), GetT(0, 13), GetT(0, 15) });

        //파이리 라인
        pokeLevelMove.Add(3, new Tuple<int, int>[] { GetT(0, 2), GetT(0, 7), GetT(7, 5), GetT(10, 9), GetT(15, 16) });
        pokeLevelMove.Add(4, new Tuple<int, int>[] { GetT(0, 2), GetT(0, 7), GetT(7, 5), GetT(10, 9), GetT(15, 16) });
        pokeLevelMove.Add(5, new Tuple<int, int>[] { GetT(0, 5), GetT(0, 9), GetT(0, 16) });

        //꼬부기 라인
        pokeLevelMove.Add(6, new Tuple<int, int>[] { GetT(0, 1), GetT(0, 7), GetT(7, 6), GetT(10, 19), GetT(15, 18) });
        pokeLevelMove.Add(7, new Tuple<int, int>[] { GetT(0, 1), GetT(0, 7), GetT(7, 6), GetT(10, 19), GetT(15, 18) });
        pokeLevelMove.Add(8, new Tuple<int, int>[] { GetT(0, 6), GetT(0, 19), GetT(0, 18) });

        //캐터피 라인
        pokeLevelMove.Add(9, new Tuple<int, int>[] { GetT(0, 1), GetT(0, 20) });
        pokeLevelMove.Add(10, new Tuple<int, int>[] { GetT(0, 21) });
        pokeLevelMove.Add(11, new Tuple<int, int>[] { GetT(0, 8) });

        //독침붕 라인
        pokeLevelMove.Add(12, new Tuple<int, int>[] { GetT(0, 23), GetT(0, 20) });
        pokeLevelMove.Add(13, new Tuple<int, int>[] { GetT(0, 21) });
        pokeLevelMove.Add(14, new Tuple<int, int>[] { GetT(0, 24) });

        //구구
        pokeLevelMove.Add(15, new Tuple<int, int>[] { GetT(0, 1), GetT(0, 10), GetT(5, 11), GetT(10, 12) });
        pokeLevelMove.Add(16, new Tuple<int, int>[] { GetT(0, 1), GetT(0, 10), GetT(5, 11), GetT(10, 12) });
        pokeLevelMove.Add(17, new Tuple<int, int>[] { GetT(0, 1), GetT(0, 10), GetT(5, 11), GetT(10, 12) });

        //꼬렛
        pokeLevelMove.Add(18, new Tuple<int, int>[] { GetT(0, 1), GetT(0, 7), GetT(5, 25), GetT(10, 10) });
        pokeLevelMove.Add(19, new Tuple<int, int>[] { GetT(0, 1), GetT(0, 7), GetT(5, 25), GetT(10, 10) });

        //깨비참
        pokeLevelMove.Add(20, new Tuple<int, int>[] { GetT(0, 3), GetT(0, 8), GetT(5, 25), GetT(10, 27) });
        pokeLevelMove.Add(21, new Tuple<int, int>[] { GetT(0, 3), GetT(0, 8), GetT(5, 25), GetT(10, 27) });

        //피카츄
        pokeLevelMove.Add(22, new Tuple<int, int>[] { GetT(0, 28), GetT(0, 7), GetT(5, 25), GetT(10, 29) });

        //잉어킹
        pokeLevelMove.Add(23, new Tuple<int, int>[] { GetT(0, 30), GetT(5, 1) });
        pokeLevelMove.Add(24, new Tuple<int, int>[] { GetT(0, 1), GetT(0, 10), GetT(15, 15) });
    }

    private Tuple<int, int> GetT(int level, int skill)
    {
        return new Tuple<int, int>( level, skill );
    }

    public class PokemonSkill
    {
        public int id;//고유 아이디
        public string name;
        public SkillType kind;//[0]물리, [1]특수, [2]변화기
        public int damge;//위력
        public int ppMax;
        public PokemonInfo.Type type;//타입

        public PokemonSkill(int id, string name, SkillType kind, int damge, int ppMax, PokemonInfo.Type type)
        {
            this.id = id;
            this.name = name;
            this.kind = kind;
            this.damge = damge;
            this.ppMax = ppMax;
            this.type = type;
        }
    }

    public enum SkillType
    {
        PHYSICAL,
        SPECIAL,
        STATUS
    }

    public int[] GetPokemonSkillByLevel(int pokeID, int level)
    {
        int[] moves = new int[4] {0,0,0,0};
        Tuple<int, int>[] pokeMoves = pokeLevelMove[pokeID];

        Queue<int> moveQueue = new Queue<int>();


        for (int i = 0; i< pokeMoves.Length; i++)
        {
            var levelMove = pokeMoves[i];
          
            if (levelMove.Item1 <= level)
            {
                moveQueue.Enqueue(levelMove.Item2);
                if (moveQueue.Count > 4)
                {
                    moveQueue.Dequeue();
                }
            }
        }

        var cnt = 0;
        while (moveQueue.Count > 0)
        {
            moves[cnt++] = moveQueue.Dequeue();
        }


        return moves;
    }
}
