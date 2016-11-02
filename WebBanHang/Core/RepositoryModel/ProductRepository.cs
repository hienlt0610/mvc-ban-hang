using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebBanHang.Models;

namespace WebBanHang.Core.RepositoryModel
{
    public class ProductRepository : RepositoryModel<Product>
    {
        public ProductRepository(DbContext dbContext)
            : base(dbContext)
        {
        }

        public List<Product> GetNewProduct(int number)
        {
            return FetchAll().Where(item => item.Active == true)
                .OrderByDescending(item => item.CreateDate)
                .Take(number)
                .ToList();
        }

        public IEnumerable<Product> GetProductInGroup(int group)
        {
            return FetchAll().Where(item => item.GroupID == group).ToList();
        }

        public Product BestProductSale()
        {
            return null;
        }
    }
}
