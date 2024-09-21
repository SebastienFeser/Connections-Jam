using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragConnection : MonoBehaviour
{
    [SerializeField] GameObject connection;
    [SerializeField] ProductionPoint productionPoint;
   void OnMouseDown()
    {
        Debug.Log("down");
        GameObject newConnection = Instantiate(connection);
        newConnection.GetComponent<DraggableConnection>().SetStartPosition(transform.position, productionPoint);
    }
}
