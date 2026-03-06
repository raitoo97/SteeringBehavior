using UnityEngine;
public class Player : Agent
{
    override protected void Update()
    {
        RotateTo(_target.position); // Rotamos el jugador hacia el objetivo
        AddForce(Seek(_target.position)); // Aplicamos la fuerza de direccion hacia el objetivo al jugador
        base.Update();
    }
    private void RotateTo(Vector3 target)
    {
        var rotationVector = target - transform.position; // Vector de rotacion hacia el objetivo
        if (rotationVector != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(rotationVector); // Rotacion hacia el objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5); // Rotamos suavemente hacia el objetivo
        }
    }
}
