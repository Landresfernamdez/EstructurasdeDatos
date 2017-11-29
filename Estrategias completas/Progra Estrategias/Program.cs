using System;
using System.Linq;
using System.Diagnostics;
/// <summary>
/// Desarrolladores: Luis Andres Fernandez, Jurguen Romero
/// Fecha inicio 19/10/2016
/// Fecha final 22/11/2016 8:14 PM :)
/// </summary>
namespace Estrategias
  {
    /// <summary>
    /// Parametros y funciones necesarias
    /// </summary>
    class Program
    {
        public static int[,] matrizRutas;
        public static Stopwatch temporizador;
        //Variable que se encarga de tomar el conteo de arcos visitados en las 3 estrategias de diseno
        static int cantiArcosvisitados=0;
        //Variables que se tomar la cantidad de memoria consumida
        static long bits=0;
        //........................................................Variables Algoritmo Genetico
        static int n;
        static int randing=0 ;
        static int ptoParada=0;
        static bool bandera1=false;
        static int limite=n/2;
        static char[] arreglo;//arreglo de vertices que no tiene la madre y si el padre
        static Ruta[] poblacionInicial = new Ruta[8];//Poblacion inicial de la especie
        static int counGeneraciones=0;//contador de generaciones para llevar el indice del arreglo
        static int[] casados;//vertices que ya tienen pareja a la hora de realizar las combinaciones de cromosomas
        static int councasa;//contador que lleva el indice de los cromosomas casados
        static pareja[] parejas=new pareja[8];//parejas de cromosomas
        static int countaparejas=0;//contador que lleva el indice de los cromosomas
        static Ruta f=new Ruta(0,"");//padre
        static Ruta mo = new Ruta(0, "");//madre
        static Ruta hu = new Ruta(0, "");//primer hijo
        static Ruta hd = new Ruta(0, "");//Segundo hijo
        static bool padre =false;//para saber si el padre ya fue elegido
        static bool h1=false;//Se utiliza para detectar cual hijo va ha mutar
        static bool h2= false;//Se utiliza para detectar cual hijo va ha mutar
        static bool bandera=false ;//bandera
        static int puntoCruce=0;//punto de cruce global
        static int posPunCrupa=0;//posicion del punto de cruce del padre
        static int posPunCruma=0;//posicion del punto de cruce de la madre
        static string masPesado="00";//lleva el par del mas pesado
        static string salto="00";//lleva el salto
        static Random r = new Random();//variable para generar las elecciones aleatorias
        static char[] listaVertices;//Se encarga de almacenar los vertices por los que ya paso
        static int countaVertices=0;//Se encarga de almacenar la posicion de la lista de vertices
        //........................................................Finalizacion de variables Algoritmo Genetico
        //...............................................................................................................................................................
        //Variables de medicion algortimo de backtraking
        static int poda=0;
        //cositas que necesito
        static double distanciaCorta = 999999999;
        static string caminoCorto="";
        static bool[,] listaI;
        static int saltos = 5;
        public static long time;//tiempo
        
        
        //------------probabilistico-------------
        static double[] listaProbabilidadXY;
        static double fI = 0.1;
        static double[,] matrizFeromonas;
        static bool[,] matrizVisitados;

        /// <summary>
        /// Funcion desde donde es compilado el programa
        /// </summary>
        /// <param name="args"></param>

        static void Main(string[] args)
        {
            principalMenu();
            Console.ReadKey();
        }

        static void imprimirMatriz()
        {
            string cadena = "";
            bits += 16;
            bits += 32;
            for (int i = 0; i < n; i++)
            {
                bits += 32;
                for (int j = 0; j < n; j++)
                {
                    cadena = cadena + matrizRutas[i, j] + " | ";
                }
                cadena = cadena + "\n";
            }
            Console.WriteLine(cadena);
        }/// <summary>
        /// Funcion que se encarga de generar la matriz
        /// </summary>
        public static void generarMatriz()
        {
            //Creamos un nuevo array del tamaño especificado
            //ahora en cada posición del array insertaremos
            //un numero aleatorio entre 1, y 20
            matrizRutas = new int[n, n];
            for (int j = 0; j < n; j++)
            {
                for (int x = 0; x < n; x++)
                {
                    if (j == x)
                    {
                        matrizRutas[x, j] = 0;
                    }
                    else
                    {
                        matrizRutas[x, j] = r.Next(1,9);
                    }
                }
            }
        }
        
    

        /// <summary>
        /// Este es el menu principal de la aplicacion de estartegias de diseno para la ruta mas corta
        /// </summary>

        public static void principalMenu()
        {
            temporizador = Stopwatch.StartNew();
            //Variables para tomar la mediciones
            bits = 0;
            poda = 0;
            cantiArcosvisitados = 0;
            
            Console.WriteLine("Digite el algoritmo que desea implementar");
            Console.WriteLine("1.Genetico");
            Console.WriteLine("2.Backtraking");
            Console.WriteLine("3.Probabilistico");
            string option = Console.ReadLine();
            if (option == "1")
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        n = 6;
                        limite = n / 3+1;
                        bits = 0;
                        cantiArcosvisitados = 0;
                    }
                    else if (i == 1)
                    {
                        n = 8;
                        limite = n / 3+1;
                        bits = 0;
                        cantiArcosvisitados = 0;
                    }

                    else if (i == 2)
                    {
                        n = 9;
                        limite = n / 3;
                        bits = 0;
                        cantiArcosvisitados = 0;
                    }
                    else
                    {
                        n = 10;
                        limite = n / 3;
                        bits = 0;
                        cantiArcosvisitados = 0;
                    }
                    generarMatriz();
                    listaVertices = new char[n];
                    arreglo = new char[n];
                    casados = new int[n];
                    temporizador = Stopwatch.StartNew();
                    //Se le suma a la cantidad de bits las variables staticas que ya fueron inicializadas y utiiza el genetico
                    bits += 17 * 32;//integers declarados
                    bits += 4 * 8;//booleans declarados
                    bits += matrizRutas.Length * 32;
                    bits += casados.Length * 32;
                    bits += poblacionInicial.Length * 48;
                    bits += 4 * 48;//Obgetos de tipo Ruta
                    bits += listaVertices.Length * 16;

                    geneticoRutaCorta();
                    Console.WriteLine("Cantidad de bits:" + bits);
                    Console.WriteLine("Cantidad de arcos visitados:" + cantiArcosvisitados);
                    Console.WriteLine("Tiempo de ejecucion:  " + time + "   milisegundos");

                    temporizador.Stop();
                    time = temporizador.ElapsedMilliseconds;
                    Console.ReadKey();

                }
                principalMenu();
            }



            else if (option == "2")
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        n = 10;
                    }
                    else if (i == 1)
                    {
                        n = 100;
                    }

                    else if (i == 2)
                    {
                        n = 500;
                    }
                    else
                    {
                        n = 1000;
                    }
                    generarMatriz();
                    listaI = new bool[n, n];
                    //Se le suman las variables que fueron inicializadas y utiliza el Backtraking
                    bits =bits+( 2 * 32);//integers declarados
                    bits =bits+ 16;//cadena de texto con un char
                    //matriz de booleanos
                    bits =bits+( listaI.Length * 8);//matriz de booleanos
                    bits =bits+ 64;//un double
                    temporizador = Stopwatch.StartNew();
                    distanciaCorta = 99999999;
                    //Vertice Inicio, vertice final,combinaciones,lista bool ,Vertice inicio y peso
                    Backtraking("0", "4", "", 0, listaI, 0);
                    Console.WriteLine("Camino corto: " + caminoCorto + " Distancia corta: " + distanciaCorta);
                    Console.WriteLine("Cantidad de bits:" + bits);
                    Console.WriteLine("Cantidad de podas:" + poda);
                    Console.WriteLine("Tiempo de ejecucion:  " + time + "   milisegundos"+" Limite "+limite);
                    Console.WriteLine("Matriz"+n+"x"+n);
                    temporizador.Stop();
                    time = temporizador.ElapsedMilliseconds;
                    bits = 0;
                    poda = 0;
                    distanciaCorta = 999999999999;
                    caminoCorto = "";
                    Console.ReadKey();
                }
                principalMenu();
            }

            else if (option == "3")
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        n = 10;
                    }
                    else if (i == 1)
                    {
                        n = 100;
                    }

                    else if (i == 2)
                    {
                        n = 500;
                    }
                    else
                    {
                        n = 1000;
                    }
                    generarMatriz();
                    listaProbabilidadXY = new double[n];
                    matrizFeromonas = new double[n, n];
                    matrizVisitados = new bool[n, n];
                    temporizador = Stopwatch.StartNew();
                    //Se le suman las variable del probabilistico
                    bits += listaProbabilidadXY.Length * 64;//matriz de doubles
                    bits += 64;//double
                    bits += matrizFeromonas.Length * 64;//matriz de doubles
                    bits += matrizVisitados.Length * 8;//matriz de booleanos

                    distanciaCorta = 999999999999;
                    caminoCorto = "";
                    crearMatrizFeromonas();
                    for (int y = 0; y < 56; y++)
                    {
                        probabilisticoHormigas(0, 4, 0, "", y);
                    }
                    Console.WriteLine("Mejor camino:  " + caminoCorto + "  mejor distancia:  " + distanciaCorta);
                    Console.WriteLine("bits:" + bits);
                    Console.WriteLine("Cantidad de arcos visitados:" + cantiArcosvisitados);
                    Console.WriteLine("Tiempo de ejecucion:  " + time + "   milisegundos" + " Limite " +limite);
                    temporizador.Stop();
                    time = temporizador.ElapsedMilliseconds;
                    bits = 0;
                    cantiArcosvisitados = 0;
                    Console.ReadKey();
                }
                principalMenu();
            }
            else
            {
                Console.WriteLine("Error digite lo que se le solicita");
                principalMenu();
            }
        }
        
        
        //..............................................................Algoritmo Genetico.............................................................................................
        /// <summary>
        /// Algoritmo genetico
        /// </summary>
        public static void geneticoRutaCorta()
        {
            generadorMutaciones();
            bits += 32;
            for (int x = 0; x < poblacionInicial.Length; x++)
            {
                Console.WriteLine("ruta:" + poblacionInicial[x].ruta + ",peso:" + poblacionInicial[x].peso);
            }
        }
        /// <summary>
        /// Se encarga de mutar la ruta con mayor peso de forma aleatoria
        /// </summary>
        /// <param name="cromo"></param>
        /// <returns></returns>
        public static string mutacionRazonable(string cromo)
        {
            masPesado ="00";
            bits += 32;
            for (int y = 0; y < cromo.Length; y++)
            {
                cantiArcosvisitados++;
                if (y + 1 != cromo.Length)
                {
                    salto = "" + cromo[y];
                    salto += cromo[y + 1];
                }
                else if (y + 1 == cromo.Length)
                {
                    salto = "00";
                }
                if (devuelvePeso(salto[0], salto[1]) > devuelvePeso(masPesado[0], masPesado[1]))
                {
                    masPesado = salto;
                    salto = "";
                }
            }
            for (int x = 0; x < cromo.Length; x++)
            {
                cantiArcosvisitados++;
                if (x + 1 != cromo.Length)
                {
                    if (cromo[x]==masPesado[0] && cromo[x + 1] ==masPesado[1])
                    {
                        
                        if (x == 0)
                        {
                            int temp = r.Next(0, n) + 48;
                            char g = (char)temp;
                            bits+=48;
                            while (existeVertice(g,cromo) == true)
                            {
                                temp = r.Next(0, n) + 48;
                                g = (char)temp;
                            }
                            cromo = reemplazarV(x+1, g, cromo);
                            bits += cromo.Count()* 16;
                            return cromo;
                        }
                        else 
                        {
                            int temp = r.Next(0, n) + 48;
                            char g = (char)temp;
                            bits += 48;
                            while (existeVertice(g, cromo) == true)
                            {
                                temp = r.Next(0, n) + 48;
                                g = (char)temp;
                            }
                            cromo = reemplazarV(x,g, cromo);
                            bits += cromo.Count() * 16;
                            return cromo;
                        }
                    }
                }
            }
            return cromo;
        }     
        /// <summary>
        /// Se encarga de generar las mutaciones
        /// </summary>
        public static void generadorMutaciones()
        {
                generarPoblacionInicial(0, 4);
                hu.peso = 0;
                hd.peso = 0;
                f.peso = 1;
                mo.peso = 1;
                counGeneraciones = 0;
                //Se encarga de mutar mientras alguno de los hijos sea mejor que el padre
                while ((hd.peso < mo.peso || hd.peso < f.peso) || (hu.peso < mo.peso || hu.peso < f.peso))
                {
                //Instancias importantes para realizar cruces
                                for (int x = 0; x < 4; x++)
                                {
                                    casados = new int[8];
                                    councasa = 0;
                                    parejas = new pareja[4];
                                    countaparejas = 0;
                                    padre = true;
                                    bandera = true;
                                    //Instancias importantes para mutar
                                    f = new Ruta(0, "");
                                    mo = new Ruta(0, "");
                                    hu = new Ruta(0, "");
                                    hd = new Ruta(0, "");
                                    int[] v = new int[2];
                                    salto = "";
                                    bandera1 = false;
                                    bits += v.Length*32;
                                    //............................
                                    //Se encarga de generar nuevas combinaciones para que no se realicen los mismos cruces
                                    generaCombinaciones();
                                    
                                    generarCruses(x);
                                    if (f.peso > mo.peso)
                                    {
                                        if (hu.peso < f.peso && existeEnrutasprincipales(hu.ruta).Equals(false))
                                        {
                                            cambioEvolutivo(f,hu);
                                        }
                                        else if (hd.peso < f.peso && existeEnrutasprincipales(hd.ruta).Equals(false))
                                        {
                                            cambioEvolutivo(f, hd);
                                        }
                                        else if (hu.peso < f.peso && hd.peso < mo.peso && existeEnrutasprincipales(hu.ruta).Equals(false) && existeEnrutasprincipales(hd.ruta).Equals(false))
                                        {
                                            cambioEvolutivo(f, hu);
                                            cambioEvolutivo(mo, hd);
                                        }

                                        else if (hd.peso < f.peso && hu.peso < mo.peso && existeEnrutasprincipales(hu.ruta).Equals(false) && existeEnrutasprincipales(hd.ruta).Equals(false))
                                        {
                                            cambioEvolutivo(f, hd);
                                            cambioEvolutivo(mo, hu);
                                        }
                                    }
                                    else
                                    {
                                        if (hu.peso < mo.peso && existeEnrutasprincipales(hu.ruta).Equals(false))
                                        {
                                            cambioEvolutivo(mo, hu);
                                        }
                                        else if (hd.peso < mo.peso && existeEnrutasprincipales(hd.ruta).Equals(false))
                                        {
                                            cambioEvolutivo(mo, hd);
                                        }
                                        else if (hu.peso < f.peso && hd.peso < mo.peso && existeEnrutasprincipales(hu.ruta).Equals(false) && existeEnrutasprincipales(hd.ruta).Equals(false))
                                        {
                                            cambioEvolutivo(f, hu);
                                            cambioEvolutivo(mo, hd);
                                        }

                                        else if (hd.peso < f.peso && hu.peso < mo.peso && existeEnrutasprincipales(hu.ruta).Equals(false) && existeEnrutasprincipales(hd.ruta).Equals(false))
                                        {
                                            cambioEvolutivo(f, hd);
                                            cambioEvolutivo(mo, hu);
                                        }
                                    }
                    
                                }
            }
        }        
        /// <summary>
        /// Funcion que se encarga de generar los cruces apartir de la poblacion inicial
        /// </summary>
        public static void generarCruses(int x)
        {
            bits += 32;
            bandera = true;
            bits += 8;
            while (bandera == true)
            {
                bits += 32;
                for (int y = 0; y < poblacionInicial.Length; y++)
                {
                    cantiArcosvisitados++;
                    if ((y + 1 == parejas[x].cromosoma[0]) || (y + 1 == parejas[x].cromosoma[1]))
                    {
                        if (padre == true)
                        {
                            f = poblacionInicial[y];
                            bandera = false;
                            padre = false;

                        }
                        else
                        {
                            mo = poblacionInicial[y];
                            padre = true;
                            bandera = false;
                        }
                    }
                }

            }
            while (existePuntoCruce(f, mo) == false)
            {
                mutarParacruce();
            }
            puntoCruce = DevuelvePuntoCruce(f, mo);
            Cruce(f.ruta, mo.ruta, puntoCruce);

            hu.peso = determinarPesoRuta(hu.ruta);
            hd.peso = determinarPesoRuta(hd.ruta);
            ///Se termina de definir el cruce
            Console.WriteLine(".....................Familia" + x + ".....................");
            //Imprimir cruce de familia con su respectivo peso
            Console.WriteLine(".....................Generacion:" + counGeneraciones + ".....................");
            Console.WriteLine("..........Cruce..........");
            Console.WriteLine("Padre:" + "ruta:" + f.ruta + ", peso:" + f.peso);
            Console.WriteLine("Madre:" + "ruta:" + mo.ruta + ", peso:" + mo.peso);
            Console.WriteLine("Hijo 1:" + "ruta:" + hu.ruta + ", peso:" + hu.peso);
            Console.WriteLine("Hijo 2:" + "ruta:" + hd.ruta + ", peso:" + hd.peso);
            //Console.WriteLine("eliminar arcos mas pesados adaptabilidad");
            adaptabilidad();//Se encarga de eliminar los arcos mas pesados
            //Console.WriteLine("cambiar arcos mas pesados");
            hu.ruta = mutacionRazonable(hu.ruta);//se encarga de cambiar los arcos mas pesados
            hd.ruta = mutacionRazonable(hd.ruta);
            bandera1 = false;
            hu.ruta = mientrasExistanrepetidos(hu.ruta);//Se encarga de eliminar los ciclos osea los vertices repetidos
            //Console.WriteLine("h1:"+hu.ruta);
            bandera1 = false;
            hd.ruta = mientrasExistanrepetidos(hd.ruta);
            Console.WriteLine(hu.ruta);
            hu.peso = determinarPesoRuta(hu.ruta);//Se encarga de determinar el peso de la ruta
            hd.peso = determinarPesoRuta(hd.ruta);
            //Imprimir mutacion con el peso
            Console.WriteLine("..........mutacion..........");
            Console.WriteLine("Padre:" + "ruta:" + f.ruta + ", peso:" + f.peso);
            Console.WriteLine("Madre:" + "ruta:" + mo.ruta + ", peso:" + mo.peso);
            Console.WriteLine("Hijo 1:" + "ruta:" + hu.ruta + ", peso:" + hu.peso);
            Console.WriteLine("Hijo 2:" + "ruta:" + hd.ruta + ", peso:" + hd.peso);
            counGeneraciones++;
            Console.ReadLine();
        }
        /// <summary>
        /// Verifica si existe en la lista principal
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        public static bool existeEnrutasprincipales(string ruta)
        {
            bits += 16;
            bits += 32;
            for (int x = 0; x < poblacionInicial.Length; x++)
            {
                cantiArcosvisitados++;
                if (poblacionInicial[x].ruta == ruta)
                    {
                        return true;
                    }
                
            }
            return false;
        }
        /// <summary>
        /// Metodo que inicia la adaptacion del cromosoma
        /// </summary>
        public static void adaptabilidad()
        {
            if (hu.ruta.Length > limite)
            {
                h1 = true;
                mutacionRango();
            }
            if (hu.ruta.Length < limite)
            {
                h1 = false;
            }
            if (hd.ruta.Length > limite)
            {
                h2 = true;
                mutacionRango();
            }

            if (hd.ruta.Length < limite)
            {
                h2 = false;
            }
        }       
        /// <summary>
         /// Mutacion que se da cuando un cromosoma sobrepasa el tamano establecido
         /// </summary>
        public static void mutacionRango()
        {
            if (h1 == true)
            {
                string valor=hu.ruta;
                if (valor.Length > limite)
                {
                    while (valor.Length > limite)
                    {//Eliminar el vertice mas pesado
                        valor = retornarRutanormalizada(hu.ruta);
                        bits += valor.Count() * 16;
                        hu.ruta = valor;
                    }
                }
                else
                {
                    return;
                }
            }
            else 
            {
                string valor = hu.ruta;
                if (valor.Length > limite)
                {

                    while (valor.Length > limite)
                    {//Eliminar el vertice mas pesado
                        valor = retornarRutanormalizada(hd.ruta);
                        bits += valor.Count() * 16;
                        hd.ruta = valor;
                    }
                }
                else
                {
                    return;
                }
            }

        }
        /// <summary>
        /// Se encarga de retornar la ruta al tamano necesario 50% eliminando los arcos mas pesados
        /// </summary>
        /// <param name="cromo"></param>
        /// <returns></returns>
        public static string retornarRutanormalizada(string cromo)
        {
            bits += cromo.Length * 16;
            masPesado = "00";
            bits += 32;
            for (int y = 0; y < cromo.Length; y++)
            {
                cantiArcosvisitados++;
                if (y + 1 != cromo.Length)
                {
                    salto = "" + cromo[y];
                    salto += cromo[y + 1];
                }
                else if (y + 1 == cromo.Length)
                {
                    salto = "00";
                }
                if (devuelvePeso(salto[0],salto[1]) > devuelvePeso(masPesado[0], masPesado[1]))
                {
                    masPesado = salto;
                    salto = "";
                }
            }
            for (int x = 0; x < cromo.Length; x++)
            {
                cantiArcosvisitados++;
                if (x + 1 != cromo.Length)
                {
                    if (cromo[x] == masPesado[0] && cromo[x + 1] == masPesado[1])
                    {
                        int verticeEliminar;
                        bits += 32;
                        if (x == 0)
                        {
                            verticeEliminar = x + 1;
                            string parte1 = cromo.Substring(0, verticeEliminar);
                            string parte2 = devuelveComplemento(cromo, verticeEliminar);
                            cromo = parte1 + parte2;
                            bits += parte1.Count() * 16;
                            bits += parte2.Count() * 16;
                            ptoParada--;
                            return cromo;
                        }
                        if (x != 0 && x != cromo.Length - 1)
                        {
                            verticeEliminar = x;
                            string parte1 = cromo.Substring(0, verticeEliminar);
                            string parte2 = devuelveComplemento(cromo, verticeEliminar);
                            cromo = parte1 + parte2;
                            bits += parte1.Count() * 16;
                            bits += parte2.Count() * 16;
                            ptoParada--;
                            return cromo;
                        }
                        else if(x==cromo.Length-1)//que psa si la posicion es el punto de cruce...pero si el punto de cruce es el segundo vertice o el penultimo 
                        {
                            verticeEliminar = x-1;
                            string parte1 = cromo.Substring(0, verticeEliminar);
                            string parte2 = devuelveComplemento(cromo, verticeEliminar);
                            cromo = parte1 + parte2;
                            bits += parte1.Count() * 16;
                            bits += parte2.Count() * 16;
                            ptoParada--;
                            return cromo;
                        }
                    }
                }
            }
            return cromo;
        }     
        /// <summary>
        /// Devuelve lo que le falta al cromosoma eliminado el vertice que pertenece al arco de mas peso
        /// </summary>
        /// <param name="cromosoma"></param>
        /// <param name="particion"></param>
        /// <returns></returns>
        public static string devuelveComplemento(string cromosoma,int particion)
        {
            bits += cromosoma.Length * 16;
            bits += 32;
            string cromo = "";
            bits += 48;
            for(int x = 0; x < cromosoma.Length; x++)
            {
                cantiArcosvisitados++;
                if (x > particion)
                {
                    cromo += cromosoma[x];
                }
            }
            return cromo;
        }    
        /// <summary>
        /// Retorna el peso de una arista del grafo
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static int devuelvePeso(char x,char y)
        {
            bits += 32;
            int temp = int.Parse(x.ToString());
            int temp1 = int.Parse(y.ToString());
            bits += 64;
            return matrizRutas[temp,temp1];
        }       
        /// <summary>
        /// Se encarga de substituir los hijos que son mejores que los padre en el arreglo de soluciones
        /// </summary>
        /// <param name="padre"></param>
        /// <param name="hijo"></param>
        public static void cambioEvolutivo(Ruta padre,Ruta hijo)
        {
            bits += 96;
            bits +=32;
            for(int x = 0; x < poblacionInicial.Length; x++)
            {
                cantiArcosvisitados++;
                if (padre.ruta.Equals(poblacionInicial[x].ruta))
                {
                    poblacionInicial[x].ruta = hijo.ruta;
                    poblacionInicial[x].peso = hijo.peso;
                }
            }
        }
        public static string mientrasExistanrepetidos(string cromo)
        {
            bits += cromo.Length * 16;
            while (bandera1==false)
            {
                cromo =adaptibilidadSecondpart(cromo);
            }
            return cromo;
        }      
        /// <summary>
        /// Se encarga de que no se repitan vertices en los hijos para no crear ciclos
        /// </summary>
        /// <param name="cromo"></param>
        public static string adaptibilidadSecondpart(string cromo)
        {
            listaVertices = new char[cromo.Length];
            countaVertices = 0;
            bits += cromo.Length * 16;
            bits += listaVertices.Length * 16;
            bits += 32;
            for(int x = 0; x < cromo.Length; x++)
            {
                cantiArcosvisitados++;
                if (estaVertice(cromo[x])== false)
                {
                    listaVertices[countaVertices] = cromo[x];
                    countaVertices++;
                }
                else
                {
                    int temp= r.Next(0, n)+48;
                    char g = (char)temp;
                    bits += 48;
                    while (existeVertice(g,cromo)==true)
                    {
                        temp = r.Next(0,n) + 48;
                        g = (char)temp;
                    }
                    cromo=reemplazarV(x,g,cromo);
                    bits += cromo.Count() * 16;
                    bandera1 = false;
                    return cromo;
                }
            }
            bandera1 = true;
            return cromo;
        }
        public static string reemplazarV(int a,char b,string cromo)
        {
            bits += 32;
            bits += 16;
            bits += cromo.Length;
            char[] arreglo = new char[cromo.Length];
            string temp = "";
            bits += arreglo.Length*16;
            bits += 48;
            for (int x = 0; x < cromo.Length; x++)
            {
                cantiArcosvisitados++;
                arreglo[x] = cromo[x];  
            }
            bits += 32;
            for (int y = 0; y < arreglo.Length; y++)
            {
                cantiArcosvisitados++;
                if (y==a && y == arreglo.Length - 1)
                {
                    if (cromo[a] == arreglo[y])
                    {
                        bits += 32;
                        for (int i = 0; i < cromo.Length; i++)
                        {
                            if (arreglo[a] == arreglo[i])
                            {
                                arreglo[i] = b;
                                break;
                            }
                        }
                    }
                }
                else if (y == a &&  y != arreglo.Length - 1)
                {
                    arreglo[y] = b;
                    break;
                }
            }
            bits += 32;
            for (int x = 0; x < arreglo.Length; x++)
            {
                cantiArcosvisitados++;
                temp += arreglo[x];
            }
            return temp;
        }      
        /// <summary>
        /// Se encarga de comprobar si existe el vertice en la lista de vertices
        /// </summary>
        /// <param name="gen"></param>
        /// <returns></returns>
        public static bool estaVertice(char gen)
        {
            bits += 32;
            bits += 16;
            for (int x = 0; x < listaVertices.Length; x++)
            {
                cantiArcosvisitados++;
                if (listaVertices[x]==gen)
                {
                    return true;
                }
            }
            return false;
        }     
        /// <summary>
        /// Se encarga de conprobar si existe un un gen en un cromosoma
        /// </summary>
        /// <param name="gen"></param>
        /// <param name="cromo"></param>
        /// <returns></returns>
        public static bool existeVertice(char gen,string cromo)
        {
            bits += cromo.Length * 16;
            bits += 16;
            bits += 32;
            for (int x = 0; x < cromo.Length; x++)
            {
                cantiArcosvisitados++;
                if (cromo[x] == gen)
                {
                    return true;
                }
            }
            return false;
        }     
        /// <summary>
        /// Se encarga de mutar en caso de que no exista cruce
        /// </summary>
        public static void mutarParacruce()
        {
            arreglo = new char[f.ruta.Length-2];
            int counta = 0;
            bits += 64;
            bits += f.ruta.Length - 2 * 16;
            for (int x = 1; x < f.ruta.Length-1; x++)
            {
                cantiArcosvisitados++;
                if (lamadreloTiene(f.ruta[x]) == false)
                {
                    arreglo[counta]= f.ruta[x];
                    counta++;
                }
            }
                int ran1 = r.Next(0, arreglo.Length - 1);
                int ran = r.Next(1, f.ruta.Length - 2);
                string temp = mo.ruta;
                bits += 64;
                bits += temp.Length * 16;
                temp = temp.Replace(temp[ran], arreglo[ran1]);
                mo.setRuta(temp);
            
            
        }      
        /// <summary>
        /// Verifica si la madre tiene un gen en especifico
        /// </summary>
        /// <param name="num"></param>
        /// 
        /// <returns></returns>
        public static bool lamadreloTiene(char num)
        {
            bits += 16;
            bits += 32;
            for (int x = 1; x < mo.ruta.Length - 1; x++)
            {
                cantiArcosvisitados++;
                if (mo.ruta[x] == num)
                {
                    return true;
                }
            }
            return false;
        }      
        /// <summary>
        /// Se encarga de realizar el cruce entre dos cromosomas
        /// </summary>
        /// <param name="padre"></param>
        /// <param name="madre"></param>
        /// <param name="puntoC"></param>
        public static void Cruce(String padre, String madre, int puntoC)
        {
            bits += padre.Length * 16;
            bits += madre.Length * 16;
            bits += 32;
            posPunCruma = devuelveposicionDePuntocruce(puntoC, madre);
            posPunCrupa = devuelveposicionDePuntocruce(puntoC, padre);
            hu = new Ruta(0, "");
            hd = new Ruta(0, "");
            bits += 128;
            for (int x = 0; x < posPunCrupa; x++)//copiar padre 1
            {
                cantiArcosvisitados++;
                hu.ruta+= padre[x];
            }
            bits += 32;
            for (int y = posPunCruma; y < madre.Length; y++)//copiar madre 2
            {
                cantiArcosvisitados++;
                hu.ruta += madre[y];
            }
            bits += 32;
            for (int x = 0; x < posPunCruma; x++)//copiar madre 1
            {
                cantiArcosvisitados++;
                hd.ruta += madre[x];
            }
            bits += 32;
            for (int y = posPunCrupa; y < padre.Length; y++)//copiar padre 2
            {
                cantiArcosvisitados++;
                hd.ruta += padre[y];
            }
        }        
        /// <summary>
        /// Devuelve la posicion en la que se encuentra el punto de cruce
        /// </summary>
        /// <param name="puntoCr"></param>
        /// <param name="cromosoma"></param>
        /// <returns></returns>
        public static int devuelveposicionDePuntocruce(int puntoCr,string cromosoma)
        {
            bits += 32;
            bits += cromosoma.Length * 16;
            bits += 32;
            for (int x =1; x < cromosoma.Length-1; x++)
            {
                cantiArcosvisitados++;
                int temp = int.Parse(cromosoma[x].ToString());
                bits += 32;
                if (temp==puntoCr)
                {
                    return x;
                }
            }
            return 0;
        }      
        /// <summary>
        /// Esta funcion se encarga de devolver el punto de cruce existente
        /// </summary>
        /// <param name="father"></param>
        /// <param name="mother"></param>
        /// <returns></returns>
        public static int DevuelvePuntoCruce(Ruta father,Ruta mother)
        {
            bits += father.ruta.Length * 16;
            bits += mother.ruta.Length * 16;
            bits += 64;
            bits += 32;
            for (int x = 0; x <n;x++)
            {
              cantiArcosvisitados++;
              if((estaNum(father.ruta,x)==true) && (estaNum(mother.ruta, x)==true))
                {
                    return x;
                }  
            }
            return -1;
        }
        /// <summary>
        /// Se encarga de verificar si existe algun punto de cruce
        /// </summary>
        /// <param name="father"></param>
        /// <param name="mother"></param>
        /// <returns></returns>
        public static bool existePuntoCruce(Ruta father, Ruta mother)
        {
            bits += father.ruta.Length * 16;
            bits += mother.ruta.Length * 16;
            bits += 64;
            bits += 32;
            for (int x = 0; x < n; x++)
            {
                cantiArcosvisitados++;
                if ((estaNum(father.ruta, x) == true) && (estaNum(mother.ruta, x) == true))
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Se encarga de verificar si el vertice existe en la ruta que recibe,osea si el gen se encuentra en el cromosoma
        /// </summary>
        /// <param name="cromosoma"></param>
        /// <param gen="gen"></param>
        /// <returns></returns>
        public static bool estaNum(string cromosoma,int gen)
        {
            bits += cromosoma.Length * 16;
            bits += 16;
            bits += 32;
            for (int x = 1; x < cromosoma.Length-1; x++)
            {
                cantiArcosvisitados++;
                bits += 32;
                int n1 = int.Parse(cromosoma[x].ToString());
                if (n1 == gen)
                {
                    return true;
                }
            }
            return false;
        }/// <summary>
        /// Se encarga de generar las combinaciones posibles entre las rutas
        /// </summary>
        public static void generaCombinaciones()
        {
            int counta = 0;
            bits += 32;
            randing = r.Next(0, poblacionInicial.Length);
            while (counta < 4)
            {
                int coun = 0;
                int[] temp = new int[2];
                bits += 64;
                while (coun < 2)
                {
                    while (estaCasado(randing)==false)
                    {
                        randing = r.Next(1, poblacionInicial.Length+1);
                    }
                    temp[coun] = randing;
                    casados[councasa] = randing;
                    councasa++;
                    coun++;

                }
                pareja p = new pareja(temp);
                bits += p.cromosoma.Count() * 16;
                parejas[countaparejas]=p;
                
                countaparejas++;
                counta++;
            }
        }/// <summary>
        /// Verifica si uno de las rutas ya tiene pareja
        /// </summary>
        /// <param name="ran"></param>
        /// <returns></returns>
        public static bool estaCasado(int ran)
        {
            bits += 64;
            for(int x = 0; x < casados.Length; x++)
            {
                if (casados[x] == ran)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Se encarga de generar la poblacion inicial
        /// </summary>
        public static void generarPoblacionInicial(int a,int b)
        {
            bits += 64;
            string ruta = ""+a;
            int counta = 0;
            bits += ruta.Count() * 16;
            bits += 32;
            while (counta < 8)
            {
                ruta = "" + a;
                int vertices = 1;
                int vertice = 0;
                bits += 64;
                while (vertices <= limite-2)//cuando termine el algoritmo restarle dos al limite
                {
                    int t = int.Parse(ruta[vertices - 1].ToString());
                    bits += 32;
                    while (t == vertice)
                    {
                        vertice = r.Next(0, n - 1);
                    }
                    if (vertices == limite-2)//cuando termine el algoritmo restarle dos al limite
                    {
                        while (vertice == b)
                        {
                            vertice = r.Next(0, n - 1);
                        }
                    }
                    ruta += vertice;
                    vertices++;
                }
                ruta += b;
                Ruta rut = new Ruta(determinarPesoRuta(ruta), ruta);
                bits += rut.ruta.Count() * 16;
                poblacionInicial[counta] = rut;
                counta++;
            }
        }/// <summary>
        /// Se encarga de sacar el peso de una ruta
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        public static int determinarPesoRuta(string ruta) {
            bits += ruta.Length * 16;
            int origen= int.Parse(Char.ToString(ruta[0]));
            int destino;
            int peso = 0;
            bits += 128;
            for(int x = 1; x < ruta.Length; x++)
            {
                int temp = int.Parse(Char.ToString(ruta[x]));
                bits += 32;
                destino = temp;
                peso += matrizRutas[origen,destino];
                origen = destino;
            }
            return peso;
            
                                                           }
        /// <summary>
        /// Clase que se encarga de representar una pareja
        /// </summary>
        public class pareja
        {
            public int[] cromosoma = new int[2];
            public pareja(int[] par)
            {
                cromosoma = par;
            }
        }/// <summary>
        /// Clase que se encarga de representar una rta
        /// </summary>
        public class Ruta
        {
            public int peso;
            public string ruta;
            public Ruta(int p,string r)
            {
                peso = p;
                ruta = r;
            }
            public void setRuta(string rut)
            {
                ruta = rut;
            }
            
        }
        //..............................................................FIN Algoritmo Genetico.............................................................................................
        //..............................................................Algoritmo BackTraking.............................................................................
        /// <summary>
        /// Algoritmo de Backtraking
        /// </summary>
        /// <param name="VI"></param>
        /// <param name="VF"></param>
        /// <param name="comb"></param>
        /// <param name="k"></param>
        /// <param name="lista"></param>
        /// <param name="dist"></param>
        static void Backtraking(string VI, string VF, string comb, int k, bool[,] lista, int dist)
        {

            bits =bits+ (VI.Length * 16);
            bits =bits+ (VF.Length * 16);
            bits =bits+ (comb.Length * 16);
            bits = bits+64;
            bits =bits+(lista.Length * 8);
            //retorna true o false del primer vertic concatenado, evalua que los saltos
                if (dist < distanciaCorta && verticesRecorridos(comb) <= saltos)
                {
                    if (k == Int32.Parse(VF.ToString()))
                    {

                        Console.WriteLine(comb + dist);
                        distanciaCorta = dist;
                        caminoCorto = comb;
                    }
                    else
                    {
                        for (int i = k; i < n; i++)
                        {
                            //evalua que los vertices no se repitan 
                            if (!lista[k, i])
                            {
                                lista[k, i] = true;
                                for (int j = 0; j < n; j++)
                                {
                                    if (i != j && i != Int32.Parse(VF.ToString()))
                                    {
                                        //marca los vertices para que el programa lleve una linea coherente en los caminos
                                        bits =bits+ 32;
                                        for (int h = 0; h < n; h++)
                                            lista[i, h] = true;
                                        //concatena y envia el vertice soguiente con el peso
                                        Backtraking(VI, VF, comb + i + "," + j + " | ", j, lista, dist + matrizRutas[i, j]);
                                    }
                                }
                                lista[k, i] = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    poda++;
                }
             
        }
            //si se sobre pasa retorna(poda)
            
        /// <summary>
        /// Verifica que empieza en el vertice que se necesita
        /// </summary>
        /// <param name="comb"></param>
        /// <param name="VI"></param>
        /// <returns></returns>
        
        /// <summary>
        /// Encuentra los vertices recorridos
        /// </summary>
        /// <param name="comb"></param>
        /// <returns></returns>
        static int verticesRecorridos(string comb)
        {
            bits += comb.Length * 16;
            int cont = 0;
            bits += 64;
            for (int i = 0; i < comb.Length; i++)
            {
                if (comb[i] == '|')
                {
                    cont++;
                }
            }
            return cont;
        }
//...................................................................Probabilistico........................................................................
        
        /// <summary>
        /// Crea la matriz inicial de feromonas
        /// </summary>
        static void crearMatrizFeromonas()
        {
            bits += 32;
            for (int i = 0; i < n; i++)
            {
                bits += 32;
                for (int j = 0; j < n; j++)
                {
                    matrizFeromonas[i, j] = (1 - 0.01) * fI;
                }
            }
        }
        /// <summary>
        ///Esta funcion agrega en la matriz de feromonas esto es donde cada hormiga al pasar va dejando su pequenna feromona 
        /// </summary>
        /// <param name="combR"></param>
        /// <param name="costo"></param>
        static void agregarEnMatriz(string combR, int costo)
        {
            bits += combR.Length * 16;
            bits += 32;
            string num1 = "";
            string num2 = "";
            bool band = false;
            bits += 32;
            bits += 8;
            bits += 32;
            for (int x = 0; x < combR.Length; x++)
            {
                if (combR[x] == 'C')
                {
                    break;
                }
                else if (combR[x] == '|')
                {
                    matrizFeromonas[Int32.Parse(num1), Int32.Parse(num2)] = matrizFeromonas[Int32.Parse(num1), Int32.Parse(num2)] + (1.0 / costo);
                    num2 = "";
                    num1 = "";
                }
                else
                {
                    if (combR[x] != ',' & band != true)
                    {
                        num1 = num1 + combR[x];
                    }
                    else if (combR[x] != ',' & band == true)
                    {
                        num2 = num2 + combR[x];
                    }
                    else
                    {
                        if (band)
                        {
                            band = false;
                        }
                        else
                        {
                            band = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Imprime las feromonas
        /// </summary>
        static void imprimirMatrizFeromonas()
        {
            string cadena = "";
            bits += 16;
            bits += 32;
            for (int i = 0; i < n; i++)
            {
                bits += 32;
                for (int j = 0; j < n; j++)
                {
                    cadena = cadena + matrizFeromonas[i, j] + " | ";
                }
                cadena = cadena + "\n";
            }
            Console.WriteLine(cadena);
        }
        /// <summary>
        ///Calcula la sumatoria de todos los vertices 
        /// </summary>
        /// <param name="VI"></param>
        /// <returns></returns>
        static double calcularSumatoria(int VI)
        {
            bits += 32;
            double sumatoria = 0.0;
            // Consigue los numeros de la sumatoria
            bits += 64;
            bits += 32;
            for (int i = 0; i < n; i++)
            {
                if (matrizRutas[VI, i] != 0)
                {
                    sumatoria = sumatoria + (fI * (1.0 / (double)matrizRutas[VI, i]));
                }
            }
            return sumatoria;
        }/// <summary>
        /// Se encarga de multiplica rlas feromosnas iniciales con esto se encarga de determinar cual es el camino que tiene mayor cantidad de feromonas
        /// </summary>
        /// <param name="VI"></param>
        /// <param name="sumatoria"></param>
        static void calcularVisibilidad(int VI, double sumatoria)
        {
            bits += 32;
            bits += 64;
            // a partir de la formula saca el porcentaje de cada numero
            int aux = 1;
            bits += 64;
            for (int i = 0; i < n; i++)
            {
                if (matrizRutas[VI, i] != 0)
                {
                    //se saca el la visibilidad la cual es el inverso del costo de camino
                    if (i == 0)
                    {
                        listaProbabilidadXY[i] = ((fI * (1.0 / (double)matrizRutas[VI, i])) / sumatoria);
                    }
                    else
                    {
                        listaProbabilidadXY[i] = listaProbabilidadXY[i - aux] + ((fI * (1.0 / (double)matrizRutas[VI, i])) / sumatoria);
                        aux = 1;
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        aux = 1;
                    }
                    else
                    {
                        aux = 2;
                    }
                    listaProbabilidadXY[i] = 0;
                }
            }
        }
        /// <summary>
        /// funcion para algoritmo probabilista
        /// </summary>
        /// <param name="VI"></param>
        /// <param name="VF"></param>
        /// <param name="costo"></param>
        /// <param name="combRutas"></param>
        /// <param name="hormiga"></param>
        static void probabilisticoHormigas(int VI, int VF, int costo, string combRutas, int hormiga)
        {
            bits += 128;
            bits += combRutas.Length * 16;
            double sumatoria = calcularSumatoria(VI);
            calcularVisibilidad(VI, sumatoria);
            Random rnd = new Random();
            bits += 32;
            double numA = rnd.NextDouble();
            bits += 128;
            //intentando tomar un camino
            bits += 32;
            for (int i = 0; i < n; i++)
            {
                if (numA <= listaProbabilidadXY[i] & listaProbabilidadXY[i] != 0)
                {

                    //manda el siguiente vertice que fue la hormiga
                    if (VF != i)
                    {
                        if (!matrizVisitados[i, 0])
                        {
                            costo += matrizRutas[VI, i];
                            //marca toda la fila true para que no se regrese a este vertice
                            bits += 32;
                            for (int j = 0; j < n; j++)
                            {
                                matrizVisitados[VI, j] = true;
                            }
                            //envia a la hormiga seguir eligiendo rutas
                            cantiArcosvisitados++;
                            probabilisticoHormigas(i, VF, costo, combRutas + VI + "," + i + "|", hormiga);
                            break;
                        }
                        else
                        {
                            //la hormiga quer'ia regresarse entonces se cambia el randon y se hace que se evalue de nuevo
                            //esto para no repetir verices
                            numA = rnd.NextDouble();
                            i = 0;
                        }
                    }

                    else
                    {
                        cantiArcosvisitados++;
                        costo = costo+matrizRutas[VI, i];
                        if (distanciaCorta >= costo & limite >= verticesRecorridos(combRutas))
                        {
                            distanciaCorta = costo;
                            caminoCorto = combRutas+ VI + "," + i + "|";
                            Console.Write("Hormiga " + hormiga );
                        }
                        combRutas = combRutas + VI + "," + i + "|" + "Costo " + costo;
                        Console.WriteLine(" Rutas " + combRutas);
                        agregarEnMatriz(combRutas, costo);
                        imprimirMatrizFeromonas();
                        break;

                    }
                }
            }
        }

    }
}
