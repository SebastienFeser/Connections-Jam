using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragConnection : MonoBehaviour
{
    [SerializeField] GameObject connection;
    [SerializeField] ProductionPoint productionPoint;

    public DraggableConnection createConnection()
    {
        GameObject newConnection = Instantiate(connection);
        DraggableConnection draggableConnection = newConnection.GetComponent<DraggableConnection>();
        draggableConnection.SetStartPosition(transform.position, productionPoint);
        return draggableConnection;
    }

    void OnMouseDown()
    {
        createConnection();
    }
}
