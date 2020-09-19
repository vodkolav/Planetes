using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PolygonCollision
{

    public partial class MainForm : Form
    {

        List<Polygon> polygons = new List<Polygon>();
        Polygon player;

        public MainForm()
        {
            InitializeComponent();
            Paint += new PaintEventHandler(Form1_Paint);
            KeyDown += new KeyEventHandler(Form1_KeyDown);

            KeyPreview = true;
            DoubleBuffered = true;

            Polygon p = new Polygon();
            //p.Points.Add(new Vector(100, 0));
            //p.Points.Add(new Vector(150, 50));
            //p.Points.Add(new Vector(100, 150));
            //p.Points.Add(new Vector(0, 100));

            p.Vertices.Add(new Vector(50, 50));
            p.Vertices.Add(new Vector(80, 50));
            p.Vertices.Add(new Vector(100, 60));
            p.Vertices.Add(new Vector(80, 70));
            p.Vertices.Add(new Vector(50, 70));
            p.Rotate(20);
            polygons.Add(p);

            p = new Polygon();
            p.Vertices.Add(new Vector(50, 50));
            p.Vertices.Add(new Vector(100, 0));
            p.Vertices.Add(new Vector(150, 150));
            p.Offset(80, 80);

            polygons.Add(p);

            p = new Polygon();
            p.Vertices.Add(new Vector(0, 50));
            p.Vertices.Add(new Vector(50, 0));
            p.Vertices.Add(new Vector(150, 80));
            p.Vertices.Add(new Vector(160, 200));
            p.Vertices.Add(new Vector(-10, 190));
            p.Offset(300, 300);

            polygons.Add(p);

            foreach (Polygon polygon in polygons) polygon.BuildEdges();

            player = polygons[0];
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            Vector p1;
            Vector p2;
            foreach (Polygon polygon in polygons)
            {
                for (int i = 0; i < polygon.Vertices.Count; i++)
                {
                    p1 = polygon.Vertices[i];
                    if (i + 1 >= polygon.Vertices.Count)
                    {
                        p2 = polygon.Vertices[0];
                    }
                    else
                    {
                        p2 = polygon.Vertices[i + 1];
                    }
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.DrawLine(new Pen(Color.Black), p1, p2);
                    e.Graphics.FillPolygon(Brushes.Blue, polygon.PointFs);
                }
            }

            int r = 5;
            e.Graphics.DrawEllipse(new Pen(Color.Red), player.Center.X - r, player.Center.Y - r, 2 * r, 2 * r);
            e.Graphics.DrawLine(new Pen(Color.Black), player.Center, player.Center + player.MTV * 100);
            Invalidate();
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int i = 14;
            Vector velocity = new Vector();

            switch (e.KeyValue)
            {

                case 32: //SPACE

                    break;

                case 38: // UP

                    velocity = new Vector(0, -i);
                    break;

                case 40: // DOWN

                    velocity = new Vector(0, i);
                    break;

                case 39: // RIGHT

                    velocity = new Vector(i, 0);
                    break;

                case 37: // LEFT

                    velocity = new Vector(-i, 0);
                    break;

            }

            Vector playerTranslation = velocity;

            foreach (Polygon polygon in polygons)
            {
                if (polygon == player) continue;

                PolygonCollisionResult r = player.Collides(polygon, velocity);

                if (r.WillIntersect)
                {
                    playerTranslation = velocity + r.MinimumTranslationVector;
                    //player.Wat = r.MinimumTranslationVector;
                    break;
                }
            }

            player.Offset(playerTranslation);

        }

    }

}