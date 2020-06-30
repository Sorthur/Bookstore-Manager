using BookstoreManager.OrderManager;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookstoreManager.Bootstrap
{
    internal class DiConfig
    {
        private IKernel _kernel = null;

        public IKernel GetKernel()
        {
            if (_kernel != null)
            {
                return _kernel;
            }

            _kernel = new StandardKernel();
            _kernel.Bind<IOrderManager>().To<OrderManager.OrderManager>();

            return _kernel;
        }
    }
}