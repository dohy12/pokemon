using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PotalInfo;
using static UnityEditor.PlayerSettings;

public class Player : Unit
{
    public static Player player;

    private GlobalInput input;

    public float inputStun = 0;
    public float inputStunImageIndex = 0;
    private float jumpingCheck = 0;
    private bool isTileCheck = false;
    private bool isGoPotal = false;
    private Vector3 goPotalVector;

    private MapManager mapManager;
    private GameObject shadow;
    private SpriteRenderer enterGrassRenderer;
    public Sprite[] enterGrassSprites;
    private float enterGrassCheck;
    private bool isEnterGrass = false;

    private UIManager uiManager;

    void Awake()
    {
        player = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UnitStart();

        input = GlobalInput.globalInput;

        mapManager = MapManager.mapManager;

        shadow = transform.Find("Shadow").gameObject;
        shadow.SetActive(false);

        enterGrassRenderer = transform.Find("EnterGrass").GetComponent<SpriteRenderer>();
        enterGrassRenderer.gameObject.SetActive(false);

        uiManager = UIManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (ControllCheck())
        {            
            InputUpdate(); 
            PushAButton();
        }

        MoveUpdate();
        JumpUpdate();
        EnterGrassUpdate();

        SpriteUpdate();
        SetSprite();
    } 


