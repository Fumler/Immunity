using System.Collections.Generic;

namespace immunity
{
    internal class UnitFactory
    {
        //Static methods
        /// <summary>
        /// Creates new units based on an array of unit types.
        /// </summary>
        /// <param name="units">Ints representing unit types.</param>
        /// <param name="unitList">List where finished units will be placed.</param>
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