using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.player;
    }

    // Update is called once per frame
    void Update()
    {
        float x = player.transform.position.x;
        float y = player.transform.position.y;
        transform.position = new Vector3(x, y, -10f);
    }

    private void LateUpdate()
    {
        float x = player.transform.position.x;
        float y = player.transform.position.y;
        transform.position = new Vector3(x, y, -10f);
    }

}
