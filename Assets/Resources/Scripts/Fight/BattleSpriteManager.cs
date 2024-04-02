using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSpriteManager : MonoBehaviour
{
    public static BattleSpriteManager instance;

    public Sprite[] effSprs;

    private FightManager fight;

    public int effKind;

    private float endTime = 1f;
    private int target;
    private int other;
    private float timeCh;
    private bool isActive = false;
    private Image[] pokeImgs;
    private Image[] objSkillEffs;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        fight = FightManager.instance;
        pokeImgs = fight.pokeSprites;

        objSkillEffs = new Image[10];

        for (var i = 0; i < 10; i++)
        {
            objSkillEffs[i] = transform.GetChild(i).GetComponent<Image>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        TimeUpdate();
        EffUpdate();
    }

    private void TimeUpdate()
    {
        if (isActive)
        {
            timeCh += Time.deltaTime;

            if (timeCh >= endTime) 
            {
                UnActive();
            }

            FightQueueManager.instance.eventCh = 0.1f;
            GlobalInput.globalInput.InputStun(0.2f);


        }
    }

    private void UnActive()
    {
        timeCh = 0f;
        isActive = false;

        for (var i = 0; i < 10; i++)
        {
            var tmp = ((RectTransform)objSkillEffs[i].transform);
            tmp.anchoredPosition = new Vector2(500f, 300f);
            tmp.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void Active(int target, int effKind)
    {
        Debug.Log("EFFKIND : " + effKind);
        this.target = target;
        this.other = Mathf.Abs(this.target - 1);
        this.effKind = effKind;
        isActive = true;

        EffActive();
    }

    private void EffActive()
    {
        if (effKind == 1)//¸öÅë¹ÚÄ¡±â
        {
            endTime = 0.4f;
            objSkillEffs[0].sprite = effSprs[3];
            if (target == 1)
            {
                ((RectTransform)objSkillEffs[0].transform).anchoredPosition = new Vector2(176.3f, 60.9f);
            }
            else
            {
                ((RectTransform)objSkillEffs[0].transform).anchoredPosition = new Vector2(-209f, -81f);
            }
            objSkillEffs[0].enabled = false;
        }


        if (effKind == 2)//ÇÒÄû±â
        {
            endTime = 0.6f;
            objSkillEffs[0].sprite = effSprs[0];
            if (target == 1)
            {
                ((RectTransform)objSkillEffs[0].transform).anchoredPosition = new Vector2(176.3f, 60.9f);
            }
            else
            {
                var tmp = ((RectTransform)objSkillEffs[0].transform);
                tmp.anchoredPosition = new Vector2(-209f, -81f);
                tmp.localScale = new Vector3(-1f, 1f, 1f);
            }
            objSkillEffs[0].enabled = false;
        }
    }

    private void EffUpdate()
    {
        if (isActive)
        {
            if (effKind == 1)//¸öÅë¹ÚÄ¡±â
            {
                if (timeCh < 0.3f)
                {
                    var xx = Mathf.Sin(timeCh / 0.3f * Mathf.PI) * 40f;

                    if (other != 0) xx *= -1;

                    var originPos = fight.imgStartPos[other];
                    ((RectTransform)pokeImgs[other].transform).anchoredPosition = new Vector2(originPos.x + xx, originPos.y);
                }
                else
                {
                    objSkillEffs[0].enabled = true;
                    var imgIndex = 3;
                    objSkillEffs[0].sprite = effSprs[imgIndex];
                }
            }

            if (effKind == 2)//ÇÒÄû±â
            {
                if (timeCh < 0.3f)
                {
                    var xx = Mathf.Sin(timeCh / 0.3f * Mathf.PI) * 40f;

                    if (other != 0) xx *= -1;

                    var originPos = fight.imgStartPos[other];
                    ((RectTransform)pokeImgs[other].transform).anchoredPosition = new Vector2(originPos.x + xx, originPos.y);
                }
                else
                {
                    objSkillEffs[0].enabled = true;
                    var imgIndex = (int)((timeCh - 0.3f) * 9);
                    objSkillEffs[0].sprite = effSprs[imgIndex];
                }
                
            }
        }
    }

}
