using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Collectable
{
    [SerializeField] int goldAmount = 1;

    public override void Collect(Player player)
    {
        player.GetComponent<PlayerLevel>().AddExp(goldAmount);
        AudioManager.instance.Play("CoinCollect");
        _DisableCollectable(this);
    }
}
