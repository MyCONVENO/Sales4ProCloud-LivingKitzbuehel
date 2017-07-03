using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.Helper
{
    public class UploadItemClosedOrder
    {
        public ShoppingCart ShoppingCart { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}