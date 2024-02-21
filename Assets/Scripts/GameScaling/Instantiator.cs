using UnityEngine;

public class Instantiator : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    public bool doInstantiation = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (doInstantiation)
        {
            Instantiate(_gameObject, transform.position,Quaternion.identity);
            doInstantiation = false;
        }
    }
}
