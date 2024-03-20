using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeDexInfo : MonoBehaviour
{
    public static PokeDexInfo instance;
    public Dictionary<int, Info> info;

    private void Awake()
    {
        instance = this;
        
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
        info = new Dictionary<int, Info>();
        info.Add(0, new Info("이상해씨", "씨앗포켓몬", "0.7m", "6.9kg", "등의 씨앗 안에는 영양이 가득하다. 씨앗은 몸과 함께 커진다."));
        info.Add(1, new Info("이상해풀", "씨앗포켓몬", "1.0m", "13.0kg", "등의 꽃봉오리에서 달콤한 향기가 나기 시작했다는 것은 이제 곧 커다란 꽃이 핀다는 증거다."));
        info.Add(2, new Info("이상해꽃", "씨앗포켓몬", "2.0m", "100.0kg", "내리쪼이는 태양빛을 에너지로 변환할 수 있기 때문에 여름에 더욱 강해진다."));
        info.Add(3, new Info("파이리", "도룡뇽포켓몬", "0.6m", "8.5kg", "태어날 때부터 꼬리의 불꽃이 타오르고 있다. 불꽃이 꺼지면 그 생명이 다하고 만다."));
        info.Add(4, new Info("리자드", "화염포켓몬", "1.1m", "19.0kg", "불타는 꼬리를 휘두르며 날카로운 발톱으로 상대를 베어 가르는 몹시 거친 성격이다."));
        info.Add(5, new Info("리자몽", "화염포켓몬", "1.7m", "90.5kg", "날개로 넓은 하늘을 높게 난다. 싸움의 경험을 쌓으면 불꽃의 온도가 높아진다."));
        info.Add(6, new Info("꼬부기", "거북포켓몬", "0.5m", "9.0kg", "태어나서 조금 지나면 딱딱해지는 등껍질에는 탄력성이 있어서 손가락으로 찌르면 튕겨버린다."));
        info.Add(7, new Info("어니부기", "거북포켓몬", "1.0m", "22.5kg", "장수의 상징으로 여겨진다. 등껍질에 이끼가 붙어 있는 것은 특히 장수한 어니부기다."));
        info.Add(8, new Info("거북왕", "껍질포켓몬", "1.6m", "85.5kg", "등껍질의 로켓포에서 뿜어져 나오는 제트 수류는 두꺼운 철판도 뚫는다."));
        info.Add(9, new Info("캐터피", "애벌레포켓몬", "0.3m", "2.9kg", "푸른 피부로 감싸져 있다. 탈피하여 성장하면 실을 늘어뜨려 번데기로 바꾼다."));
        info.Add(10, new Info("단데기", "번데기포켓몬", "0.7m", "9.9kg", "껍질 안은 진화의 준비로 매우 부드럽고 부서지기 쉽다. 가능하면 움직이지 않으려 하고 있다."));
        info.Add(11, new Info("버터플", "나비포켓몬", "1.1m", "32.0kg", "매일 꿀을 모은다. 발의 솜털에 꿀을 묻혀 둥지에 가지고 돌아가는 습성을 가졌다."));
        info.Add(12, new Info("뿔충이", "송충이포켓몬", "0.3m", "3.2kg", "독바늘은 매우 강력. 눈에 잘 띄는 몸의 색은 상대에게 경계시키기 위해서이다."));
        info.Add(13, new Info("딱충이", "번데기포켓몬", "0.6m", "10.0kg", "번데기지만 약간이라면 움직인다. 적에게 습격받게 되면 독바늘을 뿜는 일도 있다."));
        info.Add(14, new Info("독침붕", "독벌포켓몬", "1.0m", "29.5kg", "어떠한 상대라도 강력한 독침으로 쏴 죽인다. 가끔은 집단적으로 공격해온다."));
        info.Add(15, new Info("구구", "아기새포켓몬", "0.3m", "1.8kg", "보통은 풀숲에 숨어있다. 싸움은 좋아하지 않아 습격받으면 필사적으로 모래를 뿌려 몸을 지킨다."));
        info.Add(16, new Info("피죤", "새포켓몬", "1.1m", "30.0kg", "매우 시력이 좋다. 아무리 높은 장소에서라도 먹이의 움직이는 모습을 구별한다."));
        info.Add(17, new Info("피죤투", "새포켓몬", "1.5m", "39.5kg", "발달된 가슴의 근육은 가볍게 퍼덕이는 것만으로 큰 바람을 일으킬 정도다."));
        info.Add(18, new Info("꼬렛", "쥐포켓몬", "0.3m", "3.5kg", "어떠한 것이라도 먹기에 먹이가 되는 것이 있는 장소에 정착해 점점 아이를 낳는다."));
        info.Add(19, new Info("레트라", "쥐포켓몬", "0.7m", "18.5kg", "단단한 송곳니로 무엇이라도 갉는다. 콘크리트로 된 빌딩이라도 갉아서 쓰러뜨려버린다."));
        info.Add(20, new Info("깨비참", "아기새포켓몬", "0.3m", "2.0kg", "자신의 구역을 지키기위해 작은 날개를 쳐서 바쁘게 주위를 날아 맴돈다."));
        info.Add(21, new Info("깨비드릴조", "부리포켓몬", "1.2m", "38.0kg", "단숨에 높은 하늘까지 날아올라 거기서부터 급강하로 단숨에 공격해온다."));
        info.Add(22, new Info("피카츄", "생쥐포켓몬", "0.4m", "6.0kg", "양 볼에는 전기를 저장하는 주머니가 있다. 화가 나면 저장한 전기를 단숨에 방출한다."));
    }

    public class Info
    {
        public string name;
        public string kind;
        public string height;
        public string weight;
        public string detail;

        public Info(string name, string kind, string height, string weight, string detail)
        {
            this.name = name;
            this.kind = kind;
            this.height = height;
            this.weight = weight;
            this.detail = detail;
        }
    }

}
