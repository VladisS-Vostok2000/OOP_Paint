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
using static OOP_Paint.FiguresEnum;

//!!!#Projekt84: Добавить перегрузку построения фигуры
namespace OOP_Paint {
    public sealed class MainCode {
        public delegate void BuildingMethodHandler(BuildingMethod buildingMethod, EventArgs e);
        public delegate void FigureHandler(Figure figure, EventArgs e);
        #region API
        public event FigureHandler SelectedFigureChanged;
        public event BuildingMethodHandler SelectedBuildingVariantChanged;
        public event ListChangedEventHandler FiguresListChanged;
        //Figures.ListChanged;
        #endregion

        private Figure selectedFigure;
        public Figure SelectedFigure {
            set {
                if (selectedFigure != value) {
                    CloseConstructor();
                    SelectedFigureChanged?.Invoke(value, EventArgs.Empty);
                    selectedFigure = value;

                    SelectedBuildingMethod = ReturnPossibleBuildingVariants(SelectedFigure)[0];

                }
            }
            get => selectedFigure;
        }
        private BuildingMethod selectedBuildingMethod;
        public BuildingMethod SelectedBuildingMethod {
            set {
                if (selectedBuildingMethod != value) {
                    selectedBuildingMethod = value;
                    SelectedBuildingVariantChanged?.Invoke(value, EventArgs.Empty);

                }
            }
            get => selectedBuildingMethod;
        }
        private Int32 currConstructorStage = 0;
        //!!!MainCode#54: пересоздать FiguresList благодаря ивентам
        //Лист реализует Binding-логику, которая необходима для привязки и реализации событий в GUI,
        //однако он становится публичным и все его элементы доступны для редактирования снаружи.
        private readonly BindingList<MyFigure> Figures = new BindingList<MyFigure>();
        private readonly List<MyFigure> supportFigures = new List<MyFigure>();
        private readonly List<Point> pointsList = new List<Point>();

        private static readonly Pen supportPen = new Pen(Color.Blue) { Width = 1, DashStyle = DashStyle.Dash };
        private static readonly Pen supportPen2 = new Pen(Color.Red, 1);
        private static readonly Pen figurePen = new Pen(Color.Black);



        //???Странная защита листа: он один фиг передаётся как sender, а использование
        //события ради события выглядит нелепо. Стоит сделать list public?
        public MainCode() {
            Figures.ListChanged += Figures_ListChanged;
        }



        private void Figures_ListChanged(Object sender, ListChangedEventArgs e) {
            FiguresListChanged?.Invoke(sender, e);
        }


        //???По-хорошему нужен отдельный метод для прорисовки
        //и обработки клика мыши, однако дублировать switsch 
        //очень плохо, тем более такой длинный. Тогда нужен MouseEvent
        //для понимания, клик это или движение, что в свою очередь
        //ведёт упаковку в object, т.к. это класс.
        //upd: Повезло, класс один. Но всё равно один метод вмещает 2 из-за switsch ("if (e.Button == MouseButtons.Left)".)
        //???Что насчёт AddPoint(enum Strong/Soft)
        //!!!MainCode#83: разбить ThreatMouseEvent на AddPoint(enum Strong/Soft)
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
                        case BuildingMethod.CircleInRectangleByTwoDots:
                            switch (currConstructorStage) {
                                case 0:
                                    //Кликнули ЛКМ
                                    if (e.Button == MouseButtons.Left && e.Clicks == 1) {
                                        supportFigures.Add(new MyRectangle(e.X, e.Y, e.X, e.Y, supportPen));
                                        supportFigures.Add(new MyCircle(e.X, e.Y, e.X, e.Y, supportPen2));
                                        pointsList.Add(e.Location);
                                        currConstructorStage++;
                                        out_result = new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                        break;
                                    }
                                    else return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                case 1:
                                    //Кликнули ЛКМ
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
                                        //supportFigures[supportFigures.Count - 1] = new MyRectangle(pointsList[0].X, pointsList[0].Y, e.X, e.Y, supportPen);
                                        (supportFigures[0] as MyRectangle).Resize(pointsList[0].X, pointsList[0].Y, e.X, e.Y);
                                        (supportFigures[1] as MyCircle).Resize(pointsList[0].X, pointsList[0].Y, e.X, e.Y);
                                        out_result = new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                        break;
                                    }
                                default:
                                    throw new Exception();
                            }
                            break;
                        case BuildingMethod.CircleCenterRadius:
                            switch (currConstructorStage) {
                                case 0:
                                    if (e.Button == MouseButtons.Left && e.Clicks == 1) {
                                        //supportFigures.Add(new Point())
                                    }
                                    break;

                            }
                            out_result = new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                            //throw new NotImplementedException();
                            break;
                        default: throw new Exception("Фигура задана, но не задан вариант построения.");
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

    }
}
