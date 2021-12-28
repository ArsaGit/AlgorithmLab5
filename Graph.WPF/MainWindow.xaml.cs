using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Graph.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		PointCollection points;
		List<(int p1, int p2)> links;
		int radius = 30;
		double L => Math.Sqrt(800 ^ 2 / points.Count);
		double tolerance = 10;
		double step = 50;


		public MainWindow()
		{
			InitializeComponent();
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

			CalculateMovement( points, links);
			
		}

		private void canvas_Loaded(object sender, RoutedEventArgs e)
		{
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

			CalculateMovement(points, links);
		}

		private void GeneratePoints()
		{
			Random random = new(1);

			for(int i = 0; i < 10; i++)
			{
				points.Add(new Point(random.Next(500), random.Next(500)));
			}
		}

		private void DrawNodes(PointCollection pc)
		{
			for(int i = 0; i < 10; i++)
			{
				Ellipse ellipse = new();
				ellipse.Width = radius;
				ellipse.Height = radius;
				ellipse.Fill = Brushes.White;
				ellipse.Stroke = Brushes.Black;
				ellipse.StrokeThickness = 1;
				Canvas.SetLeft(ellipse, pc[i].X);
				Canvas.SetTop(ellipse, pc[i].Y);
				canvas.Children.Add(ellipse);

				TextBlock text = new();
				text.Text = i.ToString();
				Canvas.SetLeft(text, pc[i].X + 5);
				Canvas.SetTop(text, pc[i].Y + 5);
				canvas.Children.Add(text);
			}
		}

		private void DrawLinks(PointCollection pc)
		{
			foreach(var l in links)
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

		private double CalculateDistance(Point p1, Point p2)
		{
			return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
		}

		private Vector GetRepulsion(Point p1, Point p2)
		{
			return Math.Pow(L, 2) / CalculateDistance(p1, p2) *
				(p1 - p2) / CalculateDistance(p1, p2);
		}

		private Vector GetAttraction(Point p1, Point p2)
		{
			return -Math.Pow(CalculateDistance(p1, p2), 2) / L * (p1 - p2) / CalculateDistance(p1, p2);
		}

		private void CalculateMovement(PointCollection pc, List<(int p1, int p2)> links)
		{
			PointCollection tempInitPos = new();

			do
			{
				foreach (var p in pc) tempInitPos.Add(p);

				for(int i = 0; i < pc.Count; i++)
				{
					Vector vector = new(0, 0);

					for(int j = 0; j < pc.Count; j++)
					{
						if (AreAdjacent(j, i)) vector += GetAttraction(pc[i], pc[j]);
						if (i != j) vector += GetRepulsion(pc[i], pc[j]);
						pc[i] = new(step * (vector.X / vector.Length), step * (vector.Y / vector.Length));
					}
				}

				DrawLinks(tempInitPos);
				DrawNodes(tempInitPos);
				

			} while (IsRunning(tempInitPos, pc));
		}

		private bool IsRunning(PointCollection oldCol, PointCollection newCol)
		{
			int count = 0;
			for(int i = 0; i < points.Count; i++)
			{
				if (CalculateDistance(newCol[i], oldCol[i]) < tolerance * L) count++;
			}

			return count != points.Count;
		}

		private bool AreAdjacent(int i, int j)
		{
			foreach(var l in links)
			{
				if ((l.p1 == i && l.p2 == j) ||
					(l.p1 == j && l.p2 == i)) return true;
			}

			return false;
		}
	}
}
