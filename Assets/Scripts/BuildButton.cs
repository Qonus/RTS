using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameObject hologram;
    [SerializeField] GameObject building;
    [SerializeField] LayerMask ground;
    [SerializeField] float dist = 10f;

    bool isPlanning;
    Camera cam;
    GameObject newHologram;
    Ray mousePosToRay;

    private void Awake()
    {
        cam = Camera.main;
        newHologram = null;
    }

    private void Update()
    {
        mousePosToRay = cam.ScreenPointToRay(Input.mousePosition);

        if (isPlanning && Input.GetMouseButtonDown(0) && newHologram == null)
        {
            newHologram = Instantiate(hologram, Vector3.zero, Quaternion.identity);
        }

        if (newHologram != null)
        {
            if (isPlanning)
            {
                RaycastHit hit;
                if (Physics.Raycast(mousePosToRay, out hit, Mathf.Infinity, ground))
                {
                    newHologram.transform.position = hit.point;
                }
                else
                {
                    newHologram.transform.position = mousePosToRay.origin + (mousePosToRay.direction * dist);
                }
            }

            if (Input.GetMouseButtonUp(0) &&
                newHologram != null)
            {
                isPlanning = false;
                Vector3 pos = newHologram.transform.position;
                Destroy(newHologram);
                Instantiate(building, pos, Quaternion.identity);
                UnitSelection.canDrag = true;
            }
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UnitSelection.canDrag = false;
        isPlanning = true;
    }
}
