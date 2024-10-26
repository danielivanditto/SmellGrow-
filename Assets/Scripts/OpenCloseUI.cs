using UnityEngine;

public class OpenCloseUI : MonoBehaviour
{
    [SerializeField] private RectTransform objectToOpen;
    public void OpenAndClose()
    {
        objectToOpen.gameObject.SetActive(!objectToOpen.gameObject.activeSelf);
    }
}
