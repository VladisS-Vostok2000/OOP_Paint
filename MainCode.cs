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
        private static readonly Pen selectPen = new Pen(Color.Blue) { Width = 1, DashStyle = DashStyle.Dash };



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


        //!!!MainCode#45: реализовать динамический показ сообщений при движении мыши тоже (ConstructorOperationResult += Continius)
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
                                    //???Тут проверка какая-то должна быть, что ли...
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
                                    supportFigures.Add(new MyRectangle(_point.X, _point.Y, _point.X, _point.Y, selectPen) { });
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
                                    int radius = MyFigure.FindLength(_point, pointsList[0]);
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
                                    int radius = MyFigure.FindLength(_point, pointsList[0]);
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
        //!!!MainCode#01: рассмотреть возомжность открыть лист Figures
        public void SelectFigure(int _id) {
            for (int i = 0; i < figures.Count; i++) {
                if (figures[i].Id == _id) {
                    figures[i].IsSelected = true;
                    break;
                }
            }
        }
        public void UnselectFigure(int _id) {
            for (int i = 0; i < figures.Count; i++) {
                if (figures[i].Id == _id) {
                    figures[i].IsSelected = false;
                    break;
                }
            }
        }
        public int GetFiguresCount() {
            return figures.Count;
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