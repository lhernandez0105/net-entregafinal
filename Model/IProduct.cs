
namespace Proyecto_Maquina_Expendedora.Model
{
    public interface IProduct
    {
        int Quantity { get; set; }
        string Name { get; set; }
        int Price { get; }

        string DisplayProduct();
    }
}