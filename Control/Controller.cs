using Proyecto_Maquina_Expendedora.Model;


namespace Proyecto_Maquina_Expendedora.Control
{
    public sealed class Controller
    {
        private static Controller _instance;
        public List<IProduct> ListaProductos { get; set; }
        private List<string> registroVentas;

        private Controller()
        {
            ListaProductos = new List<IProduct>
            {
                new Consumable("Cocacola", 3000, 15),
                new Consumable("PapitasMayo", 2500, 7),
                new Consumable("Chocolatina", 2200, 4)
            };
            registroVentas = new List<string>();
        }

        public static Controller GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Controller();
            }
            return _instance;
        }

        // Método para mostrar la lista de productos disponibles
        public string DisplayProductList()
        {
            string products = "";
            foreach (IProduct product in ListaProductos)
            {
                products += product.DisplayProduct() + "\n";
            }
            return products;
        }

        // Método para verificar si un producto existe
        public bool ProductExists(string productName)
        {
            return ListaProductos.Any(product => product.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));
        }


        // Método para verificar si hay inventario disponible para un producto
        public bool ProductHasInventory(string productName)
        {
            IProduct product = ListaProductos.FirstOrDefault(p => p.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));
            return product != null && product.Quantity > 0;
        }

        // Método para procesar una compra y devolver el cambio
        public void ProcessPurchase(string productName, int amountPaid)
        {
            IProduct product = ListaProductos.FirstOrDefault(p => p.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));

            if (product != null && product.Quantity > 0)
            {
                if (amountPaid >= product.Price)
                {
                    int change = amountPaid - product.Price;
                    product.Quantity--; // Resta al inventario

                    // Calcular y devolver el cambio
                    int[] coinsToReturn = CalculateChange(change);

                    // Mostrar el objeto comprado y el cambio
                    Console.WriteLine($"Producto comprado: {productName}");
                    Console.WriteLine("Cambio:");

                    for (int i = 0; i < Denominations.Coins.Length; i++)
                    {
                        if (coinsToReturn[i] > 0)
                        {
                            Console.WriteLine($"{Denominations.Coins[i]}: {coinsToReturn[i]} moneda(s)");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Monto insuficiente para comprar el producto.");
                }
            }
            else
            {
                Console.WriteLine("Producto no válido o sin inventario.");
            }
        }

        // Método para calcular el cambio utilizando las denominaciones de monedas
        private int[] CalculateChange(int changeAmount)
        {
            int[] change = new int[Denominations.Coins.Length];
            int remainingChange = changeAmount;

            for (int i = 0; i < Denominations.Coins.Length; i++)
            {
                change[i] = remainingChange / Denominations.Coins[i];
                remainingChange %= Denominations.Coins[i];
            }

            return change;
        }

           
        // método para agregar productos y rellenar inventario.
        public void AddProduct(string name, int price, int quantity)
        {
            IProduct newProduct = new Consumable(name, price, quantity);
            ListaProductos.Add(newProduct);
        }

        // Metodo para rellenar el inventario de un producto existente
        public void RefillInventory(string productName, int quantityToAdd)
        {
            IProduct product = ListaProductos.FirstOrDefault(p => p.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));
            if (product != null)
            {
                product.Quantity += quantityToAdd;
                Console.WriteLine($"Inventario de {productName} rellenado con {quantityToAdd} unidades.");
            }
            else
            {
                Console.WriteLine("Producto no encontrado. No se pudo rellenar el inventario.");
            }
        }

        //Método de las monedas y billetes disponibles
        public static class Denominations
        {
            public static readonly int[] Coins = { 500, 200, 100, 50 };
            public static readonly int[] Bills = { 1000, 2000, 5000, 10000, 20000, 50000, 100000 };
        }

       



    }
}
