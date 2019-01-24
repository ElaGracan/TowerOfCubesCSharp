using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfCubes
{

    class ToC
    {
        int[][] cubes; //polje u koje spremamo boje koje učitamo
        int[][] graph; //0/1 matrica 1 - kocku i s bojom k ukrenutom gore mozemo staviti na kocku j s bojom l gore
        int[] indegree; // broj ulaznih puteva u i-tu kocku okrenutu stranom k prema gore
        int n;
        int[] sorted;
        int[][] MaxAndPrevious; // prvi redak sadrzi duljinu najviseg tornja do cvora j, a drugi je index cvora koji mu prethodi u tornju
        Queue<int> zeroin; // red za obradu cvorova koji mogu biti prvi u tornju (na dnu)

        public ToC(int n, int[][] cubes)
        {
            this.n = n;
            this.cubes = cubes;
            graph = new int[6 * n][];
            indegree = new int[6 * n];
            zeroin = new Queue<int>();
            sorted = new int[6 * n];
            MaxAndPrevious = new int[2][];

            //konstruiramo graf

            for (int i = 0; i < n; i++) 
                for (int k = 0; k < 6; k++)
                {
                    graph[i * 6 + k] = new int[6 * n];
                    for (int j = 0; j < i; j++) // kocka ne moze biti sama na sebi pa idemo do i
                        for (int l = 0; l < 6; l++)
                        {
                            if (cubes[i][k] == cubes[j][l + 1 * ((l + 1) % 2) + (-1) * (l % 2)]) // donja strana
                            {
                                graph[i * 6 + k][j * 6 + l] = 1;
                                indegree[j * 6 + l]++; // kocku [stupac] mogu staviti na kocku [redak]
                            }
                        }
                }
        }

        void topSort()
        {
            // na kraju se u sorted polju nalaze svi čvorovi, ali redom od onih koji su imali najmanji indegree do vecih
            //Console.WriteLine();
            //Console.WriteLine(string.Join(" ", indegree));
            //Console.WriteLine();
            int j = 0;
            for (int i = 0; i < n * 6; i++)
                if (indegree[i] == 0) zeroin.Enqueue(i); //ovo znaci da i moze biti pocetni vrh (niti jedan put ne vodi u taj vrh), tj moze biti dno tornja
            while(zeroin.Count() != 0)
            {
                int el = zeroin.Dequeue();
                sorted[j] = el;
                j++;

                for (int i = 0; i < n * 6; i++)
                {
                    if(graph[el][i] == 1) // u koje je cvorove moguce doci iz el (outdegree od el)
                    {
                        indegree[i]--;
                        if (indegree[i] == 0) zeroin.Enqueue(i);
                    }
                }


            }

        }

        void calculateTowers()
        {
            MaxAndPrevious[0] = new int[6 * n];
            MaxAndPrevious[1] = new int[6 * n]; 
            MaxAndPrevious[1] = Enumerable.Repeat(-1, 6*n).ToArray();

            

            for (int i = 0; i < n * 6; i++)
            {
                for (int j = 0; j < n * 6; j++)
                {
                   
                    if (graph[sorted[i]][j] == 1) // gledam retke matrice graph redosljedom kako pise u sorted polju
                    { // j je cvor u koji mogu doci iz cvora sorted[i] (j mogu staviti na s[i])
                        if (MaxAndPrevious[0][j] < MaxAndPrevious[0][sorted[i]] + 1) // ako dosad je najdulji toranj do cvora j manji od dosad najduljeg tornja do s[i]+1
                            MaxAndPrevious[0][j] = MaxAndPrevious[0][sorted[i]] + 1; // znaci da cu radije ici putem do cvora s[i] pa zatim do j
                        MaxAndPrevious[1][j] = sorted[i]; // namjestimo s[i] na prethodnika od j da znamo rekonstruirati put
                    }
                }
            }

           

        }

        int getMaxTower()
        {
            int max = 0;
            for (int i = 1; i < n * 6; i++)
                if (MaxAndPrevious[0][i] > MaxAndPrevious[0][max])
                    max = i;
            return max;
        }

        public void printTower()
        {
            this.topSort();
            this.calculateTowers();
            int max = this.getMaxTower();

            //Console.WriteLine(MaxAndPrevious[0][max] + 1) ;
            int i = max;
            while (true)
            {
                Console.Write("{0} ", i / 6 + 1);
                switch (i % 6)
                {
                    case 0:
                        Console.WriteLine("front");
                        break;
                    case 1:
                        Console.WriteLine("back");
                        break;
                    case 2:
                        Console.WriteLine("left");
                        break;
                    case 3:
                        Console.WriteLine("right");
                        break;
                    case 4:
                        Console.WriteLine("top");
                        break;
                    case 5:
                        Console.WriteLine("bottom");
                        break;
                }

                if (MaxAndPrevious[1][i] == -1) break;

                i = MaxAndPrevious[1][i];
            }


            // Console.WriteLine("");
        }

        public void printGraph()
        {
            for (int i = 0; i < 6*n; i++)
            {
                Console.WriteLine(string.Join(" ", graph[i]));
            }
            Console.WriteLine();
            Console.WriteLine(string.Join(" ", indegree));
            Console.WriteLine();

            Console.WriteLine(string.Join(" ", MaxAndPrevious[0]));
            Console.WriteLine(string.Join(" ", MaxAndPrevious[1]));

            Console.WriteLine();

            Console.WriteLine(string.Join(" ", sorted));

            Console.WriteLine();



        }

        public void printCubes()
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(string.Join(" ", cubes[i]));
            }
            Console.WriteLine();
        }
    }
}
