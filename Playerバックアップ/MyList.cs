using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachShooting
{
    /// <summary>
    /// 装備の種類
    /// </summary>
    public enum Equipment
    {
        STABILITY,
        SUPER_CHARGE,
        CHARGE,
        OVERALL,
        DOUBLE
    }

    /// <summary>
    /// 武器種→文字列変換
    /// </summary>
    public static class EquipmentExt
    {
        // 拡張メソッド
        public static string GetName(this Equipment value)
        {
            string[] values = { "通常型", "超溜め型", "溜め型", "拡散型", "双連射型" };
            return values[(int)value];
        }
    }
}
