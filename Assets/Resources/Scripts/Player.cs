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

    private float movingCheck = 0;
    private float movingStep = 0; 
    private float movingStun = 0;
    private float movingStunImageIndex = 0;
    private float movingSpeed = 5f; //�̵��ӵ�
    private int movingWalk = 2;//�޹�, ������

    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;   

    void Awake()
    {
        player = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        input = GlobalInput.globalInput;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCheck();

        SetSprite();
    }    

    private void MoveCheck()
    {
        movingCheck -= Time.deltaTime;
        movingStep -= Time.deltaTime;
        movingStun -= Time.deltaTime;
        movingStunImageIndex -= Time.deltaTime;

        float hCheck = input.horizontal;
        float vCheck = input.vertical;

        if (movingStep > 0)
        {
            //�����̰� ���� ��

            Vector3 movingVector = GetVector2fromDirec(direc);
            transform.position += movingVector * Time.deltaTime * movingSpeed;
            movingCheck = 0.3f;
        }
        else
        {
            //���� ���� ��
            float tmpX = (float)Math.Round(transform.position.x);
            float tmpY = (float)Math.Round(transform.position.y);
            transform.position = new Vector3(tmpX, tmpY, 0); // ��ǥ ����

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

        Vector2 GetVector2fromDirec(int direc)//���� 0:�Ʒ� 1:����, 2:��, 3:������
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

        void StartMove()
        {
            if (!CheckCollision())
            {
                movingStep = 1f / movingSpeed;//���

                if (movingWalk == 1)//�޹� ������
                {
                    movingWalk = 2;
                }
                else
                {
                    movingWalk = 1;
                }
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direcVector, 1f);

            if (hit)
            {
                return true;
            }

            return false;
        }
    }  

    private void SetSprite()
    {        
        if (movingStunImageIndex < 0){
            if (movingStep <0)//���� ���� ���
            {
                spriteRenderer.sprite = sprites[direc*3];
            }
            else
            {
                int tmpIndex = 0;

                if (movingStep < 0.5f/movingSpeed)
                {
                    tmpIndex = movingWalk;
                }                

                spriteRenderer.sprite = sprites[direc*3 + tmpIndex];
            }
        }
        else//���� ��ȯ �� ����
        {
            spriteRenderer.sprite = sprites[direc*3 + 1];
        }
        

        
    }
}
