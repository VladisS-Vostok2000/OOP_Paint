using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Configuration;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static OOP_Paint.FiguresEnum;

namespace OOP_Paint {
    public sealed class MainCode {
        public delegate void BuildingMethodHandler(BuildingMethod buildingMethod, EventArgs e);
        public delegate void FigureHandler(Figure figure, EventArgs e);

        #region API
        public event FigureHandler SelectedFigureChanged;
        public event BuildingMethodHandler SelectedBuildingVariantChanged;
        public event ListChangedEventHandler FiguresListChanged;
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

        private readonly BindingList<MyFigure> figures = new BindingList<MyFigure>();
        private readonly List<MyFigure> supportFigures = new List<MyFigure>();
        private readonly List<Point> pointsList = new List<Point>();

        private static readonly Pen supportPen = new Pen(Color.Gray) { Width = 1, DashStyle = DashStyle.Dash };
        private static readonly Pen supportFigurePen = new Pen(Color.White, 1);
        private static readonly Pen figurePen = new Pen(Color.Black);
        private static readonly Pen selectPen = new Pen(Color.White) { Width = 1, DashStyle = DashStyle.Dash };



        //???Лист реализует Binding-логику, которая необходима для привязки и реализации событий в GUI,
        //однако он становится публичным и все его элементы доступны для редактирования снаружи.
        //Странная защита листа: он один фиг передаётся как sender, а использование
        //события ради события выглядит нелепо. Стоит сделать list public?
        //Но зато API красивее смотрится.
        public MainCode() {
            figures.ListChanged += Figures_ListChanged;
        }



        private void Figures_ListChanged(Object sender, ListChangedEventArgs e) {
            FiguresListChanged?.Invoke(sender, e);
        }


