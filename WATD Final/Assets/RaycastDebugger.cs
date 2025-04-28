using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class RaycastDebugger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            Debug.Log("UI elements under mouse:");

            foreach (var result in results)
            {
                Debug.Log(result.gameObject.name);
            }

            if (results.Count == 0)
            {
                Debug.Log("Nothing hit by raycast.");
            }
        }
    }
}
