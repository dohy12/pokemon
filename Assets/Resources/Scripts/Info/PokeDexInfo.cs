using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeDexInfo : MonoBehaviour
{
    public static PokeDexInfo instance;
    public Dictionary<int, Info> info;

    private void Awake()
    {
        instance = this;
        info = new Dictionary<int, Info>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class Info
    {
        public string name;
        public string kind;
        public string height;
        public string weight;
        public string detail;

        public Info(string name, string kind, string height, string weight, string detail)
        {
            this.name = name;
            this.kind = kind;
            this.height = height;
            this.weight = weight;
            this.detail = detail;
        }
    }

}
