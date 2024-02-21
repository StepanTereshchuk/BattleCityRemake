using UnityEngine;

public interface IMovable
{
    protected void RotateVertical(float _vertical);

    public void RotateHorizontal(float _horizontal);

    public void MoveVertical(float _vertical);

    public void MoveHorizontal(float _horizontal);

    public bool CheckAvailableDirection(Vector2 direction);

    public void CheckAllAvailableDirections();
}