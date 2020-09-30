using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Paint {
    //#FB1: некорректное проектирование: начало слияние FigureBuilding с MyFigure
    public readonly struct FigureBuilding {
        //???У меня readonly перечисление, их уникальное количество ограничено,
        //однако создать таких структур можно бесконечно много. Ошибка делать такую структуру?
        //По сравнению с создаенем двух методов-конвертеров перечисления в имя.
        public enum Method {
            None,
            InRectangleTwoDots,
            DotRadius,
        }

        public readonly String Name;
        //Блин, одинаковые имена нельзя использовать; нет идей
        public readonly Method Methodd;


        public FigureBuilding(Method _bv) {
            Methodd = _bv;
            Name = ReturnBuildingVariantName(_bv);
        }


        public static Boolean operator ==(FigureBuilding _cm1, FigureBuilding _cm2) {
            if (_cm1.Methodd == _cm2.Methodd) {
                return true;
            }
            else {
                return false;
            }
        }
        public static Boolean operator !=(FigureBuilding _cm1, FigureBuilding _cm2) {
            if (_cm1.Methodd != _cm2.Methodd) {
                return true;
            }
            else {
                return false;
            }
        }


        private static String ReturnBuildingVariantName(Method _bv) {
            switch (_bv) {
                case Method.DotRadius: return "Точка, радиус";
                case Method.InRectangleTwoDots: return "Прямой угол, точка";
                default: throw new NotImplementedException();
            }

        }

    }


}
