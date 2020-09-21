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

        private static readonly Pen supportPen = new Pen(Color.Red) { Width = 2, DashStyle = DashStyle.Dash };
        private static readonly Pen supportPen2 = new Pen(Color.Black, 1);
        private static readonly Pen normalPen = new Pen(Color.Black);


        public ConstructorResult AddMouseClick(Point _coord) {
            ConstructorResult out_result;
            //???По-хорошему buildingVariant должен быть перечислением, ибо если
            //хоть что-то изменить в списке вариантов, нужно трогать свич.
            //Но перечисления для всех фигур невероятно громоздки.
            switch (currSelectedFigure) {
                case Figure.Null:
                    out_result = new ConstructorResult(ConstructorResult.OperationStatus.None, "");
                    break;
                case Figure.Circle:
                    switch (currBuildingVariant) {
                        case BuildingVariants.InRectangleTwoDots:
                            switch (currConstructorStage) {
                                case 0:
                                    supportFigures.Add(new MyRectangle(_coord.X, _coord.Y, _coord.X, _coord.Y, supportPen));
                                    supportFigures.Add(new MyCircle(_coord.X, _coord.Y, _coord.X, _coord.Y, supportPen2));
                                    pointsList.Add(_coord);
                                    currConstructorStage++;
                                    out_result =  new ConstructorResult(ConstructorResult.OperationStatus.Continious, "Задайте вторую точку");
                                    break;
                                case 1:
                                    if (pointsList[0] == _coord) {
                                        out_result = new ConstructorResult(ConstructorResult.OperationStatus.Continious, "Задайте вторую точку");
                                        break;
                                    }

                                    Figures.Add(new MyCircle(pointsList[0].X, pointsList[0].Y, _coord.X, _coord.Y, normalPen));
                                    CloseConstructor();
                                    out_result = new ConstructorResult(ConstructorResult.OperationStatus.Finished, "");
                                    break;
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
            foreach(var figure in supportFigures) {
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
