using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//#1CM: перевести код на структуру
namespace OOP_Paint {
    public readonly struct ConstructionMethod {
        //???У меня readonly перечисление, их уникальное количество ограничено,
        //однако создать таких структур можно бесконечно много. Ошибка делать такую структуру?
        //По сравнению с создаенем двух методов-конвертеров перечисления в имя.
        public enum BuildingVariants {
            None,
            InRectangleTwoDots,
            DotRadius,
        }
        public ConstructionMethod(BuildingVariants _bv) {
            Method = _bv;
            Name = ReturnBuildingVariantName(_bv);
        }

        public readonly String Name;
        public readonly BuildingVariants Method;

        private static String ReturnBuildingVariantName(BuildingVariants _bv) {
            switch (_bv) {
                case BuildingVariants.DotRadius: return "Точка, радиус";
                case BuildingVariants.InRectangleTwoDots: return "Прямой угол, точка";
                default: throw new NotImplementedException();
            }

        }

        public static Boolean operator ==(ConstructionMethod _cm1, ConstructionMethod _cm2) {
            if (_cm1.Method == _cm2.Method) {
                return true;
            }
            else {
                return false;
            }
        }
        public static Boolean operator !=(ConstructionMethod _cm1, ConstructionMethod _cm2) {
            if (_cm1.Method != _cm2.Method) {
                return true;
            }
            else {
                return false;
            }
        }
    }


}
