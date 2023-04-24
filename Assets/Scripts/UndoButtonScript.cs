using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoButtonScript : MonoBehaviour
{
    public bool undo = false;

    public void undoFunction()
    {
        undo = true;
    }
}
