using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class denialBoss : MonoBehaviour
{
    public List<bossBeam> beams = new List<bossBeam>(); // Assign 9 beams in the inspector
    public float minOnTime = 3f;
    public float maxOnTime = 6f;
    public float minOffTime = 2f;
    public float maxOffTime = 5f;
    public float flickerDuration = 1f;
    public int maxActiveBeams = 2;

    private List<bossBeam> activeBeams = new List<bossBeam>();
    public GameObject[] tennisBalls; // Prefab to spawn
    public HealthBar hb;
    public int health = 3;
    public bool flag = true;

    public GameObject toDestroy;
    public GameObject slider;

    void Start()
    {
        StartCoroutine(ControlBeams());
        hb.setMaxHealth(3);
        int randomIndex = Random.Range(0,3);
        tennisBalls[randomIndex].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            health = 0;
        }
        if(health == 2 && flag)
        {
            int randomIndex = Random.Range(3, 6);
            tennisBalls[randomIndex].SetActive(true);
            flag = false;
        }
        else if(health == 1 && flag)
        {
            int randomIndex = Random.Range(6, 9);
            tennisBalls[randomIndex].SetActive(true);
            flag = false;
        }
        else if(health <= 0)
        {
            this.gameObject.SetActive(false);
            GameManager.instance.FightingBoss = false;
            UIController.Instance.UnlockAbility(1);
            Destroy(toDestroy);
            Destroy(slider);

            ToastNotification.Show("You've overcome Denial. Anger now lies ahead.", 4f, "success");
            UIController.Instance.StartCoroutine(UIController.Instance.PostDenialSequence());


            AudioManager.instance.PlayBGM();

            
        }
    }

    IEnumerator ControlBeams()
    {
        while (true)
        {
            // Ensure only 2 beams are on
            while (activeBeams.Count < maxActiveBeams)
            {
                bossBeam newBeam = GetRandomInactiveBeam();
                if (newBeam != null)
                {
                    StartCoroutine(ToggleBeam(newBeam));
                    activeBeams.Add(newBeam);
                }
            }

            yield return new WaitForSeconds(Random.Range(minOnTime, maxOnTime));

            // Turn off one of the active beams
            if (activeBeams.Count > 0)
            {
                bossBeam beamToTurnOff = activeBeams[0];
                beamToTurnOff.TurnOff();
                activeBeams.RemoveAt(0);
            }

            yield return new WaitForSeconds(Random.Range(minOffTime, maxOffTime));
        }
    }

    IEnumerator ToggleBeam(bossBeam beam)
    {
        yield return StartCoroutine(beam.FlickerOn(flickerDuration));
    }

    bossBeam GetRandomInactiveBeam()
    {
        List<bossBeam> inactiveBeams = beams.FindAll(b => !b.IsActive);
        if (inactiveBeams.Count == 0) return null;
        return inactiveBeams[Random.Range(0, inactiveBeams.Count)];
    }

    
}
