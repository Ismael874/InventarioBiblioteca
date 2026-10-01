// ============================================================================
// Inventario de libros por sucursal
// Práctica sobre arreglos bidimensionales y análisis de complejidad Big O
//
// En este arreglo:
// - Las filas representan los libros (5).
// - Las columnas representan las sucursales (3).
// - stock[fila, columna] indica cuántas unidades de un libro hay disponibles
//   en una sucursal determinada.
//
// Para analizar la complejidad:
// - L = cantidad de libros (filas).
// - S = cantidad de sucursales (columnas).
// ============================================================================

using System;

namespace InventarioBiblioteca
{
    internal class Program
    {
        // Nombres de los libros (uno por cada fila de la matriz).
        static readonly string[] libros =
        {
            "Cien años de soledad",
            "Don Quijote de la Mancha",
            "El principito",
            "1984",
            "Rayuela"
        };

        // Nombres de las sucursales (una por cada columna de la matriz).
        static readonly string[] sucursales =
        {
            "Central",
            "Norte",
            "Sur"
        };

        // Matriz de inventario: 5 filas (libros) x 3 columnas (sucursales).
        static readonly int[,] stock =
        {
            //  Central  Norte  Sur
            {      8,      2,    5 },   // Cien años de soledad
            {      3,      7,    1 },   // Don Quijote de la Mancha
            {     10,      4,    6 },   // El principito
            {      0,      9,    3 },   // 1984
            {      5,      5,   12 }    // Rayuela
        };

        // Límite para considerar que un libro tiene bajo inventario.
        const int LIMITE_BAJO_INVENTARIO = 3;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            int opcion;

            do
            {
                MostrarMenu();
                opcion = LeerEntero("Seleccione una opción: ", 0, 7);
                Console.WriteLine();

                switch (opcion)
                {
                    case 1: MostrarInventario(); break;
                    case 2: ConsultarDisponibilidad(); break;
                    case 3: ActualizarDisponibilidad(); break;
                    case 4: TotalPorLibro(); break;
                    case 5: TotalPorSucursal(); break;
                    case 6: MostrarBajoInventario(); break;
                    case 7: SucursalConMayorDisponibilidad(); break;
                    case 0: Console.WriteLine("Saliendo del sistema. ¡Hasta luego!"); break;
                }

                if (opcion != 0)
                {
                    Console.WriteLine("\nPresione Enter para continuar...");
                    Console.ReadLine();
                    if (!Console.IsOutputRedirected)
                        Console.Clear();
                }
            } while (opcion != 0);
        }

        // --------------------------------------------------------------------
        //  Menú
        // --------------------------------------------------------------------
        static void MostrarMenu()
        {
            Console.WriteLine("====== INVENTARIO POR SUCURSAL ======");
            Console.WriteLine();
            Console.WriteLine("1. Mostrar inventario completo");
            Console.WriteLine("2. Consultar disponibilidad");
            Console.WriteLine("3. Actualizar disponibilidad");
            Console.WriteLine("4. Total disponible de un libro");
            Console.WriteLine("5. Total de inventario por sucursal");
            Console.WriteLine("6. Mostrar libros con bajo inventario");
            Console.WriteLine("7. Sucursal con mayor disponibilidad");
            Console.WriteLine("0. Salir");
            Console.WriteLine();
        }

