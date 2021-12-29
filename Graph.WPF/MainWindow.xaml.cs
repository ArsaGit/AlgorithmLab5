using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AlgorithmLab5;

namespace GraphProject.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Field field;
		private string input;
		private Action action;
		private WriteLog WriteLog;

		enum Action
		{
			AddNode, AddLink, RemoveNode, RemoveLink, BFT, DFT, MaxFlow
		}

		public MainWindow()
		{
			InitializeComponent();
			WriteLog = WriteInTextBox;

			

			FileWorker fileWorker = new();
			string[] graphData = fileWorker.ReadFile("input.csv");
			Graph graph = new();
			graph = GraphSerializer.Deserialize(graphData, GraphSerializer.GraphStorageType.MyType);
			field = new(canvas, WriteLog, graph);

			GraphAlgorithms algorithms = new(graph, WriteLog);

			//WriteLog("Максимальный поток равен "
			//				  + algorithms.FordFulkerson(graph, "0", "4") + "\n");


			field.ArrangeInCircle(M_Window.Width * 2 / 3, M_Window.Height);
			field.Draw();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			canvas.Children.Clear();
			log.Text = "";
			input = textBox.Text;

			switch (action)
			{
				case Action.AddNode:
					field.AddNode(input);
					break;
				case Action.AddLink:
					field.AddLink(input);
					break;
					break;
				case Action.RemoveNode:
					field.RemoveNode(input);
					break;
				case Action.RemoveLink:
					field.RemoveLink(input);
					break;
				case Action.BFT:
					field.BFT(input);
					break;
				case Action.DFT:
					field.DFT(input); 
					break;
				case Action.MaxFlow:
					field.MaxFlow(input);
					break;
				default:
					throw new Exception();
			}

			field.ArrangeInCircle(M_Window.Width * 2 / 3, M_Window.Height);
			field.Draw();
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			action = (Action)comboBox.SelectedIndex;
		}

		public void WriteInTextBox(string text)
		{
			log.Text += text;
		}
	}
}
