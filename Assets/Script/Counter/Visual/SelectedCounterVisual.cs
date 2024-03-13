using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    // Start is called before the first frame update
    private BaseCounter selectedCounter;
    [SerializeField] private GameObject selectedVisual;

    private void Start()
    {
        selectedCounter = GetComponentInParent<BaseCounter>();

        //Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        if(Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        }
        else
        {
            Player.PlayerAnySpawn += PlayerAnySpawn_Event;
        }
    }

    private void PlayerAnySpawn_Event(object sender, EventArgs e)
    {
        Player.LocalInstance.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;
        Player.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(selectedCounter == e.selectedCounter)
        {
            selectedVisual.SetActive(true);
        }
        else
        {
            selectedVisual.SetActive(false);
        }
    }

}
