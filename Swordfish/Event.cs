using UnityEngine;
using System;
using System.Collections;

namespace Swordfish
{

public class Event : EventArgs
{
    public bool cancel = false;
    public void Cancel(bool cancel = true)
    {
        this.cancel = cancel;
    }
}

}