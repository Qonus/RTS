using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] GameObject selectedSprite;
    [SerializeField] float speed = 5;

    public Vector3 target = Vector3.zero;
    public bool isSelected = false;
    public bool isMoving = false;
    bool previousIsSelected = false;

    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        selectedSprite.SetActive(false);
        UnitSelection.units.Add(this);
    }

    private void Update()
    {
        if (isSelected != previousIsSelected)
        {
            selectedSprite.SetActive(isSelected);
        }
        previousIsSelected = isSelected;

        if (isMoving)
        {
            transform.LookAt(target);
            rb.AddForce(transform.forward * speed);
        }
    }
}
