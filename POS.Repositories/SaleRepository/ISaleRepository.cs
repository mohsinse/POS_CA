using POS.Model.Enum;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repositories.SaleRepository
{
    public interface ISaleRepository
    {
        public void startNewSale();

        public void AddProductToSale(Product product);

        public Sale GetCurrentSale();

        public double GetSaleAmount();

        public bool AddProductToSale(Product product, int quantity, User user);

        Task EndCurrentSale();
    }
}
