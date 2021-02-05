using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }
        public IResult Add(Product product)
        {
            _productDal.Add(product);
            return new SuccessResult(message: Messages.ProductAdded);
        }

        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult(message: Messages.ProductDeleted);
        }

        public IDataResult<Product> GetById(int ProductId)
        {
            return new SuccessDataResult<Product>(data: _productDal.Get(p => p.ProductId == ProductId));
        }

        public IDataResult<List<Product>> GetList()
        {
            return new SuccessDataResult<List<Product>>(data: _productDal.GetList().ToList()) ;
        }

        public IDataResult<List<Product>> GetListByCategory(int CategoryId)
        {

            return new SuccessDataResult<List<Product>>(data: _productDal.GetList(p=>p.CategoryId==CategoryId).ToList());
        }

        public IResult Update(Product product)
        {
           _productDal.Update(product);
            return new SuccessResult(message: Messages.ProductUpdated);
        }
    }
}
