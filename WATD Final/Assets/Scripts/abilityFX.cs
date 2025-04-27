using UnityEngine;
using System.Collections;

public class abilityFX : MonoBehaviour
{
    Animator animator;
    private bool right = true;
    public SpriteRenderer m_SpriteRenderer;
    public SpriteRenderer dog_SR;
    public Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        m_SpriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void bark()
    {
        print("BARK");
        m_SpriteRenderer.enabled = true;
        StartCoroutine(waitOneSecond());
        if (dog_SR.flipX == true)
        {
            m_SpriteRenderer.flipX = true;
            this.transform.position = new Vector2(target.position.x - 2.4f,
            transform.position.y);
        }
        else
        {
            m_SpriteRenderer.flipX = false;
            this.transform.position = new Vector2(target.position.x + 2.4f,
            transform.position.y);
        }
        animator.SetTrigger("normalBark");
    }

    IEnumerator waitOneSecond()
    {
        yield return new WaitForSeconds(0.5f);
        m_SpriteRenderer.enabled = false;
    }

    //public void moveAndFlip()
    //{
    //    print("fuck");
    //    if(right == true)
    //    {
    //        print("shit");
    //        //-2.43
    //        //then we are flipping to face left
    //        right = false;
    //        m_SpriteRenderer.flipX = true;
    //    }
    //    else
    //    {
    //        //then we are facing left and need to flip to be right
    //        right = true;
    //        m_SpriteRenderer.flipX = false;
    //    }
    //}
}
