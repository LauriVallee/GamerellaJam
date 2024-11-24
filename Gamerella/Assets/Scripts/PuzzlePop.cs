using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzlePop : MonoBehaviour
{
    public static bool puzzleUp;

    public GameObject CanvaPuzzle;
    public AudioSource MusiqueEnfant;
    public AudioSource MusiquePuzzle;
    public AudioSource MusiqueAdulte;
    
    
    // Start is called before the first frame update
    void Start()
    {
        CanvaPuzzle.SetActive(false);
        puzzleUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        CanvaPuzzle.SetActive(true);
        puzzleUp = true;
        MusiqueEnfant.Stop();
        MusiqueAdulte.Play();

}
}
