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
        public delegate void ConstructorOperationStatusHandler(ConstructorOperationStatus ConstructorOperationStatus, EventArgs e);

        #region API
        public event FigureHandler SelectedToolChanged;
        public event BuildingMethodHandler SelectedBuildingVariantChanged;
        public event EventHandler FiguresListChanged;
        public event ConstructorOperationStatusHandler ConstructorOperationStatusChanged;
        #endregion

        private Figure selectedTool;
        public Figure SelectedTool {
            set {
                if (selectedTool != value) {
                    CloseConstructor();
                    SelectedToolChanged?.Invoke(value, EventArgs.Empty);
                    selectedTool = value;

                    SelectedBuildingMethod = ReturnPossibleBuildingVariants(SelectedTool)[0];
                }
            }
            get => selectedTool;
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
        private ConstructorOperationStatus constructorOperationStatus;
        public ConstructorOperationStatus ConstructorOperationStatus {
            set {
                if (constructorOperationStatus != value) {
                    constructorOperationStatus = value;
                    ConstructorOperationStatusChanged?.Invoke(value, EventArgs.Empty);
                }
            }
            get => constructorOperationStatus;
        }
        private Int32 currConstructorStage = 0;


        public readonly MyListContainer<MyFigure> figuresContainer = new MyListContainer<MyFigure>();
        private readonly List<MyFigure> supportFigures = new List<MyFigure>();
        private readonly List<Point> pointsList = new List<Point>();

        private static readonly Pen supportPen = new Pen(Color.Gray) { Width = 1, DashStyle = DashStyle.Dash };
        private static readonly Pen supportFigurePen = new Pen(Color.White, 1);
        private static readonly Pen figurePen = new Pen(Color.Black);
        private static readonly Pen selectPen = new Pen(Color.White) { Width = 1, DashStyle = DashStyle.Dash };

        private static readonly Pen snapPen = new Pen(Color.Green, 2);
        private static readonly MyRectangle snapPoint = new MyRectangle(0, 0, 6, 6, snapPen) { IsHide = true };
        private static readonly Pen PolarPen = new Pen(Color.Lime, 1) { DashStyle = DashStyle.Dash };
        private static readonly MyCut polarLine = new MyCut(PolarPen, new PointF(0, 0), new PointF(0, 0));



        public MainCode() {
            figuresContainer.ContainerChanged += FiguresContainer_ContainerChanged;
        }

        private void FiguresContainer_ContainerChanged(object sender, EventArgs e) => FiguresListChanged?.Invoke(sender, e);



        //!!!MainCode#10: реализовать динамический показ сообщений при движении мыши тоже (ConstructorOperationStatus += Continius)
        //!!!MainCode#01: Запретить выделение "линией"
        public void AddSoftPoint(in PointF point) {
            if (currConstructorStage == 0 && SelectedTool != Figure.None) {
                return;
            }

            ///currSelectedFigure -> Выбор фигуры построения
            ///currBuildingVariant -> Выбор варианта построения фигуры
            ///currConstructorStage -> Выбор текущей стадии построения (могут отличаться вспомогательные фигуры)
            switch (SelectedTool) {
                case Figure.None:
                    foreach (var figure in figuresContainer) {
                        figure.IsHightLighed = false;
                    }

                    List<Int32> indexes = FindFiguresNearPoint(point);
                    for (Int32 i = 0; i < indexes.Count; i++) {
                        figuresContainer[indexes[i]].IsHightLighed = true;
                    }
                    return;
                case Figure.Select:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.None:
                            switch (currConstructorStage) {
                                case 1:
                                    if (pointsList[0] == point) {

                                    }
                                    (supportFigures[0] as MyRectangle).Resize(pointsList[0].X, pointsList[0].Y, point.X, point.Y);
                                    return;
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedTool} выбрана, но вариант построения {SelectedBuildingMethod} не реализован.");
                    }
                case Figure.Rectangle:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.RectangleTwoPoints:
                            switch (currConstructorStage) {
                                case 1:
                                    (supportFigures[0] as MyRectangle).Resize(pointsList[0].X, pointsList[0].Y, point.X, point.Y);
                                    return;
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedTool} выбрана, но вариант построения {SelectedBuildingMethod} не реализован.");
                    }
                case Figure.Circle:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.CircleInRectangleByTwoDots:
                            switch (currConstructorStage) {
                                case 1:
                                    (supportFigures[0] as MyRectangle).Resize(pointsList[0].X, pointsList[0].Y, point.X, point.Y);
                                    (supportFigures[1] as MyCircle).Resize(pointsList[0].X, pointsList[0].Y, point.X, point.Y);
                                    return;
                                default:
                                    throw new Exception();
                            }
                        case BuildingMethod.CircleCenterRadius:
                            switch (currConstructorStage) {
                                case 1:
                                    (supportFigures[0] as MyCut).P2 = point;
                                    (supportFigures[1] as MyCircle).Radius = MyFigure.FindLength(point, pointsList[0]);
                                    return;
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedTool} выбрана, но вариант построения {SelectedBuildingMethod} не реализован.");
                    }
                case Figure.Cut:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.CutTwoPoints:
                            switch (currConstructorStage) {
                                case 1:
                                    (supportFigures[0] as MyCut).P2 = point;
                                    return;
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedTool} выбрана, но вариант построения {SelectedBuildingMethod} не реализован.");
                    }
                default: throw new NotImplementedException($"Фигура {SelectedTool} не реализована.");
            }
        }
        public void SetPoint(Point point) {
            //currSelectedFigure -> Выбор фигуры построения
            //currBuildingVariant -> Выбор варианта построения фигуры
            //currConstructorStage -> Выбор текущей стадии построения (могут отличаться вспомогательные фигуры)
            switch (SelectedTool) {
                case Figure.None:
                    return;
                case Figure.Select:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.None:
                            switch (currConstructorStage) {
                                case 0:
                                    supportFigures.Add(new MyRectangle(point.X, point.Y, point.X, point.Y, selectPen) { IsFill = true, FillColor = Color.FromArgb(50, Color.Blue) });
                                    pointsList.Add(point);
                                    currConstructorStage++;
                                    ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                    return;
                                case 1:
                                    if (pointsList[0] == point) {
                                        return;
                                    }

                                    List<MyFigure> myFigures = FindFiguresTouchesRect(pointsList[0], point);
                                    foreach (var figure in myFigures) {
                                        figure.IsSelected = true;
                                    }

                                    CloseConstructor();
                                    return;
                                default:
                                    throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedTool} выбрана, но не задан вариант построения.");
                    }
                case Figure.Circle:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.CircleInRectangleByTwoDots:
                            switch (currConstructorStage) {
                                case 0:
                                    supportFigures.Add(new MyRectangle(point.X, point.Y, point.X, point.Y, supportPen));
                                    supportFigures.Add(new MyCircle(point.X, point.Y, point.X, point.Y, supportFigurePen));
                                    pointsList.Add(point);
                                    currConstructorStage++;
                                    ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                    return;
                                case 1:
                                    if (pointsList[0].X == point.X || pointsList[0].Y == point.Y) {
                                        return;
                                    }

                                    figuresContainer.Add(new MyCircle(pointsList[0].X, pointsList[0].Y, point.X, point.Y, figurePen));
                                    CloseConstructor();
                                    return;
                                default:
                                    throw new Exception();
                            }
                        case BuildingMethod.CircleCenterRadius:
                            switch (currConstructorStage) {
                                case 0:
                                    supportFigures.Add(new MyCut(supportPen, point, point));
                                    supportFigures.Add(new MyCircle(supportFigurePen, point, 0));
                                    pointsList.Add(point);
                                    currConstructorStage++;
                                    ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Continious, $"Центр: ({pointsList[0].X}, {pointsList[0].Y}). Задайте радиус.");
                                    return;
                                case 1:
                                    Single radius = MyFigure.FindLength(point, pointsList[0]);
                                    if (radius == 0) {
                                        return;
                                    }

                                    figuresContainer.Add(new MyCircle(figurePen, pointsList[0], radius));
                                    CloseConstructor();
                                    ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Finished, "");
                                    return;
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedTool} выбрана, но не задан вариант построения.");
                    }
                case Figure.Rectangle:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.RectangleTwoPoints:
                            switch (currConstructorStage) {
                                case 0:
                                    supportFigures.Add(new MyRectangle(point.X, point.Y, point.X, point.Y, supportPen));
                                    pointsList.Add(point);
                                    currConstructorStage++;
                                    ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                    return;
                                case 1:
                                    if (pointsList[0].X == point.X || pointsList[0].Y == point.Y) {
                                        return;
                                    }

                                    figuresContainer.Add(new MyRectangle(pointsList[0].X, pointsList[0].Y, point.X, point.Y, figurePen));
                                    CloseConstructor();
                                    return;
                                default:
                                    throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedTool} выбрана, но не задан вариант построения.");
                    }
                case Figure.Cut:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.CutTwoPoints:
                            switch (currConstructorStage) {
                                case 0:
                                    pointsList.Add(point);
                                    supportFigures.Add(new MyCut(supportFigurePen, pointsList[0], pointsList[0]));
                                    currConstructorStage++;
                                    ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                    return;
                                case 1:
                                    if (pointsList[0] == point) {
                                        return;
                                    }

                                    figuresContainer.Add(new MyCut(figurePen, pointsList[0], point));
                                    CloseConstructor();
                                    ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Finished, "");
                                    return;
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedTool} выбрана, но не задан вариант построения.");
                    }
                default: throw new NotImplementedException($"Фигура {SelectedTool} не реализована.");
            }
        }



        //!!!MainCode#02: добавить класс-контейнер Figures
        public void SelectFigure(Int32 id) {
            for (Int32 i = 0; i < figuresContainer.Count; i++) {
                if (figuresContainer[i].Id == id) {
                    figuresContainer[i].IsSelected = true;
                    break;
                }
            }
        }
        public void UnselectFigure(Int32 id) {
            for (Int32 i = 0; i < figuresContainer.Count; i++) {
                if (figuresContainer[i].Id == id) {
                    figuresContainer[i].IsSelected = false;
                    break;
                }
            }
        }
        public Int32 GetFiguresCount() {
            return figuresContainer.Count;
        }


        public void AddSnapPoint(Point location) {
            snapPoint.Location = new Point(location.X - 3, location.Y - 3);
            snapPoint.IsHide = false;
        }
        public void RemoveSnapPoint() {
            snapPoint.IsHide = true;
        }

        /// <summary> Выстраивает полярную линию с последней точки построения в сторону данной. </summary>
        public void AddPolarLine(in PointF p1) {
            if (currConstructorStage == 0 || pointsList.Count == 0) {
                throw new Exception();
            }

            polarLine.P1 = pointsList[pointsList.Count - 1];
            checked {
                //polarLine.P2 = new PointF(p2.X + 1000 * Math.Sign(p2.X), p2.Y + 1000 * Math.Sign(p2.Y));
            }
        }
        /// <summary> По двум точкам находит ближайшую горизонтальную, вертикальную или диагональную прямую, проходящие через первую точку. </summary>
        /// <returns> Точка, лежащая на ближайшей прямой с направлением второй точки. </returns>
        public PointF ChoosePolarLine(in PointF p1, in PointF p2) {
            Single a = p2.X - p1.X;
            Single b = p2.Y - p1.Y;
            Single k = b / a;

            //Вторая точка уже лежит на прямой
            if (k == 0 || Math.Abs(k) == 1 || Single.IsInfinity(k)) {
                return new PointF(p1.X + a, p1.Y + b);
            }

            var p3 = new PointF(a, b);
            Single c;
            PointF p4;
            //Выясняем, с какой прямой будем сравнивать
            if (Math.Abs(k) < 1) {
                //С горизонтальной
                p4 = new PointF(a, 0);
                c = p3.X;
            }
            else {
                //С вертикальной
                p4 = new PointF(0, b);
                c = p3.Y;
            }

            Single sqrt2 = (Single)Math.Sqrt(2);
            var p5 = new PointF(Math.Sign(a) * Math.Abs(c) / sqrt2, Math.Sign(b) * Math.Abs(c) / sqrt2);
            
            Boolean isDiagonalCloser = Math.Abs(a * p4.X + b * p4.Y) <= a * p5.X + b * p5.Y;
            if (isDiagonalCloser) {
                return new PointF(p1.X + p5.X, p1.Y + p5.Y);
            }
            else {
                return new PointF(p1.X + p4.X, p1.Y + p4.Y);
            }

        }

        /// <summary> Находит координаты ближайшей к точке вершины фигуры для непустого списка фигур. </summary>
        public PointF FindNearestVertex(PointF target) => FindNearestVertex(figuresContainer, target);
        private PointF FindNearestVertex(MyListContainer<MyFigure> figures, PointF target) {
            if (figures.Count == 0) {
                throw new Exception();
            }

            Single minDistance = Single.MaxValue;
            PointF outvertex = new PointF(0, 0);
            Boolean isOk = false;
            foreach (var figure in figures) {
                foreach (var vetrex in figure.Vertexes) {
                    Single distance = MyFigure.FindLength(vetrex, target);
                    if (distance <= minDistance) {
                        isOk = true;
                        minDistance = distance;
                        outvertex = vetrex;
                    }
                }
            }

            if (isOk) {
                return outvertex;
            }
            else {
                throw new Exception();
            }
        }


        //!!!MainCode#45: добавить выделение прямоугольника и круга
        private List<MyFigure> FindFiguresTouchesRect(Point p1, Point p2) {
            var outlist = new List<MyFigure>();

            //Все грани выделяющего прямоугольника
            Point[,] selectRectLinePoints = {
                { p1, new Point(p2.X, p1.Y) },
                { p2, new Point(p2.X, p1.Y) },
                { p2, new Point(p1.X, p2.Y) },
                { p1, new Point(p1.X, p2.Y) }
            };

            foreach (var figure in figuresContainer) {
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
                            outlist.Add(figure);
                            break;
                        }
                    }
                }
            }

            return outlist;
        }
        private PointF FindCross(PointF p1, PointF p2, PointF p3, PointF p4) {
            //параллельны/что-то совпадает
            Boolean isParallel = IsParallel(p1, p2, p3, p4);
            if (isParallel) {
                throw new Exception();
            }

            Single y = ((p4.X * p3.Y - p3.X * p4.Y) * (p1.Y - p2.Y) - (p3.Y - p4.Y) * (p2.X * p1.Y - p1.X * p2.Y)) / ((p3.Y - p4.Y) * (p1.X - p2.X) + (p4.X - p3.X) * (p1.Y - p2.Y));
            Single x;
            if (p1.Y - p2.Y == 0) {
                x = (y * (p3.X - p4.X) + (p4.X * p3.Y - p3.X * p4.Y)) / (p3.Y - p4.Y);
            }
            else {
                x = (y * (p1.X - p2.X) + (p2.X * p1.Y - p1.X * p2.Y)) / (p1.Y - p2.Y);
            }

            return new PointF(x, y);
        }

        /// <summary> Определяет параллельность/коллинеарность отрезков </summary>
        private Boolean IsParallel(PointF p1, PointF p2, PointF p3, PointF p4) {
            //Если отношения смещений на клетку х и у двух отрезков по модулю равны, то они параллельны (k коэфф один)
            //И по свойству пропорции:
            if (Math.Abs((p1.X - p2.X) * (p3.Y - p4.Y)) == Math.Abs((p1.Y - p2.Y) * (p3.X - p4.X))) {
                return true;
            }

            return false;
        }
        private Boolean CheckIsPointInCut(PointF cutP1, PointF cutP2, PointF target) {
            Boolean isInLine = CheckIsPointInLine(cutP1, cutP2, target);
            if (!isInLine) {
                return false;
            }

            return target.X <= Math.Max(cutP1.X, cutP2.X) && target.X >= Math.Min(cutP1.X, cutP2.X);
        }
        private Boolean CheckIsPointInLine(PointF cutP1, PointF cutP2, PointF target) {
            #region Человеческий вид
            //float a = p2.X - p1.X;
            //float b = p2.Y - p1.Y;
            //float c = target.X - p1.X;
            //float d = target.X - p1.X;

            //int e = Math.Sign(a * d - b * c);
            //if (e != 0) {
            //    return false;
            //}
            #endregion

            return ((cutP2.X - cutP1.X) * (target.Y - cutP1.Y) - (cutP2.Y - cutP1.Y) * (target.X - cutP1.X)) == 0;
        }


        /// <summary>
        /// Находит все фигуры на заданном от цели расстоянии
        /// </summary>
        /// <returns>
        /// Целочисленный массив с индексами figures
        /// </returns>
        private List<Int32> FindFiguresNearPoint(PointF target, Single interval = 5) {
            var outlist = new List<Int32>();
            for (Int32 i = 0; i < figuresContainer.Count; i++) {
                if (figuresContainer[i] is MyCut) {
                    var cut = figuresContainer[i] as MyCut;
                    PointF[] area = FindCutArea(cut.P1, cut.P2, interval);
                    Boolean isInArea = IsPointInArea(target, area);
                    if (isInArea) {
                        outlist.Add(i);
                    }
                }
            }

            return outlist;
        }

        /// <summary>
        /// Возвращает последовательные вершины прямоугольника, образованного "перпендикулярным" сдвигом отрезка на интервал
        /// в обе стороны.
        /// </summary>
        private PointF[] FindCutArea(PointF p1, PointF p2, Single interval) {
            Single cutLength = MyFigure.FindLength(p1, p2);
            Single z = (p2.X - p1.X) * interval / cutLength;
            Single a = (p2.Y - p1.Y) * interval / cutLength;
            PointF[] rect = {
                new PointF(p1.X - a, p1.Y + z),
                new PointF(p1.X + a, p1.Y - z),
                new PointF(p2.X + a, p2.Y - z),
                new PointF(p2.X - a, p2.Y + z),
            };

            return rect;
        }

        /// <summary>
        /// Возвращает false, если точка лежит за пределами области
        /// </summary>
        /// <param name="area">Замкнутый выпуклый полигон с последовательными вершинами</param>
        private Boolean IsPointInArea(PointF point, PointF[] area) {
            if (area.Length < 2) {
                throw new Exception();
            }
            //Такое уже включает в себя проверка
            foreach (var apex in area) {
                if (apex == point) {
                    return true;
                }
            }

            //Здесь можно проще: как-то через бинарный поиск
            //По-моему, тут цикл на 2 можно увеличить и подключить процентик
            Int32 last = area.Length - 1;
            Int32 prelast = area.Length - 2;
            Single a = area[last].X - area[prelast].X;
            Single b = area[last].Y - area[prelast].Y;

            Single c = area[0].X - area[prelast].X;
            Single d = area[0].Y - area[prelast].Y;

            Single e = point.X - area[prelast].X;
            Single f = point.Y - area[prelast].Y;

            Boolean isOk = Math.Sign(a * d - b * c) == Math.Sign(a * f - b * e);
            if (!isOk) {
                return false;
            }

            a = area[0].X - area[last].X;
            b = area[0].Y - area[last].Y;

            c = area[1].X - area[last].X;
            d = area[1].Y - area[last].Y;

            e = point.X - area[last].X;
            f = point.Y - area[last].Y;

            isOk = Math.Sign(a * d - b * c) == Math.Sign(a * f - b * e);
            if (!isOk) {
                return false;
            }

            for (Int32 i = 0; i < area.Length - 2; i++) {
                //Основная сторона
                a = area[i + 1].X - area[i].X;
                b = area[i + 1].Y - area[i].Y;
                //Внутренняя сторона
                c = area[i + 2].X - area[i].X;
                d = area[i + 2].Y - area[i].Y;
                //Вектор от стороны к точке
                e = point.X - area[i].X;
                f = point.Y - area[i].Y;

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
        /// <param name="p1">
        /// Координаты первой точки прямой
        /// </param>
        /// <param name="p2">
        /// Коордианы второй точки прямой</param>
        /// <param name="p3">
        /// Координаты точки
        /// </param>
        private Int32 IsPointOverLine(in PointF p1, in PointF p2, in PointF p3) {
            #region Неоптимальный способ
            //return point.Y > ((point.X - p1.X) * (p2.Y - p1.Y) + point.Y * (p2.X - p1.X))/(p2.X - p1.X);
            #endregion

            //Если точки совпадают или прямая вертикальна
            if (p1.X == p2.X) {
                throw new Exception();
            }

            #region Человеческий вид
            //Single a = p2.X - p1.X;
            //Single b = -p2.Y - -p1.Y;
            //Single c = p3.X - p1.X;
            //Single d = -p3.Y - -p1.Y;

            //Int32 e = Math.Sign(a * d - b * c);

            //if (a > 0) {
            //    return e;
            //}
            //else {
            //    return -e;
            //}
            #endregion

            Single a = p2.X - p1.X;
            if (a > 0) {
                return Math.Sign((-a * (p3.Y - p1.Y)) + (p2.Y - p1.Y) * (p3.X - p1.X));
            }
            else {
                return -Math.Sign((-a * (p3.Y - p1.Y)) + (p2.Y - p1.Y) * (p3.X - p1.X));
            }
        }


        private void CloseConstructor() {
            //CurrBuildingVariant = BuildingVariants.None;
            currConstructorStage = 0;
            supportFigures.Clear();
            pointsList.Clear();
        }


        public void DrawFigures(Graphics screen) {
            screen.Clear(Color.FromArgb(250, 64, 64, 64));
            foreach (var figure in figuresContainer) {
                figure.Draw(screen);
            }
            foreach (var figure in supportFigures) {
                figure.Draw(screen);
            }
            snapPoint.Draw(screen);
        }

    }
}
//MainCode#84: сделать static многие из методов
//[Closed]: нет причин для этого, метод примет слишком много параметров. Класс по определению подходит здесь.