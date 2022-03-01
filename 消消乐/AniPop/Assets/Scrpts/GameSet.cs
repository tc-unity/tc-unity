using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSet
{
    public static int mRow = 9;
    public static int mCol = 9;


    public static float mWidth = 1;
    public static float mHight = 1;

    public Vector2 TopPlace;

    #region 单例
    private GameSet()
    {
        TopPlace = new Vector2(
            -(mCol * mWidth / 2 - mWidth / 2),
            mRow * mHight / 2 - mHight / 2
            );
    }
    public static readonly GameSet Instance = new GameSet();




    #endregion
}
