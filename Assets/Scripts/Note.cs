using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteInfo noteInfo;

    public void SetNoteInfo(NoteInfo noteInfo)
    {
        this.noteInfo = noteInfo;
    }

    public NoteInfo GetNoteInfo()
    {
        return noteInfo;
    }
}
