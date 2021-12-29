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
		PointCollection points;
		List<(int p1, int p2)> links;
		int radius = 30;
		double L => Math.Sqrt(500 ^ 2 / points.Count);
		double tolerance = 10;
		double step = 100;
		Canvas canvas;

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
				points.Add(new Point(random.Next(500), random.Next(500)));
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
				canvas.Children.Add(line);
			}
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


	}
}
