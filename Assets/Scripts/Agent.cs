using System.Collections.Generic;
using UnityEngine;
public abstract class Agent : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _maxSpeed; // Velocidad maxima a la que se puede mover el agente
    [SerializeField][Range(0,1)]private float _maxForce; // Fuerza maxima que se puede aplicar al agente para cambiar su velocidad(que tan brusco rota)
    [SerializeField] protected Transform _target;
    //Vectores de direccion y fuerza
    private Vector3 _velocity; // Direccion hacia la que se mueve el agente
    protected Vector3 _desired; // Direccion deseada hacia el objetivo
    protected Vector3 _steer; // Fuerza de direccion que se debe aplicar al agente para moverlo hacia el objetivo
    // Debug: Historial de trayectoria
    private List<Vector3> _history = new List<Vector3>();
    protected virtual void Update()
    {
        transform.position += _velocity * Time.deltaTime; // Mover el agente en la direccion de su velocidad
        // Guardamos la posicion del agente en la historia para dibujar su trayectoria (solo debug)
        _history.Add(transform.position);
        if (_history.Count > 500)
            _history.RemoveAt(0);
    }
    public void AddForce(Vector3 direction)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + direction, _maxSpeed); // Sumamos una direccion a la direccion actual del agente y limitamos su velocidad a la maxima.
        _velocity.y = 0; // Mantenemos el movimiento en el plano horizontal (y=0) para evitar que el agente se eleve o hunda.
    }
    public Vector3 Seek(Vector3 target)
    {
        _desired = target - transform.position; // Direccion deseada hacia el objetivo
        _desired = _desired.normalized * _maxSpeed; // Normalizamos la direccion deseada y la multiplicamos por la velocidad maxima para obtener la velocidad deseada.
        _steer = _desired - _velocity; // Calculamos la fuerza de direccion restando la velocidad actual del agente a la velocidad deseada.
        _steer = Vector3.ClampMagnitude(_steer, _maxForce); // Limitamos la fuerza de direccion a la fuerza maxima la cual el agente puede cambiar su direccion.
        return _steer; // Devolvemos la fuerza de direccion que se debe aplicar al agente para moverlo hacia el objetivo.
    }
    public Vector3 Flee(Vector3 target)
    {
        _desired = transform.position - target; // Direccion deseada alejandose del objetivo
        _desired = _desired.normalized * _maxSpeed; // Normalizamos la direccion deseada y la multiplicamos por la velocidad maxima para obtener la velocidad deseada.
        _steer = _desired - _velocity; // Calculamos la fuerza de direccion restando la velocidad actual del agente a la velocidad deseada.
        _steer = Vector3.ClampMagnitude(_steer, _maxForce); // Limitamos la fuerza de direccion a la fuerza maxima la cual el agente puede cambiar su direccion.
        return _steer; // Devolvemos la fuerza de direccion que se debe aplicar al agente para alejarse del objetivo.
    }
    private void OnDrawGizmos()//Dibujamos la velocidad, direccion deseada y trayectoria del agente en el editor para debuggear su comportamiento.
    {
        if (_target == null) return;
        Vector3 pos = transform.position;
        float dist = Vector3.Distance(pos, _target.position);
        // Velocity
        Gizmos.color = Color.blue;
        Vector3 velDebug = _velocity.normalized * dist;
        Gizmos.DrawLine(pos, pos + velDebug);
        // Desired
        Gizmos.color = Color.green;
        Vector3 desDebug = _desired.normalized * dist;
        Gizmos.DrawLine(pos, pos + desDebug);
        // Trayectoria
        Gizmos.color = Color.yellow;
        for (int i = 0; i < _history.Count - 1; i++)
        {
            Gizmos.DrawLine(_history[i], _history[i + 1]);
        }
    }
}
