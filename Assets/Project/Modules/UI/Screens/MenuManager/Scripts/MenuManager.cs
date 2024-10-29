using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{

    [Header("First Selected Options")]
    [SerializeField] private GameObject[] _firstButtonsList;

    [Header("First Selected Button")]
    [SerializeField] private GameObject _firstButtonStart;

    void Start()
    {
        _firstButtonStart = _firstButtonsList[0];
        EventSystem.current.SetSelectedGameObject(_firstButtonStart);  
    }

}
