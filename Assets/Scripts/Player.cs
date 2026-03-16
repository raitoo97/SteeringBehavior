using UnityEngine;
public class Player : Agent
{
    public bool isFleeing; // Variable para alternar entre huir o perseguir al objetivo
    override protected void Update()
    {
        RotateTo(_target.position); // Rotamos el jugador hacia el objetivo
        AddForce(Flee(_target.position)); // Aplicamos la fuerza de direccion hacia el objetivo al jugador
        base.Update();
    }
    private void RotateTo(Vector3 target)
    {
        Vector3 rotationVector = Vector3.zero;
        if (!isFleeing)
            rotationVector = target - transform.position;
        else
            rotationVector = transform.position - target;
        if (rotationVector != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(rotationVector); // Rotacion hacia el objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5); // Rotamos suavemente hacia el objetivo
        }
    }
}
