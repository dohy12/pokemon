using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public static Player player;

    private GlobalInput input;
    public bool canControl;

    public float inputStun = 0;
    public float inputStunImageIndex = 0;
    private float jumpingCheck = 0;
    private bool isTileCheck = false;

    private MapManager mapManager;
    private GameObject shadow;
    private SpriteRenderer enterGrassRenderer;
    public Sprite[] enterGrassSprites;
    private float enterGrassCheck;
    private bool isEnterGrass = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpdate();
        InputUpdate();
        JumpUpdate();
        SpriteUpdate();

        SetSprite();

        EnterGrassUpdate();
    }    


    private void InputUpdate()
    {
        movingCheck -= Time.deltaTime;
        inputStun -= Time.deltaTime;
        inputStunImageIndex -= Time.deltaTime;

        float hCheck = input.horizontal;
        float vCheck = input.vertical;

        if (jumpingCheck < 0 && movingStep < 0)
        {
            if (isTileCheck)
            {
                // 다음 타일에 도착했을 때
                isTileCheck = false;
                CheckTile();
            }            

            if (Mathf.Abs(hCheck) > 0.1f || Mathf.Abs(vCheck) > 0.1f)//키 입력이 있을 때
            {                               
                if (inputStun < 0)
                if (movingCheck < 0)
                {
                    float tmpDirec = direc;
                    direc = GetDirecFromVector(vCheck, hCheck, 0, direc); //저항값을 0을 줘서 가볍게 눌러도 방향 바뀌게

                    //멈춰 있을 때 
                    //1. 같은 방향으로 키를 눌렀을 경우 가볍게 눌러도 나아감
                    //2. 방향이 바뀌었을 경우 0.n초 스턴 걸림

                    if (tmpDirec == direc)//1. 같은 방향으로 키를 눌렀을 경우 가볍게 눌러도 나아감
                    {
                        if (Mathf.Abs(hCheck) > 0.3f || Mathf.Abs(vCheck) > 0.3f)//살짝 누른 키는 인식 안되게
                        {
                            StartMove();
                        }
                    }
                    else //2. 방향이 바뀌었을 경우 0.n초 스턴 걸림
                    {
                        inputStun = 0.12f;
                        inputStunImageIndex = 0.12f;
                    }                    
                }
                else
                {
                    direc = GetDirecFromVector(vCheck, hCheck, 0.7f, direc); //저항값을 0.7을 줘서 떼고 있는 키는 무시

                    if (Mathf.Abs(hCheck) > 0.7f || Mathf.Abs(vCheck) > 0.7f)//살짝 누른 키는 인식 안되게
                    {
                        StartMove();
                    }
                }

            }
        }      

        int GetDirecFromVector(float v, float h, float resist, int direc)
        {
            if (v > resist) return 2;
            if (v < -resist) return 0;
            if (h < -resist) return 1;
            if (h > resist) return 3;

            return direc;
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
                inputStun = 0.4f;
                inputStunImageIndex = 0.2f;
                Walk();
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

        }

        void GoPotal(string potalName)
        {
            int potalId = Int32.Parse(potalName.Substring(5));
            PotalInfo potalInfo = mapManager.GetPotalInfo();

            PotalInfo.Potal potal = potalInfo.GetPotal(potalId);

            direc = potal.direc;
            moveX = potal.pos.x;
            moveY = potal.pos.y;
            MoveOrder(direc);
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
}
