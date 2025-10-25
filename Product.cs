namespace StockManager.Models
{
    public class Product
    {
        public int Id 
        { 
            get => Id; 
            set
            {  
                if (value < 0) throw new ArgumentException("Id cannot be negative.");
                id = value; 
            }
        public string Name 
        { 
            get => Name; 
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Name cannot be empty.");
                name = value;
            }
        }
        public string Category 
        { 
            get => Category; 
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Category cannot be empty.");
                category = value;
            } 
        }
        public decimal Price 
        { 
            get => Price; 
            set
            {
                if (value < 0) throw new ArgumentException("Price cannot be negative.");
                price = value;
            } 
        }
        public int? Quantity // nullable for services
        { 
            get => Quantity; 
            // Quantity tracks the system’s recorded units for this product.
            // It may go negative to reflect returns, mischarges, or temporary discrepancies.
            // Physical stock may differ; this allows the program to simulate realistic sales scenarios.
            // Null value indicates the product is a type of service, which does not have a physical quantity.
            set = value;
            if (quantity < 0)
                {
                    // Simple flag for audit purposes
                    Console.WriteLine($"[FLAG] Quantity for product '{Name}' (ID: {Id}) is negative: {quantity}");
                }
        } 
    }    
}


