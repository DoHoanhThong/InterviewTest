using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public bool isBallMoving;
    [SerializeField] Camerafollow _cameraFollow;
    [SerializeField] GameObject _kickButton;
    [SerializeField] GameObject _effect;
    GameObject _player;
    [Range(0, 1)]
    [SerializeField] float _speedMove;
    [SerializeField] List<GameObject> _goal;
    [SerializeField] List<GameObject> _listBall;
    [SerializeField] GameObject _ball;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        isBallMoving = false;
        //_kickButton.SetActive(false);
        PlayerController.collideWithBall += ActiveButtonKick;
        _player = GameObject.FindGameObjectWithTag("player");
    }
    private void OnDestroy()
    {
        PlayerController.collideWithBall -= ActiveButtonKick;
    }
    void ActiveButtonKick(bool tmp)
    {
        _kickButton.SetActive(tmp);
    }
    public void SetBall(GameObject tmpBall)
    {
        _ball = tmpBall;
    }
    float distanceMove;
    GameObject GoalNearest()
    {
        isBallMoving = true;
        GameObject target = _goal[1];
        float distance1 = Vector3.Distance(_goal[1].transform.position, _ball.transform.position);
        distanceMove = distance1;
        float distance2 = Vector3.Distance(_goal[0].transform.position, _ball.transform.position);
        if (distance2 < distance1)
        {
            target = _goal[0];
            distanceMove = distance2;
        }
        return target;
    }
    public void Kick()
    {
        if (AreOnSameSide(_player, GoalNearest(), _ball))
        {
            Vector3 newPlayerPosition = GetSymmetricPosition(_player.transform.position, _ball.transform.position);
            _player.transform.position = newPlayerPosition;

            
        }
        StartCoroutine(BeginMoveBall(GoalNearest()));

    }
    bool AreOnSameSide(GameObject a, GameObject b, GameObject c)
    {
        Vector3 toA = a.transform.position - c.transform.position;
        Vector3 toB = b.transform.position - c.transform.position;

        return Vector3.Dot(toA, toB) > 0;
    }
    Vector3 GetSymmetricPosition(Vector3 originalPosition, Vector3 centerPosition)
    {

        return 2 * centerPosition - originalPosition;
    }
    public void AutoKick()
    {
        if (_listBall.Count == 0 || _listBall == null)
            return;
        isBallMoving = true;
        float distance = 0;
        foreach (GameObject tmp in _listBall)
        {
            if (Vector3.Distance(_player.transform.position, tmp.transform.position) > distance)
            {
                distance = Vector3.Distance(_player.transform.position, tmp.transform.position);
                _ball = tmp;
            }
        }
        StartCoroutine(BeginMoveBall(GoalNearest()));
    }
    IEnumerator BeginMoveBall(GameObject target)
    {
        Vector3 directionToGoal = (GoalNearest().transform.position - _ball.transform.position).normalized;
        _player.transform.forward = directionToGoal;
        _cameraFollow.SetTarget(_ball.transform);
        _listBall.Remove(_ball);
        _ball.transform.DOMove(target.transform.position, distanceMove * _speedMove).OnComplete(() =>
        {
            _effect.transform.position = _player.transform.position;
            _effect.SetActive(true);
        });
        yield return new WaitForSeconds(distanceMove * _speedMove + 2);
        _effect.SetActive(false);
        _cameraFollow.SetTarget(_player.transform);
        isBallMoving = false;
    }
}
