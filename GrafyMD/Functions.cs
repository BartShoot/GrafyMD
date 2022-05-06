using System;

namespace GrafyMD
{
    public static class Functions
    {
        /// <summary>
        /// Funkcja zerująca macierz
        /// </summary>
        /// <param name="matrix">Macierz do zerowania</param>
        public static void ResetMatrix(int[,] matrix)
        {
            int size = matrix.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// Funkcja generująca wierzchołki i ich połączenia
        /// </summary>
        /// <param name="vertexCount">Ilość wierzchołków</param>
        /// <param name="propability">Prawdopodobieństwo połączenia</param>
        /// <param name="rand">Funkcja losująca</param>
        /// <param name="connectionMatrix">Macierz połączeń</param>
        /// <param name="weightMatrix">Macierz wag połączeń</param>
        /// <returns>Zwraca tekst do zapisania</returns>
        public static (string text, int stat) CreateVertexesAndSetWeight(int vertexCount, double propability, 
            Random rand, int[,] connectionMatrix, double[,] weightMatrix)
        {
            int multiplier = 100;
            string str = null;
            int counter = 0;
            for (int i = 0; i < vertexCount - 1; i++)
            {
                for (int j = i + 1; j < vertexCount; j++)
                {
                    double a = rand.NextDouble();

                    //Tworzenie krawedzi oraz nadanie jej wagi jesli losowa liczba jest mniejsza niz prawdopodobieństwo podane przez użytkownika
                    if (a < propability)
                    {
                        double random = rand.NextDouble();

                        //symetrycznośc macierzy 1 = istnieje krawędź
                        connectionMatrix[i, j] = 1;
                        connectionMatrix[j, i] = 1;

                        str += $"Krawędź w ({i + 1}, {j + 1})\n";
                        counter++;
                        //Określanie wagi 
                        weightMatrix[i, j] = Math.Floor(10 + multiplier * (random));
                        weightMatrix[j, i] = weightMatrix[i, j];
                    }
                }
            }
            return (str, counter);
        }

        public static string DrawGraph(int[,] A)
        {
            int z = 1;
            string str = null;
            foreach (int e in A)
            {
                str += Convert.ToString(z % Math.Sqrt(A.Length) == 0 ? $"{e}\n" : e);
                z += 1;
            }
            return str;
        }

        public static (string text, int stat) FindCycle3(int[,] A, int n, double[,] W, Cycles C)
        {
            int counter = 0;
            string str = null;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (A[i, j] == 1)
                    {
                        for (int k = 1; k < n; k++)
                        {
                            if (A[i, k] == 1 && A[j, k] == 1 && !C.Is3Cycle(i, j, k))
                            {
                                counter++;
                                double weight = W[i, j] + W[i, k] + W[j, k];
                                int[] temp = new int[] { i, j, k, (int)weight };
                                C.cycle.Add(temp);
                                str += $"Cykl: {i + 1} {j + 1} {k + 1}\n";
                                str += $"Waga cyklu: {weight}\n";
                            }
                        }
                    }
                }
            }
            return (str, counter);
        }

        public static (string text, int stat) FindCycle4(int[,] A, int n, double[,] W, Cycles C)
        {
            int counter = 0;
            string str = null;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (A[i, j] == 1)
                    {
                        for (int k = i + 1; k < n; k++)
                        {
                            if (j != k)
                            {
                                for (int l = i + 1; l < n; l++)
                                {
                                    if (l != k && l != j)
                                    {
                                        if (A[i, k] == 1 && A[j, l] == 1 && A[k, l] == 1 && !C.Is4Cycle(i, j, k, l))
                                        {
                                            counter++;
                                            double weight = W[i, j] + W[i, k] + W[j, l] + W[k, l];
                                            int[] temp = new int[] { i, j, k, l, (int)weight };
                                            C.cycle.Add(temp);
                                            str += $"Cykl: {i + 1} {j + 1} {k + 1} {l + 1}\n";
                                            str += $"Waga cyklu: {weight}\n";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return (str, counter);
        }
    }
}
