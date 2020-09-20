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

        private Figure currSelectedFigure = Figure.Null;
        public Figure CurrSelectedFigure { 
            set {
                CloseConstructor();
                currSelectedFigure = value;
            }
            get => currSelectedFigure;
        }
        private Int32 currConstructorStage = 0;

        public readonly BindingList<MyFigure> Figures = new BindingList<MyFigure>();
        private readonly List<MyFigure> supportFigures = new List<MyFigure>();
        private readonly List<Point> pointsList = new List<Point>();

        private static readonly Pen supportPen = new Pen(Color.Red) { Width = 2, DashStyle = DashStyle.Dash };
        private static readonly Pen supportPen2 = new Pen(Color.Black, 1);
        private static readonly Pen normalPen = new Pen(Color.Black);


        public ConstructorResult AddMouseClick(Point _coord) {
            if (currSelectedFigure == Figure.Null) {
                return new ConstructorResult(ConstructorResult.OperationStatus.None, "");
            }

            switch (currSelectedFigure) {
                case Figure.Circle:
                    switch (currConstructorStage) {
                        case 0:
                            supportFigures.Add(new MyRectangle(_coord.X, _coord.Y, _coord.X, _coord.Y, supportPen));
                            supportFigures.Add(new MyCircle(_coord.X, _coord.Y, _coord.X, _coord.Y, supportPen2));
                            pointsList.Add(_coord);
                            currConstructorStage++;
                            return new ConstructorResult(ConstructorResult.OperationStatus.Continious, "Задайте вторую точку");
                        case 1:
                            if (pointsList[0] == _coord) {
                                return new ConstructorResult(ConstructorResult.OperationStatus.Continious, "Задайте вторую точку");
                            }

                            Figures.Add(new MyCircle(pointsList[0].X, pointsList[0].Y, _coord.X, _coord.Y, normalPen));
                            CloseConstructor();
                            return new ConstructorResult(ConstructorResult.OperationStatus.Finished, "");
                        default: throw new Exception();
                    }

                default: throw new NotImplementedException();
            }
        }
        public void DrawFigures(Graphics _screen) {
            foreach (var figure in Figures) {
                figure.Draw(_screen);
            }
        }
        private void CloseConstructor() {
            currConstructorStage = 0;
            supportFigures.Clear();
            pointsList.Clear();
        }

    }
}
