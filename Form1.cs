using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        //private string selectedShape;
        private string _selectedShape = "";
        private Tool selectedTool = Tool.Pencil;
        private bool isDrawing = false;
        private Point _startPoint;
        private Point _endPoint;
        private Bitmap _drawingSurface;
        private Pen _currentPen; private ColorDialog _colorDialog;
        private Color selectedColor = Color.Black;
        private int startX, startY;
        private Bitmap drawingSurface;
        private Stack<Bitmap> undoStack = new Stack<Bitmap>();
        private Stack<Bitmap> redoStack = new Stack<Bitmap>();
        private float zoomFactor = 1.0f;
        int originalWidth;
        int originalHeight;
        public enum Tool { Pencil, Eraser, Line, Parker, Circle, Fill,Text, Magnifier }
        private Tool currentTool = Tool.Pencil;

        public Form1()
        {
            InitializeComponent();
            _drawingSurface = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _currentPen = new Pen(Color.Black, 3);
            _colorDialog = new ColorDialog();
            btncolor1.Paint += button1_Paint;
            btncolor2.Paint += button1_Paint;
            btncolor3.Paint += button1_Paint;
            btncolor4.Paint += button1_Paint;
            btncolor5.Paint += button1_Paint;
            btncolor6.Paint += button1_Paint;
            btncolor7.Paint += button1_Paint;
            btncolor8.Paint += button1_Paint;
            btncolor9.Paint += button1_Paint;
            btncolor10.Paint += button1_Paint;
            btncolor11.Paint += button1_Paint;
            btncolor12.Paint += button1_Paint;
            btncolor13.Paint += button1_Paint;
            btncolor14.Paint += button1_Paint;
            btnwhite.Paint += button1_Paint;
            btnblack.Paint += button1_Paint;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            pictureBox1.Paint += pictureBox1_Paint;
            btnrectangle.Click += ShapeButton_Click;
            btnEllipse.Click += ShapeButton_Click;
            btnTriangle.Click += ShapeButton_Click;
            btnCircle.Click += ShapeButton_Click;
            btnArrowup.Click += ShapeButton_Click;
            btnArrowdown.Click += ShapeButton_Click;
            btnSquare.Click += ShapeButton_Click;
            btnLinee.Click += ShapeButton_Click;
            originalWidth = pictureBox1.Width;
            originalHeight = pictureBox1.Height;
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button; if (btn != null)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.Clear(btn.Parent.BackColor); // Clear the button area
                e.Graphics.FillEllipse(new SolidBrush(btn.BackColor), 0, 0, btn.Width, btn.Height);
                e.Graphics.DrawEllipse(Pens.Black, 0, 0, btn.Width - 1, btn.Height - 1);
                selectedColor = btn.BackColor;
                _currentPen = new Pen(selectedColor, 2);
            
        }


            
        }
    


        private void btncolor1_Click(object sender, EventArgs e)
        {

        }

        private void btncolor2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btncolor7_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //selectedShape = "heart";
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //selectedShape = "lighting";
        }

        private void btnrectangle_Click(object sender, EventArgs e)
        {
            //selectedShape = "Rectangle";
        }
        private void ShapeButton_Click(object sender, EventArgs e) 
        { Button button = sender as Button; 
            if (button != null && button.Tag != null)
            { _selectedShape = button.Tag.ToString();
            } 
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) 
            {
                _startPoint = e.Location; 
            }
        }

       
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (isDrawing)
            {
                using (Graphics g = Graphics.FromImage(_drawingSurface))
                {
                    switch (selectedTool)
                    {
                        case Tool.Pencil:
                            g.DrawLine(new Pen(Color.Black, 2), _startPoint, e.Location);
                            break;
                        case Tool.Eraser:
                            // Erase a portion of the image
                            g.FillRectangle(Brushes.White, e.X - 5, e.Y - 5, 10, 10);
                            break;
                        case Tool.Fill:
                            Fill(e.X, e.Y, selectedColor);
                            break;
                        case Tool.Parker:
                            // Implement Parker pen style
                            g.DrawLine(new Pen(selectedColor, 2), _startPoint, e.Location);
                            // Add specific Parker pen effects, like line width variation or texture
                            break;
                        case Tool.Magnifier:
                            int magnificationFactor = 2; // Adjust the magnification factor as needed
                            int x = e.X - magnificationFactor / 2;
                            int y = e.Y - magnificationFactor / 2;
                            Rectangle rect = new Rectangle(x, y, magnificationFactor, magnificationFactor);

                            Bitmap magnifiedImage = new Bitmap(magnificationFactor, magnificationFactor);
                            using (Graphics g2 = Graphics.FromImage(magnifiedImage))
                            {
                                g2.DrawImage(_drawingSurface, new Rectangle(0, 0, magnificationFactor, magnificationFactor), rect, GraphicsUnit.Pixel);
                            }

                            g.DrawImage(magnifiedImage, e.X - magnificationFactor / 2, e.Y - magnificationFactor / 2);
                            break;
                    }
                }
                _endPoint = e.Location;
                pictureBox1.Invalidate(); // Invalidate to trigger a repaint
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _endPoint = e.Location;
                using (Graphics g = Graphics.FromImage(_drawingSurface))
                { 
                    DrawShape(g, _selectedShape, _startPoint, _endPoint);
                }
                pictureBox1.Invalidate(); // Final repaint
                             // _startPoint = Point.Empty;
                                          //_endPoint = Point.Empty; }
            }
        }
        
        
        
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
                    if (_startPoint != Point.Empty && _endPoint != Point.Empty) { DrawShape(e.Graphics, _selectedShape, _startPoint, _endPoint); }
                    e.Graphics.DrawImage(_drawingSurface, 0, 0);
            e.Graphics.ScaleTransform(zoomFactor, zoomFactor);
            e.Graphics.DrawImage(_drawingSurface, 0, 0, _drawingSurface.Width * zoomFactor, _drawingSurface.Height * zoomFactor);
        }

        private void DrawShape(Graphics g, string shape, Point start, Point end)
        { switch (shape)
            { 
                case "Line":
                    g.DrawLine(_currentPen, start, end);
                    break;
                case "Rectangle":
                    g.DrawRectangle(_currentPen, GetRectangle(start, end));
                    break;
                case "Circle": DrawCircle(g, _currentPen, start, end); break;
                // break;
                case "Square":
                    DrawSquare(g,_currentPen, start, end);
                    break;
               
                // case "Heart": DrawHeart(g, _currentPen, start, end);
                //   break;
                case "Triangle": DrawTriangle(g, _currentPen, start, end); 
                    break;
                //case "Polygon": DrawPolygon(g, _currentPen, start, end); break;
                case "ArrowUp": DrawArrowUp(g, _currentPen, start, _endPoint);
                    break;
                default: // Default drawing behavior (e.g., freehand drawing)
                // g.FillEllipse(new SolidBrush(selectedColor), start.X - 2, start.Y - 2, 4, 4);
                  g.FillEllipse(new SolidBrush(Color.Black), start.X, start.Y, 4, 4);
                    break;
            }
        }
        private void DrawTriangle(Graphics g, Pen pen, Point start, Point end)
        {
            Point[] points = { new Point((start.X + end.X) / 2, start.Y), // Top
                             new Point(start.X, end.Y), // Bottom left
                            new Point(end.X, end.Y) // Bottom right
                          }; g.DrawPolygon(pen, points); 
        }
        /* private void DrawHeart(Graphics g, Pen pen, Point start, Point end)
         { // Custom drawing logic for the heart shape // This is a simple example and might need adjustments for your exact needs
           /int width = Math.Abs(start.X - end.X);
             int height = Math.Abs(start.Y - end.Y); 
             int centerX = Math.Min(start.X, end.X) + width / 2; 
             int centerY = Math.Min(start.Y, end.Y) + height / 2; // Draw two circles and a triangle to form a heart g.DrawEllipse(pen, centerX - width / 4, Math.Min(start.Y, end.Y), width / 2, height / 2); g.DrawEllipse(pen, centerX, Math.Min(start.Y, end.Y), width / 2, height / 2); Point[] points = { new Point(centerX - width / 2, centerY), new Point(centerX + width / 2, centerY), new Point(centerX, centerY + height / 2) }; g.DrawPolygon(pen, points);
         }*/
        private void DrawArrowUp(Graphics g, Pen pen, Point start, Point end)
        {
            int width = end.X - start.X;
            int height = end.Y - start.Y;
            Point[] points = {
                new Point(start.X + width / 2, start.Y), // Arrow tip
              new Point(start.X, start.Y + height / 2), // Left
              new Point(start.X + width / 4, start.Y + height / 2), // Inner left
              new Point(start.X + width / 4, end.Y), // Bottom left
              new Point(end.X - width / 4, end.Y), // Bottom right
              new Point(end.X - width / 4, start.Y + height / 2), // Inner right
              new Point(end.X, start.Y + height / 2) // Right
              };
            g.DrawPolygon(pen, points);
        }
        private void DrawCircle(Graphics g, Pen pen, Point start, Point end)
        {
            g.DrawEllipse(pen, GetRectangle(start, end));
        }
        private void DrawArrowDown(Graphics g, Pen pen, Point start, Point end)
        {
            int width = end.X - start.X;
            int height = end.Y - start.Y;
            Point[] points = {
                new Point(start.X + width / 2, end.Y), // Arrow tip
                new Point(start.X, start.Y + height / 2), // Left
                new Point(start.X + width / 4, start.Y + height / 2), // Inner left
                new Point(start.X + width / 4, start.Y), // Top left
                new Point(end.X - width / 4, start.Y), // Top right
                new Point(end.X - width / 4, start.Y + height / 2), // Inner right
                new Point(end.X, start.Y + height / 2) // Right
                  };
            g.DrawPolygon(pen, points);
        }
        private void DrawSquare(Graphics g, Pen pen, Point start, Point end)
        { // Ensure the square maintains equal width and height
          int sideLength = Math.Min(Math.Abs(start.X - end.X), Math.Abs(start.Y - end.Y)); 
            Rectangle squareRect = new Rectangle( Math.Min(start.X, end.X), Math.Min(start.Y, end.Y), sideLength, sideLength);
            g.DrawRectangle(pen, squareRect);
        }
        private void DrawLightning(Graphics g, Pen pen, Point start, Point end)
        { // Example points for a simple lightning bolt
            Point[] points = { new Point(start.X, start.Y),
                new Point(start.X + (end.X - start.X) / 2, start.Y + (end.Y - start.Y) / 4),
                new Point(start.X + (end.X - start.X) / 4, start.Y + (end.Y - start.Y) / 2),
                new Point(end.X, start.Y + (end.Y - start.Y) / 2), new Point(start.X + (end.X - start.X) / 2, end.Y),
                new Point(start.X + (end.X - start.X) * 3 / 4, start.Y + (end.Y - start.Y) * 3 / 4) };
            g.DrawPolygon(pen, points);
        }
            private Rectangle GetRectangle(Point p1, Point p2) { return new Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y)); }
        private void ClearDrawingSurface()
        {
            using (Graphics g = Graphics.FromImage(drawingSurface)) { g.Clear(Color.White); }
            this.Invalidate(); // Redraw the form
             } 
        private void Form1_Load(object sender, EventArgs e) { 
            ClearDrawingSurface(); 
        }

        private void btnEraser_Click(object sender, EventArgs e)
        {
            selectedTool = Tool.Eraser;
        }

        private void btnPencil_Click(object sender, EventArgs e)
        {
            selectedTool = Tool.Pencil;
        }

        private void btnText_Click(object sender, EventArgs e)
        {
            selectedTool = Tool.Text;
        }

        private void btnParker_Click(object sender, EventArgs e)
        {
            selectedTool = Tool.Parker;
        }

        private void btnMagnifier_Click(object sender, EventArgs e)
        {
            selectedTool = Tool.Magnifier;
        }

        private void btncolor8_Click(object sender, EventArgs e)
        {

        }
        private void RotateImage(float angle)
        {
            Bitmap rotatedImage = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(rotatedImage.Width / 2, rotatedImage.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-rotatedImage.Width / 2, -rotatedImage.Height / 2);
                g.DrawImage(pictureBox1.Image, 0, 0);
            }
            pictureBox1.Image = rotatedImage;
        }
        private void Magnify(Graphics g, Point mousePosition, int magnificationFactor)
        {
            int x = mousePosition.X - magnificationFactor / 2;
            int y = mousePosition.Y - magnificationFactor / 2;
            Rectangle rect = new Rectangle(x, y, magnificationFactor, magnificationFactor);

            Bitmap magnifiedImage = new Bitmap(magnificationFactor, magnificationFactor);
            using (Graphics g2 = Graphics.FromImage(magnifiedImage))
            {
                g2.DrawImage(_drawingSurface, new Rectangle(0, 0, magnificationFactor, magnificationFactor), rect, GraphicsUnit.Pixel);
            }

            g.DrawImage(magnifiedImage, mousePosition.X - magnificationFactor / 2, mousePosition.Y - magnificationFactor / 2);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _drawingSurface = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Invalidate();
        }

        private void NewFile_click(object sender, EventArgs e)
        {
            _drawingSurface = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Invalidate();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _drawingSurface = new Bitmap(openFileDialog.FileName);
                pictureBox1.Invalidate();
            }
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _drawingSurface.Save(saveFileDialog.FileName);
            }
        }

        private void ExitFile_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push((Bitmap)drawingSurface.Clone());
                drawingSurface = undoStack.Pop();
                pictureBox1.Invalidate();
            }
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push((Bitmap)drawingSurface.Clone());
                drawingSurface = redoStack.Pop();
                pictureBox1.Invalidate();
            }
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoomFactor += 0.1f;
            pictureBox1.Width = (int)(originalWidth * zoomFactor);
            pictureBox1.Height = (int)(originalHeight * zoomFactor);
            pictureBox1.Invalidate();
        }

        private void ZoomOut_Click_1(object sender, EventArgs e)
        {
            if (zoomFactor > 1.0f)
            {
                zoomFactor -= 0.1f;
                pictureBox1.Width = (int)(originalWidth * zoomFactor);
                pictureBox1.Height = (int)(originalHeight * zoomFactor);
                pictureBox1.Invalidate();
            }
        }

        private void Fill(int x, int y, Color fillColor)
        {
            if (x < 0 || x >= _drawingSurface.Width || y < 0 || y >= _drawingSurface.Height)
                return;

            Color originalColor = _drawingSurface.GetPixel(x, y);
            if (originalColor != fillColor)
            {
                _drawingSurface.SetPixel(x, y, fillColor);
                pictureBox1.Invalidate();

                Fill(x - 1, y, fillColor);
                Fill(x + 1, y, fillColor);
                Fill(x, y - 1, fillColor);
                Fill(x, y + 1, fillColor);
            }
        }

    }
}

