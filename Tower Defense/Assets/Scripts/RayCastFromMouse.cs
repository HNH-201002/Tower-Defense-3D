using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayCastFromMouse : MonoBehaviour
{
    public LayerMask interactableLayer;
    public static event Action PlayerClickedOut;
    private EventSystem eventSystem;
    private Camera mainCamera;
    private RaycastHit[] hits = new RaycastHit[1]; 

    void Start()
    {
        eventSystem = EventSystem.current;
        mainCamera = Camera.main; 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            int hitCount = Physics.RaycastNonAlloc(ray, hits, Mathf.Infinity, interactableLayer);
            if (hitCount > 0)
            {
                Collider hitCollider = hits[0].collider;
                BuildingPoint buildingPoint = hitCollider.GetComponent<BuildingPoint>();
                if (buildingPoint != null)
                {
                    buildingPoint.Interact();
                }
                Debug.Log(hitCollider.name);
            }
            else
            {
                PlayerClickedOut?.Invoke();
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(eventSystem)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}