using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graph.WPF
{
	public class Field
	{
		public PointCollection points;
		List<(int p1, int p2)> links;
		int radius = 30;
		double L => Math.Sqrt(500 ^ 2 / points.Count);
		double tolerance = 10;
		double step = 100;
		Canvas canvas;

		private const double ATTRACTION_CONSTANT = 0.1;     // spring constant
		private const double REPULSION_CONSTANT = 100;    // charge constant

		private const double DEFAULT_DAMPING = -0.1;		//0.5
		private const int DEFAULT_SPRING_LENGTH = 100;
		private const int DEFAULT_MAX_ITERATIONS = 500;

		public Field(Canvas canvas)
		{
			this.canvas = canvas;
			points = new();

			links = new();
			links.Add((0, 1));
			links.Add((0, 2));
			links.Add((1, 2));
			links.Add((1, 3));
			links.Add((2, 5));
			links.Add((4, 6));
			links.Add((6, 7));
			links.Add((7, 8));
			links.Add((8, 9));
			links.Add((7, 2));

			GeneratePoints();
		}

		private void GeneratePoints()
		{
			Random random = new(1);

			for (int i = 0; i < 10; i++)
			{
				points.Add(new Point(random.Next(600), random.Next(600)));
			}
		}

		private void DrawNodes(PointCollection pc)
		{
			for (int i = 0; i < 10; i++)
			{
				Ellipse ellipse = new();
				ellipse.Width = radius;
				ellipse.Height = radius;
				ellipse.Fill = Brushes.White;
				ellipse.Stroke = Brushes.Black;
				ellipse.StrokeThickness = 1;
				ellipse.RenderTransform = new TranslateTransform(pc[i].X, pc[i].Y);
				canvas.Children.Add(ellipse);

				TextBlock text = new();
				text.Text = i.ToString();
				text.RenderTransform = new TranslateTransform(pc[i].X + 5, pc[i].Y + 5);
				canvas.Children.Add(text);
			}
		}

		private void DrawLinks(PointCollection pc)
		{
			foreach (var l in links)
			{
				Line line = new();
				line.X1 = pc[l.p1].X + radius / 2;
				line.Y1 = pc[l.p1].Y + radius / 2;
				line.X2 = pc[l.p2].X + radius / 2;
				line.Y2 = pc[l.p2].Y + radius / 2;
				line.Stroke = Brushes.Black;
				line.StrokeThickness = 2;
				canvas.Children.Add(line);

				DrawArrow(points[l.p1].X + radius / 2, points[l.p1].Y + radius / 2,
					points[l.p2].X + radius / 2, points[l.p2].Y + radius / 2);
			}
		}

		private void DrawArrow(double x1, double y1, double x2, double y2)
		{
			double d = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

			double X = x2 - x1;
			double Y = y2 - y1;

			double X3 = x2 - (X / d) * 25;
			double Y3 = y2 - (Y / d) * 25;

			double Xp = y2 - y1;
			double Yp = x1 - x2;

			double X4 = X3 + (Xp / d) * 5;
			double Y4 = Y3 + (Yp / d) * 5;
			double X5 = X3 - (Xp / d) * 5;
			double Y5 = Y3 - (Yp / d) * 5;

			Line line = new Line();

			line = new Line
			{
				Stroke = Brushes.Black,
				X1 = x2 - (X / d) * 10,
				Y1 = y2 - (Y / d) * 10,
				X2 = X4,
				Y2 = Y4
			};
			canvas.Children.Add(line);

			line = new Line
			{
				Stroke = Brushes.Black,
				X1 = x2 - (X / d) * 10,
				Y1 = y2 - (Y / d) * 10,
				X2 = X5,
				Y2 = Y5
			};
			canvas.Children.Add(line);
		}

		public void Draw()
		{
			DrawLinks(points);
			DrawNodes(points);
		}




		private double CalculateDistance(Point p1, Point p2)
		{
			return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
		}

		private Vector GetRepulsion(Point p1, Point p2)
		{
			return -Math.Pow(L, 2) / CalculateDistance(p1, p2) *
				(p1 - p2) / CalculateDistance(p1, p2);
		}

		private Vector GetAttraction(Point p1, Point p2)
		{
			return -Math.Pow(CalculateDistance(p1, p2), 2) / L * (p1 - p2) / CalculateDistance(p1, p2);
		}

		public void CalculateMovement()
		{
			for (int i = 0; i < points.Count; i++)
			{
				Vector vector = new(800, 800);

				for (int j = 0; j < points.Count; j++)
				{
					if (AreAdjacent(j, i)) vector += GetAttraction(points[i], points[j]);
					if (i != j) vector += GetRepulsion(points[i], points[j]);
					points[i] = new(step * (vector.X / vector.Length), step * (vector.Y / vector.Length));
				}
			}
		}

		private bool AreAdjacent(int i, int j)
		{
			foreach (var l in links)
			{
				if ((l.p1 == i && l.p2 == j) ||
					(l.p1 == j && l.p2 == i)) return true;
			}

			return false;
		}

		private PointCollection GetAdjacent(int i)
		{
			PointCollection pts = new();
			foreach (var e in links)
			{
				if (e.p1 == i) pts.Add(points[e.p2]);
				if (e.p2 == i) pts.Add(points[e.p1]);
			}
			return pts;
		}

		private bool IsAdjacentTo(int i, Point p)
		{
			int j = Int32.MaxValue;
			for (int k = 0; k < points.Count; k++)
			{
				if (points[k] == p) j = k;
			}

			foreach (var l in links)
			{
				if ((l.p1 == i && l.p2 == j) ||
					(l.p1 == j && l.p2 == i)) return true;
			}

			return false;
		}




		private double GetBearingAngle(Point start, Point end)
		{
			Point half = new Point(start.X + ((end.X - start.X) / 2), start.Y + ((end.Y - start.Y) / 2));

			double diffX = (double)(half.X - start.X);
			double diffY = (double)(half.Y - start.Y);

			if (diffX == 0) diffX = 0.001;
			if (diffY == 0) diffY = 0.001;

			double angle;
			if (Math.Abs(diffX) > Math.Abs(diffY))
			{
				angle = Math.Tanh(diffY / diffX) * (180.0 / Math.PI);
				if (((diffX < 0) && (diffY > 0)) || ((diffX < 0) && (diffY < 0))) angle += 180;
			}
			else
			{
				angle = Math.Tanh(diffX / diffY) * (180.0 / Math.PI);
				if (((diffY < 0) && (diffX > 0)) || ((diffY < 0) && (diffX < 0))) angle += 180;
				angle = (180 - (angle + 90));
			}

			return angle;
		}

		public static int CalcDistance(Point a, Point b)
		{
			double xDist = (a.X - b.X);
			double yDist = (a.Y - b.Y);
			return (int)Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
		}

		private Vector CalcAttractionForce(Point x, Point y, double springLength)
		{
			int proximity = Math.Max(CalcDistance(x, y), 1);

			// Hooke's Law: F = -kx
			double force = ATTRACTION_CONSTANT * Math.Max(proximity - springLength, 0);
			double angle = GetBearingAngle(x, y);

			return new Vector(force, angle);
		}

		private Vector CalcRepulsionForce(Point x, Point y)
		{
			int proximity = Math.Max(CalcDistance(x, y), 1);

			// Coulomb's Law: F = k(Qq/r^2)
			double force = -(REPULSION_CONSTANT / Math.Pow(proximity, 2));
			double angle = GetBearingAngle(x, y);

			return new Vector(force, angle);
		}

		public void Arrange()
		{
			Arrange(DEFAULT_DAMPING, DEFAULT_SPRING_LENGTH, DEFAULT_MAX_ITERATIONS, true);
		}

		public void Arrange(double damping, int springLength, int maxIterations, bool deterministic)
		{
			// random starting positions can be made deterministic by seeding System.Random with a constant
			Random rnd = deterministic ? new Random(0) : new Random();

			// copy nodes into an array of metadata and randomise initial coordinates for each node
			NodeLayoutInfo[] layout = new NodeLayoutInfo[points.Count];
			for (int i = 0; i < points.Count; i++)
			{
				layout[i] = new NodeLayoutInfo(points[i], new Vector(), new Point(0,0));
			}

			double totalDisplacement = 0;

			for (int i = 0; i < layout.Length; i++)
			{
				NodeLayoutInfo current = layout[i];

				// express the node's current position as a vector, relative to the origin
				Vector currentPosition = new Vector(CalcDistance(new Point(0,0), current.Node), GetBearingAngle(new Point(0, 0), current.Node));
				Vector netForce = new Vector(0, 0);

				// determine repulsion between nodes
				foreach (var other in points)
				{
					if (other != current.Node) netForce += CalcRepulsionForce(current.Node, other);
				}

				// determine attraction caused by connections
				foreach (var child in GetAdjacent(i))
				{
					netForce += CalcAttractionForce(current.Node, child, springLength);
				}
				foreach (var parent in points)
				{
					if (IsAdjacentTo(i, parent)) netForce += CalcAttractionForce(current.Node, parent, springLength);
				}

				// apply net force to node velocity
				current.Velocity = (current.Velocity + netForce) * damping;

				// apply velocity to node position
				current.NextPosition = new Point(currentPosition.X + current.Velocity.X,
					currentPosition.Y + current.Velocity.Y);
			}

			// move nodes to resultant positions (and calculate total displacement)
			for (int i = 0; i < layout.Length; i++)
			{
				NodeLayoutInfo current = layout[i];

				totalDisplacement += CalcDistance(current.Node, current.NextPosition);
				current.Node = current.NextPosition;
			}

			for(int i = 0; i < points.Count; i++)
			{
				points[i] = layout[i].Node;
			}
		}

		private class NodeLayoutInfo
		{

			public Point Node;           // reference to the node in the simulation
			public Vector Velocity;     // the node's current velocity, expressed in vector form
			public Point NextPosition;  // the node's position after the next iteration

			/// <summary>
			/// Initialises a new instance of the Diagram.NodeLayoutInfo class, using the specified parameters.
			/// </summary>
			/// <param name="node"></param>
			/// <param name="velocity"></param>
			/// <param name="nextPosition"></param>
			public NodeLayoutInfo(Point node, Vector velocity, Point nextPosition)
			{
				Node = node;
				Velocity = velocity;
				NextPosition = nextPosition;
			}
		}

		public void ArrangeInCircle(double width, double height)
		{
			for (int i = 0; i < 10; i++)
			{
				points[i] = new(0.4 * width * Math.Cos(i * 2 * Math.PI / 10) + width * 0.475,
					0.4 * height * Math.Sin(i * 2 * Math.PI / 10) + height * 0.475);
			}
		}
	}
}

