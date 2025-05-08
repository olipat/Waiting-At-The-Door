using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BridgePlatform : MonoBehaviour
{
    public GameObject[] rebuildTransitionObjects; 
    public GameObject[] builtLevel;
    public GameObject[] builtForeground; 
    public GameObject[] broken;     
    public Transform visualParent;            

    private bool isRebuilt = false;

    private void Start()
    {
        SetToBrokenState();
    }

    public void Rebuild()
    {
        if (isRebuilt) return;

        isRebuilt = true;
        StartCoroutine(RebuildSequence());
    }

    public void Break()
    {
        if (!isRebuilt) return;

        isRebuilt = false;
        SetToBrokenState();
    }

    private void SetToBrokenState()
    {
        EnableGroup(broken, true);
        EnableGroup(builtLevel, false);
        EnableGroup(builtForeground, false);
    }

    private IEnumerator RebuildSequence()
    {
        EnableGroup(broken, false);
        EnableGroup(builtLevel, false);
        EnableGroup(builtForeground, false);

        if (visualParent != null)
        {
            visualParent.DOShakePosition(0.3f, new Vector3(0.1f, 0.1f, 0), 10, 90)
                        .SetRelative(true);
        }

        float frameDuration = 0.5f;

        for (int i = 0; i < rebuildTransitionObjects.Length; i++)
        {
            EnableGroup(rebuildTransitionObjects, false); 
            rebuildTransitionObjects[i].SetActive(true);
            yield return new WaitForSeconds(frameDuration);
        }

        EnableGroup(rebuildTransitionObjects, false); 

        EnableGroup(builtLevel, true);
        EnableGroup(builtForeground, true);
    }


    private void EnableGroup(GameObject[] objs, bool enable)
    {
        foreach (GameObject obj in objs)
        {

            obj.SetActive(enable);

            Collider2D col = obj.GetComponent<Collider2D>();
            if(col != null){
                col.enabled = enable;
            }
        }
    }

    public bool IsRebuilt() => isRebuilt;
}
