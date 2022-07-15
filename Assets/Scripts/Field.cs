using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [Tooltip("更新の時間")]
    [SerializeField] float _updateTime = 0.5f;
    [Tooltip("自動更新がされるか")]
    [SerializeField] bool _isAuto = true;
    [Tooltip("周囲と判断する距離")]
    [SerializeField] int _distance = 1;
    [Tooltip("生き続けられる最小の数")]
    [SerializeField] int _minLiveCount = 2;
    [Tooltip("生き続けられる最大の数")]
    [SerializeField] int _maxLiveCount = 3;
    [Tooltip("新たに生まれる最小の数")]
    [SerializeField] int _minEngenderCount = 3;
    [Tooltip("新たに生まれる最小の数")]
    [SerializeField] int _maxEngenderCount = 3;

    /// <summary>フィールド</summary>
    Cell[,] _field;

    public Cell[,] Map
    {
        get
        {
            if (_field == null)
            {
                Debug.LogError($"フィールドの生成がされていません");
                return null;
            }
            return _field;
        }
    }

    /// <summary>更新の時間</summary>
    public float UpdateTime { get => _updateTime; set => _updateTime = value; }
    /// <summary>自動更新がされるか</summary>
    public bool IsAuto { get => _isAuto; set => _isAuto = value; }

    private void Update()
    {
    }

    private void OnValidate()
    {
        if(_distance <= 0)
        {
            _distance = 1;
            Debug.LogWarning($"{nameof(_distance)}が0以下だったため、1に設定しました");
        }
        if(_minLiveCount <= 0)
        {
            _minLiveCount = 1;
            Debug.LogWarning($"{nameof(_minLiveCount)}が0以下だったため、1に設定しました");
        }
        else if(_minLiveCount > AroundCellCount(_distance))
        {
            _minLiveCount = AroundCellCount(_distance);
            Debug.LogWarning($"{nameof(_minLiveCount)}が周囲のセルの数{AroundCellCount(_distance)}を超過していたため、{_minLiveCount}に設定しました");
        }
        if(_maxEngenderCount < _minEngenderCount)
        {
            _maxEngenderCount = _minEngenderCount;
            Debug.LogWarning($"{nameof(_maxEngenderCount)}が{nameof(_minEngenderCount)}の値{_minEngenderCount}未満だったため、{_minEngenderCount}に設定しました");
        }
        if (_maxEngenderCount > AroundCellCount(_distance))
        {
            _maxEngenderCount = AroundCellCount(_distance);
            Debug.LogWarning($"{nameof(_maxEngenderCount)}が周囲のセルの数{AroundCellCount(_distance)}を超過していたため、{_maxLiveCount}に設定しました");
        }
        if (_minEngenderCount <= 0)
        {
            _minEngenderCount = 1;
            Debug.LogWarning($"{nameof(_minEngenderCount)}が0以下だったため、1に設定しました");
        }
        else if (_minEngenderCount > AroundCellCount(_distance))
        {
            _minEngenderCount = AroundCellCount(_distance);
            Debug.LogWarning($"{nameof(_minEngenderCount)}が周囲のセルの数{AroundCellCount(_distance)}を超過していたため、{_minEngenderCount}に設定しました");
        }
        if (_maxEngenderCount < _minEngenderCount)
        {
            _maxEngenderCount = _minEngenderCount;
            Debug.LogWarning($"{nameof(_maxEngenderCount)}が{nameof(_minEngenderCount)}の値{_minEngenderCount}未満だったため、{_minEngenderCount}に設定しました");
        }
        if (_maxEngenderCount > AroundCellCount(_distance))
        {
            _maxEngenderCount = AroundCellCount(_distance);
            Debug.LogWarning($"{nameof(_maxEngenderCount)}が周囲のセルの数{AroundCellCount(_distance)}を超過していたため、{_maxEngenderCount}に設定しました");
        }
    }

    /// <summary>
    /// 世代の自動進行を担う
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
    /// フィールドの状況を確認する
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
    /// セルの変更を検証する
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
    /// 周囲の生きているセルの数を返す
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
    /// 距離に応じた周囲のセルの数を返す
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
    /// 世代を1つ進める
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
    /// フィールドの生成
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
    /// セルの状態を反転する
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
