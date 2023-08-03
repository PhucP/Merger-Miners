using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public int _level;

    [Header("Size")]
    [SerializeField] private int _width;
    [SerializeField] private int _heightGrid;
    [SerializeField] private int _heightInv;
    [SerializeField] private float Space;

    [Header("Prefab")]
    [SerializeField] private GameObject _inventoryPrefab;
    public List<GameObject> ListGridPrefab;

    [Header("Parent Transform")]
    public Transform _parentGrid;
    public Transform _parentInv;

    public List<GameObject> _listInventory = new List<GameObject>();
    public List<GridBase> _listGrid = new List<GridBase>();

    public static CreateMap Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void Spawn()
    {
        Spawn2();
    }

    public void ReSpawn()
    {
        Reset();
        Spawn();
    }

    public void Reset()
    {
        foreach (var grid in _listGrid)
        {
            grid.gameObject.SetActive(false);
            Destroy(grid.gameObject);
        }

        foreach (var inventory in _listInventory)
        {
            Destroy(inventory);
        }

        _listGrid.Clear();
        _listInventory.Clear();
    }

#if UNITY_EDITOR
    public void SaveData()
    {

        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        string path = "Assets/Scripts/Config/LevelConfig/LevelData/LevelData_" + _level + ".asset";

        List<GridData> gridDatas = new List<GridData>();
        foreach (var grid in _listGrid)
        {
            GridData newGrid = new GridData();
            newGrid.Type = grid.Type;
            newGrid.Position = grid.Position;

            gridDatas.Add(newGrid);
        }

        levelData.Width = _width;
        levelData.GridHeight = _heightGrid;
        levelData.InventoryHeight = _heightInv;
        levelData.Space = Space;
        levelData.ListGrid = gridDatas;
        levelData.AnchorsPosY = Camera.main.GetComponent<RectTransform>().anchoredPosition.y;
        levelData.FieldOfView = Camera.main.GetComponent<Camera>().fieldOfView;

        AssetDatabase.CreateAsset(levelData, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif

    public void Spawn2()
    {
        //spawn grid
        SpawnMap(_heightGrid, _width, _parentGrid, ListGridPrefab[0]);
        //spawn inventory
        SpawnMap(_heightInv, _width, _parentInv, _inventoryPrefab);
    }

    private void SpawnMap(int width, int height, Transform parent, GameObject prefab)
    {
        var startPos = CalucalteStartPos();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var grid = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
                grid.transform.localPosition = new Vector3(startPos + i * Space, -j * Space, 0);


                GridBase gridBase = grid.GetComponent<GridBase>();
                if (gridBase != null)
                {
                    gridBase.Position = new Vector2Int(i, j);
                    _listGrid.Add(gridBase);
                }
                else
                {
                    _listInventory.Add(grid.gameObject);
                }
            }
        }
    }

    private float CalucalteStartPos()
    {
        float startX = 0;
        startX = (_width % 2 != 0) ? -_width / 2 * Space : -(_width / 2 - 0.5f) * Space;
        return startX;
    }
}
