using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Field _field;
    [SerializeField] Cell _cellPrefab;
    // Start is called before the first frame update
    void Start()
    {
        _field.CreateField(4, 4, _cellPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        Cell[,] cells = (Cell[,])_field;
    }
}
