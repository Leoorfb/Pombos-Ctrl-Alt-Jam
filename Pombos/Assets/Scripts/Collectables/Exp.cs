using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    [SerializeField] int expAmount = 1;

    private Action<Exp> _DisableCollectable;

    public void Collect(Player player)
    {
        player.GetComponent<PlayerLevel>().AddExp(expAmount);
        _DisableCollectable(this);
    }

    public void Init(Action<Exp> disableCollectable)
    {
        _DisableCollectable = disableCollectable;
    }
}
