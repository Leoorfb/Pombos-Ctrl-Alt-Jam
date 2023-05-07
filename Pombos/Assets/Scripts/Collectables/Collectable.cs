using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{

    protected Action<Collectable> _DisableCollectable;

    abstract public void Collect(Player player);

    public void Init(Action<Collectable> disableCollectable)
    {
        _DisableCollectable = disableCollectable;
    }
}
