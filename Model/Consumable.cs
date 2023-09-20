namespace Proyecto_Maquina_Expendedora.Model
{
    public class Consumable : IProduct
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public Consumable(string name, int price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public string DisplayProduct()
        {
            return $"Nombre: {Name} - Precio: {Price} ({Quantity})";
          
        }
    }
}