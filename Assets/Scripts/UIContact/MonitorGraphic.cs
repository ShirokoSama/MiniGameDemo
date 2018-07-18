using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using HaruScene;

public class MonitorGraphic : Graphic, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private KunController player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<KunController>();
    }

    // a graphic class with no drawcall
    public override void SetMaterialDirty() { }
    public override void SetVerticesDirty() { }
    protected override void OnPopulateMesh(VertexHelper vh) { }

    public void OnPointerDown(PointerEventData eventData)
    {
        player.PlayerTouchDown(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.PlayerTouchUp();
    }

    public void OnDrag(PointerEventData eventData)
    {
        player.PlayerDrag(eventData.position);
    }
}
