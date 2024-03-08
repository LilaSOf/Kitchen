using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Move : NetworkBehaviour
{
    // Start is called before the first frame update

    public float speed;
    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)
            return;
       if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, -speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }



}
