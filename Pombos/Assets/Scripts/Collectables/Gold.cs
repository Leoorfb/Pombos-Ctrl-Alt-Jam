using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Collectable
{
    [SerializeField] int goldAmount = 1;

    private void Awake()
    {
        Debug.Log("Gold");
    }

    public override void Collect(Player player)
    {
        player.GetComponent<PlayerGold>().AddGold(goldAmount);
        AudioManager.instance.Play("CoinCollect");

        Destroy(gameObject);
        //_DisableCollectable(this);
    }
}
