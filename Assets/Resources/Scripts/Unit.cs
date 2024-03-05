using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int direc = 0;//방향 0:아래 1:왼쪽, 2:위, 3:오른쪽

    public float moveX = 0;
    public float moveY = 0;
    public float moveZ = 0;

    public float movingCheck = 0;
    public float movingStep = 0;    
    public float movingSpeed = 5f; //이동속도
    public int movingWalk = 2;//왼발, 오른발
    public bool isMoving = false;

    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    public bool isSpriteMov = false;
    public Dictionary<int, Vector2> direcToVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnitStart()
    {
        moveX = transform.position.x;
        moveY = transform.position.y;

        spriteRenderer = GetComponent<SpriteRenderer>();

        SetDirecVector();
    }

    private void SetDirecVector()
    {
        direcToVector = new Dictionary<int, Vector2>();
        direcToVector.Add(0, new Vector2(0f, -1f));
        direcToVector.Add(1, new Vector2(-1f, 0f));
        direcToVector.Add(2, new Vector2(0f, 1f));
        direcToVector.Add(3, new Vector2(1f, 0f));
    }

    public void DirecOrder(int direc)
    {
        this.direc = direc;
    }

    public void MoveOrder(int direc)
    {
        this.direc = direc;
        movingStep = 1f / movingSpeed;//출발
        isMoving = true;
        Walk();
    }

    public void SetSprite()
    {
        if (isSpriteMov)
        {
            spriteRenderer.sprite = sprites[direc * 3 + movingWalk];
        }
        else
        {
            spriteRenderer.sprite = sprites[direc * 3];
        }
    }

    public void Walk()
    {
        if (movingWalk == 1)//왼발 오른발
        {
            movingWalk = 2;
        }
        else
        {
            movingWalk = 1;
        }
    }

    public Vector2 GetVector2fromDirec(int direc)//방향 0:아래 1:왼쪽, 2:위, 3:오른쪽
    {
        return direcToVector[direc];
    }

    public void MoveUpdate()
    {        
        movingStep -= Time.deltaTime;
        if (movingStep > 0)
        {
            //움직이고 있을 때

            Vector3 movingVector = GetVector2fromDirec(direc);
            moveX += movingVector.x * Time.deltaTime * movingSpeed;
            moveY += movingVector.y * Time.deltaTime * movingSpeed;
            movingCheck = 0.2f;
        }
        else
        {
            if (isMoving)
            {
                //좌표 안정
                float tmpX = (float)Mathf.Round(moveX);
                float tmpY = (float)Mathf.Round(moveY);
                moveX = tmpX;
                moveY = tmpY;
                isMoving = false;
            }
        }

        transform.position = new Vector3(moveX, moveY + moveZ, 0);
    }
}
