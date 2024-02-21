using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameScaler : MonoBehaviour
{
    [SerializeField] private float worldScreenHeight;
    [SerializeField] private float worldScreenWidth;
    [SerializeField] private RectTransform _rightPanel;
    public float _gameScaleX { get; private set; }
    public float _gameScaleY { get; private set; }

    public Vector3 _gameScale { get; private set; }
    private Grid _grid;
    private GridMap _gridMap;
    private Tilemap _tilemapGround;
    private BoundsInt _groundMapBoundaries;
    private Image _rightPanelImage;
    public Vector2 maxBoundaryMap { get; private set; }
    public Vector2 minBoundaryMap { get; private set; }

    public Vector3 GetRandomPositionInsideScreen(GameObject obj)
    {
        SpriteRenderer spriteRenderer;
        Vector3 randomPosition = new Vector3(Random.Range(minBoundaryMap.x, maxBoundaryMap.x), Random.Range(minBoundaryMap.y, maxBoundaryMap.y), 0);

        spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = obj.GetComponentInChildren<SpriteRenderer>();

        randomPosition += spriteRenderer.bounds.size;
        if (randomPosition.x > maxBoundaryMap.x || randomPosition.y > maxBoundaryMap.y)
        {
            randomPosition -= spriteRenderer.bounds.size * 2;
        }
        return randomPosition;
    }

    public void ApplyProperPrefabScaling(GameObject obj)
    {
        obj.transform.localScale = new Vector3(_gameScaleX, _gameScaleY, 0);
    }

    private void Start()
    {
        _rightPanelImage = _rightPanel.GetComponent<Image>();
        _gridMap = GetComponentInChildren<GridMap>();
        _grid = _gridMap.GetComponent<Grid>();
        _tilemapGround = GameObject.FindGameObjectWithTag("Ground").GetComponent<Tilemap>();
        _groundMapBoundaries = _tilemapGround.cellBounds;
        ScaleLevelMap();
        CalculateMapBoundaries();
    }
    private void ScaleLevelMap()
    {
        worldScreenHeight = Camera.main.orthographicSize * 2;
        worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        float _tileMapSizeX = (_tilemapGround.size.x - 2) * _tilemapGround.cellSize.x; // extruding 2 tiles for hiding boundaries
        float _tileMapSizeY = (_tilemapGround.size.y - 2) * _tilemapGround.cellSize.y;

        float rightPanelWidth = _rightPanel.rect.width * _rightPanelImage.pixelsPerUnit * _rightPanel.transform.lossyScale.x;

        _gameScaleX = (worldScreenWidth - rightPanelWidth / 2) / (_tileMapSizeX);  // need to divide by 2. 7.2/2=3.6
        _gameScaleY = worldScreenHeight / (_tileMapSizeY);
        _gameScale = new Vector3(_gameScaleX, _gameScaleY, 0);

        _gridMap.GetComponent<AnchoringGameObject>().anchorOffset = new Vector3(rightPanelWidth / 4 * -1, 0, 0);
        _gridMap.transform.localScale = new Vector3(_gameScaleX, _gameScaleY, 1);

    }
    private void CalculateMapBoundaries()
    {
        Vector3 _gridScale = _grid.transform.localScale;
        maxBoundaryMap = new Vector2((_groundMapBoundaries.max.x - 1) * _gridScale.x, (_groundMapBoundaries.max.y - 1) * _gridScale.y);
        minBoundaryMap = new Vector2((_groundMapBoundaries.min.x + 1) * _gridScale.x, (_groundMapBoundaries.min.y + 1) * _gridScale.y);
    }
    private void OnDrawGizmos()
    {
        if (maxBoundaryMap != null && minBoundaryMap != null && _tilemapGround && _tilemapGround.transform.position != null)
        {
            var c1 = new Vector3(minBoundaryMap.x, minBoundaryMap.y) + _tilemapGround.transform.position;
            var c2 = new Vector3(minBoundaryMap.x, maxBoundaryMap.y) + _tilemapGround.transform.position;

            var c3 = new Vector3(maxBoundaryMap.x, maxBoundaryMap.y) + _tilemapGround.transform.position;
            var c4 = new Vector3(maxBoundaryMap.x, minBoundaryMap.y) + _tilemapGround.transform.position;

            Debug.DrawLine(c1, c2, Color.red);
            Debug.DrawLine(c2, c3, Color.blue);
            Debug.DrawLine(c3, c4, Color.green);
            Debug.DrawLine(c4, c1, Color.yellow);
        }
    }
}