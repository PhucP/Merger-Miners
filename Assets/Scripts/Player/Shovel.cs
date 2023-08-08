using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using DG.Tweening;

public class Shovel : MonoBehaviour, ICollision
{
    private Game game => Game.Instance;
    private UIManager ui => UIManager.Instance;

    private ShovelData _shovelData;
    private int _heal;
    private int _damage;
    private Rigidbody rb => GetComponent<Rigidbody>();
    private Vector3 _mousePos;
    private Inventory _currentInventory;
    private int _cost;

    private bool _isPlay;
    [SerializeField] private ShovelType _type;

    public int Cost { get; set; }
    public ShovelType Type
    {
        get => _type;
        set => _type = value;
    }
    public Inventory CurrentInventory
    {
        get => _currentInventory;
        set => _currentInventory = value;
    }

    private void Awake()
    {
        ui.PlayAction += OnPlayEvent;
    }

    private void OnDisable()
    {
        //unsubcribe for play action
        ui.PlayAction -= OnPlayEvent;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _isPlay = game.IsPlay;
        _shovelData = game.GetShovelData(_type);
        _damage = _shovelData.Damage;
        _heal = _shovelData.Heal;
        _cost = _shovelData.Cost;
    }

    public void OnPlayEvent()
    {
        _isPlay = true;

        if (rb != null) rb.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var block = other.gameObject.GetComponent<Block>();
        if (block != null && _isPlay)
        {
            if (_damage < block.Heal) rb.velocity = Vector3.up * 10f;
            TakeDamage(block.Damage);
            block.TakeDamage(_damage);

            var hitVFX = Instantiate(game.Data.listVFX[0], block.transform.position, Quaternion.identity);
            Destroy(hitVFX, 2f);

        }
    }

    private void Update()
    {
        if (_isPlay) transform.Rotate(Vector3.forward, 175 * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        _heal -= damage;

        if (_heal < 1)
        {
            DestroyByHeal();
        }
    }

    public void DestroyByHeal()
    {
        gameObject.SetActive(false);
        game.IsLose();
        //Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        //disable mouse when play
        if (_isPlay) return;
        _mousePos = Input.mousePosition - GetMousePos();
    }

    private void OnMouseDrag()
    {
        //disable mouse when play
        if (_isPlay) return;
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - _mousePos);
    }

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseUp()
    {
        if (_isPlay) return;

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
        if (inventory.CurrentShovel.Type != this._type || game.Data._saveData.Gold < _cost)
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


