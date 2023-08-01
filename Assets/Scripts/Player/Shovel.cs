using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Shovel : MonoBehaviour, ICollision
{
    private Game game => Game.Instance;
    private ShovelData _shovelData;
    private int _heal;
    private int _damage;
    private Rigidbody rb;
    private Vector3 _mousePos;
    private Inventory _currentInventory;

    private bool _isActive;
    [SerializeField] private ShovelType _type;

    public ShovelType Type
    {
        get => _type;
        set => _type = value;
    }
    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _isActive = false;
        _shovelData = game.Data.ListShoveConfig.FirstOrDefault(shovel => shovel.ShovelType == _type);
        _damage = _shovelData.Damage;
        _heal = _shovelData.Heal;
    }

    public void Onplay()
    {
        _isActive = true;
        rb.useGravity = true;
    }


    private void OnCollisionEnter(Collision other)
    {
        //move the shovel
        if (_isActive)
        {
            rb.AddForce(Vector3.up * 8, ForceMode.Impulse);

            var block = other.gameObject.GetComponentInParent<ICollision>();
            if (block != null)
            {
                DecreaseCollisionTime();
                block.DecreaseCollisionTime();
            }
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, 65 * Time.deltaTime);
    }

    public void DecreaseCollisionTime()
    {
        _heal--;
        DestroyByCollisionTime();
    }

    public void DestroyByCollisionTime()
    {
        if (_heal < 1)
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
        if (_isActive) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, Mathf.Infinity);

        foreach (RaycastHit hit in hits)
        {
            Inventory inventory = hit.collider.GetComponent<Inventory>();
            if (inventory != null && inventory != _currentInventory)
            {
                if (inventory.CurrentShovel == null)
                {
                    MoveShovel(inventory);
                    break;
                }

                if (CheckMerge(inventory))
                {
                    MergeShovel(inventory);
                }
                break;
            }
        }

        if (_currentInventory != null)
        {
            transform.position = _currentInventory.transform.position;
        }
    }

    public void MergeShovel(Inventory inventory)
    {
        Destroy(this.gameObject);
        inventory.MergeShovel();
    }

    private bool CheckMerge(Inventory inventory)
    {
        if (inventory.CurrentShovel.Type != this._type)
        {
            return false;
        }

        ShovelType maxType = System.Enum.GetValues(typeof(ShovelType)).Cast<ShovelType>().Max();
        if (_type == maxType) return false;
        return true;
    }

    private void MoveShovel(Inventory inventory)
    {
        inventory.SetShovel(this);
        if (_currentInventory != null)
        {
            _currentInventory.ChangeShovel();
        }
        _currentInventory = inventory;
    }

    public void LevelUpForShavel()
    {
        _type = (ShovelType)(int)_type + 1;
    }
}


