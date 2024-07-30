using POS.Model.Enum;
using POS.Model;
using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Repositories.SaleRepository;

namespace POS.Services.SaleServices
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleService;
        private Sale sale;
        private double currentSaleAmount;

        public SaleService(ISaleRepository saleService)
        {
            _saleService = saleService;
        }

        public void startNewSale()
        {
            sale = new Sale();
            currentSaleAmount = 0;
        }

        public void AddProductToSale(Product product)
        {
            _saleService.AddProductToSale(product);
        }

        public Sale GetCurrentSale()
        {
            return _saleService.GetCurrentSale();
        }

        public double GetSaleAmount()
        {
            return _saleService.GetSaleAmount();
        }

        public bool AddProductToSale(Product product, int quantity, User user)
        {
            return _saleService.AddProductToSale(product, quantity, user);
        }

        public async Task EndCurrentSale()
        {
            await _saleService.EndCurrentSale();
        }
    }
}
