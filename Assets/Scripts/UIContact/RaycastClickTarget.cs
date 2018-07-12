using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;

public class RaycastClickTarget : MonoBehaviour {

    private EventSystem m_EventSystem;
    private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;

    void Start()
    {
        m_EventSystem = EventSystem.current;
        m_Raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            List<RaycastResult> results = new List<RaycastResult>();
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            m_Raycaster.Raycast(m_PointerEventData, results);
            RaycastResult result = results[0];
        }
    }

}
