using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Mouve : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;
    public GameObject CanvaPuzzle;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PuzzlePop.puzzleUp)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);
        }
    }
}
