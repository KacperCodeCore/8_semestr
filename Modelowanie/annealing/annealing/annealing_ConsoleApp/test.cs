// namespace TSP_Annealing
// {
//     class TSP_Annealing_Program
//     {
//         static void Main(string[] args)
//         {
//             int nCities = 60;
//             Random rnd = new Random(2);
//             int maxIter = 200000;
//             double startTemperature = 10000.0;
//             double alpha = 0.95;

//             (int[] soln, int iteration) = Solve(nCities, rnd, maxIter, startTemperature, alpha);
//             Console.WriteLine($"Finished solve({iteration}) ");

//             Console.WriteLine("\nBest solution found: ");
//             ShowArray(soln);
//             double dist = TotalDist(soln);
//             Console.WriteLine("\nTotal distance = " +  dist.ToString("F1"));

//             Console.ReadLine();
//         } 

//         static double TotalDist(int[] route)
//         {
//             double d = 0.0;  // total distance between cities
//             int n = route.Length;
//             for (int i = 0; i < n - 1; ++i)
//             {
//                 if (route[i] < route[i + 1])
//                     d += (route[i + 1] - route[i]) * 1.0;
//                 else
//                     d += (route[i] - route[i + 1]) * 1.5;
//             }
//             return d;
//         }

//         static double Error(int[] route)
//         {
//             int n = route.Length;
//             double d = TotalDist(route);
//             double minDist = n - 1;
//             return d - minDist;
//         }

//         static int[] Adjacent(int[] route, Random rnd)
//         {
//             int n = route.Length;
//             int[] result = (int[])route.Clone();  // shallow is OK
//             int i = rnd.Next(0, n); int j = rnd.Next(0, n);
//             int tmp = result[i];
//             result[i] = result[j]; result[j] = tmp;
//             return result;
//         }

//         static void Shuffle(int[] route, Random rnd)
//         {
//             // Fisher-Yates algorithm
//             int n = route.Length;
//             for (int i = 0; i < n; ++i)
//             {
//                 int rIndx = rnd.Next(i, n);
//                 int tmp = route[rIndx];
//                 route[rIndx] = route[i];
//                 route[i] = tmp;
//             }
//         }

//         static void ShowArray(int[] arr)
//         {
//             int n = arr.Length;
//             Console.Write("[ ");
//             for (int i = 0; i < n; ++i)
//                 Console.Write(arr[i].ToString().PadLeft(2) + " ");
//             Console.WriteLine(" ]");
//         }

//         static (int[], int totalIterations) Solve(int nCities, Random rnd, int maxIter, double startTemperature, double alpha)
//         {
//             double currTemperature = startTemperature;
//             int[] soln = new int[nCities];
//             for (int i = 0; i < nCities; ++i) { soln[i] = i; }
//             Shuffle(soln, rnd);
//             Console.WriteLine("Initial guess: ");
//             ShowArray(soln);

//             double err = Error(soln);
//             int iteration = 0;
//             while (iteration < maxIter && err > 0.0)
//             {
//                 int[] adjRoute = Adjacent(soln, rnd);
//                 double adjErr = Error(adjRoute);
//                 if (adjErr < err)  // better route 
//                 {
//                     soln = adjRoute; err = adjErr;
//                 }
//                 else
//                 {
//                     double acceptProb =
//                       Math.Exp((err - adjErr) / currTemperature);
//                     double p = rnd.NextDouble(); // corrected
//                     if (p < acceptProb)  // accept anyway
//                     {
//                         soln = adjRoute; err = adjErr;
//                     }
//                 }

//                 if (currTemperature < 0.00001)
//                     currTemperature = 0.00001;
//                 else
//                     currTemperature *= alpha;
//                 ++iteration;
//             }

//             return (soln, iteration);
//         }

//     }
// }
// Edit : I don't appear to be having much luck with this. As suggested, I tried writing the example again, but this time in 2D. To get the next route, I followed the suggestion in this tutorial https://youtu.be/AEeYp5VtI08?t=1174 which picks a random segment and reverses it.

