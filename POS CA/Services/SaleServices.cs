using POS_CA.Data;
using POS_CA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_CA.Services
{
    public class SaleServices
    {
        private readonly DataContext _context;
        private Sale sale;
        private double currentSaleAmount;

        public SaleServices(DataContext context)
        {
            _context = context;
        }

        public void startNewSale()
        {
            sale = new Sale();
            currentSaleAmount = 0;
        }

        public void AddProductToSale(Product product)
        {
            sale.SaleProducts.Add(product);
            //_context.Sales
        }

        public Sale GetCurrentSale()
        {
            return sale;
        }

        public double GetSaleAmount()
        {
            return currentSaleAmount;
        }

        public bool AddProductToSale(Product product, int quantity, User user)
        {
            if (user.UserRole == UserRole.Cashier && product.Quantity >= quantity)
            {
                AddProductToSale(product);
                product.Quantity -= quantity;
                _context.Products.Update(product);

                currentSaleAmount += product.Price * quantity;
                return true;
            }
            return false;
        }

        public async Task EndCurrentSale()
        {
            if (_context != null && sale != null)
            {
                _context.Sales.AddAsync(sale);
                await _context.SaveChangesAsync();
                sale = null;
                currentSaleAmount = 0;

            }
        }

    }
}
