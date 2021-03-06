﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace MachShooting
{
    /// <summary>
    /// 背景を表すクラス
    /// </summary>
    internal static class Back
    {
        internal static void Draw()
        {
            if (!Config.Instance.Low)
            {
                //Zバッファで描画可能領域
                DX.SetUseZBufferFlag(DX.TRUE);
                DX.DrawBoxToZBuffer(0, 0, Game.WINDOW_R * 2, Game.WINDOW_R * 2, DX.TRUE, DX.DX_ZWRITE_MASK);
                DX.DrawCircleToZBuffer(Game.WINDOW_R, Game.WINDOW_R, Game.WINDOW_R, DX.TRUE, DX.DX_ZWRITE_CLEAR);


                //背景
                //画像サイズ
                int imgW;
                int imgH;
                DX.GetGraphSize(DXImage.Instance.Back, out imgW, out imgH);

                //描画回数
                int x = (Game.WINDOW_R * 2) / imgW + 1;

                int y = (Game.WINDOW_R * 2) / imgH + 1;

                //描画
                for (int i = 0; i < y; i++)
                {
                    for (int j = 0; j < x; j++)
                    {
                        DX.DrawGraph(j * imgW, i * imgH, DXImage.Instance.Back, DX.FALSE);
                    }
                }


                //Zバッファ解除
                DX.SetUseZBufferFlag(DX.FALSE);
            }
            else//低負荷
            {
                DX.DrawCircle(Game.WINDOW_R, Game.WINDOW_R, Game.WINDOW_R, DXColor.Instance.White,DX.FALSE);
            }
        }
    }
}
