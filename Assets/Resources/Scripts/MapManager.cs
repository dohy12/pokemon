using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static PotalInfo;

public class MapManager : MonoBehaviour
{
    public static MapManager mapManager;
    private PotalInfo potalInfo;

    private void Awake()
    {
        mapManager = this;
        potalInfo = new PotalInfo();
    }

    private void Start()
    {
        SetPotalInfo();
        SetEventInvisible();
    }


    public PotalInfo GetPotalInfo()
    {
        return potalInfo;
    }


    private void SetEventInvisible()
    {
        Transform events = transform.Find("Events"); 

        for (int i=0; i< events.childCount; i++)
        {
            Transform parent = events.GetChild(i);
            if (!parent.name.Equals("GrassEvents"))
            {
                for (int j = 0; j < parent.childCount; j++)
                {
                    Transform child = parent.GetChild(j);
                    SpriteRenderer childSprite = child.GetComponent<SpriteRenderer>();
                    childSprite.enabled = false;
                }
            }
            else
            {
                TilemapRenderer tilemapRenderer = parent.GetComponent<TilemapRenderer>();
                tilemapRenderer.enabled = false;
            }
            
        }
    }

    private void SetPotalInfo()
    {
        potalInfo.AddPotal(1, new Vector2(-63f, 17f), 2); //���� ��
        potalInfo.AddPotal(2, new Vector2(3f, 3f), 0); //������ ���ö�

        potalInfo.AddPotal(3, new Vector2(-64f, 37f), 0); //��� ��
        potalInfo.AddPotal(4, new Vector2(-61f, 25f), 0); //��� ���ö�

        potalInfo.AddPotal(5, new Vector2(-47f, 17f), 2); //������ ��
        potalInfo.AddPotal(6, new Vector2(-4f, 5f), 0); //������ ���ö�

        potalInfo.AddPotal(7, new Vector2(-14f, 17f), 2); //��Ʈ1 ��
        potalInfo.AddPotal(8, new Vector2(-7f, -5f), 0); //��Ʈ1 ���ö�

        potalInfo.AddPotal(9, new Vector2(-30f, 17f), 2); //���ϸ� ����1 ��
        potalInfo.AddPotal(10, new Vector2(0f, -5f), 0); //���ϸ� ����1 ���ö�

        potalInfo.AddPotal(11, new Vector2(5f, 17f), 2); //ü���� ��
        potalInfo.AddPotal(12, new Vector2(-60f, -3f), 0); //ü���� ���ö�

        potalInfo.AddPotal(13, new Vector2(-14f, 30f), 2); //��Ʈ2 ��
        potalInfo.AddPotal(14, new Vector2(-60f, 5f), 0); //��Ʈ2 ���ö�

        potalInfo.AddPotal(15, new Vector2(-30f, 30f), 2); //���ϸ� ����2 ��
        potalInfo.AddPotal(16, new Vector2(-54f, 5f), 0); //���ϸ� ����2 ���ö�
    }

}