    private void InputUpdate()
    {
        movingCheck -= Time.deltaTime;
        inputStun -= Time.deltaTime;    
        

        float hCheck = input.horizontal;
        float vCheck = input.vertical;

        float hCheckRaw = input.horizontalRaw;
        float vCheckRaw = input.verticalRaw;

        if (jumpingCheck < 0 && movingStep < 0)
        {
            if (isTileCheck)
            {
                // 다음 타일에 도착했을 때
                isTileCheck = false;
                CheckTile();
            }            
                           
            if (inputStun < 0)
            {
                if (movingCheck < 0)
                {
                    float tmpDirec = direc;
                    direc = GetDirecFromVector(vCheck, hCheck, direc); //방향 전환

                    if (tmpDirec != direc)
                    {
                        inputStunImageIndex = 0.12f;
                    }

                    if (Mathf.Abs(hCheck) > 0.3f || Mathf.Abs(vCheck) > 0.3f)
                    {
                        StartMove();
                    }                            
                }
                else
                {
                    direc = GetDirecFromVector(vCheckRaw, hCheckRaw, direc); //방향 전환
                    if (hCheckRaw !=0 || vCheckRaw != 0)
                    {
                        StartMove();
                    }
                }
            }
            
        }      

        int GetDirecFromVector(float v, float h, int direc)
        {
            var vValue = MathF.Abs(v);
            var hValue = MathF.Abs(h);


            if (vValue > 0 || hValue > 0)
            {
                if (vValue >= hValue)
                {
                    if (v > 0)
                        return 2;
                    else
                        return 0;
                }
                else
                {
                    if (h > 0)
                        return 3;
                    else
                        return 1;
                }
            }
            else
            {
                return direc;
            }

            
        }         

        void StartMove()
        {
            if (!CheckCollision())
            {
                isTileCheck = true;
                MoveOrder(direc);
            }
            else
            {
                inputStun = 0.25f;
                inputStunImageIndex = 0.15f;
                Walk();//왼발 오른발 교차
            } 
        }       

        bool CheckCollision()
        {
            Vector2 direcVector = GetVector2fromDirec(direc);
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + (Vector3)direcVector, direcVector, 0.01f);
            
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.CompareTag("Grass"))
                {
                    EnterGrass();
                }
            }

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.CompareTag("Potal"))
                {
                    GoPotal(hits[i].transform.name);                    
                    return true;
                }
            }

            for (int i=0; i < hits.Length; i++)
            {
                if (hits[i].transform.CompareTag("Block"))
                {
                    CheckJump(direcVector);
                    return true;
                }                    
            }            

            return false;
        }

        void EnterGrass()
        {
            enterGrassCheck = 3f;
            enterGrassRenderer.gameObject.SetActive(true);
            isEnterGrass = true;
        }

        void CheckTile()
        {
            if (isGoPotal)
            {
                direc = (int)goPotalVector.z;
                moveX = goPotalVector.x;
                moveY = goPotalVector.y;
                MoveOrder(direc);
                isGoPotal = false;
                isTileCheck = true;
            }

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, 0.01f);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.CompareTag("Event"))
                {
                    ActiveEvent(hits[i].transform.name);
                }
            }
        }

        void ActiveEvent(string eventIDstr )
        {
            int eventID = Int32.Parse(eventIDstr.Substring(5));
            EventManager.instance.StartEvent(eventID);
        }

        void GoPotal(string potalName)
        {
            int potalId = Int32.Parse(potalName.Substring(5));
            PotalInfo potalInfo = mapManager.GetPotalInfo();

            PotalInfo.Potal potal = potalInfo.GetPotal(potalId);

            isTileCheck = true;
            isGoPotal = true;
            goPotalVector = new Vector3(potal.pos.x, potal.pos.y, potal.direc);
            MoveOrder(direc);

            uiManager.ActiveBlackOut(movingSpeed);
        }

        void CheckJump(Vector2 direcVector)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direcVector, 0.01f);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.CompareTag("Jump"))
                {
                    RaycastHit2D[] hits2 = Physics2D.RaycastAll(transform.position + (Vector3)direcVector, direcVector, 0.01f);
                    for (int j = 0; j < hits2.Length; j++)
                    {
                        if (hits2[j].transform.CompareTag("Jump"))
                        {
                            Jump();
                            break;
                        }
                    }
                }
            }
        }        
    }

    public void Jump()
    {
        jumpingCheck = 0.5f;
        inputStun = 0.7f;
        shadow.SetActive(true);
    }


    private void JumpUpdate()
    {
        
        jumpingCheck -= Time.deltaTime;

        if (jumpingCheck > 0)
        {
            moveZ = -16 * jumpingCheck * jumpingCheck + 8 * jumpingCheck;

            Vector3 movingVector = GetVector2fromDirec(direc);
            moveX += movingVector.x * Time.deltaTime * 4;
            moveY += movingVector.y * Time.deltaTime * 4;

            shadow.transform.position = new Vector3(moveX, -0.4f + moveY, 0f);
        }
        else
        {
            moveZ = 0f;
            shadow.SetActive(false);
        }
    }  


    private void SpriteUpdate()
    {
        inputStunImageIndex -= Time.deltaTime;

        if (jumpingCheck < 0)
        {
            if (inputStunImageIndex < 0)
            {
                if (movingStep < 0 || movingStep > 0.5f / movingSpeed)//멈춰 있을 경우
                {
                    isSpriteMov = false;
                }
                else
                {
                    isSpriteMov = true;
                }
            }
            else//방향 전환 시 움찔
            {
                isSpriteMov = true;
            }
        }
        else
        {
            isSpriteMov = true;
        }
         
    }

    private void EnterGrassUpdate()
    {
        enterGrassCheck -= Time.deltaTime * 12;
        if (enterGrassCheck > 0)
        {
            int spriteIndex = Mathf.FloorToInt(3 - enterGrassCheck);
            enterGrassRenderer.sprite = enterGrassSprites[spriteIndex];
        }
        else
        {
            if (isEnterGrass)
            {
                enterGrassRenderer.gameObject.SetActive(false);
                isEnterGrass = false;
            }
        }
    }

    private void PushAButton()
    {
        if (movingStep < 0)
        {
            if (input.aButtonDown)
            {
                Vector2 direcVector = GetVector2fromDirec(direc);
                Npc npc = CheckUnit(transform.position + (Vector3)direcVector, direcVector);
                if (npc != null)
                {
                    if (!npc.isMoving)
                    {
                        npc.ActiveDialog();
                    }
                }
            }
        }
    }

    private Npc CheckUnit(Vector3 pos, Vector3 direcVector)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, direcVector, 0.01f);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("Block"))
            {
                var unit = hits[i].transform.GetComponent<Npc>();
                if (unit != null)
                {
                    return unit;
                }
            }

            if (hits[i].transform.CompareTag("Jump"))
            {
                return CheckUnit(pos + (Vector3)direcVector, direcVector);
            }
        }

        return null;
    }
}
