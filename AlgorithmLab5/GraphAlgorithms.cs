﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLab5
{
    public class GraphAlgorithms
    {
        private Graph graph;

        public GraphAlgorithms()
        {

        }

        public GraphAlgorithms(Graph graph)
        {
            this.graph = graph;
        }

        //Рекурсивный метод
        private void DFTRecursive(string node, Dictionary<string, bool> visited)
        {
			//Отмечаем полученный узел
			Console.WriteLine("Отмечаем узел {0}", node);
			visited[node] = true;

            //Выполняем подобное для каждого соседнего узла
            List<string> neighbours = graph.Nodes[node].Neighbours;
			Console.Write("Соседи узла {0}:", node);
			foreach (var e in neighbours)
			{
				Console.Write(" {0}", e);
			}
			Console.Write("\n------\n");
            foreach (var n in neighbours)
            {
                if (!visited[n]) DFTRecursive(n, visited);
            }
        }

		//Обход в глубину
		public void DFT(string node)
        {
			//Список посещенных узлов
			Dictionary<string, bool> visitedNodes = new();
            foreach (var e in graph.Nodes)
            {
                visitedNodes.Add(e.Key, false);
            }

			//Вызов рекурсивного метода
			Console.WriteLine("Обход в глубину\n" +
				"Начинаем с узла {0}", node);
            DFTRecursive(node, visitedNodes);
        }

        //Обход в ширину
        public void BFT(string node)
        {

            //Список посещенных узлов
            Dictionary<string, bool> visitedNodes = new();
            foreach (var e in graph.Nodes)
            {
                visitedNodes.Add(e.Key, false);
            }

            //Очередь узлов для посещения
            LinkedList<string> queue = new();

			Console.WriteLine("Обход в ширину\n" +
				"Начинаем с узла {0}", node);
			//Отмечаем первый узел и добавляем в очередь
			Console.WriteLine("Отмечаем узел {0}", node);
			visitedNodes[node] = true;
			Console.WriteLine("Добавляем узел {0} в очередь", node);
			queue.AddLast(node);
			Console.Write("Очередь:");
			foreach (var e in queue)
			{
				Console.Write(" {0}", e);
			}
			Console.Write("\n-----\n");

			while (queue.Any())
            {
				//Убираем первый узел в очереди
				node = queue.First();
				Console.WriteLine("Берём первый узел из очереди: {0}", node);
				queue.RemoveFirst();

                //Получаем список соседних узлов
				//Отмечаем их и добавляем в очередь
                List<string> neighbours = graph.Nodes[node].Neighbours;
				Console.Write("Соседи узла {0}:", node);
				foreach (var e in neighbours)
				{
					Console.Write(" {0}", e);
				}
				Console.Write("\n");

				foreach (var val in neighbours)
                {
                    if (!visitedNodes[val])
                    {
                        visitedNodes[val] = true;
                        queue.AddLast(val);
                    }
                }

				Console.Write("Очередь:");
				foreach (var e in queue)
				{
					Console.Write(" {0}", e);
				}
				Console.Write("\n------\n");
			}
        }



		//Максимальный поток
		public int FordFulkerson(Graph graph, string s, string t)
		{
			string u, v;

			//Граф остаточного потока
			Dictionary<string, int> rGraph = new();
			foreach(var l in graph.Links)
			{
				rGraph.Add(l.ToString(), l.Weight);
			}

			//Путь к стоку, заполняемый методом обхода в ширину
			Dictionary<string, string> path = new();

			//Обнуляем максимальный поток
			int maxFlow = 0;

			Console.WriteLine("Начинаем поиск максимального потока");
			Console.WriteLine("Из узла {0} в {1}", s, t);

			//Пока путь есть
			while (BFS(rGraph, s, t, path))
			{
				Console.WriteLine("Текущий максимальный поток: ", maxFlow);
				Console.Write("Текущий путь:");

				//Ищем минимальный поток у данного пути
				int pathFlow = int.MaxValue;
				for (v = t; v != s; v = path[v])
				{
					u = path[v];
					pathFlow = Math.Min(pathFlow, rGraph[ToLink(u, v)]);
					Console.Write(" " + ToLink(u, v) + "[" + rGraph[ToLink(u, v)] + "]");
				}
				Console.WriteLine("\nМинимальный поток у этого пути: {0}", pathFlow);

				//Уменьшаем пропускную способность
				for (v = t; v != s; v = path[v])
				{
					u = path[v];
					rGraph[ToLink(u, v)] -= pathFlow;
				}

				//Увеличиваем максимальный поток на поток отдельного пути
				maxFlow += pathFlow;

				Console.WriteLine("-----");
			}

			return maxFlow;
		}

		//Поиск обходом в ширину
		//Проверяет есть ли путь. Если да, то заполняет его
		private bool BFS(Dictionary<string, int> rGraph, string s, string t, Dictionary<string, string> path)
		{
			Dictionary<string, bool> visitedNodes = new();
			foreach (var e in graph.Nodes)
			{
				visitedNodes.Add(e.Key, false);
			}

			List<string> queue = new();
			queue.Add(s);
			visitedNodes[s] = true;
			path[s] = null;

			while (queue.Count != 0)
			{
				string u = queue[0];
				queue.RemoveAt(0);

				List<string> list = graph.Nodes[u].Neighbours;

				foreach (var v in list)
				{
					if (visitedNodes[v] == false
						&& rGraph[ToLink(u, v)] > 0)
					{
						if (v == t)
						{
							path[v] = u;
							return true;
						}
						queue.Add(v);
						path[v] = u;
						visitedNodes[v] = true;
					}
				}
			}

			return false;
		}

		private static string ToLink(string s, string t)
		{
			return s.ToString() + "-" + t.ToString();
		}
	}
}