using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonSkillInfo : MonoBehaviour
{
    public static PokemonSkillInfo Instance;
    public Dictionary<int, PokemonSkill> skills;


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
        skills.Add(1, new PokemonSkill(1, "�����ġ��", SkillType.PHYSICAL, 40, 35, PokemonInfo.Type.NORMAL));
        skills.Add(2, new PokemonSkill(2, "������", SkillType.PHYSICAL, 40, 35, PokemonInfo.Type.NORMAL));
        skills.Add(3, new PokemonSkill(3, "�ɱ�", SkillType.PHYSICAL, 35, 35, PokemonInfo.Type.FLY));
        skills.Add(4, new PokemonSkill(4, "�ٳ�������", SkillType.PHYSICAL, 40, 25, PokemonInfo.Type.GRASS));
        skills.Add(5, new PokemonSkill(5, "�Ҳɼ���", SkillType.SPECIAL, 40, 25, PokemonInfo.Type.FIRE));
        skills.Add(6, new PokemonSkill(6, "������", SkillType.SPECIAL, 40, 25, PokemonInfo.Type.WATER));
        skills.Add(7, new PokemonSkill(7, "��������", SkillType.STATUS, 0, 30, PokemonInfo.Type.NORMAL));
        skills.Add(8, new PokemonSkill(8, "�����Ҹ�", SkillType.STATUS, 0, 40, PokemonInfo.Type.NORMAL));
        skills.Add(9, new PokemonSkill(9, "����", SkillType.STATUS, 0, 20, PokemonInfo.Type.NORMAL));
        skills.Add(10, new PokemonSkill(10, "�𷡻Ѹ���", SkillType.STATUS, 0, 15, PokemonInfo.Type.GROUND));
        skills.Add(11, new PokemonSkill(11, "�ٶ�����Ű��", SkillType.SPECIAL, 40, 35, PokemonInfo.Type.FLY));
        skills.Add(12, new PokemonSkill(12, "����ġ��", SkillType.PHYSICAL, 60, 35, PokemonInfo.Type.FLY));
        skills.Add(13, new PokemonSkill(13, "������", SkillType.STATUS, 0, 35, PokemonInfo.Type.POISION));
        skills.Add(14, new PokemonSkill(14, "��������", SkillType.STATUS, 0, 30, PokemonInfo.Type.GRASS));
        skills.Add(15, new PokemonSkill(15, "�ް��巹��", SkillType.SPECIAL, 40, 15, PokemonInfo.Type.GRASS));
        skills.Add(16, new PokemonSkill(16, "ȭ�����", SkillType.SPECIAL, 60, 25, PokemonInfo.Type.FIRE));
        skills.Add(17, new PokemonSkill(17, "�������", SkillType.PHYSICAL, 70, 20, PokemonInfo.Type.NORMAL));
        skills.Add(18, new PokemonSkill(18, "��ǰ����", SkillType.SPECIAL, 65, 20, PokemonInfo.Type.WATER));
        skills.Add(19, new PokemonSkill(19, "������ ����", SkillType.STATUS, 0, 40, PokemonInfo.Type.WATER));
        skills.Add(20, new PokemonSkill(20, "�ǻձ�", SkillType.STATUS, 0, 40, PokemonInfo.Type.BUG));
        skills.Add(21, new PokemonSkill(21, "�ܴ�������", SkillType.STATUS, 0, 30, PokemonInfo.Type.NORMAL));
        skills.Add(22, new PokemonSkill(22, "������", SkillType.SPECIAL, 50, 25, PokemonInfo.Type.PSY));
        skills.Add(23, new PokemonSkill(23, "��ħ", SkillType.PHYSICAL, 15, 35, PokemonInfo.Type.POISION));
        skills.Add(24, new PokemonSkill(24, "�����", SkillType.PHYSICAL, 80, 20, PokemonInfo.Type.POISION));
        skills.Add(25, new PokemonSkill(25, "������ȭ", SkillType.PHYSICAL, 40, 30, PokemonInfo.Type.NORMAL));
        skills.Add(26, new PokemonSkill(26, "�ʻ�մ�", SkillType.PHYSICAL, 80, 15, PokemonInfo.Type.NORMAL));
        skills.Add(27, new PokemonSkill(27, "ȸ���θ�", SkillType.PHYSICAL, 80, 20, PokemonInfo.Type.FLY));
        skills.Add(28, new PokemonSkill(28, "�����ũ", SkillType.SPECIAL, 40, 30, PokemonInfo.Type.ELEC));
        skills.Add(29, new PokemonSkill(29, "10����Ʈ", SkillType.SPECIAL, 90, 15, PokemonInfo.Type.ELEC));
        skills.Add(30, new PokemonSkill(30, "Ƣ�������", SkillType.STATUS, 0, 40, PokemonInfo.Type.NORMAL));
        skills.Add(31, new PokemonSkill(31, "�����θ���", SkillType.PHYSICAL, 120, 10, PokemonInfo.Type.NORMAL));
        skills.Add(32, new PokemonSkill(32, "�ı�����", SkillType.SPECIAL, 150, 5, PokemonInfo.Type.NORMAL));
    }

    public class PokemonSkill
    {
        public int id;//���� ���̵�
        public string name;
        public SkillType kind;//[0]����, [1]Ư��, [2]��ȭ��
        public int damge;//����
        public int ppMax;
        public PokemonInfo.Type type;//Ÿ��

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
}
