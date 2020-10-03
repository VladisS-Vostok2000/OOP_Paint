using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
    public sealed class MainCode {


        public event EventHandler SelectedFigureChanged;
        public event EventHandler SelectedBuildingVariantChanged;

        private MyFigure.BuildingMethod selectedBuildingMethodIndex;
        public MyFigure.BuildingMethod SelectedBuildingMethod {
            set {
                if (selectedBuildingMethodIndex != value) {
                    SelectedBuildingMethod = value;
                    SelectedBuildingVariantChanged?.Invoke(value, EventArgs.Empty);
                    //BuildingVariantChanged?.Invoke(CurrBuildingVariant, new PropertyChangedEventArgs("CurrBuildingVariant"));

                }
            }
            get => selectedBuildingMethodIndex;
        }
        //???Не совсем понятно назначение этого перечисления. Единственный плюс - не нужно возиться
        //с объектом. Из минусов: каждый раз его нужно конвертировать в этот самый объект. Пустой.
        //
        private Figure selectedFigure;
        public Figure SelectedFigure {
            set {
                if (selectedFigure != value) {
                    CloseConstructor();
                    SelectedFigureChanged?.Invoke(value, EventArgs.Empty);
                    selectedFigure = value;

                    if (FingPossibleBuildingVariants(value).Count != 0) {
                        SelectedBuildingMethod = FindPossibleBuildingVariants()[0];
                    }
                    else {
                        SelectedBuildingMethod = new MyFigureBuildingsVariants(MyFigureBuildingsVariants.Method.None);
                    }

                }
            }
            get => selectedFigure;
        }
        private Int32 currConstructorStage = 0;

        //???Лист реализует Binding-логику, которая необходима для реализации событий в GUI,
        //однако он становится публичным и все его фигуры доступны для редактирования снаружи.
        public readonly BindingList<MyFigure> Figures = new BindingList<MyFigure>();
        private readonly List<MyFigure> supportFigures = new List<MyFigure>();
        private readonly List<Point> pointsList = new List<Point>();

        private static readonly Pen supportPen = new Pen(Color.Blue) { Width = 1, DashStyle = DashStyle.Dash };
        private static readonly Pen supportPen2 = new Pen(Color.Black, 2);
        private static readonly Pen figurePen = new Pen(Color.Black);



        //???По-хорошему нужен отдельный метод для прорисовки
        //и обработки клика мыши, однако дублировать switsch 
        //очень плохо, тем более такой длинный. Тогда нужен MouseEvent
        //для понимания, клик это или движение, что в свою очередь
        //ведёт упаковку в object, т.к. это класс.
        //upd: Повезло, класс один. Но всё равно один метод вмещает 2 из-за switsch ("if (e.Button == MouseButtons.Left)".)
        //???Что насчёт AddPoint(enum Strong/Soft)
        public ConstructorOperationResult ThreatMouseEvent(MouseEventArgs e) {
            ConstructorOperationResult out_result;
            //???По-хорошему buildingVariant должен быть перечислением, ибо если
            //хоть что-то изменить в списке вариантов, нужно трогать свич.
            //Но перечисления для всех фигур невероятно громоздки.
            ///currSelectedFigure -> Выбор фигуры построения
            ///currBuildingVariant -> Выбор варианта построения фигуры
            ///currConstructorStage -> Выбор текущей стадии построения (могут отличаться вспомогательные фигуры)
            ///MouseButtons -> Выбор, движение мыши (изменение вспомогательных фигур) или клик, переход к следующей стадии построения.
            switch (SelectedFigure) {
                case Figure.None:
                    out_result = new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                    break;
                case Figure.Circle:
                    switch (SelectedBuildingMethod) {
                        case MyFigure.BuildingMethod.CircleInRectangleByTwoDots:
                            switch (currConstructorStage) {
                                case 0:
                                    if (e.Button == MouseButtons.Left && e.Clicks == 1) {
                                        supportFigures.Add(new MyRectangle(e.X, e.Y, e.X, e.Y, supportPen));
                                        supportFigures.Add(new MyCircle(e.X, e.Y, e.X, e.Y, supportPen));
                                        pointsList.Add(e.Location);
                                        currConstructorStage++;
                                        out_result = new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                        break;
                                    }
                                    else return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                case 1:
                                    if (e.Button == MouseButtons.Left && e.Clicks == 1) {
                                        if (pointsList[0].X == e.X || pointsList[0].Y == e.Y) {
                                            out_result = new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                            break;
                                        }

                                        Figures.Add(new MyCircle(pointsList[0].X, pointsList[0].Y, e.X, e.Y, figurePen));
                                        CloseConstructor();
                                        out_result = new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Finished, "");
                                        break;
                                    }
                                    else {
                                        supportFigures[supportFigures.Count - 1] = new MyRectangle(pointsList[0].X, pointsList[0].Y, e.X, e.Y, supportPen);
                                        out_result = new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                        break;
                                    }
                                default:
                                    throw new Exception();
                            }
                            break;
                        case MyFigure.BuildingMethod.CircleDotRadius:
                            throw new NotImplementedException();
                        default: throw new Exception("Неверный вариант построения выбранной фигуры.");
                    }
                    break;
                default: throw new NotImplementedException($"Фигура {SelectedFigure} не реализована.");
            }

            return out_result;
        }


        private void CloseConstructor() {
            //CurrBuildingVariant = BuildingVariants.None;
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
        //#MC20: неверное расположение FingPossibleBuildingVariants: должен быть в классе

    }
}
