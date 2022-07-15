using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cell : MonoBehaviour
{
    [Tooltip("�Z���̐���")]
    [SerializeField] CellState _state;
    [Tooltip("�����Ă���Z���̐F")]
    [SerializeField] Color _liveColor = Color.green;
    [Tooltip("����ł���Z���̐F")]
    [SerializeField] Color _deadColor = Color.white;

    Image _image;
    
    bool _isAlive = false;

    //public

    // Start is called before the first frame update
    void Start()
    {
        _isAlive = true;
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnValidate()
    {
        Transcription();
    }

    public void Transcription()
    {
        if (!_isAlive)
        {
            Start();
        }
        if (_state == CellState.Live || _state == CellState.Die)
        {
            _image.color = _liveColor;
        }
        else if(_state == CellState.Dead || _state == CellState.Engender)
        {
            _image.color = _deadColor;
        }
    }
}

/// <summary>
/// �Z���̏��
/// </summary>
public enum CellState
{
    /// <summary>�����Ă���</summary>
    Live,
    /// <summary>����ł���</summary>
    Dead,
    /// <summary>���܂��</summary>
    Engender,
    /// <summary>����</summary>
    Die,
}
