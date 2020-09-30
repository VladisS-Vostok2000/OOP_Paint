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
        public enum Figure {
            None = 0,
            Circle,
            Rectangle,
        }



        public event EventHandler SelectedFigureChanged;
        public event EventHandler SelectedBuildingVariantChanged;

        private FigureBuilding selectedBuildingVariant;
        public FigureBuilding SelectedBuildingVariant {
            set {
                if (selectedBuildingVariant != value) {
                    SelectedBuildingVariant = value;
                    SelectedBuildingVariantChanged?.Invoke(value, EventArgs.Empty);
                    //BuildingVariantChanged?.Invoke(CurrBuildingVariant, new PropertyChangedEventArgs("CurrBuildingVariant"));

                }
            }
            get => selectedBuildingVariant;
        }
        private Figure selectedFigure = Figure.None;
        //???CloseContstructor не обнулят текущюю фигуру, тем не менее, использовать
        //фигуру без определения варианта построения невозможно.
        //Тогда в коде должен быть реализован автоматически обновляемый лист
        //с доступными вариантами фигур, но выбрана архитектура, когда его нет,
        //т.к. это громоздко. Тогда нужно либо обнулять фигуру, но тогда свойство или зациклится,
        //или не среагирует на изменение value, либо создавать ивенты и как-то вязать это с GUI.
        //Либо же всегда в метод передавать, какую фигуру строим. Но если у нас фигура поменяется в процессе?
        //Кстати, в таком случае можно закрыть конструктор.
        //Создавать ивент глупо, т.к. зачем коду нужно менять выбранную фигуру в процессе? Это
        //должен делать GUI. Отменять её внезапно.
        //fixed: CloseConstructor больше не обнуляет текущий вариант построения
        //!!!Устанавливается текущий вариант построения, однако GUI этого не видит, возможны расхождения.
        //При выборе фигуры должен быть задан вариант построения по умолчанию, и он отнюдь не первый.
        //Отсюда нужно уведомлять GUI, какой вариант был задан. Отсюда двусторонняя привязка.
        public Figure SelectedFigure {
            set {
                if (selectedFigure != value) {
                    CloseConstructor();
                    SelectedFigureChanged?.Invoke(value, EventArgs.Empty);
                    selectedFigure = value;

                    if (FingPossibleBuildingVariants(value).Count != 0) {
                        SelectedBuildingVariant = FindPossibleBuildingVariants()[0];
                    }
                    else {
                        SelectedBuildingVariant = new FigureBuilding(FigureBuilding.Method.None);
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

        private static readonly Pen supportPen = new Pen(Color.Red) { Width = 1, DashStyle = DashStyle.Dash };
        private static readonly Pen supportPen2 = new Pen(Color.Black, 2);
        private static readonly Pen normalPen = new Pen(Color.Black);



        //???По-хорошему нужен отдельный метод для прорисовки
        //и обработки клика мыши, однако дублировать switsch 
        //очень плохо, тем более такой длинный. Тогда нужен MouseEvent
        //для понимания, клик это или движение, что в свою очередь
        //ведёт упаковку в object, т.к. это класс.
        //upd: Повезло, класс один. Но всё равно один метод вмещает 2 из-за switsch ("if (e.Button == MouseButtons.Left)".)
        public ConstructorOperationResult ThreatMouseEvent(MouseEventArgs e) {
            ConstructorOperationResult out_result;
            //???По-хорошему buildingVariant должен быть перечислением, ибо если
            //хоть что-то изменить в списке вариантов, нужно трогать свич.
            //Но перечисления для всех фигур невероятно громоздки.
            ///currSelectedFigure -> Выбор фигуры построения
            ///currBuildingVariant -> Выбор варианта построения фигуры
            ///currConstructorStage -> Выбор текущей стадии построения (могут отличаться вспомогательные фигуры)
            ///MouseButtons -> Выбор, движение мыши (изменение вспомогательных фигур) или клик, переход к следующей стадии построения.

            switch (SelectedBuildingVariant.Methodd) {
                case FigureBuilding.Method.InRectangleTwoDots:
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

                                Figures.Add(new MyCircle(pointsList[0].X, pointsList[0].Y, e.X, e.Y, normalPen));
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
                case FigureBuilding.Method.DotRadius:
                    throw new NotImplementedException();
                default: throw new Exception("Не может быть выбрана фигура без варианта построения.");
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
        public List<FigureBuilding> FingPossibleBuildingVariants(Figure _figure) {
            var out_list = new List<FigureBuilding>();
            switch (_figure) {
                case Figure.Circle:
                    out_list.Add(new FigureBuilding(FigureBuilding.Method.DotRadius));
                    out_list.Add(new FigureBuilding(FigureBuilding.Method.InRectangleTwoDots));
                    break;
                default: throw new Exception();
            }
            return out_list;
        }
        /// <summary>
        /// Возвращает список доступных вариантов строительства фигур
        /// текущей выбранной фигуры.
        /// </summary>
        /// <returns>Список MainCode.BuildingVariants </returns>
        public List<FigureBuilding> FindPossibleBuildingVariants() {
            Figure figure = selectedFigure;
            var out_list = new List<FigureBuilding>();
            switch (figure) {
                case Figure.None: return out_list;
                case Figure.Circle:
                    out_list.Add(new FigureBuilding(FigureBuilding.Method.InRectangleTwoDots));
                    out_list.Add(new FigureBuilding(FigureBuilding.Method.DotRadius));
                    break;
                default: throw new NotImplementedException();
            }
            return out_list;
        }

    }
}
