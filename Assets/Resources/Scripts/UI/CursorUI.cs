using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CursorUI
{

    bool GetActive();

    void CursorChoose(int num);

    void CursorInit(Cursor cursor);

    void CursorChange(int pageTmp);
}
