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

    void Awake()
    {
        player = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        input = GlobalInput.globalInput;
        spriteRenderer = GetComponent<SpriteRenderer>();

        mapManager = MapManager.mapManager;
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpdate();
        InputUpdate();
        JumpUpdate();
        SpriteUpdate();

        SetSprite();        
    }    


    private void InputUpdate()
    {
        
        inputStun -= Time.deltaTime;
        inputStunImageIndex -= Time.deltaTime;

        float hCheck = input.horizontal;
        float vCheck = input.vertical;

        if (jumpingCheck < 0 && movingStep < 0)
        {
            if (isTileCheck)
            {
                // ���� Ÿ�Ͽ� �������� ��
                isTileCheck = false;
                CheckTile();
            }            

            if (Mathf.Abs(hCheck) > 0.1f || Mathf.Abs(vCheck) > 0.1f)//Ű �Է��� ���� ��
            {                               
                if (inputStun < 0)
                if (movingCheck < 0)
                {
                    float tmpDirec = direc;
                    direc = GetDirecFromVector(vCheck, hCheck, 0, direc); //���װ��� 0�� �༭ ������ ������ ���� �ٲ��

                    //���� ���� �� 
                    //1. ���� �������� Ű�� ������ ��� ������ ������ ���ư�
                    //2. ������ �ٲ���� ��� 0.n�� ���� �ɸ�

                    if (tmpDirec == direc)//1. ���� �������� Ű�� ������ ��� ������ ������ ���ư�
                    {
                        if (Mathf.Abs(hCheck) > 0.3f || Mathf.Abs(vCheck) > 0.3f)//��¦ ���� Ű�� �ν� �ȵǰ�
                        {
                            StartMove();
                        }
                    }
                    else //2. ������ �ٲ���� ��� 0.n�� ���� �ɸ�
                    {
                        inputStun = 0.12f;
                        inputStunImageIndex = 0.12f;
                    }                    
                }
                else
                {
                    direc = GetDirecFromVector(vCheck, hCheck, 0.7f, direc); //���װ��� 0.7�� �༭ ���� �ִ� Ű�� ����

                    if (Mathf.Abs(hCheck) > 0.7f || Mathf.Abs(vCheck) > 0.7f)//��¦ ���� Ű�� �ν� �ȵǰ�
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
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direcVector, 1f);

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
                        if (hits[j].transform.CompareTag("Jump"))
                        {
                            Jump();
                        }
                    }
                }
            }
        }

        void Jump()
        {
            jumpingCheck = 0.5f;
            inputStun = 0.7f;
        }
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
        }
        else
        {
            moveZ = 0f;
        }
    }  


    private void SpriteUpdate()
    {
        if (jumpingCheck < 0)
        {
            if (inputStunImageIndex < 0)
            {
                if (movingStep < 0 || movingStep > 0.5f / movingSpeed)//���� ���� ���
                {
                    isSpriteMov = false;
                }
                else
                {
                    isSpriteMov = true;
                }
            }
            else//���� ��ȯ �� ����
            {
                isSpriteMov = true;
            }
        }
        else
        {
            isSpriteMov = true;
        }
         
    }
}
