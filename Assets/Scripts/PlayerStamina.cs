using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private PlayerAttributes attributes;

    [SerializeField] private Transform staminaLayout;

    [Header("Sprites")]
    [SerializeField] private Sprite regularPoint;
    [SerializeField] private Sprite halfPoint;

    [Space][Header("Prefabs")]
    [SerializeField] private GameObject staminaPoint;

    [Header("Variables")]
    [SerializeField] private float currentTimer; // for RegainStamina, this timer is like WaitForSeconds()

    private void Start()
    {
        player = GetComponent<PlayerMovement>();
        attributes = player.attributes;
        StartCoroutine(RegainStamina());
    }

    public IEnumerator RegainStamina()
    {
        currentTimer = attributes.staminaTimer;
        for (int i = 0; i < attributes.maxStaminaCount; i++)
        {
            if (i > staminaLayout.childCount - 1)
            {
                yield return new WaitUntil(() => currentTimer <= 0);
                currentTimer = attributes.staminaTimer;
                Instantiate(staminaPoint).transform.SetParent(staminaLayout.transform);
            }
            else
            {
                Image currentPointImage = staminaLayout.GetChild(i).GetComponent<Image>();
                if (currentPointImage.sprite != regularPoint || currentPointImage.color != Color.white)
                {
                    yield return new WaitUntil(() => currentTimer <= 0);
                    currentTimer = attributes.staminaTimer;
                    currentPointImage.sprite = regularPoint;
                    currentPointImage.color = Color.white;
                }
            }
        }
    }

    public void RemoveStaminaPoint(int amount, bool removeHalf)
    {
        for (int i = 0; i < amount; i++)
        {
            for (int j = staminaLayout.childCount - 1; j >= 0; j--)
            {
                GameObject currentPoint = staminaLayout.GetChild(j).gameObject;
                Image currentPointImage = currentPoint.GetComponent<Image>();

                if (removeHalf)
                {
                    if (currentPointImage.sprite != regularPoint)
                    {
                        currentPointImage.sprite = regularPoint;
                        currentPointImage.color = new Color32(255, 255, 255, 100);
                        break;
                    }
                    else if (currentPointImage.sprite == regularPoint && currentPointImage.color.a == 1)
                    {
                        currentPointImage.sprite = halfPoint;
                        currentPointImage.color = new Color32(255, 255, 255, 255);
                        break;
                    }
                }
                else
                {
                    if (currentPointImage.sprite == regularPoint && currentPointImage.color == Color.white)
                    {
                        currentPointImage.sprite = regularPoint;
                        currentPointImage.color = new Color32(255, 255, 255, 100);
                        break;
                    }
                }
            }
        }
    }

    public bool CheckForAvailablePoints()
    {
        for (int i = 0; i < staminaLayout.childCount - 1; i++)
        {
            Image point = staminaLayout.GetChild(i).GetComponent<Image>();

            if (point.color == Color.white)
            {
                return true;
            }
        }

        return false;
    }

    private void RegainFullHp() // admin only
    {
        for (int i = 0; i <= staminaLayout.childCount - 1;  i++)
        {
            Image currentPoint = staminaLayout.GetChild(i).GetComponent<Image>();

            currentPoint.sprite = regularPoint;
            currentPoint.color = new Color32(255, 255, 255, 255);
        }
    }

    private void Update()
    {
        currentTimer -= Time.deltaTime;
    }
}
