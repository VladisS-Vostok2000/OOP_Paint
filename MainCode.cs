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

//!!!!MainCode#20: переименовать перечисления-названия фигур в инструменты
namespace OOP_Paint {
    public sealed class MainCode {
        public delegate void BuildingMethodHandler(BuildingMethod buildingMethod, EventArgs e);
        public delegate void FigureHandler(Figure figure, EventArgs e);

        #region API
        public event FigureHandler SelectedFigureChanged;
        public event BuildingMethodHandler SelectedBuildingVariantChanged;
        //!!!!Сюда нужно засунуть MyFiguresContainer
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

        //private readonly BindingList<MyFigure> figures = new BindingList<MyFigure>();
        public readonly MyFiguresContainer figuresContainer = new MyFiguresContainer();
        private readonly List<MyFigure> supportFigures = new List<MyFigure>();
        private readonly List<Point> pointsList = new List<Point>();

        private static readonly Pen supportPen = new Pen(Color.Gray) { Width = 1, DashStyle = DashStyle.Dash };
        private static readonly Pen supportFigurePen = new Pen(Color.White, 1);
        private static readonly Pen figurePen = new Pen(Color.Black);
        private static readonly Pen selectPen = new Pen(Color.White) { Width = 1, DashStyle = DashStyle.Dash };

        private static readonly Pen SnapPen = new Pen(Color.Green, 2);
        private static readonly MyRectangle SnapPoint = new MyRectangle(0, 0, 5, 5, SnapPen);



        //???Лист реализует Binding-логику, которая необходима для привязки и реализации событий в GUI,
        //однако он становится публичным и все его элементы доступны для редактирования снаружи.
        //Странная защита листа: он один фиг передаётся как sender, а использование
        //события ради события выглядит нелепо. Стоит сделать list public?
        //Но зато API красивее смотрится.
        public MainCode() {
            figuresContainer.ListChanged += Figures_ListChanged;
        }



        private void Figures_ListChanged(Object sender, ListChangedEventArgs e) {
            FiguresListChanged?.Invoke(sender, e);
        }


        //!!!MainCode#10: реализовать динамический показ сообщений при движении мыши тоже (ConstructorOperationResult += Continius)
        //!!!MainCode#01: Запретить выделение "линией"
        public ConstructorOperationResult AddSoftPoint(in PointF _point) {
            if (currConstructorStage == 0 && SelectedFigure != Figure.None) {
                return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.None, "");
            }

