using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cell : MonoBehaviour
{
    [Tooltip("セルの状態")]
    [SerializeField] CellState _state = CellState.Dead;
    [Tooltip("生きているセルの色")]
    [SerializeField] Color _liveColor = Color.green;
    [Tooltip("死んでいるセルの色")]
    [SerializeField] Color _deadColor = Color.white;
    [Tooltip("セルの大きさ")]
    [SerializeField] float _size = 100;

    /// <summary>セルのイメージ</summary>
    Image _image;
    /// <summary>セルの初期化がなされているか</summary>
    bool _isAlive = false;

    /// <summary>セルの状態</summary>
    public CellState State { get => _state;}
    /// <summary>セルの生死</summary>
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
    /// セルの初期化
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
    /// セルが死ぬ
    /// </summary>
    public void Death()
    {
        _state = CellState.Die;
    }

    /// <summary>
    /// セルが生まれる
    /// </summary>
    public void Engender()
    {
        _state = CellState.Engender;
    }

    /// <summary>
    /// 世代を1つ進める
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
/// セルの状態
/// </summary>
public enum CellState
{
    /// <summary>生きている</summary>
    Live,
    /// <summary>死んでいる</summary>
    Dead,
    /// <summary>生まれる</summary>
    Engender,
    /// <summary>死ぬ</summary>
    Die,
}
