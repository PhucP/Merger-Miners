using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class CreateMap : MonoBehaviour
{
    [FormerlySerializedAs("_level")] public int level;

    [FormerlySerializedAs("_width")]
    [Header("Size")]
    [SerializeField] private int width;
    [FormerlySerializedAs("_heightGrid")] [SerializeField] private int heightGrid;
    [FormerlySerializedAs("_heightInv")] [SerializeField] private int heightInv;
    [FormerlySerializedAs("Space")] [SerializeField] private float space;

    [FormerlySerializedAs("_inventoryPrefab")]
    [Header("Prefab")]
    [SerializeField] private GameObject inventoryPrefab;
    [FormerlySerializedAs("ListGridPrefab")] public List<GameObject> listGridPrefab;

    [FormerlySerializedAs("_parentGrid")] [Header("Parent Transform")]
    public Transform parentGrid;
    [FormerlySerializedAs("_parentInv")] public Transform parentInv;

    [FormerlySerializedAs("_listInventory")] public List<GameObject> listInventory = new List<GameObject>();
    [FormerlySerializedAs("_listGrid")] public List<GridBase> listGrid = new List<GridBase>();

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
        foreach (var grid in listGrid)
        {
            grid.gameObject.SetActive(false);
            Destroy(grid.gameObject);
        }

        foreach (var inventory in listInventory)
        {
            Destroy(inventory);
        }

        listGrid.Clear();
        listInventory.Clear();
    }

#if UNITY_EDITOR
    public void SaveData()
    {

        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        string path = "Assets/Scripts/Config/LevelConfig/LevelData/LevelData_" + level + ".asset";

        List<GridData> gridDatas = new List<GridData>();
        foreach (var grid in listGrid)
        {
            GridData newGrid = new GridData();
            newGrid.type = grid.type;
            newGrid.position = grid.position;

            gridDatas.Add(newGrid);
        }

        levelData.width = width;
        levelData.gridHeight = heightGrid;
        levelData.inventoryHeight = heightInv;
        levelData.space = space;
        levelData.listGrid = gridDatas;
        levelData.anchorsPosY = Camera.main.GetComponent<RectTransform>().anchoredPosition.y;
        levelData.fieldOfView = Camera.main.GetComponent<Camera>().fieldOfView;
        levelData.invPosY = parentInv.transform.position.y;

        AssetDatabase.CreateAsset(levelData, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif

    public void Spawn2()
    {
        //spawn grid
        SpawnMap(heightGrid, width, parentGrid, listGridPrefab[0]);
        //spawn inventory
        SpawnMap(heightInv, width, parentInv, inventoryPrefab);
    }

    private void SpawnMap(int width, int height, Transform parent, GameObject prefab)
    {
        var startPos = CalucalteStartPos();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var grid = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
                grid.transform.localPosition = new Vector3(startPos + i * space, -j * space, 0);


                GridBase gridBase = grid.GetComponent<GridBase>();
                if (gridBase != null)
                {
                    gridBase.position = new Vector2Int(i, j);
                    listGrid.Add(gridBase);
                }
                else
                {
                    listInventory.Add(grid.gameObject);
                }
            }
        }
    }

    private float CalucalteStartPos()
    {
        float startX = 0;
        startX = (width % 2 != 0) ? -width / 2 * space : -(width / 2 - 0.5f) * space;
        return startX;
    }
}
