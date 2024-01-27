using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct NoteInfo
{
    public int noteDir;
    public bool fartNote;
    public bool longNote;
    public float duration;
    public float interval;

    public NoteInfo(int noteDir, bool fartNote, bool longNote, float interval)
    {
        this.noteDir = noteDir;
        this.fartNote = fartNote;
        this.longNote = longNote;
        this.duration = -1.0f;
        this.interval = interval;
    }

    public NoteInfo(int noteDir, bool fartNote, bool longNote, float duration, float interval)
    {
        this.noteDir = noteDir;
        this.fartNote = fartNote;
        this.longNote = longNote;
        this.duration = duration;
        this.interval = interval;
    }

    public NoteInfo(float duration)
    {
        this.noteDir = 0;
        this.fartNote = true;
        this.longNote = true;
        this.duration = duration;
        this.interval = 1.0f;
    }
}
public class NoteController : MonoBehaviour
{
    [SerializeField] private GameObject notePrefab;
    private GameObject pointer;
    [SerializeField] private Transform noteCollection;

    public NoteInfo[] notes;

    private List<GameObject> generatedNotes = new List<GameObject>();

    private float speed;

    private float rightEnd;
    private float leftEnd;

    [SerializeField] float introTime;
    private float nextGen;
    private float genInterval;

    //current note info
    [SerializeField] private NoteInfo currentNote;
    [SerializeField] private bool onNote;


    private void Awake()
    {
        speed = 5.0f;
        rightEnd = 22.0f;
        leftEnd = -22.0f;
        nextGen = introTime;
        genInterval = 1.0f;
        pointer = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextGen)
        {
            nextGen += genInterval;
            GameObject newNote = Instantiate(notePrefab, noteCollection);
            Collider2D noteCollider = newNote.AddComponent<BoxCollider2D>();
            noteCollider.isTrigger = true;
            Note newNoteInfo = newNote.AddComponent<Note>();
            newNoteInfo.SetNoteInfo(new NoteInfo(Time.time));
            newNote.transform.position = new Vector3(leftEnd - notePrefab.transform.localScale.x / 2, 0.0f, -1.0f);
            generatedNotes.Add(newNote);
        }
        //Moving pointer on the screen
        if (generatedNotes.Count > 0)
        {
            List<GameObject> markedNotes = new List<GameObject>();
            foreach (GameObject note in generatedNotes)
            {
                if (note.transform.position.x - note.transform.localScale.x / 2 > rightEnd)
                {
                    markedNotes.Add(note);//mark notes for destruction
                }
                else
                {
                    note.transform.Translate(new Vector3(speed, 0.0f, 0.0f) * Time.deltaTime);
                }
            }

            foreach (GameObject markedNote in markedNotes)
            {
                generatedNotes.Remove(markedNote);
                Destroy(markedNote);
            }

            foreach (GameObject note in generatedNotes)
            {
                note.transform.Translate(new Vector3(speed, 0.0f, 0.0f) * Time.deltaTime);
            }
        }

        float currentTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        onNote = true;
        Note note = other.gameObject.GetComponent<Note>();
        currentNote = note.GetNoteInfo();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        onNote = false;
    }

    public bool canFart()
    {
        if (!onNote) return false;
        return currentNote.fartNote;
    }
}
