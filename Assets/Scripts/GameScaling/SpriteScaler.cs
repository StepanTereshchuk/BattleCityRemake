using UnityEngine;

public class SpriteScaler : MonoBehaviour
{
    [SerializeField] private float worldScreenHeight;
    [SerializeField] private SpriteRenderer sr;
    public float worldScreenWidth;

    private void Start()
    {
        //  WrongScale();
        ApropriateScale();
    }

    private void ApropriateScale()
    {
        // world colliderHeight is always camera's orthographicSize * 2
        worldScreenHeight = Camera.main.orthographicSize * 2;

        // world colliderWidth is calculated by diving world colliderHeight with screen heigh
        // then multiplying it with screen colliderWidth
        worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // to scale the game object we divide the world screen colliderWidth with the
        // size x of the sprite, and we divide the world screen colliderHeight with the
        // size y of the sprite
        transform.localScale = new Vector3(
        worldScreenWidth / sr.sprite.bounds.size.x,
        worldScreenHeight / sr.sprite.bounds.size.y, 1);
    }

    private void WrongScale()
    {
        transform.localScale = new Vector3(Screen.width,Screen.height, 1);
    }
}
