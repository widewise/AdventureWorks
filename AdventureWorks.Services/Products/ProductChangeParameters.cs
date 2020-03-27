using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorks.Services.Products
{
    public class ProductChangeParameters
    {
        public ProductChangeParameters(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}