using UnityEngine;
using System.Collections;

public class spike : MonoBehaviour
{
    private Animator spikeAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spikeAnimator = GetComponent<Animator>();
    }

    public void attack()
    {
       spikeAnimator.Play("Spike Animation");
    }
}
