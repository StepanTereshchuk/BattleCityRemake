using UnityEngine;

public class BonusTank : MonoBehaviour
{
    private SpriteRenderer _body1;
    private float _blinkRepeatRate = 0.3f;
    private float _blinkDelay = 0f;
    private bool bonusTank = false;

    public bool IsBonusTankCheck()
    {
        return bonusTank;
    }

    public void MakeBonusTank()
    {
        _body1 = gameObject.GetComponentInChildren<SpriteRenderer>();
        bonusTank = true;
        InvokeRepeating("Blink", _blinkDelay, _blinkRepeatRate);
    }
    private void Blink()
    {
        if(_body1.color == Color.white)
        {
            _body1.color = Color.Lerp(Color.white, Color.red, 0.4f);
        }
        else
        {
            _body1.color = Color.white;
        }
    }
}
