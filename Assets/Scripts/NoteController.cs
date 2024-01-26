using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NoteController : MonoBehaviour
{
    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject leftBar;
    [SerializeField] private GameObject rightBar;

    public int[] notes;

    private float speed;

    private float rightEnd;
    private float leftEnd;

    private float barTime;
    private float nextUpdateTime;
    private bool updateLeft;


    private void Awake()
    {
        speed = 10.0f;
        rightEnd = 22.0f;
        leftEnd = -22.0f;
        updateLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Moving pointer on the screen
        if (pointer.transform.position.x>rightEnd)
        {
            pointer.transform.position = new Vector3(leftEnd, pointer.transform.position.y, -1.0f);
        }
        pointer.transform.Translate(new Vector3(speed, 0.0f, 0.0f) * Time.deltaTime);

        float currentTime = Time.time;
        if (currentTime > nextUpdateTime)
        {
            nextUpdateTime += barTime;
            UpdateBar(updateLeft);
        }
    }

    private void UpdateBar(bool updateLeft)
    {
        if (updateLeft)
        {

        }
        else
        {

        }
        updateLeft = !updateLeft;
    }

    public bool isSafe()
    {
        return true;
    }
}
