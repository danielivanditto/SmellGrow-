using System.Collections;
using TMPro;
using UnityEngine;

public class CombineCollectItems : MonoBehaviour
{
    [Header("Object Info")]
    public int objectCombineAmount;
    public NewObject obj;

    [Space][Header("Prefabs and Objects")]
    [SerializeField] private TextMeshPro instanceText;
    [SerializeField] public TextMeshPro currentText;
    [SerializeField] public TextMeshProUGUI uiTextCount;

    [Space][Header("Animations")]
    [SerializeField] private Animator anim;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<CombineCollectItems>() && other.gameObject.GetComponent<CombineCollectItems>().obj.ID == obj.ID)
        {
            CombineItemsTogether(other);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerInventroy>())
        {
            CollectItem(other);
        }
    }

    private void CombineItemsTogether(Collision2D other)
    {
        if (objectCombineAmount > other.gameObject.GetComponent<CombineCollectItems>().objectCombineAmount || transform.GetSiblingIndex() > other.gameObject.transform.GetSiblingIndex() && objectCombineAmount == other.gameObject.GetComponent<CombineCollectItems>().objectCombineAmount)
        {
            if (other.gameObject.GetComponent<CombineCollectItems>().currentText != null)
                Destroy(other.gameObject.GetComponent<CombineCollectItems>().currentText.gameObject);

            objectCombineAmount += other.gameObject.GetComponent<CombineCollectItems>().objectCombineAmount;
            Destroy(other.gameObject);
        }
        else if (objectCombineAmount < other.gameObject.GetComponent<CombineCollectItems>().objectCombineAmount)
        {
            StartCoroutine(DestroyWait(1));
            return;
        }

        if (currentText == null)
        {
            currentText = Instantiate(instanceText);
        }

        anim.SetTrigger("Combine");
    }

    private void CollectItem(Collider2D other)
    {
        other.gameObject.GetComponent<PlayerInventroy>().AddItem(obj, objectCombineAmount);
        other.gameObject.GetComponent<PlayerInventroy>().UpdateInventoryItemCount(uiTextCount, obj);

        StartCoroutine(DestroyWait(1));
    }

    private void Update()
    {
        if (currentText != null)
        {
            currentText.transform.position = new Vector2(transform.position.x + 1, transform.position.y + 0.5f);
            currentText.text = objectCombineAmount.ToString();
        }
    }

    private IEnumerator DestroyWait(float time)
    {
        if (currentText != null)
            Destroy(currentText.gameObject);

        anim.SetTrigger("Collect");

        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}