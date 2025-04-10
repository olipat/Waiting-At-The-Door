using UnityEngine;

public class spikeHitbox : MonoBehaviour
{
    public GameObject UIcontrolReferemce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIcontrolReferemce = GameObject.FindGameObjectWithTag("UiControl");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            UIcontrolReferemce.GetComponent<UIController>().ApplyDamage();
        }
    }
}
