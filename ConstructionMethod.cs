using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public readonly struct ConstructionMethod {
        public enum BuildingVariant {
            None,
            InRectangleTwoDots,
            DotRadius,
        }
//#1CM: перевести код на структуру

        public readonly string Name;
        public readonly BuildingVariant BuildingVariant;
    }
}
