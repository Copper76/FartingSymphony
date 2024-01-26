using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private NoteController noteController;

    [SerializeField] private Slider fartBar;

    private bool isFarting;

    private float fartingSpeed;
    // Start is called before the first frame update
    void Start()
    {
        isFarting = false;
        fartingSpeed = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fart"))
        {
            if (noteController.isSafe())
            {
                Debug.Log("Farting");
                isFarting = true;
            }
            else
            {
                Lose();
            }
        }

        if (Input.GetButtonUp("Fart"))
        {
            Debug.Log("I'm done");
            isFarting = false;
        }

        if (isFarting && fartBar.value > 0)
        {
            fartBar.value -= fartingSpeed * Time.deltaTime;
        }
    }

    //The player has lost
    void Lose()
    {

    }
}
