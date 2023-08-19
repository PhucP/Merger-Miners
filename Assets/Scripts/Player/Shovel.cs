using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using DG.Tweening;
using UnityEngine.Serialization;

public class Shovel : MonoBehaviour, ICollision
{
    private Game Game => Game.Instance;
    private UIManager UI => UIManager.Instance;

    private ShovelData _shovelData;
    private int _heal;
    private int _damage;
    private Rigidbody Rb => GetComponent<Rigidbody>();
    private Vector3 _mousePos;
    private Inventory _currentInventory;
    private int _cost;
    private Block _histBlock;

    private bool _isPlay;
    [FormerlySerializedAs("_type")] [SerializeField] private ShovelType type;

    public int Cost { get; set; }
    public ShovelType Type
    {
        get => type;
        set => type = value;
    }
    public Inventory CurrentInventory
    {
        get => _currentInventory;
        set => _currentInventory = value;
    }

    private void Awake()
    {
        UI.PlayAction += OnPlayEvent;
    }

    private void OnDisable()
    {
        //unsubcribe for play action
        UI.PlayAction -= OnPlayEvent;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _isPlay = Game.isPlay;
        _shovelData = Game.GetShovelData(type);
        _damage = _shovelData.damage;
        _heal = _shovelData.heal;
        _cost = _shovelData.cost;
        _histBlock = null;
    }

    public void OnPlayEvent()
    {
        _isPlay = true;

        if (Rb != null) Rb.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var block = other.gameObject.GetComponent<Block>();
        _histBlock = block;
        if (block != null && _isPlay)
        {
            //Move Camera
            MoveCamera(_histBlock);

            if (_damage < block.Heal) Rb.velocity = Vector3.up * 10f;
            TakeDamage(block.Damage);
            block.TakeDamage(_damage);

            var hitVFX = Instantiate(Game.data.listVFX[0], block.transform.position, Quaternion.identity);
            Destroy(hitVFX, 2f);
        }
    }

    private void MoveCamera(Block block)
    {
        int maxPosy = Game.GetLevelDataByLevel(Game.data.saveData.level).gridHeight - 3;

        if (block.Pos.y > maxPosy) return;

        if (!Game.listHistBlock.Contains(block))
        {
            Game.listHistBlock.Add(block);
        }

        float deepest = float.MaxValue;
        foreach (var i in Game.listHistBlock)
        {
            if (i != null && i.transform.localPosition.y < deepest)
            {
                deepest = i.transform.localPosition.y;
            }
        }

        Camera.main.transform.DOMoveY(deepest, 1.5f).SetEase(Ease.Linear);
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
            Game.listHistBlock.Remove(_histBlock);
            DestroyByHeal();
        }
    }

    public void DestroyByHeal()
    {
        gameObject.SetActive(false);
        Game.CheckGameStat();

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
        if (inventory.CurrentShovel.Type != this.type || Game.data.saveData.gold < _cost)
        {
            return false;
        }

        ShovelType maxType = System.Enum.GetValues(typeof(ShovelType)).Cast<ShovelType>().Max();
        if (type == maxType) return false;
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
        type = (ShovelType)(int)type + 1;
    }
}


