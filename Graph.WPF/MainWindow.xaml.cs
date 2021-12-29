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

			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromMilliseconds(100);
			timer.Tick += timer_Tick;
			timer.Start();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			canvas.Children.Clear();
			//MoveCircle();
			//canvas.Children.Add(test);
			field.CalculateMovement();
			field.Draw();
		}

		private void MoveCircle()
		{
			translateTransform.X += 1;
			translateTransform.Y += 1;
			test.RenderTransform = translateTransform;
		}
	}
}
