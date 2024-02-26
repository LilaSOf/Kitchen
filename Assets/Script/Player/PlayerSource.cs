using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSource : MonoBehaviour
{
    // Start is called before the first frame update
    private Player player;

    private float sourceTimer;
    private float maxSourceTimer = 0.2f;
    void Start()
    {
        player = GetComponent<Player>();    
    }

    // Update is called once per frame
    void Update()
    {
        sourceTimer-= Time.deltaTime;
        if(sourceTimer < 0)
        {
            sourceTimer = maxSourceTimer;
            if(player.isWalking)
            {
                AudioManager.Instance.PlayFootStopSource(player.transform.position, 1);
            }
        }
    }
}
