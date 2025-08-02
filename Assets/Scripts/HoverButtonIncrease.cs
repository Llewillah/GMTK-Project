using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButtonIncrease : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject objectGame;
    private Vector3 originalTransform;
    [SerializeField] private Vector3 newTransform;

    void Start()
    {
        objectGame = gameObject;
        originalTransform = transform.localScale;
    }

    //back to og scale
    private void OnDisable()
    {
        if (objectGame != null && objectGame.transform != null)
        {
            objectGame.transform.localScale = originalTransform;
        }
    }

    //mouse over thing
    public void OnPointerEnter(PointerEventData eventData)
    {
        objectGame.transform.localScale = newTransform;
    }

    //mouse exit thing
    public void OnPointerExit(PointerEventData eventData)
    {
        objectGame.transform.localScale = originalTransform;
    }
}
