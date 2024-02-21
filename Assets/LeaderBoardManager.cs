using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] private GameObject _leaderBoardStringPrefab;
    [SerializeField] private GameObject _loadingIconObject;
    [SerializeField] private Transform _tableTransform;
    private List<GameObject> _tableRows;
    private LeaderBoardData _leaderBoardData;
    private FireBaseManager _fireBaseManager;
    [Inject]
    private void Construct(FireBaseManager fireBaseManager)
    {
        _fireBaseManager = fireBaseManager;
    }

    private void Start()
    {
        _tableRows = new List<GameObject>();
        if (_fireBaseManager)
        {
            _fireBaseManager.Notify += InitializeLeaderBoard;
        }
    }
    private void ClearTable()
    {
        for (int i = 0; i < _tableRows.Count; i++)
        {
            Destroy(_tableRows[i]);
        }
        _tableRows.Clear();
    }

    private void OnEnable()
    {
        if (_fireBaseManager.leaderBoard == null)
        {
            _loadingIconObject.SetActive(true);
            _fireBaseManager.LoadLeaderBoardData();
        }
    }

    private void OnDestroy()
    {
        _fireBaseManager.Notify -= InitializeLeaderBoard;
    }

    private void InitializeLeaderBoard()
    {
        ClearTable();

        // need to wait result
        //if (_fireBaseManager.userData == null)
        //    _fireBaseManager.LoadUserData();

        _loadingIconObject.SetActive(false);
        _leaderBoardData = _fireBaseManager.leaderBoard;
        TextMeshProUGUI[] texts = _leaderBoardStringPrefab.GetComponentsInChildren<TextMeshProUGUI>();
        int pos = 1;
        _leaderBoardData.users.Reverse();
        foreach (var user in _leaderBoardData.users)
        {
            // forming table row
            texts[0].text = pos.ToString();
            texts[1].text = user.name;
            texts[2].text = user.tanksDestroyed.ToString();
            texts[3].text = user.highScore.ToString();
            _tableRows.Add(Instantiate(_leaderBoardStringPrefab, _tableTransform));

            if (_fireBaseManager.userData == null)
            {
                Debug.Log("NULL USER ");
            }
            else if (_fireBaseManager.userData.email == null)
            {
                Debug.Log("NULL USER Email");
            }

            if (_fireBaseManager.userData.email.Equals(user.email))
            {
                _tableRows[_tableRows.Count - 1].GetComponent<Image>().color = Color.yellow;
            }
            if (user.userIconTexture != null)
            {
                _tableRows[_tableRows.Count - 1].GetComponentInChildren<RawImage>().texture = user.userIconTexture;
            }

            pos++;
        }
    }

}