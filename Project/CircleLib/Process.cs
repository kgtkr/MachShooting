using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleLib
{
    /// <summary>
    /// 図形操作
    /// </summary>
    public static partial class Graphic
    {
        /// <summary>
        /// 矩形と矩形の当たり判定を行います
        /// </summary>
        /// <param name="rect1">矩形1</param>
        /// <param name="rect2">矩形2</param>
        /// <returns></returns>
        public static bool Hit(Rect rect1, Rect rect2)
        {
            return (rect2.L < rect1.R) &&
                    (rect2.R > rect1.L) &&
                    (rect2.T < rect1.B) &&
                    (rect2.B > rect1.T);
        }

        /// <summary>
        /// カプセルとカプセルの当たり判定を行います
        /// </summary>
        /// <param name="capsule1"></param>
        /// <param name="capsule2"></param>
        /// <returns></returns>
        public static bool Hit(Capsule capsule1,Capsule capsule2)
        {
            if (!Hit(capsule1.Rect, capsule2.Rect))
            {
                return false;
            }
            double d = Distance(capsule1.Line, capsule2.Line) - capsule1.R - capsule2.R;
            return (d <= 0.0);
        }

        /// <summary>
        /// 線と線の距離を求めます
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static double Distance(Line line1, Line line2)
        {
            Vec l1d1 = (Vec)line1.Dot1;
            Vec l1d2 = (Vec)line1.Dot2;
            Vec l2d1 = (Vec)line2.Dot1;
            Vec l2d2 = (Vec)line2.Dot2;


            //各線が点であるか調べる
            bool s1Dot = line1.Dot1.X == line1.Dot2.X && line1.Dot1.Y == line1.Dot2.Y;
            bool s2Dot = line2.Dot1.X == line2.Dot2.X && line2.Dot1.Y == line2.Dot2.Y;

            //どちらも点
            if (s1Dot && s2Dot)
            {
                return Math.Sqrt(Math.Pow(line2.Dot1.X - line1.Dot1.X, 2) + Math.Pow(line2.Dot1.Y - line1.Dot1.Y, 2));
            }

            //Xが同じ
            if (l1d1.X == l1d2.X && l1d1.X == l2d1.X && l1d1.X == l2d2.X)
            {
                double l1t = Math.Min(l1d1.Y, l1d2.Y);
                double l1b = Math.Max(l1d1.Y, l1d2.Y);
                double l2t = Math.Min(l2d1.Y, l2d2.Y);
                double l2b = Math.Max(l2d1.Y, l2d2.Y);

                //重なっている
                if (l2t <= l1b && l2b >= l1t)
                {
                    return 0;
                }

                if (l1t < l2t)//l1が上
                {
                    return l2t - l1b;
                }else//l2が上
                {
                    return l1t - l2b;
                }
            }

            //Yが同じ
            if (l1d1.Y == l1d2.Y && l1d1.Y == l2d1.Y && l1d1.Y == l2d2.Y)
            {
                double l1l = Math.Min(l1d1.X, l1d2.X);
                double l1r = Math.Max(l1d1.X, l1d2.X);
                double l2l = Math.Min(l2d1.X, l2d2.X);
                double l2r = Math.Max(l2d1.X, l2d2.X);

                //重なっている
                if (l2l <= l1r && l2r >= l1l)
                {
                    return 0;
                }

                if (l1l < l2l)//l1が左
                {
                    return l2l - l1r;
                }
                else//l2が右
                {
                    return l1l - l2r;
                }
            }

            // 線分の方向ベクトル
            Vec[] v = {
            line1.Vec,
            line2.Vec
            };

            // 基点から相手の端点までのベクトル
            Vec[][] vp = {
                new Vec[]{ l2d1 - l1d1, l2d2 - l1d1},
                new Vec[]{ l1d1 - l2d1, l1d2 - l2d1}
            };

            // VとVPの外積値
            double[][] crossToVP = {
            new double[]{ v[0]/vp[0][0] , v[0]/vp[0][1] },
            new double[]{ v[1]/vp[1][0] , v[1]/vp[1][1] }
            };

            // 交差判定
            if (
                crossToVP[0][0] * crossToVP[0][1] <= 0.0 &&
                crossToVP[1][0] * crossToVP[1][1] <= 0.0
            )
            {
                return 0;
            }

            // 最小距離を計算
            Line[] seg = { line1, line2 };
            double minDist = double.MaxValue;
            for (int s = 0; s < 2; s++)
            {

                double dotVV = v[s] * v[s];
                double dist = 0.0f;
                bool isInner = true;

                for (int i = 0; i < 2; i++)
                {

                    // 対象点の媒介変数算出
                    double t = vp[s][i] * v[s] / dotVV;

                    if (t >= 0.0f && t <= 1.0f)
                    {
                        // 線分内にある
                        dist = Math.Abs(crossToVP[s][i]) / Math.Sqrt(dotVV);

                    }
                    else if (t < 0.0f)
                    {
                        // 基点寄り外側
                        dist = (((Vec)(seg[(s + 1) % 2])[i]) - ((Vec)seg[s].Dot1)).Length;
                        isInner = false;

                    }
                    else {
                        // 終点寄り外側
                        dist = (((Vec)(seg[(s + 1) % 2])[i]) - ((Vec)seg[s].Dot2)).Length;
                        isInner = false;
                    }

                    minDist = (dist < minDist ? dist : minDist);
                }
                // もし両点とも線分の内側だったら終了
                if (isInner == true)
                    break;
            }

            return minDist;
        }
    }
}
