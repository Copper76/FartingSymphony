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
        this.duration = 1.0f;
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
        this.longNote = false;
        this.duration = duration;
        this.interval = 1.0f;
    }
}
public class NoteController : MonoBehaviour
{
    [SerializeField] private GameObject notePrefab;
    private GameObject pointer;
    [SerializeField] private Transform noteCollection;

    [SerializeField] private GameObject UpPrefab;
    [SerializeField] private GameObject DownPrefab;
    [SerializeField] private GameObject LeftPrefab;
    [SerializeField] private GameObject RightPrefab;

    public List<NoteInfo> notes;

    private List<GameObject> generatedNotes = new List<GameObject>();

    private float speed;

    private float rightEnd;
    private float leftEnd;

    public float introTime;
    private float nextGen;
    private float genInterval;

    //current note info
    [SerializeField] private NoteInfo currentNote;
    private bool noteUsed;
    [SerializeField] private bool onNote;

    private bool finished;


    private void Awake()
    {
        speed = 5.0f;
        rightEnd = 22.0f;
        leftEnd = -22.0f;
        nextGen = introTime;
        genInterval = 1.0f;
        pointer = transform.GetChild(1).gameObject;
        finished = false;
        noteUsed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (notes.Count > 0)
        {
            if (Time.time > nextGen)
            {
                NoteInfo nextNoteInfo = notes[0];
                notes.RemoveAt(0);
                nextGen += nextNoteInfo.interval + nextNoteInfo.duration;//when we have the next note generated
                //TODO: Change this line to spawn the right prefabs
                GameObject newNote;
                switch(nextNoteInfo.noteDir)
                {
                    case 0:
                        newNote = Instantiate(LeftPrefab, noteCollection);
                        break;
                    case 1:
                        newNote = Instantiate(UpPrefab, noteCollection);
                        break;
                    case 2:
                        newNote = Instantiate(RightPrefab, noteCollection);
                        break;
                    case 3:
                        newNote = Instantiate(DownPrefab, noteCollection);
                        break;
                    default:
                        newNote = Instantiate(LeftPrefab, noteCollection);
                        break;
                }
                newNote.transform.localScale.Scale(new Vector3(nextNoteInfo.duration, 1.0f, 1.0f));//Scale the note to fit the duration
                if (nextNoteInfo.fartNote)
                {
                    //change to a fart note
                }
                Collider2D noteCollider = newNote.AddComponent<BoxCollider2D>();
                noteCollider.isTrigger = true;
                Note newNoteInfo = newNote.AddComponent<Note>();
                newNoteInfo.SetNoteInfo(nextNoteInfo);
                newNote.transform.position = new Vector3(leftEnd - newNote.transform.localScale.x / 2, 0.0f, -1.0f);
                generatedNotes.Add(newNote);
            }
        }
        else
        {
            finished = true;
        }
        
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
        
        //Moving notes on screen
        if (generatedNotes.Count > 0)
        {
            List<GameObject> markedNotes = new List<GameObject>();
            foreach (GameObject note in generatedNotes)
            {
                if (note.transform.position.x - note.transform.localScale.x / 2 > rightEnd)//completely off the screen
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
        else if (finished)//The game has already started so this means the game is over
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        onNote = true;
        Note note = other.gameObject.GetComponent<Note>();
        currentNote = note.GetNoteInfo();
        noteUsed = false;
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

    public bool MatchDir(int dir)
    {
        if (dir == -1) return false;
        if (!onNote) return false;
        if (noteUsed) return false;
        return dir == currentNote.noteDir;
    }

    public bool ConsumeNote()
    {
        if (!currentNote.longNote)
        {
            noteUsed = true;
            return true;
        }
        return false;
    }
}
