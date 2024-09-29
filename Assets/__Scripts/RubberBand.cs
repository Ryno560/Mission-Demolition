using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberBand : MonoBehaviour
{
    [SerializeField] private LineRenderer rubber;
    [SerializeField] private Transform firstPoint;
    [SerializeField] private Transform secondPoint;
    // Start is called before the first frame update
    void Start()
    {
        rubber.SetPosition(0, firstPoint.position);
        rubber.SetPosition(2, secondPoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)){
            rubber.SetPosition(1, GetMousePositionInWorld());
        }
    }

    Vector3 GetMousePositionInWorld(){
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z += Camera.main.transform.position.z;
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        return mousePositionInWorld - transform.position;
    }
}
