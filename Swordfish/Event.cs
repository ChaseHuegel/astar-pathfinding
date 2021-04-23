using UnityEngine;
using System;
using System.Collections;

namespace Swordfish
{

public class Event : EventArgs
{
    public bool cancel = false;
    public void SetCancelled(bool cancel)
    {
        this.cancel = cancel;
    }
}

}