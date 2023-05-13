using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : Collectable
{
    [SerializeField] int healAmount = 10;

    public override void Collect(Player player)
    {
        player.Heal(healAmount);
        AudioManager.instance.Play("SFX_PlayerRestoreHP");
        Destroy(gameObject);
        //_DisableCollectable(this);
    }

}
