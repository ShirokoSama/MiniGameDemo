using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonitorGraphic : Graphic, IPointerDownHandler, IPointerUpHandler
{
    private GameObject player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void SetMaterialDirty() { }
    public override void SetVerticesDirty() { }
    protected override void OnPopulateMesh(VertexHelper vh) { }

    public void OnPointerDown(PointerEventData eventData)
    {
        player.SendMessage("PlayerTouchDown", eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.SendMessage("PlayerTouchUp");
    }
}
