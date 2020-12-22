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
using static CAD_Client.ToolEnum;
//MyClient#41: вычленить SnapPoint из MainCode[MyClient]
//!!MyClient#20: переименовать перечисления-названия фигур в инструменты с соответствующим
namespace CAD_Client {

    internal sealed class MyClient {
        private Tool selectedTool;
        internal Tool SelectedTool {
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
        internal BuildingMethod SelectedBuildingMethod {
            set {
                if (selectedBuildingMethod != value) {
                    selectedBuildingMethod = value;
                    SelectedBuildingVariantChanged?.Invoke(value, EventArgs.Empty);

                }
            }
            get => selectedBuildingMethod;
        }
        private ConstructorOperationStatus constructorOperationStatus;
        internal ConstructorOperationStatus ConstructorOperationStatus {
            set {
                if (constructorOperationStatus != value) {
                    constructorOperationStatus = value;
                    ConstructorOperationStatusChanged?.Invoke(value, EventArgs.Empty);
                }
            }
            get => constructorOperationStatus;
        }
        private int currConstructorStage = 0;


        internal readonly MyListContainer<MyFigure> figuresContainer = new MyListContainer<MyFigure>();
        //!!!Исправить List на MyContainer
        private readonly List<MyFigure> supportFigures = new List<MyFigure>();
        private readonly List<Point> pointsList = new List<Point>();

        private static readonly Pen supportPen = new Pen(Color.Gray) { Width = 1, DashStyle = DashStyle.Dash };
        private static readonly Pen supportFigurePen = new Pen(Color.White, 2);
        private static readonly Pen figurePen = new Pen(Color.Black);
        private static readonly Pen selectPen = new Pen(Color.White) { Width = 1, DashStyle = DashStyle.Dash };

        private static readonly Pen polarPen = new Pen(Color.Lime, 1) { DashStyle = DashStyle.Dash };
        private static readonly MyRay polarLine = new MyRay(polarPen, new PointF(0, 0), new PointF(1, 1)) { IsHide = true };


        internal delegate void BuildingMethodHandler(BuildingMethod buildingMethod, EventArgs e);
        internal delegate void FigureHandler(Tool figure, EventArgs e);
        internal delegate void ConstructorOperationStatusHandler(ConstructorOperationStatus ConstructorOperationStatus, EventArgs e);

        #region API
        internal event FigureHandler SelectedToolChanged;
        internal event BuildingMethodHandler SelectedBuildingVariantChanged;
        internal event EventHandler FiguresListChanged;
        internal event ConstructorOperationStatusHandler ConstructorOperationStatusChanged;
        internal event EventHandler PolarLineEnablingChanged;
        #endregion


        /// <summary>
        /// Построение и прорисовка полярной линии.
        /// </summary>
        internal bool polarLineEnabled = true;
        internal bool PolarLineEnabled {
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




        internal MyClient() {
            figuresContainer.ContainerChanged += FiguresContainer_ContainerChanged;
        }



        private void FiguresContainer_ContainerChanged(object sender, EventArgs e) => FiguresListChanged?.Invoke(sender, e);


        #region Построение
        //!!!MyClient#10: реализовать динамический показ сообщений при движении мыши тоже (ConstructorOperationStatus += Continius)
        //!!!MyClient#01: Запретить выделение "линией"
        /// <param name="pointOnPolar"> Должна ли введённая точка быть спроецирована на ближайшую полярной прямую. </param>
        internal void AddSoftPoint(in PointF softPoint, bool pointOnPolar = true) {
            if (currConstructorStage == 0 && SelectedTool != Tool.None) {
                return;
            }

            ///currSelectedFigure -> Выбор фигуры построения
            ///currBuildingVariant -> Выбор варианта построения фигуры
            ///currConstructorStage -> Выбор текущей стадии построения (могут отличаться вспомогательные фигуры)
            switch (SelectedTool) {
                case Tool.None:
                    foreach (var figure in figuresContainer) {
                        figure.IsHightLighed = false;
                    }

                    List<int> indexes = FindFiguresNearPoint(softPoint);
                    for (int i = 0; i < indexes.Count; i++) {
                        figuresContainer[indexes[i]].IsHightLighed = true;
                    }
                    return;
                case Tool.Select:
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
                case Tool.Rectangle:
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
                case Tool.Circle:
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
                                    (supportFigures[1] as MyCircle).Radius = MyGeometry.FindLengthBetweenPoints(softPoint, pointsList[0]);
                                    return;
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedTool} выбрана, но вариант построения {SelectedBuildingMethod} не реализован.");
                    }
                case Tool.Cut:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.CutTwoPoints:
                            switch (currConstructorStage) {
                                case 1:
                                    PointF target;
                                    if (PolarLineEnabled) {
                                        DirectPolarLine(softPoint);
                                        if (pointOnPolar && !polarLine.IsPoint) {
                                            target = MakeProjectionOnPolarLine(softPoint);
                                        }
                                        else {
                                            target = softPoint;
                                        }
                                    }
                                    else {
                                        target = softPoint;
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
        internal void SetPoint(in Point target, bool pointOnPolar = true) {
            //currSelectedFigure -> Выбор фигуры построения
            //currBuildingVariant -> Выбор варианта построения фигуры
            //currConstructorStage -> Выбор текущей стадии построения (могут отличаться вспомогательные фигуры)
            switch (SelectedTool) {
                case Tool.None:
                    return;
                case Tool.Select:
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
                case Tool.Circle:
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
                                    float radius = MyGeometry.FindLengthBetweenPoints(target, pointsList[0]);
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
                case Tool.Rectangle:
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
                case Tool.Cut:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.CutTwoPoints:
                            switch (currConstructorStage) {
                                case 0:
                                    pointsList.Add(target);
                                    supportFigures.Add(new MyCut(supportFigurePen, pointsList[0], pointsList[0]));
                                    currConstructorStage++;

                                    //Полярная линия отслеживает построение даже будучи выключенной, т.к. может быть включена в процессе.
                                    polarLine.Location = target;
                                    if (polarLineEnabled) {
                                        polarLine.IsHide = false;
                                    }

                                    ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Continious, $"Первая точка: ({pointsList[0].X}, {pointsList[0].Y}). Задайте вторую точку");
                                    return;
                                case 1:
                                    if (pointsList[0] == target) {
                                        ConstructorOperationStatus = new ConstructorOperationStatus(ConstructorOperationStatus.OperationStatus.Exeption, $"Предыдущая точка простроения совпадает с заданной: ({pointsList[0].X};{pointsList[1].Y}).");
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
                case Tool.Moving:
                    switch (SelectedBuildingMethod) {
                        case BuildingMethod.None:
                            switch (currConstructorStage) {
                                case 0:
                                    return;
                                default: throw new Exception();
                            }
                        default: throw new Exception($"Фигура {SelectedTool} выбрана, но не задан вариант построения.");
                    }
                default: throw new NotImplementedException($"Для фигуры {SelectedTool} нет построения.");
            }
        }
        /// <summary>
        /// Свернёт конструктор построения, очистит список вспомогательных фигур.
        /// </summary>
        private void CloseConstructor() {
            currConstructorStage = 0;
            supportFigures.Clear();
            pointsList.Clear();
            polarLine.IsHide = true;
        }

        /// <summary>
        /// Направляет полярную линию с последней точки построения в сторону данной.
        /// </summary>
        private void DirectPolarLine(in PointF vector) {
            if (currConstructorStage == 0) {
                throw new Exception("Построение не ведётся, но производится создание полярной линии.");
            }
            if (!polarLineEnabled) {
                throw new Exception("Построение полярной линии не ведётся, но запрошено вопреки.");
            }

            PointF p1 = pointsList[pointsList.Count - 1];
            try {
                //????сократить p2
                PointF p2 = MyGeometry.DirectToPolarLine(p1, vector);
                polarLine.Vector = p2;
            }
            catch (ArgumentException) { }
        }

        /// <summary> Вернёт спроецированную точку на текущую полярную линию. </summary>
        /// <exception cref="Exception"> Полярная линия отсуствует </exception>
        /// <param name="p3"> Точка, выпускающая перпендикуляр </param>
        private PointF MakeProjectionOnPolarLine(in PointF p3) => MyGeometry.MakePointProjectionOnLine(polarLine.Location, polarLine.Vector, p3);
        #endregion

        #region API
        internal void SelectFigure(in int id) {
            for (int i = 0; i < figuresContainer.Count; i++) {
                if (figuresContainer[i].Id == id) {
                    figuresContainer[i].IsSelected = true;
                    break;
                }
            }
        }
        internal void UnselectFigure(in int id) {
            for (int i = 0; i < figuresContainer.Count; i++) {
                if (figuresContainer[i].Id == id) {
                    figuresContainer[i].IsSelected = false;
                    break;
                }
            }
        }
        internal int GetFiguresCount() {
            return figuresContainer.Count;
        }
        internal MyRay GetPolarLine() {
            return polarLine;
        }
        internal List<MyFigure> GetSupportFiguresList() {
            return supportFigures;
        }
        internal MyListContainer<MyFigure> GetFiguresList() {
            return figuresContainer;
        }
        #endregion

        #region Другое
        /// <summary>
        /// Вернёт координаты ближайшей к точке вершины фигуры.
        /// </summary>
        /// <exception cref="Exception"> Лист пуст. </exception>
        /// <exception cref="ArgumentNullException"> Лист null. </exception>
        internal PointF FindNearestVertex(in PointF target) => FindNearestVertex(figuresContainer, target);
        /// <summary>
        /// Вернёт координаты ближайшей к точке вершины фигуры.
        /// </summary>
        /// <exception cref="Exception"> Лист пуст. </exception>
        /// <exception cref="Exception"> Вершина не найдена при непустом листе. </exception>
        /// <exception cref="ArgumentNullException"> Лист null. </exception>
        internal PointF FindNearestVertex(MyListContainer<MyFigure> figures, PointF target) {
            if (figures == null) {
                throw new ArgumentNullException();
            }
            if (figures.Count == 0) {
                throw new Exception("Фигур нет, но проверка на поиск вершины произошла.");
            }

            float minDistance = float.PositiveInfinity;
            PointF out_vertex = new PointF(0, 0);
            foreach (var figure in figures) {
                switch (figure) {
                    case MyPoligon myPoligon:
                        foreach (var vertex in myPoligon.Vertexes) {
                            float distance = MyGeometry.FindLengthBetweenPoints(vertex, target);
                            bool isNewLower = CompareDistance(ref minDistance, in distance);
                            if (isNewLower) {
                                out_vertex = vertex;
                            }
                        }
                        break;
                    case MyCut myCut:
                        float p1L = MyGeometry.FindLengthBetweenPoints(myCut.P1, target);
                        float p2L = MyGeometry.FindLengthBetweenPoints(myCut.P2, target);
                        if (p1L < p2L) {
                            bool isNewLower = CompareDistance(ref minDistance, in p1L);
                            if (isNewLower) {
                                out_vertex = myCut.P1;
                            }
                        }
                        else {
                            bool isNewLower = CompareDistance(ref minDistance, in p1L);
                            if (isNewLower) {
                                out_vertex = myCut.P2;
                            }
                        }
                        break;
                    default: throw new Exception($"Для фигуры {figure} не реализован поиск ближайшей вершины.");
                }
            }

            if (minDistance == float.PositiveInfinity) {
                throw new Exception("Вершина не найдена, хотя список фигур не пустой.");
            }
            else {
                return out_vertex;
            }
        }
        /// <summary>
        /// Вернёт true и присвоит сравниваемому числу новое, если новое меньше.
        /// </summary>
        /// <param name="minDistance"> Сравниваемое число. </param>
        /// <param name="newDistance"> Новое число. </param>
        private bool CompareDistance(ref float minDistance, in float newDistance) {
            if (newDistance <= minDistance) {
                minDistance = newDistance;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Находит все фигуры на заданном от цели расстоянии
        /// </summary>
        /// <returns>
        /// Целочисленный массив с индексами figures
        /// </returns>
        private List<int> FindFiguresNearPoint(in PointF target, in float interval = 5) {
            var outlist = new List<int>();
            for (int i = 0; i < figuresContainer.Count; i++) {
                if (figuresContainer[i] is MyCut) {
                    var cut = figuresContainer[i] as MyCut;
                    PointF[] area = MyGeometry.FindCutArea(cut.P1, cut.P2, interval);
                    bool isInArea = MyGeometry.IsPointInArea(target, area);
                    if (isInArea) {
                        outlist.Add(i);
                    }
                }
            }

            return outlist;
        }


        //!!!MyClient: добавить выделение прямоугольника и круга
        //!!!MyClient: расчленить FindFiguresTouchesRect на MyGeomtry и MainCode
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
                    for (int i = 0; i < selectRectLinePoints.GetLength(0); i++) {
                        bool isParallel = MyGeometry.IsLinesParallel(myCut.P1, myCut.P2, selectRectLinePoints[i, 0], selectRectLinePoints[i, 1]);
                        if (isParallel) {
                            continue;
                        }

                        PointF crossPoint = MyGeometry.FindCross(myCut.P1, myCut.P2, selectRectLinePoints[i, 0], selectRectLinePoints[i, 1]);
                        //Отрезки пересекаются, если точка пересечения прямых, чрез них проходящих, принадлежит им обоим.
                        bool isTouches = MyGeometry.IsPointInCut(myCut.P1, myCut.P2, crossPoint) && MyGeometry.IsPointInCut(selectRectLinePoints[i, 0], selectRectLinePoints[i, 1], crossPoint);
                        if (isTouches) {
                            outlist.Add(figure);
                            break;
                        }
                    }
                }
            }

            return outlist;
        }
        #endregion

    }
}
//MyClient#84: сделать static многие из методов
//[Closed]: нет причин для этого, метод примет слишком много параметров. Класс по определению подходит здесь.