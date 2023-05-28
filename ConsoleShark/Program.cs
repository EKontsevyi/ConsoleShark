using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleShark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            int level = 3;
            char[,] map = ReadMap("map" + level + ".txt");
            Console.CursorVisible = false;

            

            int sharkX = 1, sharkY = 1;
            int score = 1;
            int ukrain = 0;
            List<int[]> sharkBody = new List<int[]>(); // создаем список координат сегментов тела змейки
            sharkBody.Add(new int[] { sharkX, sharkY }); // добавляем первый сегмент в список

            Randomiser(map);


            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();
            Task.Run(() =>
            {
                while (true)
                {
                    pressedKey = Console.ReadKey();
                    Console.Beep(261, 250);
                }
            });

            while (true)
            {
                Console.Clear();
                DrawMap(map);
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(0, 27);
                Console.Write($"Score: {score}");

                Console.SetCursorPosition(0, 28);
                Console.Write(GetMaxScores(map, '■'));

                Console.ForegroundColor = ConsoleColor.Green;
                // отрисовываем все сегменты тела змейки на карте
                for (int i = 0; i < sharkBody.Count; i++)
                {
                    Console.SetCursorPosition(sharkBody[i][0], sharkBody[i][1]);
                    Console.Write('■');
                    

                }
                Thread.Sleep(200);

                HandleInput(pressedKey, ref sharkX, ref sharkY, map, ref score);

                sharkBody.Insert(0, new int[] { sharkX, sharkY });
                // удаляем последний сегмент тела змейки из списка координат
                if (sharkBody.Count > score + 1)
                {
                    sharkBody.RemoveAt(sharkBody.Count - 1);
                }
                if (map[sharkX, sharkY] == '?')
                {
                    Console.Beep(566, 450);
                    map[sharkX, sharkY] = ' ';
                    ukrain++;
                    if(ukrain == 5)
                    {
                    MessageBox.Show("You open Ukrain Level!!!");
                    level = 0;
                    NewLevel(level, ref map, ref sharkX, ref sharkY, ref score, ref sharkBody);

                    }

                }

                if (score == 10)
                {
                    level++;
                    NewLevel(level,ref map,ref sharkX,ref sharkY,ref score, ref sharkBody);
                    
                }




            }




        }




        private static char[,] ReadMap(string path)
        {
            string[] file = File.ReadAllLines(path);
            char[,] map = new char[GetMaxLenght(file), file.Length];
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    map[x, y] = file[y][x];
            return map;

        }

        private static int GetMaxLenght(string[] lines)
        {
            int maxLenght = lines[0].Length;
            foreach (string line in lines)
            {
                if (line.Length > maxLenght)
                    maxLenght = line.Length;
            }
            return maxLenght;
        }
        private static void DrawMap(char[,] map)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {

                    if (map[x, y] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    if (map[x, y] == '■')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    if (map[x,y] == '?') {
                        Console.Write(' ');
                        continue;
                    }

                    Console.Write(map[x, y]);

                }
                Console.WriteLine();
            }
        }
        private static int[] GetDirection(ConsoleKeyInfo pressedKey)
        {
            int[] direction = { 0, 0 };
            switch (pressedKey.Key)
            {
                case ConsoleKey.UpArrow:
                    direction[1] = -1;
                    break;
                case ConsoleKey.DownArrow:
                    direction[1] = 1;
                    break;
                case ConsoleKey.LeftArrow:
                    direction[0] = -1;
                    break;
                case ConsoleKey.RightArrow:
                    direction[0] = 1;
                    break;
            }
            return direction;
        }

        private static void HandleInput(ConsoleKeyInfo pressedKey, ref int sharkX, ref int sharkY, char[,] map, ref int score)
        {

            int[] direction = GetDirection(pressedKey);
            int nextSharkPositionX = sharkX + direction[0];
            int nextSharkPositionY = sharkY + direction[1];
            if (map[nextSharkPositionX, nextSharkPositionY] != '#')
            {
                sharkX = nextSharkPositionX;
                sharkY = nextSharkPositionY;
            }

            if (map[sharkX, sharkY] == '■')
            {
                score++;
                map[sharkX, sharkY] = ' ';
                Console.Beep(466, 250);
                Randomiser(map);
            }
            

        }
        private static int GetMaxScores(char[,] map, char symbol)
        {
            int maxScore = 0;
            foreach (char c in map)
            {
                if (c == symbol)
                {
                    maxScore++;
                }
            }
            return maxScore;
        }

        private static void Randomiser(char[,] map)
        {
            Random random = new Random();
            bool run = true;
            while (run)
            {

                int ranX = random.Next(1, 49);
                int ranY = random.Next(1, 19);
                if (map[ranX, ranY] == '#')
                    run = true;
                else
                {
                    map[ranX, ranY] = '■';
                    run = false;
                }

            }
        }
        private static void NewLevel(int level,ref char[,] map, ref int sharkX, ref int sharkY, ref int score, ref List<int[]> sharkBody)
        {
            
            map = ReadMap("map" + level + ".txt");
            sharkX = 1; sharkY = 1;
            score = 1;
            sharkBody.Clear();
            Randomiser(map);
        }

    }
}