        // --------------------------------------------------------------------
        //  1. Mostrar la matriz completa
        //  Complejidad: O(L × S) -> dos ciclos anidados recorren todas las celdas.
        // --------------------------------------------------------------------
        static void MostrarInventario()
        {
            Console.WriteLine("---- INVENTARIO COMPLETO ----\n");

            // Encabezado con los nombres de las sucursales.
            Console.Write($"{"#",-3}{"Libro",-28}");
            for (int col = 0; col < stock.GetLength(1); col++)
                Console.Write($"{sucursales[col],10}");
            Console.WriteLine();
            Console.WriteLine(new string('-', 31 + 10 * stock.GetLength(1)));

            // Ciclo externo: filas (libros). Ciclo interno: columnas (sucursales).
            for (int fila = 0; fila < stock.GetLength(0); fila++)
            {
                Console.Write($"{fila + 1,-3}{libros[fila],-28}");
                for (int col = 0; col < stock.GetLength(1); col++)
                    Console.Write($"{stock[fila, col],10}");
                Console.WriteLine();
            }
        }

        // --------------------------------------------------------------------
        //  2. Consultar disponibilidad
        //  Complejidad: O(1) -> acceso directo por índice stock[fila, columna].
        // --------------------------------------------------------------------
        static void ConsultarDisponibilidad()
        {
            Console.WriteLine("---- CONSULTAR DISPONIBILIDAD ----\n");
            int fila = SeleccionarLibro();
            int col = SeleccionarSucursal();

            int cantidad = stock[fila, col]; // acceso directo O(1)
            Console.WriteLine($"\n\"{libros[fila]}\" en sucursal {sucursales[col]}: {cantidad} unidad(es) disponible(s).");
            Console.WriteLine($"Posición en la matriz: stock[{fila},{col}]");
        }

        // --------------------------------------------------------------------
        //  3. Actualizar disponibilidad
        //  Complejidad: O(1) -> se escribe directamente en stock[fila, columna].
        // --------------------------------------------------------------------
        static void ActualizarDisponibilidad()
        {
            Console.WriteLine("---- ACTUALIZAR DISPONIBILIDAD ----\n");
            int fila = SeleccionarLibro();
            int col = SeleccionarSucursal();

            Console.WriteLine($"\nCantidad actual de \"{libros[fila]}\" en {sucursales[col]}: {stock[fila, col]}");
            int nueva = LeerEntero("Nueva cantidad: ", 0, int.MaxValue);

            stock[fila, col] = nueva; // escritura directa O(1)
            Console.WriteLine($"\nInventario actualizado. stock[{fila},{col}] = {nueva}");
        }

        // --------------------------------------------------------------------
        //  4. Total disponible de un libro
        //  Complejidad: O(S) -> se recorren todas las columnas de una sola fila.
        // --------------------------------------------------------------------
        static void TotalPorLibro()
        {
            Console.WriteLine("---- TOTAL DISPONIBLE DE UN LIBRO ----\n");
            int fila = SeleccionarLibro();

            int total = 0;
            for (int col = 0; col < stock.GetLength(1); col++)
            {
                Console.WriteLine($"  {sucursales[col],-10}: {stock[fila, col]}");
                total += stock[fila, col];
            }

            Console.WriteLine($"\nTotal de \"{libros[fila]}\" en todas las sucursales: {total} unidad(es).");
        }

        // --------------------------------------------------------------------
        //  5. Total de inventario por sucursal
        //  Complejidad: O(L) -> se recorren todas las filas de una sola columna.
        // --------------------------------------------------------------------
        static void TotalPorSucursal()
        {
            Console.WriteLine("---- TOTAL DE INVENTARIO POR SUCURSAL ----\n");
            int col = SeleccionarSucursal();

            int total = 0;
            for (int fila = 0; fila < stock.GetLength(0); fila++)
            {
                Console.WriteLine($"  {libros[fila],-28}: {stock[fila, col]}");
                total += stock[fila, col];
            }

            Console.WriteLine($"\nInventario total en sucursal {sucursales[col]}: {total} unidad(es).");
        }

        // --------------------------------------------------------------------
        //  6. Libros con bajo inventario (cantidad <= 3)
        //  Complejidad: O(L × S) -> se debe revisar cada celda de la matriz.
        // --------------------------------------------------------------------
        static void MostrarBajoInventario()
        {
            Console.WriteLine($"---- LIBROS CON BAJO INVENTARIO (<= {LIMITE_BAJO_INVENTARIO}) ----\n");
            bool encontrado = false;

            for (int fila = 0; fila < stock.GetLength(0); fila++)
            {
                for (int col = 0; col < stock.GetLength(1); col++)
                {
                    if (stock[fila, col] <= LIMITE_BAJO_INVENTARIO)
                    {
                        Console.WriteLine($"  stock[{fila},{col}] -> \"{libros[fila]}\" en {sucursales[col]}: {stock[fila, col]}");
                        encontrado = true;
                    }
                }
            }

            if (!encontrado)
                Console.WriteLine("No hay libros con bajo inventario.");
        }

        // --------------------------------------------------------------------
        //  7. Sucursal con mayor disponibilidad de un libro
        //  Complejidad: O(S) -> se comparan las columnas de una sola fila.
        // --------------------------------------------------------------------
        static void SucursalConMayorDisponibilidad()
        {
            Console.WriteLine("---- SUCURSAL CON MAYOR DISPONIBILIDAD ----\n");
            int fila = SeleccionarLibro();

            // Se asume que la primera sucursal es la mayor y se compara con las demás.
            int colMayor = 0;
            for (int col = 1; col < stock.GetLength(1); col++)
            {
                if (stock[fila, col] > stock[fila, colMayor])
                    colMayor = col;
            }

            Console.WriteLine($"\nLa sucursal con mayor disponibilidad de \"{libros[fila]}\" es " +
                              $"{sucursales[colMayor]} con {stock[fila, colMayor]} unidad(es).");

            // Informar si hay empate con otras sucursales.
            for (int col = 0; col < stock.GetLength(1); col++)
            {
                if (col != colMayor && stock[fila, col] == stock[fila, colMayor])
                    Console.WriteLine($"(Empata con la sucursal {sucursales[col]})");
            }
        }

        // --------------------------------------------------------------------
        //  Métodos auxiliares de entrada
        // --------------------------------------------------------------------

        // Muestra la lista de libros y devuelve el índice de fila elegido (0..L-1).
        static int SeleccionarLibro()
        {
            Console.WriteLine("Libros:");
            for (int i = 0; i < libros.Length; i++)
                Console.WriteLine($"  {i + 1}. {libros[i]}");
            return LeerEntero($"Número del libro (1-{libros.Length}): ", 1, libros.Length) - 1;
        }

        // Muestra la lista de sucursales y devuelve el índice de columna elegido (0..S-1).
        static int SeleccionarSucursal()
        {
            Console.WriteLine("Sucursales:");
            for (int i = 0; i < sucursales.Length; i++)
                Console.WriteLine($"  {i + 1}. {sucursales[i]}");
            return LeerEntero($"Número de la sucursal (1-{sucursales.Length}): ", 1, sucursales.Length) - 1;
        }

        // Lee un entero validado dentro del rango [min, max].
        static int LeerEntero(string mensaje, int min, int max)
        {
            while (true)
            {
                Console.Write(mensaje);
                string? entrada = Console.ReadLine();

                if (int.TryParse(entrada, out int valor) && valor >= min && valor <= max)
                    return valor;

                Console.WriteLine($"Entrada inválida. Ingrese un número entre {min} y {max}.");
            }
        }
    }
}
