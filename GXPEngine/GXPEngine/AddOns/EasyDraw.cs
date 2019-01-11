namespace GXPEngine
{
    using System.Drawing;
    using System.Drawing.Text;

    /// <summary>
    /// Defines the CenterMode
    /// </summary>
    public enum CenterMode
    {/// <summary>
     /// Defines the Min
     /// </summary>
        Min,
        /// <summary>
        /// Defines the Center
        /// </summary>
        Center,
        /// <summary>
        /// Defines the Max
        /// </summary>
        Max
    }

    /// <summary>
    /// Creates an easy-to-use layer on top of .NET's System.Drawing methods.
	/// The API is inspired by Processing: internal states are maintained for font, fill/stroke color, etc., 
	/// and everything works with simple methods that have many overloads.
    /// </summary>
    public class EasyDraw : Canvas
    {
        /// <summary>
        /// Defines the defaultFont
        /// </summary>
        internal static Font defaultFont = new Font("Noto Sans", 15);

        /// <summary>
        /// Defines the HorizontalTextAlign
        /// </summary>
        public CenterMode HorizontalTextAlign = CenterMode.Min;

        /// <summary>
        /// Defines the VerticalTextAlign
        /// </summary>
        public CenterMode VerticalTextAlign = CenterMode.Max;

        /// <summary>
        /// Defines the HorizontalShapeAlign
        /// </summary>
        public CenterMode HorizontalShapeAlign = CenterMode.Center;

        /// <summary>
        /// Defines the VerticalShapeAlign
        /// </summary>
        public CenterMode VerticalShapeAlign = CenterMode.Center;

        /// <summary>
        /// Gets or sets the font
        /// </summary>
        public Font font { get; protected set; }

        /// <summary>
        /// Gets or sets the pen
        /// </summary>
        public Pen pen { get; protected set; }

        /// <summary>
        /// Gets or sets the brush
        /// </summary>
        public SolidBrush brush { get; protected set; }

        /// <summary>
        /// Defines the _stroke
        /// </summary>
        protected bool _stroke = true;

        /// <summary>
        /// Defines the _fill
        /// </summary>
        protected bool _fill = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="EasyDraw"/> class.
        /// </summary>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        public EasyDraw(int width, int height) : base(new Bitmap(width, height))
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EasyDraw"/> class.
        /// </summary>
        /// <param name="bitmap">The bitmap<see cref="System.Drawing.Bitmap"/></param>
        public EasyDraw(System.Drawing.Bitmap bitmap) : base(bitmap)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EasyDraw"/> class.
        /// </summary>
        /// <param name="filename">The filename<see cref="string"/></param>
        public EasyDraw(string filename) : base(filename)
        {
            Initialize();
        }

        /// <summary>
        /// The Initialize
        /// </summary>
        internal void Initialize()
        {
            pen = new Pen(Color.White, 1);
            brush = new SolidBrush(Color.White);
            font = defaultFont;
            if (!game.PixelArt)
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit; //AntiAlias;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            }
        }

        //////////// Setting Font
        /// <summary>
        /// The TextFont
        /// </summary>
        /// <param name="newFont">The newFont<see cref="Font"/></param>
        public void TextFont(Font newFont)
        {
            font = newFont;
        }

        /// <summary>
        /// The TextFont
        /// </summary>
        /// <param name="fontName">The fontName<see cref="string"/></param>
        /// <param name="pointSize">The pointSize<see cref="float"/></param>
        /// <param name="style">The style<see cref="FontStyle"/></param>
        public void TextFont(string fontName, float pointSize, FontStyle style = FontStyle.Regular)
        {
            font = new Font(fontName, pointSize, style);
        }

        /// <summary>
        /// The TextSize
        /// </summary>
        /// <param name="pointSize">The pointSize<see cref="float"/></param>
        public void TextSize(float pointSize)
        {
            font = new Font(font.OriginalFontName, pointSize, font.Style);
        }

        //////////// Setting Alignment for text, ellipses and rects
        /// <summary>
        /// The TextAlign
        /// </summary>
        /// <param name="horizontal">The horizontal<see cref="CenterMode"/></param>
        /// <param name="vertical">The vertical<see cref="CenterMode"/></param>
        public void TextAlign(CenterMode horizontal, CenterMode vertical)
        {
            HorizontalTextAlign = horizontal;
            VerticalTextAlign = vertical;
        }

        /// <summary>
        /// The ShapeAlign
        /// </summary>
        /// <param name="horizontal">The horizontal<see cref="CenterMode"/></param>
        /// <param name="vertical">The vertical<see cref="CenterMode"/></param>
        public void ShapeAlign(CenterMode horizontal, CenterMode vertical)
        {
            HorizontalShapeAlign = horizontal;
            VerticalShapeAlign = vertical;
        }

        //////////// Setting Stroke
        /// <summary>
        /// The NoStroke
        /// </summary>
        public void NoStroke()
        {
            _stroke = false;
        }

        /// <summary>
        /// The Stroke
        /// </summary>
        /// <param name="newColor">The newColor<see cref="Color"/></param>
        /// <param name="alpha">The alpha<see cref="int"/></param>
        public void Stroke(Color newColor, int alpha = 255)
        {
            pen.Color = Color.FromArgb(alpha, newColor);
            _stroke = true;
        }

        /// <summary>
        /// The Stroke
        /// </summary>
        /// <param name="grayScale">The grayScale<see cref="int"/></param>
        /// <param name="alpha">The alpha<see cref="int"/></param>
        public void Stroke(int grayScale, int alpha = 255)
        {
            pen.Color = Color.FromArgb(alpha, grayScale, grayScale, grayScale);
            _stroke = true;
        }

        /// <summary>
        /// The Stroke
        /// </summary>
        /// <param name="red">The red<see cref="int"/></param>
        /// <param name="green">The green<see cref="int"/></param>
        /// <param name="blue">The blue<see cref="int"/></param>
        /// <param name="alpha">The alpha<see cref="int"/></param>
        public void Stroke(int red, int green, int blue, int alpha = 255)
        {
            pen.Color = Color.FromArgb(alpha, red, green, blue);
            _stroke = true;
        }

        /// <summary>
        /// The StrokeWeight
        /// </summary>
        /// <param name="width">The width<see cref="float"/></param>
        public void StrokeWeight(float width)
        {
            pen.Width = width;
            _stroke = true;
        }

        //////////// Setting Fill
        /// <summary>
        /// The NoFill
        /// </summary>
        public void NoFill()
        {
            _fill = false;
        }

        /// <summary>
        /// The Fill
        /// </summary>
        /// <param name="newColor">The newColor<see cref="Color"/></param>
        /// <param name="alpha">The alpha<see cref="int"/></param>
        public void Fill(Color newColor, int alpha = 255)
        {
            brush.Color = Color.FromArgb(alpha, newColor);
            _fill = true;
        }

        /// <summary>
        /// The Fill
        /// </summary>
        /// <param name="grayScale">The grayScale<see cref="int"/></param>
        /// <param name="alpha">The alpha<see cref="int"/></param>
        public void Fill(int grayScale, int alpha = 255)
        {
            brush.Color = Color.FromArgb(alpha, grayScale, grayScale, grayScale);
            _fill = true;
        }

        /// <summary>
        /// The Fill
        /// </summary>
        /// <param name="red">The red<see cref="int"/></param>
        /// <param name="green">The green<see cref="int"/></param>
        /// <param name="blue">The blue<see cref="int"/></param>
        /// <param name="alpha">The alpha<see cref="int"/></param>
        public void Fill(int red, int green, int blue, int alpha = 255)
        {
            brush.Color = Color.FromArgb(alpha, red, green, blue);
            _fill = true;
        }

        //////////// Clear
        /// <summary>
        /// The Clear
        /// </summary>
        /// <param name="newColor">The newColor<see cref="Color"/></param>
        public void Clear(Color newColor)
        {
            graphics.Clear(newColor);
        }

        /// <summary>
        /// The Clear
        /// </summary>
        /// <param name="grayScale">The grayScale<see cref="int"/></param>
        public void Clear(int grayScale)
        {
            graphics.Clear(Color.FromArgb(255, grayScale, grayScale, grayScale));
        }

        /// <summary>
        /// The Clear
        /// </summary>
        /// <param name="red">The red<see cref="int"/></param>
        /// <param name="green">The green<see cref="int"/></param>
        /// <param name="blue">The blue<see cref="int"/></param>
        public void Clear(int red, int green, int blue)
        {
            graphics.Clear(Color.FromArgb(255, red, green, blue));
        }

        //////////// Draw & measure Text
        /// <summary>
        /// The Text
        /// </summary>
        /// <param name="text">The text<see cref="string"/></param>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        public void Text(string text, float x, float y)
        {
            float twidth, theight;
            TextDimensions(text, out twidth, out theight);
            if (HorizontalTextAlign == CenterMode.Max)
            {
                x -= twidth;
            }
            else if (HorizontalTextAlign == CenterMode.Center)
            {
                x -= twidth / 2;
            }
            if (VerticalTextAlign == CenterMode.Max)
            {
                y -= theight;
            }
            else if (VerticalTextAlign == CenterMode.Center)
            {
                y -= theight / 2;
            }
            graphics.DrawString(text, font, brush, x, y); //left+BoundaryPadding/2,top+BoundaryPadding/2);
           
        }

        /// <summary>
        /// The TextWidth
        /// </summary>
        /// <param name="text">The text<see cref="string"/></param>
        /// <returns>The <see cref="float"/></returns>
        public float TextWidth(string text)
        {
            SizeF size = graphics.MeasureString(text, font);
            return size.Width;
        }

        /// <summary>
        /// The TextHeight
        /// </summary>
        /// <param name="text">The text<see cref="string"/></param>
        /// <returns>The <see cref="float"/></returns>
        public float TextHeight(string text)
        {
            SizeF size = graphics.MeasureString(text, font);
            return size.Height;
        }

        /// <summary>
        /// The TextDimensions
        /// </summary>
        /// <param name="text">The text<see cref="string"/></param>
        /// <param name="width">The width<see cref="float"/></param>
        /// <param name="height">The height<see cref="float"/></param>
        public void TextDimensions(string text, out float width, out float height)
        {
            SizeF size = graphics.MeasureString(text, font);
            width = size.Width;
            height = size.Height;
        }

        //////////// Draw Shapes
        /// <summary>
        /// The Rect
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// <param name="width">The width<see cref="float"/></param>
        /// <param name="height">The height<see cref="float"/></param>
        public void Rect(float x, float y, float width, float height)
        {
            ShapeAlign(ref x, ref y, width, height);
            if (_fill)
            {
                graphics.FillRectangle(brush, x, y, width, height);
            }
            if (_stroke)
            {
                graphics.DrawRectangle(pen, x, y, width, height);
            }
        }

        /// <summary>
        /// The Ellipse
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// <param name="width">The width<see cref="float"/></param>
        /// <param name="height">The height<see cref="float"/></param>
        public void Ellipse(float x, float y, float width, float height)
        {
            ShapeAlign(ref x, ref y, width, height);
            if (_fill)
            {
                graphics.FillEllipse(brush, x, y, width, height);
            }
            if (_stroke)
            {
                graphics.DrawEllipse(pen, x, y, width, height);
            }
        }

        /// <summary>
        /// The Arc
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// <param name="width">The width<see cref="float"/></param>
        /// <param name="height">The height<see cref="float"/></param>
        /// <param name="startAngleDegrees">The startAngleDegrees<see cref="float"/></param>
        /// <param name="sweepAngleDegrees">The sweepAngleDegrees<see cref="float"/></param>
        public void Arc(float x, float y, float width, float height, float startAngleDegrees, float sweepAngleDegrees)
        {
            ShapeAlign(ref x, ref y, width, height);
            if (_fill)
            {
                graphics.FillPie(brush, x, y, width, height, startAngleDegrees, sweepAngleDegrees);
            }
            if (_stroke)
            {
                graphics.DrawArc(pen, x, y, width, height, startAngleDegrees, sweepAngleDegrees);
            }
        }

        /// <summary>
        /// The Line
        /// </summary>
        /// <param name="x1">The x1<see cref="float"/></param>
        /// <param name="y1">The y1<see cref="float"/></param>
        /// <param name="x2">The x2<see cref="float"/></param>
        /// <param name="y2">The y2<see cref="float"/></param>
        public void Line(float x1, float y1, float x2, float y2)
        {
            if (_stroke)
            {
                graphics.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        /// <summary>
        /// The Triangle
        /// </summary>
        /// <param name="x1">The x1<see cref="float"/></param>
        /// <param name="y1">The y1<see cref="float"/></param>
        /// <param name="x2">The x2<see cref="float"/></param>
        /// <param name="y2">The y2<see cref="float"/></param>
        /// <param name="x3">The x3<see cref="float"/></param>
        /// <param name="y3">The y3<see cref="float"/></param>
        public void Triangle(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            Polygon(x1, y1, x2, y2, x3, y3);
        }

        /// <summary>
        /// The Quad
        /// </summary>
        /// <param name="x1">The x1<see cref="float"/></param>
        /// <param name="y1">The y1<see cref="float"/></param>
        /// <param name="x2">The x2<see cref="float"/></param>
        /// <param name="y2">The y2<see cref="float"/></param>
        /// <param name="x3">The x3<see cref="float"/></param>
        /// <param name="y3">The y3<see cref="float"/></param>
        /// <param name="x4">The x4<see cref="float"/></param>
        /// <param name="y4">The y4<see cref="float"/></param>
        public void Quad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            Polygon(x1, y1, x2, y2, x3, y3, x4, y4);
        }

        /// <summary>
        /// The Polygon
        /// </summary>
        /// <param name="pt">The pt<see cref="float[]"/></param>
        public void Polygon(params float[] pt)
        {
            PointF[] pts = new PointF[pt.Length / 2];
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i] = new PointF(pt[2 * i], pt[2 * i + 1]);
            }
            Polygon(pts);
        }

        /// <summary>
        /// The Polygon
        /// </summary>
        /// <param name="pts">The pts<see cref="PointF[]"/></param>
        public void Polygon(PointF[] pts)
        {
            if (_fill)
            {
                graphics.FillPolygon(brush, pts);
            }
            if (_stroke)
            {
                graphics.DrawPolygon(pen, pts);
            }
        }

        /// <summary>
        /// The ShapeAlign
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// <param name="width">The width<see cref="float"/></param>
        /// <param name="height">The height<see cref="float"/></param>
        protected void ShapeAlign(ref float x, ref float y, float width, float height)
        {
            if (HorizontalShapeAlign == CenterMode.Max)
            {
                x -= width;
            }
            else if (HorizontalShapeAlign == CenterMode.Center)
            {
                x -= width / 2;
            }
            if (VerticalShapeAlign == CenterMode.Max)
            {
                y -= height;
            }
            else if (VerticalShapeAlign == CenterMode.Center)
            {
                y -= height / 2;
            }
        }
    }
}
