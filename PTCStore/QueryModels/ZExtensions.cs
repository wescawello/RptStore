using LinqKit;
using PTCStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTCStore.QueryModels
{
    public static class ZExtensions
    {
        public static ExpressionStarter<Purchase> GetPredicate(this QPurchase query)
        {
            var predicate = PredicateBuilder.New<Purchase>(true);
            if (query.FormateId > 0)
                predicate = predicate.And(o => o.FormateId == query.FormateId);
            else
            {
                if (query.DeviceId > 0) predicate = predicate.And(o => o.DeviceId == query.DeviceId);
                if (query.VandorId > 0) predicate = predicate.And(o => o.VandorId == query.VandorId);
            }

            if (!string.IsNullOrEmpty(query.ApplyNumber))
            {
                predicate = predicate.And(o => o.ApplyNumber.Contains(query.ApplyNumber));
            }
            if (!string.IsNullOrEmpty(query.Remark))
            {
                predicate = predicate.And(o => o.Remark.Contains(query.Remark));
            }
            if (query.ApplyDateRange != null && query.ApplyDateRange.Count == 2) {

                predicate = predicate.And(o => o.ApplyDate >= query.ApplyDateRange.Min().Date && o.ApplyDate < query.ApplyDateRange.Max().Date.AddDays(1));

            }
            if (query.PurchaseDateRange != null && query.PurchaseDateRange.Count == 2)
            {

                predicate = predicate.And(o => o.PurchaseDate >= query.PurchaseDateRange.Min().Date && o.PurchaseDate < query.PurchaseDateRange.Max().Date.AddDays(1));

            }
            if (query.TotalMax != 1000)
            {
                predicate = predicate.And(o => o.PurchasePrice * o.Quantity <= query.TotalMax);
            }
            if (query.TotalMin > 0)
            {
                predicate = predicate.And(o => o.PurchasePrice * o.Quantity >= query.TotalMin);
            }
            if (query.Quantity > 0)
            {
                predicate = predicate.And(o => o.Quantity == query.Quantity);
            }
           
            return predicate;
        }


        public static ExpressionStarter<Vandor> GetPredicate(this QVandor query)
        {
            var predicate = PredicateBuilder.New<Vandor>(true);
            if (query.Status != null)
            {
                predicate = predicate.And(o => o.Status == query.Status);
            }
            if (query.TaxStatus != null)
            {
                predicate = predicate.And(o => o.TaxStatus == query.TaxStatus);
            }
            return predicate;
        }
        public static ExpressionStarter<Device> GetPredicate(this QDevice query)
        {
            var predicate = PredicateBuilder.New<Device>(true);
            if (query.Display != null)
            {
                predicate = predicate.And(o => o.Display == query.Display);
            }
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                predicate = predicate.And(o => o.Name.Contains(query.Name));
            }
            return predicate;
        }
        public static ExpressionStarter<Formate> GetPredicate(this QFormate query)
        {
            var predicate = PredicateBuilder.New<Formate>(true);
            return predicate;
        }
        public static ExpressionStarter<StockUnit> GetPredicate(this QStockUnit query)
        {
            var predicate = PredicateBuilder.New<StockUnit>(true);
            if (!string.IsNullOrEmpty(query.Keeper))
            {
                predicate = predicate.And(o => o.Keeper.Contains(query.Keeper));
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                predicate = predicate.And(o => o.Name.Contains(query.Name));
            }
            return predicate;
        }
        public static ExpressionStarter<StockOrder> GetPredicate(this QStockOrder query)
        {
            var predicate = PredicateBuilder.New<StockOrder>(true);



            return predicate;
        }

        public static ExpressionStarter<CustomerInfo> GetPredicate(this QCustomerInfo query)
        {
            var predicate = PredicateBuilder.New<CustomerInfo>(true);
            if (!string.IsNullOrEmpty(query.Name))
            {
                predicate = predicate.And(o => o.Name.Contains(query.Name));
            }
            if (!string.IsNullOrEmpty(query.Phone))
            {
                predicate = predicate.And(o => o.Phone.Contains(query.Phone));
            }
            if (!string.IsNullOrEmpty(query.Address))
            {
                predicate = predicate.And(o => o.Phone.Contains(query.Address));
            }
            if (!string.IsNullOrEmpty(query.Connecter))
            {
                predicate = predicate.And(o => o.Connecter.Contains(query.Connecter));
            }
            if (query.DefaultPayMethod.HasValue)
            {
                predicate = predicate.And(o => o.DefaultPayMethod == query.DefaultPayMethod);
            }


            if (query.QStatus.HasValue)
            {
                predicate = predicate.And(o => o.Status == query.QStatus.Value);
            }

            return predicate;
        }

        public static ExpressionStarter<PickOrder> GetPredicate(this QPickOrder query)
        {
            var predicate = PredicateBuilder.New<PickOrder>(true);
            if (!string.IsNullOrEmpty(query.TrackId))
            {
                predicate = predicate.And(o => o.TrackId.Contains(query.TrackId));
            }
            if (!string.IsNullOrEmpty(query.Keeper))
            {
                predicate = predicate.And(o => o.TrackId.Contains(query.Keeper));
            }
            if (!string.IsNullOrEmpty(query.Remark))
            {
                predicate = predicate.And(o => o.TrackId.Contains(query.Remark));
            }
            if (query.StockUnitId > 0)
            {
                predicate = predicate.And(o => o.StockUnitId.Equals(query.StockUnitId));
            }

            if (query.CustomerInfoId > 0)
            {
                predicate = predicate.And(o => o.CustomerInfoId.Equals(query.CustomerInfoId));
            }
            if (query.ForSqle)
            {
                predicate = predicate.And(o => !o.SaleOrderId.HasValue);
            }

            return predicate;
        }
        public static ExpressionStarter<SaleOrder> GetPredicate(this QSaleOrder query)
        {
            var predicate = PredicateBuilder.New<SaleOrder>(true);
            if (!string.IsNullOrEmpty(query.ApplyNumber))
            {
                predicate = predicate.And(o => o.ApplyNumber.Contains(query.ApplyNumber));
            }
            if ( query.Customer.CustomerInfoId>0)
            {
                predicate = predicate.And(o => o.Customer.CustomerInfoId.Equals(query.Customer.CustomerInfoId));
            }
            if (!string.IsNullOrEmpty(query.Customer.Connecter))
            {
                predicate = predicate.And(o => o.Customer.Connecter.Contains(query.Customer.Connecter));
            }
            if (!string.IsNullOrEmpty(query.Invoice))
            {
                predicate = predicate.And(o => o.Invoice.Contains(query.Invoice));
            }
            if (query.SaleDateRange != null && query.SaleDateRange.Count == 2)
            {
                predicate = predicate.And(o => o.SaleDate >= query.SaleDateRange.Min().Date && o.SaleDate < query.SaleDateRange.Max().Date.AddDays(1));
            }

            if (!string.IsNullOrEmpty(query.Barcode))
            {
                predicate = predicate.And(o => o.PickOrders.SelectMany(o=>o.PickOrderSubs).Select(p=>p.Barcode.BarcodeValue).Contains(query.Barcode));
            }
            return predicate;
        }
        public static ExpressionStarter<ReturnOrder> GetPredicate(this QReturnOrder query)
        {
            var predicate = PredicateBuilder.New<ReturnOrder>(true);



            return predicate;
        }
    }
}
