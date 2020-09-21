using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

//#Projekt: Добавить перегрузку построения
namespace OOP_Paint {
    class MainCode {
        public enum Figure {
            Null = 0,
            Circle,
            Rectangle,
        }
        public enum BuildingVariants {
            InRectangleTwoDots,
            DotRadius,
        }

        public BuildingVariants BuildingVariant;
        public BuildingVariants currBuildingVariant = 0;
        private Int32 currConstructorStage = 0;

        private Figure currSelectedFigure = Figure.Null;
        public Figure CurrSelectedFigure {
            set {
                CloseConstructor();

                currSelectedFigure = value;
                BuildingVariant = 0;
            }
            get => currSelectedFigure;
        }

        public readonly BindingList<MyFigure> Figures = new BindingList<MyFigure>();
        private readonly List<MyFigure> supportFigures = new List<MyFigure>();
        private readonly List<Point> pointsList = new List<Point>();

        private static readonly Pen supportPen = new Pen(Color.Red) { Width = 1, DashStyle = DashStyle.Dash };
        private static readonly Pen supportPen2 = new Pen(Color.Black, 2);
        private static readonly Pen normalPen = new Pen(Color.Black);

        
        //???По-хорошему нужен отдельный метод для прорисовки
        //и обработки клика мыши, однако дублировать switsch 
        //очень плохо, тем более такой длинный. Тогда нужен MouseEvent
        //для понимания, клик это или движение, что в свою очередь
        //ведёт упаковку в object, т.к. это класс.
        //upd: Повезло, класс один. Но всё равно один метод вмещает 2 из-за switsch ("if (e.Button == MouseButtons.Left)".)
        public ConstructorResult ThreatMouseEvent(MouseEventArgs e) {
            ConstructorResult out_result;
            //???По-хорошему buildingVariant должен быть перечислением, ибо если
            //хоть что-то изменить в списке вариантов, нужно трогать свич.
            //Но перечисления для всех фигур невероятно громоздки.
            ///currSelectedFigure -> Выбор фигуры построения
            ///currBuildingVariant -> Выбор варианта построения фигуры
            ///currConstructorStage -> Выбор текущей стадии построения (могут отличаться вспомогательные фигуры)
            ///MouseButtons -> Выбор, движение мыши (изменение вспомогательных фигур) или клик, переход к следующей стадии построения.
            switch (currSelectedFigure) {
                case Figure.Null:
                    out_result = new ConstructorResult(ConstructorResult.OperationStatus.None, "");
                    break;
                case Figure.Circle:
                    switch (currBuildingVariant) {
                        case BuildingVariants.InRectangleTwoDots:
                            switch (currConstructorStage) {
                                case 0:
                                    if (e.Button == MouseButtons.Left) {
                                        supportFigures.Add(new MyRectangle(e.X, e.Y, e.X, e.Y, supportPen));
                                        supportFigures.Add(new MyCircle(e.X, e.Y, e.X, e.Y, supportPen));
                                        pointsList.Add(e.Location);
                                        currConstructorStage++;
                                        out_result = new ConstructorResult(ConstructorResult.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                        break;
                                    }
                                    else return new ConstructorResult(ConstructorResult.OperationStatus.None, "");
                                case 1:
                                    if (e.Button == MouseButtons.Left) {
                                        if (pointsList[0] == e.Location) {
                                            out_result = new ConstructorResult(ConstructorResult.OperationStatus.None, "");
                                            break;
                                        }

                                        Figures.Add(new MyCircle(pointsList[0].X, pointsList[0].Y, e.X, e.Y, normalPen));
                                        CloseConstructor();
                                        out_result = new ConstructorResult(ConstructorResult.OperationStatus.Finished, "");
                                        break;
                                    }
                                    else if (e.Button == MouseButtons.None) {
                                        supportFigures[supportFigures.Count - 1] = new MyRectangle(pointsList[0].X, pointsList[0].Y, e.X, e.Y, supportPen);
                                        out_result = new ConstructorResult(ConstructorResult.OperationStatus.None, "");
                                        break;
                                    }
                                    else return new ConstructorResult(ConstructorResult.OperationStatus.None, "");

                                default:
                                    throw new Exception();
                            }
                            break;
                        case BuildingVariants.DotRadius:
                            throw new NotImplementedException();
                        default: throw new Exception();
                    }
                    break;
                case Figure.Rectangle:
                    throw new NotImplementedException();
                default: throw new Exception();
            }
            return out_result;
        }


        private void CloseConstructor() {
            currBuildingVariant = 0;
            currConstructorStage = 0;
            supportFigures.Clear();
            pointsList.Clear();
        }


        public void DrawFigures(Graphics _screen) {
            _screen.Clear(Color.FromArgb(250, 64, 64, 64));
            foreach (var figure in Figures) {
                figure.Draw(_screen);
            }
            foreach (var figure in supportFigures) {
                figure.Draw(_screen);
            }
        }
        public List<BuildingVariants> FingPossibleBuildingVariants(Figure _figure) {
            var out_list = new List<BuildingVariants>();
            switch (_figure) {
                case Figure.Circle:
                    out_list.Add(BuildingVariants.DotRadius);
                    out_list.Add(BuildingVariants.InRectangleTwoDots);
                    break;
                default: throw new Exception();
            }
            return out_list;
        }

    }
}
