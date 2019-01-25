using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfCubes
{
    class Program
    {

        static void Main(string[] args)
        {
            int n, testcase = 1;
            int[][] cubes;

            do
            {

                Console.WriteLine("Upišite broj kocaka: ");
                n = int.Parse(Console.ReadLine());

                if (n == 0) break;
                cubes = new int[n][];
                // kreiramo polje kocaka
                for (int i = 0; i < n; i++)
                {
                    cubes[i] = new int[6];
                    string[] x = Console.ReadLine().Split(' ');
                    for (int j = 0; j < 6; j++)
                    {
                        cubes[i][j] = int.Parse(x[j]);                      
                        
                    }
                }

                
                ToC tower = new ToC(n, cubes);



                

                Console.WriteLine("Test case #{0}: ", testcase);
                
                //tower.printCubes();
                

                tower.printTower();


                // tower.printGraph();

                testcase++;



            }

            while (n != 0);
            
        }
    }
}
