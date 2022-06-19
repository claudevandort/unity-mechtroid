using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Mech
{
    new private void Update()
    {
        JumpInput();
        MoveInput();
        ShootInput();
        base.Update();
    }

    private void MoveInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        runningInput = Input.GetButton("Fire3");
        Move(horizontalInput, runningInput);
    }

    private void JumpInput()
    {
        if (Input.GetButtonDown("Jump"))
            Jump();
    }

    private void ShootInput()
    {
        if(Input.GetButtonDown("Fire1"))
            Shoot();
    }
}
