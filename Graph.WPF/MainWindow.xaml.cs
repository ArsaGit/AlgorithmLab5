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

namespace Graph.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Ellipse test;
		TranslateTransform translateTransform;
		Field field;

		public MainWindow()
		{
			InitializeComponent();
			field = new(canvas);

			//DispatcherTimer timer = new DispatcherTimer();
			//timer.Interval = TimeSpan.FromMilliseconds(10);
			//timer.Tick += timer_Tick;
			//timer.Start();
			
			field.ArrangeInCircle(M_Window.Width, M_Window.Height);
			field.Draw();
		}

		//private void timer_Tick(object sender, EventArgs e)
		//{
		//	canvas.Children.Clear();
		//	//MoveCircle();
		//	//canvas.Children.Add(test);
		//	field.Arrange();
		//	field.points[0] = new(400, 400);
		//	field.points[1] = new(400, 500);
		//	field.Draw();
		//}
	}
}
