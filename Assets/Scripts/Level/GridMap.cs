using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class GridMap : MonoBehaviour
{
    [SerializeField] private Tilemap brickTileMap;
    [SerializeField] private Tilemap steelTileMap;
    //[SerializeField] private Tilemap tilePaletteTileMap;
    [SerializeField] private Tile brickTile;
    [SerializeField] private Tile steelTile;
    [SerializeField] private Grid grid;
    private Vector3[] baseBorders;
    private float _timeDuration = 20f;
    private GameScaler _gameScaler;

    [Inject]
    private void Construct(GameScaler gameScaler)
    {
        _gameScaler = gameScaler;
    }
    private void Start()
    {
       
    }

    public void ActivateSpadePower()
    {
        StartCoroutine(SpadePowerUpActivated());
    }
    private IEnumerator SpadePowerUpActivated()
    {
        StartCoroutine(ChangeEagleWallToSteel());
        yield return new WaitForSeconds(_timeDuration);
        ChangeEagleWallToBrick();
    }

    private void UpdateAndRemoveTile(Vector3 position, TileBase tile, Tilemap tileMapToRemoveFrom, Tilemap tileMapToUpdate)
    {
        if (!(tileMapToRemoveFrom == steelTileMap && tileMapToRemoveFrom.GetTile(tileMapToRemoveFrom.WorldToCell(position)) == null))
        {
            tileMapToRemoveFrom.SetTile(tileMapToRemoveFrom.WorldToCell(position), null);
            tileMapToUpdate.SetTile(tileMapToUpdate.WorldToCell(position), tile);
        }
    }
    private void InitializeBaseBorders()
    {
        baseBorders = new Vector3[8];
        baseBorders[0] = new Vector3(-2f, -13f, 0f);
        baseBorders[1] = new Vector3(-2f, -12f, 0f);
        baseBorders[2] = new Vector3(-2f, -11f, 0f);
        baseBorders[3] = new Vector3(-1f, -11f, 0f);
        baseBorders[4] = new Vector3(0f, -11f, 0f);
        baseBorders[5] = new Vector3(1f, -11f, 0f);
        baseBorders[6] = new Vector3(1f, -12f, 0f);
        baseBorders[7] = new Vector3(1f, -13f, 0f);
        
        for (int i = 0; i < baseBorders.Length; i++)
        {
            baseBorders[i] += grid.transform.position;
            baseBorders[i]  = new Vector3(baseBorders[i].x * _gameScaler._gameScale.x, baseBorders[i].y * _gameScaler._gameScale.y,0);
        }
    }
    private void UpdateTile(TileBase tile, Tilemap tileMapToRemoveFrom, Tilemap tileMapToUpdate)
    {
        foreach (var blockPosiiton in baseBorders)
        {
            UpdateAndRemoveTile(blockPosiiton, tile, tileMapToRemoveFrom, tileMapToUpdate);
        }
        tileMapToUpdate.gameObject.GetComponent<TilemapCollider2D>().enabled = false;
        tileMapToUpdate.gameObject.GetComponent<TilemapCollider2D>().enabled = true;
    }
    private IEnumerator ChangeEagleWallToSteel()
    {
        InitializeBaseBorders();
        UpdateTile(steelTile, brickTileMap, steelTileMap);
        yield return new WaitForSeconds(15f);
        for (int i = 0; i < 5; i++)
        {
            UpdateTile(brickTile, steelTileMap, steelTileMap);
            yield return new WaitForSeconds(0.5f);
            UpdateTile(steelTile, steelTileMap, steelTileMap);
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void ChangeEagleWallToBrick()
    {
        UpdateTile(brickTile, steelTileMap, brickTileMap);
    }
}
