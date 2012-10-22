using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace immunity
{
    class UnitFactory
    {
        public static void createUnits(int[] units, ref List<Unit> unitList) {
            for (int i = 0; i < units.Length; i++) {
                Unit newUnit = new Unit(units[i]);
                unitList.Add(newUnit);
            }
        }
    }
}
