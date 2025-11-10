using System;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    internal class Program
    {
        static int[] GetAndFillArray(object userInput)
        {
            int temp = (int)userInput;
            Random random = new Random();
            int[] userArray = new int[temp];

            for (int i = 0; i < userArray.Length; i++)
            {
                userArray[i] = random.Next(0, 100);
            }

            Array.Sort(userArray);
            return userArray;
        }

        static void PrintArray(object userArray)
        {
            int[] temp = (int[]) userArray;
            Console.WriteLine("Массив выглядит следующим образом: ");
            for (int i = 0; i < temp.Length; i++)
            {
                Console.Write(temp[i] + " ");
            }

            Console.WriteLine();
        }

        static int sumOfArray(object userArray)
        {
            int[] temp = (int[]) userArray;
            if (temp.Length == 0)
            {
                return 0;
            }

            int sum = 0;

            for (int i = 0; i < temp.Length; i++)
            {
                sum += temp[i];
            }

            return sum;
        }

        static int maxOfArray(object userArray)
        {
            int[] temp = (int[]) userArray;
            if (temp.Length == 0)
            {
                return 0;
            }

            int max = temp[0];

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] > max)
                {
                    max = temp[i];
                }
            }
            
            return max;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите количество массива: ");
            int userInput = Convert.ToInt32(Console.ReadLine());

            int[] userArray = new int[] { };

            Func<object, int[]> firstFunc = new Func<object, int[]>(GetAndFillArray);
            Task<int[]> firstTask = new Task<int[]>(firstFunc, userInput);
            firstTask.Start();

            Action<Task<int[]>> firstAction = new Action<Task<int[]>>(PrintArray);
            Task secondTask = firstTask.ContinueWith(PrintArray);

            Func<Action<Task<int[]>>, int> secondFunc = new Func<Action<Task<int[]>>, int>(sumOfArray);
            Task<int> thirdTask = secondTask.ContinueWith(sumOfArray);
            int sum = thirdTask.Result;

            Func<Action<Task<int[]>>, int> thirdFunc = new Func<Action<Task<int[]>>, int> (maxOfArray);
            Task<int> fourthTask = thirdTask.ContinueWith(maxOfArray);
            int max = fourthTask.Result;

            Task.WhenAll(secondTask, thirdTask, fourthTask);

            Console.WriteLine($"Сумма массива - {sum}, самое большое число в массиве {max}.");
            Console.ReadKey();
        }
    }
}