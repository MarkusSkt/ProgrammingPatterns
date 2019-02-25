using UnityEngine;

/// <summary>
/// Command for basic jumps
/// </summary>
public class JumpCommand : IRigidbodyCommand
{
    private readonly Vector3 JumpForce = new Vector3(0, 400, 0);

    public void Execute(Rigidbody rb)
    {
        rb.AddForce(JumpForce);
    }
}
