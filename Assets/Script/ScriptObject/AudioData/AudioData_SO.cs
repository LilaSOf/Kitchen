using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData_SO",menuName = "Audio/AudioData")]
public class AudioData_SO : ScriptableObject
{
    // Start is called before the first frame update
    public AudioClip[] chop;
    public AudioClip[] deliveryFail;
    public AudioClip[] deliveryScucess;
    public AudioClip[] footSpeed;

    public AudioClip[] objectDrop;
    public AudioClip[] objectPick;

    public AudioClip sizzle;

    public AudioClip[] trash;

    public AudioClip[] warning;
}
