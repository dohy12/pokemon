using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GlobalInput input;
    public bool canControl;

    public int direc = 0;//���� 0:�Ʒ� 1:����, 2:��, 3:������

    private float movingCheck = 0;
    private float movingStep = 0;
    private float movingStun = 0;
    private float movingSpeed = 5f;

    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

   

    // Start is called before the first frame update
    void Start()
    {
        input = GlobalInput.globalInput;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        movingCheck -= Time.deltaTime;
        movingStep -= Time.deltaTime;
        movingStun -= Time.deltaTime;        

        float hCheck = input.horizontal;
        float vCheck = input.vertical;

        if (movingStep > 0)
        {
            //�����̰� ���� ��

            Vector3 movingVector = GetVector2fromDirec(direc);
            transform.position += movingVector * Time.deltaTime * movingSpeed;
            movingCheck = 0.2f;
        }
        else
        {
            //���� ���� ��

            if (Mathf.Abs(hCheck) < 0.1f && Mathf.Abs(vCheck) < 0.1f)//Ű �Է��� ���� ��
            {
                float tmpX = (float)Math.Round(transform.position.x);
                float tmpY = (float)Math.Round(transform.position.y);
                transform.position = new Vector3(tmpX, tmpY, 0); // ��ǥ ����
            }
            else
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
                        movingStep = 1f / movingSpeed;
                    }
                    else //2. ������ �ٲ���� ��� 0.n�� ���� �ɸ�
                    {
                        movingStun = 0.15f;
                    }                    
                }
                else
                {
                    direc = SwitchDirec(vCheck, hCheck, 0.7f, direc); //���װ��� 0.7�� �༭ ���� �ִ� Ű�� ����

                    if (Mathf.Abs(hCheck) > 0.7f || Mathf.Abs(vCheck) > 0.7f)//��¦ ���� Ű�� �ν� �ȵǰ�
                    {
                        movingStep = 1f / movingSpeed;
                    }
                }

            }
        }

        spriteRenderer.sprite = sprites[direc*3];
    }

    private int SwitchDirec(float v, float h, float resist, int direc)
    {
        if (v > resist) return 2;
        if (v < -resist) return 0;
        if (h < -resist) return 1;
        if (h > resist) return 3;

        return direc;
    }

    private Vector3 GetVector2fromDirec(int direc)//���� 0:�Ʒ� 1:����, 2:��, 3:������
    {
        switch (direc)
        {
            case 0:
                return new Vector3(0f, -1f, 0f);

            case 1:
                return new Vector3(-1f, 0f, 0f);

            case 2:
                return new Vector3(0f, 1f, 0f);

            case 3:
                return new Vector3(1f, 0f, 0f);

            default: return new Vector3(0f, 1f, 0f);
        }

    }
}
