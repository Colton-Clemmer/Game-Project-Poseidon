using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public bool Locked;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) 
        {
            Locked = !Locked;
            Cursor.lockState = Locked ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
