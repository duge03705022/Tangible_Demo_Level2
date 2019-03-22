using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameParameter
{
    public static readonly float dishSpeed = 0.05F;
    public static readonly float stageGap = 7.5F;
    public static readonly int cardOrderInLayer = 30;

    public static readonly Tuple<int, int> playBtnPos = Tuple.Create(8, 0);
    public static readonly Tuple<int, int> recipeBtnPos = Tuple.Create(6, 4);
}
