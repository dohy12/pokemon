using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player player;

    private GlobalInput input;
    public bool canControl;

    public int direc = 0;//���� 0:�Ʒ� 1:����, 2:��, 3:������

    private float moveX = 0;
    private float moveY = 0;
    private float moveZ = 0;

    private float movingCheck = 0;
    private float movingStep = 0; 
    private float movingStun = 0;
    private float movingStunImageIndex = 0;
    private float movingSpeed = 5f; //�̵��ӵ�
    private int movingWalk = 2;//�޹�, ������
    private bool isMoving = false;

    private float jumpingCheck = 0;

    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

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
        JumpUpdate();

        SetSprite();

        transform.position = new Vector3(moveX, moveY + moveZ, 0);
    }    

    private void MoveUpdate()
    {
        movingCheck -= Time.deltaTime;
        movingStep -= Time.deltaTime;
        movingStun -= Time.deltaTime;
        movingStunImageIndex -= Time.deltaTime;

        float hCheck = input.horizontal;
        float vCheck = input.vertical;

        if (jumpingCheck < 0)
        if (movingStep > 0)
        {
            //�����̰� ���� ��

            Vector3 movingVector = GetVector2fromDirec(direc);
            moveX += movingVector.x * Time.deltaTime * movingSpeed;
            moveY += movingVector.y * Time.deltaTime * movingSpeed;
            movingCheck = 0.3f;
        }
        else
        {
            if (isMoving)
            {               
                //��ǥ ����
                float tmpX = (float)Math.Round(moveX);
                float tmpY = (float)Math.Round(moveY);
                moveX = tmpX;
                moveY = tmpY;
                isMoving = false;

                // ���� Ÿ�Ͽ� �������� ��
                CheckTile();
            }            

            if (Mathf.Abs(hCheck) > 0.1f || Mathf.Abs(vCheck) > 0.1f)//Ű �Է��� ���� ��
            {                               
                if (movingStun < 0)
                if (movingCheck < 0)
                {
                    float tmpDirec = direc;
                    direc = SwitchDirec(vCheck, hCheck, 0, direc); //���װ��� 0�� �༭ ������ ������ ���� �ٲ��

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
                        movingStun = 0.12f;
                        movingStunImageIndex = 0.12f;
                    }                    
                }
                else
                {
                    direc = SwitchDirec(vCheck, hCheck, 0.7f, direc); //���װ��� 0.7�� �༭ ���� �ִ� Ű�� ����

                    if (Mathf.Abs(hCheck) > 0.7f || Mathf.Abs(vCheck) > 0.7f)//��¦ ���� Ű�� �ν� �ȵǰ�
                    {
                        StartMove();
                    }
                }

            }
        }      

        int SwitchDirec(float v, float h, float resist, int direc)
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
                MoveOrder(direc);
            }
            else
            {
                movingStun = 0.4f;
                movingStunImageIndex = 0.2f;
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
            transform.position = potal.pos;
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
        }
    }

    public void DirecOrder(int direc)
    {
        this.direc = direc;
    }

    public void MoveOrder(int direc)
    {
        this.direc = direc;
        movingStep = 1f / movingSpeed;//���
        isMoving = true;

        if (movingWalk == 1)//�޹� ������
        {
            movingWalk = 2;
        }
        else
        {
            movingWalk = 1;
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

    private Vector2 GetVector2fromDirec(int direc)//���� 0:�Ʒ� 1:����, 2:��, 3:������
    {
        switch (direc)
        {
            case 0:
                return new Vector2(0f, -1f);

            case 1:
                return new Vector2(-1f, 0f);

            case 2:
                return new Vector2(0f, 1f);

            case 3:
                return new Vector2(1f, 0f);

            default: return new Vector2(0f, 1f);
        }
    }


    private void SetSprite()
    {        
        if (jumpingCheck < 0)
        {
            if (movingStunImageIndex < 0)
            {
                if (movingStep < 0)//���� ���� ���
                {
                    spriteRenderer.sprite = sprites[direc * 3];
                }
                else
                {
                    int tmpIndex = 0;

                    if (movingStep < 0.5f / movingSpeed)
                    {
                        tmpIndex = movingWalk;
                    }

                    spriteRenderer.sprite = sprites[direc * 3 + tmpIndex];
                }
            }
            else//���� ��ȯ �� ����
            {
                spriteRenderer.sprite = sprites[direc * 3 + 1];
            }
        }
        else
        {
            spriteRenderer.sprite = sprites[direc * 3 + 1];
        }
         
    }
}
