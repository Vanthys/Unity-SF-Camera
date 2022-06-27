using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
 
public class FlyCamera : MonoBehaviour {
 
    /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
	Updated to use the new InputSystem 27-06-22
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/
    public Keyboard keyboard;
    public Mouse mouse;
    public float mainSpeed = 1.0f; //regular speed
    public float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    public float maxShift = 1000.0f; //Maximum speed when holdin gshift
    public float camSens = 0.25f; //How sensitive it with mouse
    public bool invertY = true;

    public float scrollWheelSens = 1f;

    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;
    void Awake(){
        keyboard = Keyboard.current;
        mouse = Mouse.current;
    }
    void Update()
    {
        if (mouse.rightButton.isPressed)
        {
            var mouseMoveY = invertY ? -1 * mouse.delta.y.ReadValue() : mouse.delta.y.ReadValue();
            var mouseMoveX = mouse.delta.x.ReadValue();

            var mouseMove = new Vector3(mouseMoveY, mouseMoveX, 0) * camSens;
            transform.eulerAngles = transform.eulerAngles + mouseMove;
        }

        if (mouse.rightButton.isPressed)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (!mouse.rightButton.isPressed)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        //Mouse  camera angle done.  

        //Keyboard commands
        Vector3 p = GetBaseInput();
        float f = GetUpDownInput();
        p += new Vector3(0, f, 0);
        
        if (p.sqrMagnitude > 0)
        { // only move while a direction key is pressed
            if (keyboard.shiftKey.isPressed)
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;
            Vector3 newPosition = transform.position;
            if (keyboard.spaceKey.isPressed)
            { //If player wants to move on X and Z axis only
                transform.Translate(p);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            }
            else
            {
                transform.Translate(p);
            }
        }

        var scroll =mouse.scroll.y.ReadValue();
        mainSpeed += scroll * scrollWheelSens;
    }
	
	
	// returns the verticalmovement 
    private float GetUpDownInput(){
        if(keyboard.eKey.isPressed){
            return 1f;
        }
        else if(keyboard.qKey.isPressed){
            return -1f;
        }
        return 0f;

    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        

        
        Vector3 p_Velocity = new Vector3();
        if (keyboard.wKey.isPressed)
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (keyboard.sKey.isPressed)
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (keyboard.aKey.isPressed)
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (keyboard.dKey.isPressed)
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}