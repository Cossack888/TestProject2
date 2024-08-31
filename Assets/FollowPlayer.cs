using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform player;
    PlayerAction action;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        action = player.GetComponent<PlayerAction>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, 10f, player.position.z);
        cam.orthographicSize = Mathf.Clamp(action.ScrollAmount, 5, 20);
    }
}
