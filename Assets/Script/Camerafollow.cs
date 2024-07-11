using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    Vector3 _offset;
    [SerializeField] Transform _target;
    [SerializeField] float _smoothTime;
    Vector3 _curVecloc;
    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - _target.position;
        if(_target == null)
        {
            _target = GameObject.FindGameObjectWithTag("player").transform;
        }
    }

    private void LateUpdate()
    {
        Vector3 targetPos = _target.position +_offset;
        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPos, ref _curVecloc, _smoothTime);
    }
    public void SetTarget(Transform newTar)
    {
        _target = newTar;
    }
}
