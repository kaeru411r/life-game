using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cell : MonoBehaviour
{
    [Tooltip("�Z���̏��")]
    [SerializeField] CellState _state = CellState.Dead;
    [Tooltip("�����Ă���Z���̐F")]
    [SerializeField] Color _liveColor = Color.green;
    [Tooltip("����ł���Z���̐F")]
    [SerializeField] Color _deadColor = Color.white;
    [Tooltip("�Z���̑傫��")]
    [SerializeField] float _size = 100;

    /// <summary>�Z���̃C���[�W</summary>
    Image _image;
    /// <summary>�Z���̏��������Ȃ���Ă��邩</summary>
    bool _isAlive = false;

    /// <summary>�Z���̏��</summary>
    public CellState State { get => _state;}
    /// <summary>�Z���̐���</summary>
    public bool IsLive { get => _state == CellState.Live || _state == CellState.Die; }

    private void OnValidate()
    {
        Transcription();
    }

    public void Transcription()
    {
        if (!_isAlive)
        {
            SetUp();
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

    /// <summary>
    /// �Z���̏�����
    /// </summary>
    public void SetUp()
    {
        _isAlive = true;
        _image = GetComponent<Image>();
        //if (Random.value < 0.7)
        //{
            _state = CellState.Dead;
        //}
        //else
        //{
        //    _state = CellState.Live;
        //}
        Transcription();
    }

    /// <summary>
    /// �Z��������
    /// </summary>
    public void Death()
    {
        _state = CellState.Die;
    }

    /// <summary>
    /// �Z�������܂��
    /// </summary>
    public void Engender()
    {
        _state = CellState.Engender;
    }

    /// <summary>
    /// �����1�i�߂�
    /// </summary>
    public void Step()
    {
        if(_state == CellState.Engender)
        {
            _state = CellState.Live;
        }
        else if(_state == CellState.Die)
        {
            _state = CellState.Dead;
        }
        Transcription();
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
