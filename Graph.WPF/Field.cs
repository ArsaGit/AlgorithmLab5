using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AlgorithmLab5;

namespace GraphProject.WPF
{
	public class Field
	{
		Dictionary<string, Point> points;
		Graph graph;
		GraphAlgorithms algorithms;
		int radius = 30;
		Canvas canvas;

		private int strokeThickness = 2;
		private SolidColorBrush defaultColor = Brushes.Black;
		private SolidColorBrush selectedColor = Brushes.Red;

		public Field(Canvas canvas, WriteLog WriteLog, Graph graph)
		{
			this.canvas = canvas;
			this.graph = graph;
			algorithms = new(graph, WriteLog);
			points = new();
			//GeneratePoints();
		}



		//private void GeneratePoints()
		//{
		//	Random random = new(1);

		//	for (int i = 0; i < 10; i++)
		//	{
		//		points.Add(new Point(random.Next(600), random.Next(600)));
		//	}
		//}

		private void LoadGraph()
		{

		}

		public void AddNode(string name)
		{
			graph.AddNode(name);
		}

		public void AddLink(string link)
		{
			graph.AddLink(link);
		}

		public void RemoveNode(string name)
		{
			graph.RemoveNode(name);
			points.Remove(name);
		}

		public void RemoveLink(string link)
		{
			graph.RemoveLink(link);
		}

		public void BFT(string node)
		{
			algorithms.BFT(node);
		}

		public void DFT(string node)
		{
			algorithms.DFT(node);
		}

		public void MaxFlow(string pair)
		{
			string[] arr = pair.Split('-');

			int temp = algorithms.FordFulkerson(graph, arr[0], arr[1]);
		}

		private void DrawNodes(Graph graph, Dictionary<string, Point> pc)
		{
			foreach(var k in pc.Keys)
			{
				Ellipse ellipse = new();
				ellipse.Width = radius;
				ellipse.Height = radius;
				ellipse.Fill = Brushes.White;
				ellipse.Stroke = defaultColor;
				ellipse.StrokeThickness = strokeThickness;
				ellipse.RenderTransform = new TranslateTransform(pc[k].X, pc[k].Y);
				canvas.Children.Add(ellipse);

				TextBlock text = new();
				text.FontSize = 16;
				text.Text = graph.Nodes[k].Name;
				text.RenderTransform = new TranslateTransform(pc[k].X + 5, pc[k].Y + 4);
				canvas.Children.Add(text);
			}
		}

		private void DrawLinks(Graph graph, Dictionary<string, Point> pc)
		{
			foreach (var l in graph.Links)
			{
				Line line = new();
				line.X1 = pc[l.Source].X + radius / 2;
				line.Y1 = pc[l.Source].Y + radius / 2;
				line.X2 = pc[l.Target].X + radius / 2;
				line.Y2 = pc[l.Target].Y + radius / 2;
				line.Stroke = defaultColor;
				line.StrokeThickness = strokeThickness;
				canvas.Children.Add(line);

				TextBlock text = new();
				text.FontSize = 16;
				text.Text = l.Weight.ToString();
				text.RenderTransform = new TranslateTransform((pc[l.Target].X + pc[l.Source].X + radius) / 2,
					(pc[l.Target].Y + pc[l.Source].Y + radius) / 2);
				canvas.Children.Add(text);

				DrawArrow(points[l.Source].X + radius / 2, points[l.Source].Y + radius / 2,
					points[l.Target].X + radius / 2, points[l.Target].Y + radius / 2);
			}
		}

		private bool AreEqual(Link l1, Link l2)
		{
			return (l1.Source == l2.Source && l1.Target == l2.Target)
				|| (l1.Source == l2.Target && l1.Target == l2.Source);
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
			line.StrokeThickness = strokeThickness;

			line = new Line
			{
				Stroke = Brushes.Black,
				X1 = x2 - (X / d) * 10,
				Y1 = y2 - (Y / d) * 10,
				X2 = X4,
				Y2 = Y4,
				StrokeThickness = strokeThickness
		};
			canvas.Children.Add(line);

			line = new Line
			{
				Stroke = Brushes.Black,
				X1 = x2 - (X / d) * 10,
				Y1 = y2 - (Y / d) * 10,
				X2 = X5,
				Y2 = Y5,
				StrokeThickness = strokeThickness
		};
			canvas.Children.Add(line);
		}

		public void Draw()
		{
			DrawLinks(graph, points);
			DrawNodes(graph, points);
		}

		public void ArrangeInCircle(double width, double height)
		{
			int i = 0;
			foreach(var e in graph.Nodes)
			{
				if(!points.ContainsKey(e.Key))points.Add(e.Key, new(0.4 * width * Math.Cos(i * 2 * Math.PI / graph.Nodes.Count) + width * 0.475,
					0.4 * height * Math.Sin(i * 2 * Math.PI / graph.Nodes.Count) + height * 0.475));
				i++;
			}
		}
	}
}

