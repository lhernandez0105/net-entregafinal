using Proyecto_Maquina_Expendedora.Control;
using Proyecto_Maquina_Expendedora.Model;


namespace Proyecto_Maquina_Expendedora.View
{
    internal class View
    {
        static void Main(string[] args)
        {
           
            //area de mensajes
            string texto_bienvenida = "Bienvenido a la maquina expendedora";
            string texto_login = "Escoja tipo de cliente: \n[C = Clientes] \n[P = Proveedor]";
            string texto_preambulo_productos = "La Lista de productos es: ";
            string texto_escoger_producto_cliente = "Escoja un producto de la lista: ";
            string texto_escoger_producto = "Escoja un producto válido";


            Controller controller = Controller.GetInstance();

            Console.WriteLine(texto_bienvenida);

            //declaracion de input cliente
            string inputCliente = "";

            //ciclo de inicio del programa
            while (true)
            {
                do
                {
                    //Console.ReadLine(); solicita datos al cliente
                    Console.WriteLine(texto_login);

                    inputCliente = Console.ReadLine();

                } while (inputCliente != "C" && inputCliente != "P");

                Console.WriteLine(texto_preambulo_productos);

                Console.WriteLine(controller.DisplayProductList());

                if (inputCliente == "C")
                {
                    Console.WriteLine(texto_escoger_producto_cliente);

                    bool validProduct = false;
                    string productoSeleccionado = "";

                    do
                    {
                        productoSeleccionado = Console.ReadLine();
                        validProduct = controller.ProductExists(productoSeleccionado) && controller.ProductHasInventory(productoSeleccionado);

                        if (!validProduct)
                        {
                            Console.WriteLine(texto_escoger_producto);
                        }
                    } while (!validProduct);

                    Console.Write("Ingrese el monto a pagar: ");
                    int precioProducto = -1;

                    foreach (IProduct p in controller.ListaProductos)
                    {
                        if (p.Name.Equals(productoSeleccionado, StringComparison.OrdinalIgnoreCase))
                        {
                            precioProducto = p.Price;
                            break;
                        }
                    }

                    if (precioProducto == -1)
                    {
                        Console.WriteLine("Producto no encontrado. No se pudo determinar el precio.");
                    }
                    else
                    {
                        Console.WriteLine($"Precio del producto '{productoSeleccionado}': {precioProducto}.");

                        int montoPago = 0; // Inicializar el monto ingresado por el cliente
                        int cantidadBilletesIngresados = 0; // Contador de billetes ingresados

                        while (montoPago < precioProducto)
                        {
                            int billeteIngresado;

                            Console.Write("Ingrese un billete: ");
                            if (int.TryParse(Console.ReadLine(), out billeteIngresado))
                            {
                                if (IsValidAmount(billeteIngresado))
                                {
                                    montoPago += billeteIngresado;
                                    cantidadBilletesIngresados++;
                                    int faltaParaPagar = precioProducto - montoPago;

                                    if (faltaParaPagar > 0)
                                    {
                                        Console.WriteLine($"Ha ingresado {cantidadBilletesIngresados} billete(s). Falta pagar {faltaParaPagar}.");
                                    }
                                    else if (faltaParaPagar == 0)
                                    {
                                        Console.WriteLine("Pago completado. Realizando la compra...");
                                        controller.ProcessPurchase(productoSeleccionado, montoPago);
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Ha ingresado {cantidadBilletesIngresados} billete(s). Su cambio es {Math.Abs(faltaParaPagar)}.");
                                        controller.ProcessPurchase(productoSeleccionado, montoPago);
                                        break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Billete no válido. Debe ser una denominación válida.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Entrada no válida. Ingrese un número válido.");
                            }
                        }
                    }
                }
                else if (inputCliente == "P")
                {
                    Console.WriteLine("Modo proveedor: [A = Agregar producto] [R = Rellenar inventario]");
                    string opcionProveedor = Console.ReadLine();

                    if (opcionProveedor == "A")
                    {
                        // Agregar un nuevo producto
                        Console.Write("Ingrese el nombre del nuevo producto: ");
                        string nuevoNombre = Console.ReadLine();

                        Console.Write("Ingrese el precio del nuevo producto: ");
                        int nuevoPrecio = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Ingrese la cantidad inicial en inventario: ");
                        int nuevaCantidad = Convert.ToInt32(Console.ReadLine());

                        // Llamar al método del controlador para agregar el nuevo producto
                        controller.AddProduct(nuevoNombre, nuevoPrecio, nuevaCantidad);

                        Console.WriteLine($"Producto '{nuevoNombre}' agregado con éxito.");
                    }
                    else if (opcionProveedor == "R")
                    {
                        // Rellenar inventario de un producto existente
                        Console.WriteLine("Lista de productos disponibles:");
                        Console.WriteLine(controller.DisplayProductList());

                        Console.Write("Ingrese el nombre del producto a rellenar: ");
                        string productoARellenar = Console.ReadLine();

                        Console.Write("Ingrese la cantidad a agregar al inventario: ");
                        int cantidadAgregada = Convert.ToInt32(Console.ReadLine());

                        // Llamar al método del controlador para rellenar el inventario del producto
                        controller.RefillInventory(productoARellenar, cantidadAgregada);

                        Console.WriteLine($"Inventario de '{productoARellenar}' rellenado con {cantidadAgregada} unidades.");
                    }
                    else
                    {
                        Console.WriteLine("Opción no válida para proveedor.");
                    }
                }
            }
        }

        // Método para validar si el monto ingresado es válido
        static bool IsValidAmount(int amount)
        {
            int[] validCoins = Controller.Denominations.Coins;
            int[] validBills = Controller.Denominations.Bills;

            return validCoins.Contains(amount) || validBills.Contains(amount);
        }
    }
}


