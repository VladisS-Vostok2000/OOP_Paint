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

//!!MainCode#20: переименовать перечисления-названия фигур в инструменты
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
        public event EventHandler PolarLineEnablingChanged;
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
        private static readonly Pen supportFigurePen = new Pen(Color.White, 2);
        private static readonly Pen figurePen = new Pen(Color.Black);
        private static readonly Pen selectPen = new Pen(Color.White) { Width = 1, DashStyle = DashStyle.Dash };

        private static readonly Pen snapPen = new Pen(Color.Green, 2);
        private static readonly MyRectangle snapPoint = new MyRectangle(0, 0, 6, 6, snapPen) { IsHide = true };
        private static readonly Pen polarPen = new Pen(Color.Lime, 1) { DashStyle = DashStyle.Dash };
        private static readonly MyRay polarLine = new MyRay(polarPen);

        public bool polarLineEnabled = true;
        public bool PolarLineEnabled {
            set {
                if (value != polarLineEnabled) {
                    polarLineEnabled = value;
                    PolarLineEnablingChanged.Invoke(this, EventArgs.Empty);
                }
            } 
            get {
                return polarLineEnabled;
            }
        }



        public MainCode() {
            figuresContainer.ContainerChanged += FiguresContainer_ContainerChanged;
        }



        private void FiguresContainer_ContainerChanged(object sender, EventArgs e) => FiguresListChanged?.Invoke(sender, e);
        

        //!!!MainCode#10: реализовать динамический показ сообщений при движении мыши тоже (ConstructorOperationStatus += Continius)
        //!!!MainCode#01: Запретить выделение "линией"
        /// <param name="pointOnPolar"> Должна ли точка быть спроецирована на полярной прямую </param>
        public void AddSoftPoint(in PointF softPoint, bool pointOnPolar = true) {
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

                    List<Int32> indexes = FindFiguresNearPoint(softPoint);
                    for (Int32 i = 0; i < indexes.Count; i++) {
                        figuresContainer[indexes[i]].IsHightLighed = true;
                    }
                    return;
                case Figure.Select:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.None:
                            switch (currConstructorStage) {
                                case 1:
                                    if (pointsList[0] == softPoint) {

                                    }
                                    (supportFigures[0] as MyRectangle).Resize(pointsList[0].X, pointsList[0].Y, softPoint.X, softPoint.Y);
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
                                    (supportFigures[0] as MyRectangle).Resize(pointsList[0].X, pointsList[0].Y, softPoint.X, softPoint.Y);
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
                                    (supportFigures[0] as MyRectangle).Resize(pointsList[0].X, pointsList[0].Y, softPoint.X, softPoint.Y);
                                    (supportFigures[1] as MyCircle).Resize(pointsList[0].X, pointsList[0].Y, softPoint.X, softPoint.Y);
                                    return;
                                default:
                                    throw new Exception();
                            }
                        case BuildingMethod.CircleCenterRadius:
                            switch (currConstructorStage) {
                                case 1:
                                    (supportFigures[0] as MyCut).P2 = softPoint;
                                    (supportFigures[1] as MyCircle).Radius = MyGeometry.FindLength(softPoint, pointsList[0]);
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
                                    PointF target = softPoint;
                                    if (PolarLineEnabled) {
                                        AddPolarLine(softPoint);
                                        if (pointOnPolar) {
                                            target = MakeProjectionOnPolarLine(softPoint);
                                        }
                                    }

                                    (supportFigures[0] as MyCut).P2 = target;
                                    return;
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedTool} выбрана, но вариант построения {SelectedBuildingMethod} не реализован.");
                    }
                default: throw new NotImplementedException($"Фигура {SelectedTool} не реализована.");
            }
        }
        /// <summary> Задаст следующую точку построения. </summary>
        /// <param name="pointOnPolar"> Должна ли точка быть спроецирована на полярной прямую </param>
        public void SetPoint(in Point target, bool pointOnPolar = true) {
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
                                    supportFigures.Add(new MyRectangle(target.X, target.Y, target.X, target.Y, selectPen) { IsFill = true, FillColor = Color.FromArgb(50, Color.Blue) });
                                    pointsList.Add(target);
                                    currConstructorStage++;
                                    ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                    return;
                                case 1:
                                    if (pointsList[0] == target) {
                                        return;
                                    }

                                    List<MyFigure> myFigures = FindFiguresTouchesRect(pointsList[0], target);
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
                                    supportFigures.Add(new MyRectangle(target.X, target.Y, target.X, target.Y, supportPen));
                                    supportFigures.Add(new MyCircle(target.X, target.Y, target.X, target.Y, supportFigurePen));
                                    pointsList.Add(target);
                                    currConstructorStage++;
                                    ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                    return;
                                case 1:
                                    if (pointsList[0].X == target.X || pointsList[0].Y == target.Y) {
                                        return;
                                    }

                                    figuresContainer.Add(new MyCircle(pointsList[0].X, pointsList[0].Y, target.X, target.Y, figurePen));
                                    CloseConstructor();
                                    return;
                                default:
                                    throw new Exception();
                            }
                        case BuildingMethod.CircleCenterRadius:
                            switch (currConstructorStage) {
                                case 0:
                                    supportFigures.Add(new MyCut(supportPen, target, target));
                                    supportFigures.Add(new MyCircle(supportFigurePen, target, 0));
                                    pointsList.Add(target);
                                    currConstructorStage++;
                                    ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Continious, $"Центр: ({pointsList[0].X}, {pointsList[0].Y}). Задайте радиус.");
                                    return;
                                case 1:
                                    Single radius = MyGeometry.FindLength(target, pointsList[0]);
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
                                    supportFigures.Add(new MyRectangle(target.X, target.Y, target.X, target.Y, supportPen));
                                    pointsList.Add(target);
                                    currConstructorStage++;
                                    ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                    return;
                                case 1:
                                    if (pointsList[0].X == target.X || pointsList[0].Y == target.Y) {
                                        return;
                                    }

                                    figuresContainer.Add(new MyRectangle(pointsList[0].X, pointsList[0].Y, target.X, target.Y, figurePen));
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
                                    pointsList.Add(target);
                                    supportFigures.Add(new MyCut(supportFigurePen, pointsList[0], pointsList[0]));
                                    currConstructorStage++;
                                    ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                    return;
                                case 1:
                                    if (pointsList[0] == target) {
                                        ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Exception, $"Предыдущая точка простроения совпадает с заданной: ({pointsList[0].X};{pointsList[1].Y}).");
                                        return;
                                    }

                                    PointF setPoint = target;
                                    if (PolarLineEnabled) {
                                        if (pointOnPolar) {
                                        setPoint = MakeProjectionOnPolarLine(target);
                                        }
                                    }

                                    figuresContainer.Add(new MyCut(figurePen, pointsList[0], setPoint));
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


        public void SelectFigure(in Int32 id) {
            for (Int32 i = 0; i < figuresContainer.Count; i++) {
                if (figuresContainer[i].Id == id) {
                    figuresContainer[i].IsSelected = true;
                    break;
                }
            }
        }
        public void UnselectFigure(in Int32 id) {
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


        //!!!MainCode#1:Удалить snap из кода. Он существует только для формы.
        public void AddSnapPoint(in Point location) {
            snapPoint.Location = new Point(location.X - 3, location.Y - 3);
            snapPoint.IsHide = false;
        }
        public void RemoveSnapPoint() {
            snapPoint.IsHide = true;
        }

        /// <summary> Выстраивает полярную линию с последней точки построения в сторону данной. </summary>
        private void AddPolarLine(in PointF vector) {
            if (currConstructorStage == 0 || pointsList.Count == 0) {
                throw new Exception();
            }

            PointF p1 = pointsList[pointsList.Count - 1];
            PointF p2 = MyGeometry.ChoosePolarLine(p1, vector);
            polarLine.InitializeFigure(p1, p2);
            polarLine.IsHide = false;
        }

        /// <summary> Вернёт спроецированную точку на текущую полярную линию. </summary>
        /// <exception cref="Exception"> Полярная линия отсуствует </exception>
        /// <param name="p3"> Точка, выпускающая перпендикуляр </param>
        private PointF MakeProjectionOnPolarLine(in PointF p3) {
            if (polarLine.IsHide) {
                throw new Exception();
            }

            return MyGeometry.MakePointProjectionOnLine(polarLine.Vertexes[0], polarLine.Vertexes[1], p3);
        }


        /// <summary> Находит координаты ближайшей к точке вершины фигуры для непустого списка фигур. </summary>
        public PointF FindNearestVertex(in PointF target) => FindNearestVertex(figuresContainer, target);
        private PointF FindNearestVertex(MyListContainer<MyFigure> figures, in PointF target) {
            if (figures.Count == 0) {
                throw new Exception();
            }

            Single minDistance = Single.MaxValue;
            PointF outvertex = new PointF(0, 0);
            Boolean isOk = false;
            foreach (var figure in figures) {
                foreach (var vetrex in figure.Vertexes) {
                    Single distance = MyGeometry.FindLength(vetrex, target);
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
        private List<MyFigure> FindFiguresTouchesRect(in Point p1, in Point p2) {
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
                        Boolean isParallel = MyGeometry.AreLinesParallel(myCut.P1, myCut.P2, selectRectLinePoints[i, 0], selectRectLinePoints[i, 1]);
                        if (isParallel) {
                            continue;
                        }

                        PointF crossPoint = MyGeometry.FindCross(myCut.P1, myCut.P2, selectRectLinePoints[i, 0], selectRectLinePoints[i, 1]);
                        //Отрезки пересекаются, если точка пересечения прямых, чрез них проходящих, принадлежит им обоим.
                        Boolean isTouches = MyGeometry.CheckIsPointOnCut(myCut.P1, myCut.P2, crossPoint) && MyGeometry.CheckIsPointOnCut(selectRectLinePoints[i, 0], selectRectLinePoints[i, 1], crossPoint);
                        if (isTouches) {
                            outlist.Add(figure);
                            break;
                        }
                    }
                }
            }

            return outlist;
        }
        /// <summary>
        /// Находит все фигуры на заданном от цели расстоянии
        /// </summary>
        /// <returns>
        /// Целочисленный массив с индексами figures
        /// </returns>
        private List<Int32> FindFiguresNearPoint(in PointF target, in Single interval = 5) {
            var outlist = new List<Int32>();
            for (Int32 i = 0; i < figuresContainer.Count; i++) {
                if (figuresContainer[i] is MyCut) {
                    var cut = figuresContainer[i] as MyCut;
                    PointF[] area = MyGeometry.FindCutArea(cut.P1, cut.P2, interval);
                    Boolean isInArea = MyGeometry.IsPointInArea(target, area);
                    if (isInArea) {
                        outlist.Add(i);
                    }
                }
            }

            return outlist;
        }


        private void CloseConstructor() {
            //CurrBuildingVariant = BuildingVariants.None;
            currConstructorStage = 0;
            supportFigures.Clear();
            pointsList.Clear();
            polarLine.IsHide = true;
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
            polarLine.Draw(screen);
        }

    }
}
//MainCode#84: сделать static многие из методов
//[Closed]: нет причин для этого, метод примет слишком много параметров. Класс по определению подходит здесь.