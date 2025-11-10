# Параллельное программирование и библиотека TPL

Задача. Сформировать массив случайных целых чисел (размер  задается пользователем). Вычислить сумму чисел массива и максимальное число в массиве.  Реализовать  решение  задачи  с  использованием  механизма  задач продолжения.

Решение.
```
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

        static void PrintArray(Task<int[]> task)
        {
            int[] temp = task.Result;
            Console.WriteLine("Массив выглядит следующим образом: ");
            for (int i = 0; i < temp.Length; i++)
            {
                Console.Write(temp[i] + " ");
            }

            Console.WriteLine();
        }

        static int sumOfArray(Task<int[]> task)
        {
            int[] temp = task.Result;
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

        static int maxOfArray(Task<int[]> task)
        {
            int[] temp = task.Result;
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
            Task secondTask = firstTask.ContinueWith(firstAction);

            Func<Task<int[]>, int> secondFunc = new Func<Task<int[]>, int> (sumOfArray);
            Task<int> thirdTask = firstTask.ContinueWith<int>(secondFunc);

            Func<Task<int[]>, int> thirdFunc = new Func<Task<int[]>, int> (maxOfArray);
            Task<int> fourthTask = firstTask.ContinueWith<int>(thirdFunc);

            Task.WaitAll(secondTask, thirdTask, fourthTask);

            int sum = thirdTask.Result;
            int max = fourthTask.Result;

            Console.WriteLine($"Сумма массива - {sum}, самое большое число в массиве {max}.");
            Console.ReadKey();
        }
    }
}
```
