using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class AnimationController : MonoBehaviour
{
    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
    }

    public void UpdateAnimationPlayer(PlayerState playerState)
    {
        for (int i = 0; i <= (int)PlayerState.RUN; i++)
        {
            string stateName = ((PlayerState)i).ToString();
            if (playerState == (PlayerState)i)
                _animator.SetBool(stateName, true);
            else
                _animator.SetBool(stateName, false);
        }
    }
}
