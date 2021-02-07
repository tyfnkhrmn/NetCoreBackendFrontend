﻿using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using Core.CrossCuttingConcerns.Validation;
using Core.Aspects.Autofac.Validation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Caching;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {

            _productDal.Add(product);
            return new SuccessResult(message: Messages.ProductAdded);
        }

        [CacheRemoveAspect("IProductService.Get")]
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
        [CacheAspect(1)]
        public IDataResult<List<Product>> GetListByCategory(int CategoryId)
        {

            return new SuccessDataResult<List<Product>>(data: _productDal.GetList(p=>p.CategoryId==CategoryId).ToList());
        }
        [TransactionScopeAspect]
        public IResult TransactionalOperation(Product product)
        {
            _productDal.Update(product);
            _productDal.Add(product);
            return new SuccessResult(message: Messages.ProductDeleted);
        }

        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
           _productDal.Update(product);
            return new SuccessResult(message: Messages.ProductUpdated);
        }
    }
}
