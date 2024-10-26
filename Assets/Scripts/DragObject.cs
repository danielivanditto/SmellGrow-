using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IDragHandler
{
    [Header("Object drag type")]
    [SerializeField] private bool ui;
    [SerializeField] private bool obj;

    [Space][Header("Drag properties")]
    [SerializeField] private GameObject dragObject;
    [SerializeField] private NewObject currentObj;
    private GameObject currentDrag;
    private Vector3 offset;

    [Space][Header("Other GameObjects")]
    [SerializeField] private RectTransform canvas;
    [SerializeField] private PlayerInventory inventory;

    [Space][Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D bc;
    [SerializeField] private TextMeshProUGUI slotName;
    [SerializeField] private TextMeshProUGUI slotDescription;
    [SerializeField] private TextMeshProUGUI itemCount;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        if (ui)
        {
            slotName.text = currentObj.objectFullName;
            slotDescription.text = currentObj.description;
            inventory.UpdateInventoryItemCount(itemCount, currentObj);
        }
    }

    private void OnEnable()
    {
        if (ui)
        {
            slotName.text = currentObj.objectFullName;
            slotDescription.text = currentObj.description;
            inventory.UpdateInventoryItemCount(itemCount, currentObj);

            int count = 0;
            inventory.GetItemCount(currentObj, out count);

            if (count <= 0)
            {
                itemCount.text = "0";
            }
        }
    }

    void IDragHandler.OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (currentDrag && ui)
            currentDrag.transform.position = MousePositionUI(eventData);
        else if (obj)
            transform.position = MousePosition() + offset;
    }

    void IPointerDownHandler.OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (currentDrag != null)
            Destroy(currentDrag);

        if (ui)
        {
            currentDrag = Instantiate(dragObject);
            currentDrag.transform.SetParent(canvas.transform);

            currentDrag.transform.position = MousePositionUI(eventData);
            currentDrag.GetComponent<Image>().sprite = currentObj.objSprite;
        }
        else if (obj)
        {
            bc.enabled = false;
            rb.gravityScale = 0;
            offset = transform.position - MousePosition();
            rb.angularDamping = 10;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void IPointerUpHandler.OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (currentDrag != null && ui)
        {
            Destroy(currentDrag);

            if (inventory.HasItem(currentObj))
            {
                inventory.RemoveItem(currentObj, 1);
                inventory.UpdateInventoryItemCount(itemCount, currentObj);
                var clonedObj = Instantiate(currentObj.obj);
                clonedObj.transform.position = MousePosition();
                clonedObj.GetComponent<CombineCollectItems>().uiTextCount = itemCount;
                clonedObj.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Input.GetAxis("Mouse X") * 6, Input.GetAxis("Mouse Y") * 7);
            }

            int count = 0;
            inventory.GetItemCount(currentObj, out count);

            if (count <= 0)
            {
                itemCount.text = "0";
            }
        }
        else if (obj)
        {
            bc.enabled = true;
            rb.gravityScale = 1;
            rb.linearVelocity = new Vector2(Input.GetAxis("Mouse X") * 5, Input.GetAxis("Mouse Y") * 5);
            rb.angularDamping = 2;
        }
    }

    private Vector3 MousePosition()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    private Vector3 MousePositionUI(PointerEventData eventData)
    {
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas, eventData.position, eventData.pressEventCamera, out pos);
        return pos;

    }
}
