using System.Collections.Generic;

namespace immunity
{
    internal class UnitFactory
    {
        //Static methods
        public static void CreateUnits(int[] units, ref List<Unit> unitList)
        {
            for (int i = 0; i < units.Length; i++)
            {
                Unit newUnit = new Unit(units[i]);
                unitList.Add(newUnit);
            }
        }
    }
}