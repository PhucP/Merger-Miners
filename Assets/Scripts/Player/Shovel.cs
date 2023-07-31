using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shovel : MonoBehaviour, ICollision
{
    [SerializeField] private int _collisionTime;
    private Rigidbody rb;
    private Vector3 _mousePos;

    private bool _isActive;
    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _isActive = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        //move the shovel
        if (_isActive)
        {
            rb.AddForce(Vector3.up * 8, ForceMode.Impulse);
        }

        var block = other.gameObject.GetComponentInParent<ICollision>();
        if (block != null)
        {
            DecreaseCollisionTime();
            block.DecreaseCollisionTime();
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, 65 * Time.deltaTime);
    }

    public void DecreaseCollisionTime()
    {
        _collisionTime--;
        DestroyByCollisionTime();
    }

    public void DestroyByCollisionTime()
    {
        if (_collisionTime < 1)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnMouseDown()
    {
        _mousePos = Input.mousePosition - GetMousePos();
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - _mousePos);
    }

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, Mathf.Infinity);

        foreach (RaycastHit hit in hits)
        {
            Inventory inventory = hit.collider.GetComponent<Inventory>();
            if (inventory != null)
            {
                Debug.Log("Raycast hit object: " + hit.collider.gameObject.name);
                transform.position = inventory.transform.position;
                return;
            }
        }
    }
}