            ///currSelectedFigure -> Выбор фигуры построения
            ///currBuildingVariant -> Выбор варианта построения фигуры
            ///currConstructorStage -> Выбор текущей стадии построения (могут отличаться вспомогательные фигуры)
            switch (SelectedFigure) {
                case Figure.None:
                    foreach (var figure in figuresContainer.FiguresList) {
                        figure.IsHightLighed = false;
                    }

                    List<int> indexes = FindFiguresNearPoint(_point);
                    for (int i = 0; i < indexes.Count; i++) {
                        figuresContainer[indexes[i]].IsHightLighed = true;
                    }

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

                                    figuresContainer.Add(new MyCircle(pointsList[0].X, pointsList[0].Y, _point.X, _point.Y, figurePen));
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

                                    figuresContainer.Add(new MyCircle(figurePen, pointsList[0], radius));
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

                                    figuresContainer.Add(new MyRectangle(pointsList[0].X, pointsList[0].Y, _point.X, _point.Y, figurePen));
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

                                    figuresContainer.Add(new MyCut(figurePen, pointsList[0], _point));
                                    CloseConstructor();
                                    return new ConstructorOperationResult(ConstructorOperationResult.OperationStatus.Finished, "");
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedFigure} выбрана, но не задан вариант построения.");
                    }
                default: throw new NotImplementedException($"Фигура {SelectedFigure} не реализована.");
            }
        }
        //!!!MainCode#02: добавить класс-контейнер Figures
        public void SelectFigure(Int32 _id) {
            for (Int32 i = 0; i < figuresContainer.FiguresList.Count; i++) {
                if (figuresContainer[i].Id == _id) {
                    figuresContainer[i].IsSelected = true;
                    break;
                }
            }
        }
        public void UnselectFigure(Int32 _id) {
            for (Int32 i = 0; i < figuresContainer.FiguresList.Count; i++) {
                if (figuresContainer[i].Id == _id) {
                    figuresContainer[i].IsSelected = false;
                    break;
                }
            }
        }
        public Int32 GetFiguresCount() {
            return figuresContainer.FiguresList.Count;
        }


        /// <summary> Находит координаты ближайшей к точке вершины фигуры для непустого списка фигур </summary>
        public PointF FindNearestVertex(PointF _target) => FindNearestVertex(new List<MyFigure>(figuresContainer.FiguresList), _target);
        private PointF FindNearestVertex(List<MyFigure> figures, PointF _target) {
            if (figures.Count == 0) {
                throw new Exception();
            }

            float minDistance = float.MaxValue;
            PointF out_vertex = new PointF(0, 0);
            bool isOk = false;
            foreach (var figure in figures) {
                foreach (var vetrex in figure.Vertexes) {
                    float distance = MyFigure.FindLength(vetrex, _target);
                    if (distance <= minDistance) {
                        isOk = true;
                        minDistance = distance;
                        out_vertex = vetrex;
                    }
                }
            }

            if (isOk) {
                return out_vertex;
            }
            else {
                throw new Exception();
            }
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

            foreach (var figure in figuresContainer.) {
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
            bool isParallel = IsParallel(_p1, _p2, _p3, _p4);
            if (isParallel) {
                throw new Exception();
            }

            Single y = ((_p4.X * _p3.Y - _p3.X * _p4.Y) * (_p1.Y - _p2.Y) - (_p3.Y - _p4.Y) * (_p2.X * _p1.Y - _p1.X * _p2.Y)) / ((_p3.Y - _p4.Y) * (_p1.X - _p2.X) + (_p4.X - _p3.X) * (_p1.Y - _p2.Y));
            Single x;
            if (_p1.Y - _p2.Y == 0) {
                x = (y * (_p3.X - _p4.X) + (_p4.X * _p3.Y - _p3.X * _p4.Y)) / (_p3.Y - _p4.Y);
            }
            else {
                x = (y * (_p1.X - _p2.X) + (_p2.X * _p1.Y - _p1.X * _p2.Y)) / (_p1.Y - _p2.Y);
            }

            return new PointF(x, y);
        }

        /// <summary> Определяет параллельность/коллинеарность отрезков </summary>
        private Boolean IsParallel(PointF _p1, PointF _p2, PointF _p3, PointF _p4) {
            //Если отношения смещений на клетку х и у двух отрезков по модулю равны, то они параллельны (k коэфф один)
            //И по свойству пропорции:
            if (Math.Abs((_p1.X - _p2.X) * (_p3.Y - _p4.Y)) == Math.Abs((_p1.Y - _p2.Y) * (_p3.X - _p4.X))) {
                return true;
            }

            return false;
        }
        private Boolean CheckIsPointInCut(PointF _cutP1, PointF _cutP2, PointF _target) {
            bool isInLine = CheckIsPointInLine(_cutP1, _cutP2, _target);
            if (!isInLine) {
                return false;
            }

            return _target.X <= Math.Max(_cutP1.X, _cutP2.X) && _target.X >= Math.Min(_cutP1.X, _cutP2.X);
        }
        private Boolean CheckIsPointInLine(PointF _cutP1, PointF _cutP2, PointF _target) {
            #region Человеческий вид
            //float a = _p2.X - _p1.X;
            //float b = _p2.Y - _p1.Y;
            //float c = _target.X - _p1.X;
            //float d = _target.X - _p1.X;

            //int e = Math.Sign(a * d - b * c);
            //if (e != 0) {
            //    return false;
            //}
            #endregion

            return ((_cutP2.X - _cutP1.X) * (_target.Y - _cutP1.Y) - (_cutP2.Y - _cutP1.Y) * (_target.X - _cutP1.X)) == 0;
        }


        /// <summary>
        /// Находит все фигуры на заданном от цели расстоянии
        /// </summary>
        /// <returns>
        /// Целочисленный массив с индексами figures
        /// </returns>
        private List<int> FindFiguresNearPoint(PointF _target, Single _interval = 5) {
            var out_list = new List<int>();
            for (int i = 0; i < figuresContainer.FiguresList.Count; i++) {
                if (figuresContainer[i] is MyCut) {
                    var cut = figuresContainer[i] as MyCut;
                    PointF[] area = FindCutArea(cut.P1, cut.P2, _interval);
                    bool isInArea = IsPointInArea(_target, area);
                    if (isInArea) {
                        out_list.Add(i);
                    }
                }
            }

            return out_list;
        }

        /// <summary>
        /// Возвращает последовательные вершины прямоугольника, образованного "перпендикулярным" сдвигом отрезка на интервал
        /// в обе стороны.
        /// </summary>
        private PointF[] FindCutArea(PointF _p1, PointF _p2, Single _interval) {
            Single cutLength = MyFigure.FindLength(_p1, _p2);
            Single z = (_p2.X - _p1.X) * _interval / cutLength;
            Single a = (_p2.Y - _p1.Y) * _interval / cutLength;
            PointF[] rect = {
                new PointF(_p1.X - a, _p1.Y + z),
                new PointF(_p1.X + a, _p1.Y - z),
                new PointF(_p2.X + a, _p2.Y - z),
                new PointF(_p2.X - a, _p2.Y + z),
            };

            return rect;
        }

        /// <summary>
        /// Возвращает false, если точка лежит за пределами области
        /// </summary>
        /// <param name="_area">Замкнутый выпуклый полигон с последовательными вершинами</param>
        private Boolean IsPointInArea(PointF _point, PointF[] _area) {
            if (_area.Length < 2) {
                throw new Exception();
            }
            //Такое уже включает в себя проверка
            foreach (var apex in _area) {
                if (apex == _point) {
                    return true;
                }
            }

            //Здесь можно проще: как-то через бинарный поиск
            //По-моему, тут цикл на 2 можно увеличить и подключить процентик
            int last = _area.Length - 1;
            int prelast = _area.Length - 2;
            float a = _area[last].X - _area[prelast].X;
            float b = _area[last].Y - _area[prelast].Y;

            float c = _area[0].X - _area[prelast].X;
            float d = _area[0].Y - _area[prelast].Y;

            float e = _point.X - _area[prelast].X;
            float f = _point.Y - _area[prelast].Y;

            bool isOk = Math.Sign(a * d - b * c) == Math.Sign(a * f - b * e);
            if (!isOk) {
                return false;
            }

            a = _area[0].X - _area[last].X;
            b = _area[0].Y - _area[last].Y;

            c = _area[1].X - _area[last].X;
            d = _area[1].Y - _area[last].Y;

            e = _point.X - _area[last].X;
            f = _point.Y - _area[last].Y;

            isOk = Math.Sign(a * d - b * c) == Math.Sign(a * f - b * e);
            if (!isOk) {
                return false;
            }

            for (int i = 0; i < _area.Length - 2; i++) {
                //Основная сторона
                a = _area[i + 1].X - _area[i].X;
                b = _area[i + 1].Y - _area[i].Y;
                //Внутренняя сторона
                c = _area[i + 2].X - _area[i].X;
                d = _area[i + 2].Y - _area[i].Y;
                //Вектор от стороны к точке
                e = _point.X - _area[i].X;
                f = _point.Y - _area[i].Y;

                //Вращение стороны к следующей стороне (всегда вовнутрь) должно быть равно этому же вращению стороны к вектору
                isOk = Math.Sign(a * d - b * c) == Math.Sign(a * f - b * e);
                if (!isOk) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Находит, выше (1) точка прямой, лежит на ней (0) или ниже (-1). Если прямая вертикальна или координаты её точек идентичны - exeption.
        /// </summary>
        /// <param name="_p1">
        /// Координаты первой точки прямой
        /// </param>
        /// <param name="_p2">
        /// Коордианы второй точки прямой</param>
        /// <param name="_p3">
        /// Координаты точки
        /// </param>
        private Int32 IsPointOverLine(in PointF _p1, in PointF _p2, in PointF _p3) {
            #region Неоптимальный способ
            //return _point.Y > ((_point.X - _p1.X) * (_p2.Y - _p1.Y) + _point.Y * (_p2.X - _p1.X))/(_p2.X - _p1.X);
            #endregion

            //Если точки совпадают или прямая вертикальна
            if (_p1.X == _p2.X) {
                throw new Exception();
            }

            #region Человеческий вид
            //Single a = _p2.X - _p1.X;
            //Single b = -_p2.Y - -_p1.Y;
            //Single c = _p3.X - _p1.X;
            //Single d = -_p3.Y - -_p1.Y;

            //Int32 e = Math.Sign(a * d - b * c);

            //if (a > 0) {
            //    return e;
            //}
            //else {
            //    return -e;
            //}
            #endregion

            Single a = _p2.X - _p1.X;
            if (a > 0) {
                return Math.Sign((-a * (_p3.Y - _p1.Y)) + (_p2.Y - _p1.Y) * (_p3.X - _p1.X));
            }
            else {
                return -Math.Sign((-a * (_p3.Y - _p1.Y)) + (_p2.Y - _p1.Y) * (_p3.X - _p1.X));
            }
        }


        private void CloseConstructor() {
            //CurrBuildingVariant = BuildingVariants.None;
            currConstructorStage = 0;
            supportFigures.Clear();
            pointsList.Clear();
        }


        public void DrawFigures(Graphics _screen) {
            _screen.Clear(Color.FromArgb(250, 64, 64, 64));
            foreach (var figure in figuresContainer.FiguresList) {
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