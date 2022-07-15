using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [Tooltip("�X�V�̎���")]
    [SerializeField] float _updateTime = 0.5f;
    [Tooltip("�����X�V������邩")]
    [SerializeField] bool _isAuto = true;
    [Tooltip("���͂Ɣ��f���鋗��")]
    [SerializeField] int _distance = 1;
    [Tooltip("������������ŏ��̐�")]
    [SerializeField] int _minLiveCount = 2;
    [Tooltip("������������ő�̐�")]
    [SerializeField] int _maxLiveCount = 3;
    [Tooltip("�V���ɐ��܂��ŏ��̐�")]
    [SerializeField] int _minEngenderCount = 3;
    [Tooltip("�V���ɐ��܂��ŏ��̐�")]
    [SerializeField] int _maxEngenderCount = 3;

    /// <summary>�t�B�[���h</summary>
    Cell[,] _field;

    public Cell[,] Map
    {
        get
        {
            if (_field == null)
            {
                Debug.LogError($"�t�B�[���h�̐���������Ă��܂���");
                return null;
            }
            return _field;
        }
    }

    /// <summary>�X�V�̎���</summary>
    public float UpdateTime { get => _updateTime; set => _updateTime = value; }
    /// <summary>�����X�V������邩</summary>
    public bool IsAuto { get => _isAuto; set => _isAuto = value; }

    private void Update()
    {
    }

    private void OnValidate()
    {
        if(_distance <= 0)
        {
            _distance = 1;
            Debug.LogWarning($"{nameof(_distance)}��0�ȉ����������߁A1�ɐݒ肵�܂���");
        }
        if(_minLiveCount <= 0)
        {
            _minLiveCount = 1;
            Debug.LogWarning($"{nameof(_minLiveCount)}��0�ȉ����������߁A1�ɐݒ肵�܂���");
        }
        else if(_minLiveCount > AroundCellCount(_distance))
        {
            _minLiveCount = AroundCellCount(_distance);
            Debug.LogWarning($"{nameof(_minLiveCount)}�����͂̃Z���̐�{AroundCellCount(_distance)}�𒴉߂��Ă������߁A{_minLiveCount}�ɐݒ肵�܂���");
        }
        if(_maxEngenderCount < _minEngenderCount)
        {
            _maxEngenderCount = _minEngenderCount;
            Debug.LogWarning($"{nameof(_maxEngenderCount)}��{nameof(_minEngenderCount)}�̒l{_minEngenderCount}�������������߁A{_minEngenderCount}�ɐݒ肵�܂���");
        }
        if (_maxEngenderCount > AroundCellCount(_distance))
        {
            _maxEngenderCount = AroundCellCount(_distance);
            Debug.LogWarning($"{nameof(_maxEngenderCount)}�����͂̃Z���̐�{AroundCellCount(_distance)}�𒴉߂��Ă������߁A{_maxLiveCount}�ɐݒ肵�܂���");
        }
        if (_minEngenderCount <= 0)
        {
            _minEngenderCount = 1;
            Debug.LogWarning($"{nameof(_minEngenderCount)}��0�ȉ����������߁A1�ɐݒ肵�܂���");
        }
        else if (_minEngenderCount > AroundCellCount(_distance))
        {
            _minEngenderCount = AroundCellCount(_distance);
            Debug.LogWarning($"{nameof(_minEngenderCount)}�����͂̃Z���̐�{AroundCellCount(_distance)}�𒴉߂��Ă������߁A{_minEngenderCount}�ɐݒ肵�܂���");
        }
        if (_maxEngenderCount < _minEngenderCount)
        {
            _maxEngenderCount = _minEngenderCount;
            Debug.LogWarning($"{nameof(_maxEngenderCount)}��{nameof(_minEngenderCount)}�̒l{_minEngenderCount}�������������߁A{_minEngenderCount}�ɐݒ肵�܂���");
        }
        if (_maxEngenderCount > AroundCellCount(_distance))
        {
            _maxEngenderCount = AroundCellCount(_distance);
            Debug.LogWarning($"{nameof(_maxEngenderCount)}�����͂̃Z���̐�{AroundCellCount(_distance)}�𒴉߂��Ă������߁A{_maxEngenderCount}�ɐݒ肵�܂���");
        }
    }

    /// <summary>
    /// ����̎����i�s��S��
    /// </summary>
    /// <returns></returns>
    IEnumerator AutoUpdate()
    {
        float time = 0;
        while (true)
        {
            if (_isAuto)
            {
                if (time > _updateTime)
                {
                    time -= _updateTime;
                    Step();
                }
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
            }
            yield return null;
        }
    }

    /// <summary>
    /// �t�B�[���h�̏󋵂��m�F����
    /// </summary>
    void LifeCheck()
    {
        for (int i = 0; i < _field.GetLength(0); i++)
        {
            for (int k = 0; k < _field.GetLength(1); k++)
            {
                CellCheck(k, i);
            }
        }
    }

    /// <summary>
    /// �Z���̕ύX�����؂���
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    void CellCheck(int row, int col)
    {
        if (!EreaCheck(row, col))
        {
            return;
        }
        Cell cell = _field[col, row];
        int liveCount = LiveCellCount(row, col);
        if (cell.IsLive)
        {
            if (liveCount < _minLiveCount || liveCount > _maxLiveCount)
            {
                cell.Death();
            }
        }
        else
        {
            if(liveCount >= _minEngenderCount && liveCount <= _maxEngenderCount)
            {
                cell.Engender();
            }
        }
    }

    /// <summary>
    /// ���͂̐����Ă���Z���̐���Ԃ�
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    int LiveCellCount(int row, int col)
    {
        if (!EreaCheck(row, col))
        {
            return -1;
        }
        int liveCount = 0;
        for (int i = row - _distance; i <= row + _distance; i++)
        {
            for (int k = col - _distance; k <= col + _distance; k++)
            {
                if (EreaCheck(i, k))
                {
                    if (_field[k, i].IsLive && !(row == i && col == k))
                    {
                        liveCount++;
                    }
                }
            }
        }
        return liveCount;
    }

    /// <summary>
    /// �����ɉ��������͂̃Z���̐���Ԃ�
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    int AroundCellCount(int distance)
    {
        int count = 0;
        int side = 1;
        for(int i = 0; i < _distance; i++)
        {
            count += side * 4 + 4;
            side += 2;
        }
        return count;
    }

    bool EreaCheck(int row, int col)
    {
        if (row < 0 || col < 0 || row >= _field.GetLength(1) || col >= _field.GetLength(0))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// �����1�i�߂�
    /// </summary>
    public void Step()
    {
        foreach (Cell c in _field)
        {
            c.Step();
        }
        LifeCheck();
    }

    /// <summary>
    /// �t�B�[���h�̐���
    /// </summary>
    public void CreateField(int row, int col, Cell cellPrefab)
    {
        _field = new Cell[col, row];
        for (int i = 0; i < col; i++)
        {
            for (int k = 0; k < row; k++)
            {
                Cell cell = Instantiate(cellPrefab, transform);
                _field[i, k] = cell;
                cell.SetUp();
            }
        }
        StartCoroutine(AutoUpdate());
    }

    /// <summary>
    /// �Z���̏�Ԃ𔽓]����
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    public void CellInvert(int row, int col)
    {
        Cell cell = _field[col, row];
        if (cell.State == CellState.Live)
        {
            cell.Death();
        }
        else if (cell.State == CellState.Dead)
        {
            cell.Engender();
        }
    }




    public static explicit operator Cell[,](Field field)
    {
        return field._field;
    }
}