        //!!!MainCode#10: реализовать динамический показ сообщений при движении мыши тоже (ConstructorOperationResult += Continius)
        //!!!MainCode#01: Запретить выделение "линией"
        public ConstructorOperationResult AddSoftPoint(Point _point) {
            if (currConstructorStage == 0) {
                return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
            }

            ///currSelectedFigure -> Выбор фигуры построения
            ///currBuildingVariant -> Выбор варианта построения фигуры
            ///currConstructorStage -> Выбор текущей стадии построения (могут отличаться вспомогательные фигуры)
            switch (SelectedFigure) {
                case Figure.None:
                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                case Figure.Select:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.None:
                            switch (currConstructorStage) {
                                case 1:
                                    if (pointsList[0] == _point) {

                                    }
                                    (supportFigures[0] as MyRectangle).Resize(pointsList[0].X, pointsList[0].Y, _point.X, _point.Y);
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedFigure} выбрана, но вариант построения {SelectedBuildingMethod} не реализован.");
                    }
                case Figure.Rectangle:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.RectangleTwoPoints:
                            switch (currConstructorStage) {
                                case 1:
                                    (supportFigures[0] as MyRectangle).Resize(pointsList[0].X, pointsList[0].Y, _point.X, _point.Y);
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedFigure} выбрана, но вариант построения {SelectedBuildingMethod} не реализован.");
                    }
                case Figure.Circle:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.CircleInRectangleByTwoDots:
                            switch (currConstructorStage) {
                                case 1:
                                    (supportFigures[0] as MyRectangle).Resize(pointsList[0].X, pointsList[0].Y, _point.X, _point.Y);
                                    (supportFigures[1] as MyCircle).Resize(pointsList[0].X, pointsList[0].Y, _point.X, _point.Y);
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                default:
                                    throw new Exception();
                            }
                        case BuildingMethod.CircleCenterRadius:
                            switch (currConstructorStage) {
                                case 1:
                                    (supportFigures[0] as MyCut).P2 = _point;
                                    (supportFigures[1] as MyCircle).Radius = MyFigure.FindLength(_point, pointsList[0]);
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedFigure} выбрана, но вариант построения {SelectedBuildingMethod} не реализован.");
                    }
                case Figure.Cut:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.CutTwoPoints:
                            switch (currConstructorStage) {
                                case 1:
                                    (supportFigures[0] as MyCut).P2 = _point;
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedFigure} выбрана, но вариант построения {SelectedBuildingMethod} не реализован.");
                    }
                default: throw new NotImplementedException($"Фигура {SelectedFigure} не реализована.");
            }
        }
        public ConstructorOperationResult SetPoint(Point _point) {
            ///currSelectedFigure -> Выбор фигуры построения
            ///currBuildingVariant -> Выбор варианта построения фигуры
            ///currConstructorStage -> Выбор текущей стадии построения (могут отличаться вспомогательные фигуры)
            switch (SelectedFigure) {
                case Figure.None:
                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                case Figure.Select:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.None:
                            switch (currConstructorStage) {
                                case 0:
                                    supportFigures.Add(new MyRectangle(_point.X, _point.Y, _point.X, _point.Y, selectPen) { IsFill = true, FillColor = Color.FromArgb(50, Color.Blue) });
                                    pointsList.Add(_point);
                                    currConstructorStage++;
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                case 1:
                                    if (pointsList[0] == _point) {
                                        return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                    }

                                    List<MyFigure> myFigures = FindFiguresTouchesRect(pointsList[0], _point);
                                    foreach (var figure in myFigures) {
                                        figure.IsSelected = true;
                                    }

                                    CloseConstructor();
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Finished, "");
                                default:
                                    throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedFigure} выбрана, но не задан вариант построения.");
                    }
                case Figure.Circle:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.CircleInRectangleByTwoDots:
                            switch (currConstructorStage) {
                                case 0:
                                    supportFigures.Add(new MyRectangle(_point.X, _point.Y, _point.X, _point.Y, supportPen));
                                    supportFigures.Add(new MyCircle(_point.X, _point.Y, _point.X, _point.Y, supportFigurePen));
                                    pointsList.Add(_point);
                                    currConstructorStage++;
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                case 1:
                                    if (pointsList[0].X == _point.X || pointsList[0].Y == _point.Y) {
                                        return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                    }

                                    figures.Add(new MyCircle(pointsList[0].X, pointsList[0].Y, _point.X, _point.Y, figurePen));
                                    CloseConstructor();
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Finished, "");
                                default:
                                    throw new Exception();
                            }
                        case BuildingMethod.CircleCenterRadius:
                            switch (currConstructorStage) {
                                case 0:
                                    supportFigures.Add(new MyCut(supportPen, _point, _point));
                                    supportFigures.Add(new MyCircle(supportFigurePen, _point, 0));
                                    pointsList.Add(_point);
                                    currConstructorStage++;
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Continious, $"Центр: ({pointsList[0].X}, {pointsList[0].Y}). Задайте радиус.");
                                case 1:
                                    Single radius = MyFigure.FindLength(_point, pointsList[0]);
                                    if (radius == 0) {
                                        return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                    }

                                    figures.Add(new MyCircle(figurePen, pointsList[0], radius));
                                    CloseConstructor();
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Finished, "");
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedFigure} выбрана, но не задан вариант построения.");
                    }
                case Figure.Rectangle:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.RectangleTwoPoints:
                            switch (currConstructorStage) {
                                case 0:
                                    supportFigures.Add(new MyRectangle(_point.X, _point.Y, _point.X, _point.Y, supportPen));
                                    pointsList.Add(_point);
                                    currConstructorStage++;
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                case 1:
                                    if (pointsList[0].X == _point.X || pointsList[0].Y == _point.Y) {
                                        return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                    }

                                    figures.Add(new MyRectangle(pointsList[0].X, pointsList[0].Y, _point.X, _point.Y, figurePen));
                                    CloseConstructor();
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Finished, "");
                                default:
                                    throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedFigure} выбрана, но не задан вариант построения.");
                    }
                case Figure.Cut:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.CutTwoPoints:
                            switch (currConstructorStage) {
                                case 0:
                                    pointsList.Add(_point);
                                    supportFigures.Add(new MyCut(supportFigurePen, pointsList[0], pointsList[0]));
                                    currConstructorStage++;
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                case 1:
                                    if (pointsList[0] == _point) {
                                        return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
                                    }

                                    figures.Add(new MyCut(figurePen, pointsList[0], _point));
                                    CloseConstructor();
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Finished, "");
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedFigure} выбрана, но не задан вариант построения.");
                    }
                default: throw new NotImplementedException($"Фигура {SelectedFigure} не реализована.");
            }
        }
        //!!!MainCode#02: рассмотреть возможность открыть лист Figures
        public void SelectFigure(Int32 _id) {
            for (Int32 i = 0; i < figures.Count; i++) {
                if (figures[i].Id == _id) {
                    figures[i].IsSelected = true;
                    break;
                }
            }
        }
        public void UnselectFigure(Int32 _id) {
            for (Int32 i = 0; i < figures.Count; i++) {
                if (figures[i].Id == _id) {
                    figures[i].IsSelected = false;
                    break;
                }
            }
        }
        public Int32 GetFiguresCount() {
            return figures.Count;
        }


        //!!!MainCode#45: добавить выделение прямоугольника и круга
        private List<MyFigure> FindFiguresTouchesRect(Point _p1, Point _p2) {
            var out_list = new List<MyFigure>();

            //Все грани выделяющего прямоугольника
            Point[,] selectRectLinePoints = {
                { _p1, new Point(_p2.X, _p1.Y) },
                { _p2, new Point(_p2.X, _p1.Y) },
                { _p2, new Point(_p1.X, _p2.Y) },
                { _p1, new Point(_p1.X, _p2.Y) }
            };

            foreach (var figure in figures) {
                if (figure is MyCut) {
                    var myCut = figure as MyCut;
                    for (Int32 i = 0; i < selectRectLinePoints.GetLength(0); i++) {
                        Boolean isParallel = IsParallel(myCut.P1, myCut.P2, selectRectLinePoints[i, 0], selectRectLinePoints[i, 1]);
                        if (isParallel) {
                            continue;
                        }

                        PointF crossPoint = FindCross(myCut.P1, myCut.P2, selectRectLinePoints[i, 0], selectRectLinePoints[i, 1]);
                        //Отрезки пересекаются, если точка пересечения прямых, чрез них проходящих, принадлежит им обоим.
                        Boolean isTouches = CheckIsPointInCut(myCut.P1, myCut.P2, crossPoint) && CheckIsPointInCut(selectRectLinePoints[i, 0], selectRectLinePoints[i, 1], crossPoint);
                        if (isTouches) {
                            out_list.Add(figure);
                            break;
                        }
                    }
                }
            }

            return out_list;
        }
        private PointF FindCross(PointF _p1, PointF _p2, PointF _p3, PointF _p4) {
            //параллельны/что-то совпадает
            if (Math.Abs((_p1.X - _p2.X) * (_p3.Y - _p4.Y)) == Math.Abs((_p1.Y - _p2.Y) * (_p3.X - _p4.X))) {
                throw new Exception();
            }

            Single y = (Single)((_p4.X * _p3.Y - _p3.X * _p4.Y) * (_p1.Y - _p2.Y) - (_p3.Y - _p4.Y) * (_p2.X * _p1.Y - _p1.X * _p2.Y)) / ((_p3.Y - _p4.Y) * (_p1.X - _p2.X) + (_p4.X - _p3.X) * (_p1.Y - _p2.Y));
            Single x;
            if (_p1.Y - _p2.Y == 0) {
                x = (Single)(y * (_p3.X - _p4.X) + (_p4.X * _p3.Y - _p3.X * _p4.Y)) / (_p3.Y - _p4.Y);
            }
            else {
                x = (Single)(y * (_p1.X - _p2.X) + (_p2.X * _p1.Y - _p1.X * _p2.Y)) / (_p1.Y - _p2.Y);
            }

            return new PointF(x, y);
        }
        private Boolean IsParallel(PointF _p1, PointF _p2, PointF _p3, PointF _p4) {
            if (Math.Abs((_p1.X - _p2.X) * (_p3.Y - _p4.Y)) == Math.Abs((_p1.Y - _p2.Y) * (_p3.X - _p4.X))) {
                return true;
            }

            return false;
        }
        private Boolean CheckIsPointInCut(PointF _cutP1, PointF _cutP2, PointF _p) {
            //Вертикальная прямая
            if (_cutP1.X == _cutP2.X) {
                return _p.Y >= Math.Min(_cutP1.Y, _cutP2.Y) && _p.Y <= Math.Max(_cutP1.Y, _cutP2.Y);
            }
            else {
                return _p.X <= Math.Max(_cutP1.X, _cutP2.X) && _p.X >= Math.Min(_cutP1.X, _cutP2.X);
            }
        }


        private List<Point> FindFiguresNearPoint(PointF _target, Single _interval) {
            foreach(var figure in figures) {
                if (figure is MyCut) {
                    var cut = figure as MyCut;
                    var area = FindCutArea(cut.P1, cut.P2, _interval);
                }
            }
        }
        /// <summary>
        /// Возвращает прямоугольник, образованный перпендикулярным сдвигом отрезка на интервал
        /// в одну и в другую сторону
        /// </summary>
        private PointF[] FindCutArea(PointF _p1, PointF _p2, Single _interval) {
            Single cutLength = MyFigure.FindLength(_p1, _p2);
            //double z = 1 - Math.Abs(_p1.Y - _p2.Y) / cutLength;
            //double a = 1 - Math.Abs(_p1.X - _p2.X) / cutLength;
            Single z = Convert.ToSingle((_p1.X - _p2.X) * _interval / cutLength);
            Single a = Convert.ToSingle((_p2.Y - _p1.Y) * _interval / cutLength);
            PointF[] b = { new PointF(_p1.X - a, _p1.Y + z), new PointF(_p1.X + a, _p1.Y) };
        }

        private void CloseConstructor() {
            //CurrBuildingVariant = BuildingVariants.None;
            currConstructorStage = 0;
            supportFigures.Clear();
            pointsList.Clear();
        }


        public void DrawFigures(Graphics _screen) {
            _screen.Clear(Color.FromArgb(250, 64, 64, 64));
            foreach (var figure in figures) {
                figure.Draw(_screen);
            }
            foreach (var figure in supportFigures) {
                figure.Draw(_screen);
            }
        }

    }
}
//MainCode#84: сделать static многие из методов
//[Closed]: нет причин для этого, метод примет слишком много параметров. Класс по определению подходит здесь.