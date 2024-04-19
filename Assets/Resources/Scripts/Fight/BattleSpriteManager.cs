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

    private Vector2[] particlesPos;
    private Vector2[] particlesSpeedDirec;
    private float[] particlesAngle;
    private float[] particlesAngleAdd;
    private float[] particlesSize;
    private Color[] particlesColor;

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

        particlesPos = new Vector2[10];
        particlesSpeedDirec = new Vector2[10];
        particlesAngle = new float[10];
        particlesAngleAdd = new float[10];
        particlesColor = new Color[10];
        particlesSize = new float[10];
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

    private void EffInit()
    {
        for(var i=0;i < 10; i++)
        {
            var rectT = (RectTransform)objSkillEffs[i].transform;
            rectT.anchoredPosition = Vector2.zero;
            rectT.rotation = Quaternion.Euler(0, 0, 0);
            rectT.localScale = new Vector3(1f, 1f, 1f);

            objSkillEffs[i].color = Color.white;
            objSkillEffs[i].enabled = false;
        }
        
    }

    private void EffActive()
    {
        EffInit();

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

        if(effKind == 99)//¸ó½ºÅÍº¼ ÀÌÆåÆ®
        {
            endTime = 0.6f;
            for (var i = 0; i < 10; i++)
            {
                objSkillEffs[i].sprite = effSprs[9];
                particlesPos[i] = Vector2.zero;      
                particlesAngle[i] = Random.Range(0, 2 * Mathf.PI);
                particlesAngleAdd[i] = Random.Range(-2 * Mathf.PI, 2 * Mathf.PI);
                particlesSize[i] = Random.Range(0.4f, 0.6f);
                particlesColor[i] = new Color(1f, Random.Range(0, 1f), 0f);

                var tmpAngle = Random.Range(0, 2 * Mathf.PI);
                var tmpSpeed = Random.Range(200f, 400f);
                particlesSpeedDirec[i] = new Vector2(Mathf.Cos(tmpAngle), Mathf.Sin(tmpAngle)) * tmpSpeed;

                objSkillEffs[i].enabled = false;
            }
            
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

            if(effKind == 99)
            {
                for(int i = 0; i < 10; i++)
                {
                    particlesPos[i] += particlesSpeedDirec[i] * Time.deltaTime;
                    particlesAngle[i] += particlesAngleAdd[i] * Time.deltaTime;

                    Vector2 originPos;
                    if (target == 0) originPos = new Vector2(-234, -97);
                    else originPos = new Vector2(180, 50);
                    var rectT = (RectTransform)objSkillEffs[i].transform;
                    rectT.anchoredPosition = originPos + particlesPos[i];
                    rectT.rotation = Quaternion.Euler(0, 0, particlesAngle[i]);
                    rectT.localScale = new Vector3(particlesSize[i], particlesSize[i], 1f);
                    objSkillEffs[i].color = particlesColor[i];

                    objSkillEffs[i].enabled = true;
                }
            }
        }
    }

}
