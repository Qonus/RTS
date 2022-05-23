using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public static List<Unit> units = new List<Unit>();
    public static bool canDrag = true;
    [SerializeField] LayerMask clickable;
    [SerializeField] LayerMask ground;
    [SerializeField] GameObject selectArea;
    [SerializeField] GameObject targetForUnits;

    public bool drag = false;
    Vector3 previousMousePos;
    Camera cam;
    Vector3 dragStartPos;
    Rect selectionRect;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.mousePosition != previousMousePos &&
            Input.GetMouseButton(0) && 
            !drag)
        {
            SetDrag(true);
        }
        previousMousePos = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            if (drag)
            {
                SetDrag(false);
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, clickable))
                {
                    Debug.Log("clickable");
                    if(!Input.GetKey(KeyCode.LeftShift))
                    {
                        ClearSelection();
                        targetForUnits.SetActive(false);
                    }
                    if (hit.collider.GetComponent<Unit>() != null)
                    {
                        Select(hit.collider.GetComponent<Unit>());
                    }
                }
                else if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ground))
                {
                    foreach (Unit unit in units.FindAll(i => i.isSelected))
                    {
                        targetForUnits.transform.position = new Vector3(hit.point.x, hit.point.y + 0.2f, hit.point.z);
                        targetForUnits.SetActive(true);
                        unit.target = hit.point;
                        unit.isMoving = true;
                    }
                }
            }
        }

        if (drag)
        {
            targetForUnits.SetActive(false);
            RectTransform area = selectArea.GetComponent<RectTransform>();
            DrawSelectArea(area);
            CalculateSelectionRect();
            foreach (Unit unit in units)
            {
                unit.isSelected = selectionRect.Contains(cam.WorldToScreenPoint(unit.transform.position));
                //Debug.Log(cam.WorldToScreenPoint(unit.transform.position));
            }
        }
    }

    private void DrawSelectArea(RectTransform trans)
    {
        Vector3 selectCenter = (Input.mousePosition + dragStartPos) / 2;
        trans.position = selectCenter;

        Vector3 deltaPos = Input.mousePosition - dragStartPos;
        Vector2 size = new Vector2(Mathf.Abs(deltaPos.x), Mathf.Abs(deltaPos.y));
        trans.sizeDelta = size;
    }

    private void CalculateSelectionRect()
    {
        selectionRect.xMin = dragStartPos.x > Input.mousePosition.x ? Input.mousePosition.x : dragStartPos.x;
        selectionRect.xMax = dragStartPos.x < Input.mousePosition.x ? Input.mousePosition.x : dragStartPos.x;
        selectionRect.yMin = dragStartPos.y > Input.mousePosition.y ? Input.mousePosition.y : dragStartPos.y;
        selectionRect.yMax = dragStartPos.y < Input.mousePosition.y ? Input.mousePosition.y : dragStartPos.y;
    }

    private void SetDrag(bool _drag)
    {
        if (canDrag)
        {
            if (_drag)
            {
                dragStartPos = Input.mousePosition;
            }
            drag = _drag;
            selectArea.SetActive(_drag);
        }
    }

    public static void Select(Unit unit)
    {
        unit.isSelected = !unit.isSelected;
    }

    public static void ClearSelection()
    {
        foreach (Unit unit in units)
        {
            unit.isSelected = false;
        }
    }
}
