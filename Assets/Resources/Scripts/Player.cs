using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GlobalInput input;
    public bool canControl;

    public int direc = 0;//방향 0:아래 1:왼쪽, 2:위, 3:오른쪽

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
            //움직이고 있을 때

            Vector3 movingVector = GetVector2fromDirec(direc);
            transform.position += movingVector * Time.deltaTime * movingSpeed;
            movingCheck = 0.2f;
        }
        else
        {
            //멈춰 있을 때

            if (Mathf.Abs(hCheck) < 0.1f && Mathf.Abs(vCheck) < 0.1f)//키 입력이 없을 때
            {
                float tmpX = (float)Math.Round(transform.position.x);
                float tmpY = (float)Math.Round(transform.position.y);
                transform.position = new Vector3(tmpX, tmpY, 0); // 좌표 안정
            }
            else
            {
                                
                if (movingStun < 0)
                if (movingCheck < 0)
                {
                    float tmpDirec = direc;
                    direc = SwitchDirec(vCheck, hCheck, 0, direc); //저항값을 0을 줘서 가볍게 눌러도 방향 바뀌게

                    //멈춰 있을 때 
                    //1. 같은 방향으로 키를 눌렀을 경우 가볍게 눌러도 나아감
                    //2. 방향이 바뀌었을 경우 0.n초 스턴 걸림

                    if (tmpDirec == direc)//1. 같은 방향으로 키를 눌렀을 경우 가볍게 눌러도 나아감
                    {
                        movingStep = 1f / movingSpeed;
                    }
                    else //2. 방향이 바뀌었을 경우 0.n초 스턴 걸림
                    {
                        movingStun = 0.15f;
                    }                    
                }
                else
                {
                    direc = SwitchDirec(vCheck, hCheck, 0.7f, direc); //저항값을 0.7을 줘서 떼고 있는 키는 무시

                    if (Mathf.Abs(hCheck) > 0.7f || Mathf.Abs(vCheck) > 0.7f)//살짝 누른 키는 인식 안되게
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

    private Vector3 GetVector2fromDirec(int direc)//방향 0:아래 1:왼쪽, 2:위, 3:오른쪽
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