// The example seems to work when I set the random seed to 2 and finds the shortest distance of ~1695, but any other seed results in it getting stuck with distances much greater.

// namespace TSP_Annealing
// {
//     public class Map
//     {
//         public const int Size = 1000;
//         public List<Point> Cities = new();

//         public Map(int cities)
//         {
//             var r = new Random(0);
//             for (int i = 0; i < cities; i++)
//             {
//                 int x = r.Next(0, Size);
//                 int y = r.Next(0, Size);

//                 Cities.Add(new Point(x, y));
//             }
//         }
//     }

//     public class TSM
//     {
//         private Map _map = new Map(8);
//         private const int _maxIterations = 20000;
//         private const double _startTemperature = 10000;
//         private const double _alpha = 0.95;
//         private Random _rnd = new Random(2);

//         public void Solve()
//         {
//             double currTemp = _startTemperature;
//             var shortestRoute = Shuffle(Enumerable.Range(0, _map.Cities.Count).ToArray());
//             double shortestDistance = ComputeDistance(shortestRoute);

//             int iteration = 0;

//             while (iteration < _maxIterations)
//             {
//                 int[] nextRoute = NextRoute(shortestRoute);
//                 double nextDistance = ComputeDistance(nextRoute);

//                 if (nextDistance < shortestDistance)
//                 {
//                     shortestRoute = nextRoute;
//                     shortestDistance = nextDistance;
//                 }
//                 else
//                 {
//                     double acceptProb = Math.Exp(-(nextDistance - shortestDistance) / currTemp);
//                     double p = _rnd.NextDouble();
//                     if (p < acceptProb)
//                     {
//                         shortestRoute = nextRoute;
//                         shortestDistance = nextDistance;
//                     }
//                 }

//                 if (currTemp < 0.00001)
//                     currTemp = 0.00001;
//                 else
//                     currTemp *= _alpha;

//                 ++iteration;
//             }

//             Debug.WriteLine($"Shortest : {shortestDistance}, Iterations: {iteration}");
//         }

//         private double ComputeDistance(int[] route)
//         {
//             double total = 0.0;

//             for (int i = 0; i < route.Length - 1; ++i)
//             {
//                 Point a = _map.Cities[route[i]];
//                 Point b = _map.Cities[route[i + 1]];
//                 double x = b.X - a.X;
//                 double y = b.Y - a.Y;
//                 double length = Math.Sqrt(x * x + y * y);
//                 total += length;
//             }

//             return total;
//         }

//         private int[] NextRoute(int[] route)
//         {
//             // pick two random points to define a segment
//             int p1 = _rnd.Next(0, route.Length);
//             int p2 = _rnd.Next(p1, route.Length);

//             while ((p2 - p1) <= 1)
//             {
//                 p1 = _rnd.Next(0, route.Length);
//                 p2 = _rnd.Next(p1, route.Length);
//             }

//             // reverse the segment
//             var copy = (int[])route.Clone();
//             for (int i = 0; i < route.Length; ++i)
//             {
//                 int n = p1 + i;
//                 int m = p2 - i;

//                 if (n >= m) break;

//                 int temp = copy[n];
//                 copy[n] = copy[m];
//                 copy[m] = temp;
//             }

//             return copy;
//         }

//         private int[] Shuffle(int[] route)
//         {
//             int n = route.Length;
//             for (int i = 0; i < n; ++i)
//             {
//                 int rIndx = _rnd.Next(i, n);
//                 int tmp = route[rIndx];
//                 route[rIndx] = route[i];
//                 route[i] = tmp;
//             }

//             return route;
//         }
//     }

//     class TSP_Annealing_Program
//     {
//         static void Main(string[] args)
//         {
//             var tsm = new TSM();
//             tsm.Solve();
//         }
//     }
// }
