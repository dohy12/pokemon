using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        SetPotalInvisible();
    }


    public PotalInfo GetPotalInfo()
    {
        return potalInfo;
    }


    private void SetPotalInvisible()
    {
        for (int i=0;i< transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.CompareTag("Potal"))
            {
                SpriteRenderer childSprite = child.GetComponent<SpriteRenderer>();
                childSprite.enabled = false;
            }
        }
        
    }

    private void SetPotalInfo()
    {
        potalInfo.AddPotal(1, new Vector2(-63f, 17f), 2); //���� ��
        potalInfo.AddPotal(2, new Vector2(3f, 3f), 0); //������ ���ö�

        potalInfo.AddPotal(3, new Vector2(-64f, 37f), 0); //��� ��
        potalInfo.AddPotal(4, new Vector2(-61f, 25f), 0); //��� ���ö�
    }

}
