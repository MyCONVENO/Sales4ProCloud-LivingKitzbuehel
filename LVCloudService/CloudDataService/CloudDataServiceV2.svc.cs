using CloudDataService.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace CloudDataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CloudDataServiceV2 : ICloudDataServiceV2
    {
        BlobstorageFileHandler blobhandler = new BlobstorageFileHandler("myconvenocoredata", "7STxKhoKsGQt2sed2JGb4gWtSIvzYj2SJ/PIFLW3AL2ch0FyTCJ1QAvEYBOyQo64iQZIa2z/NcwDTKGn/660vQ==", "syncdata-livingkb");
        LIVINGKITZBUEHLEntities currentData = getData();
        long initDatetimeTicks = new DateTime(2000, 1, 1).Ticks;
        DateTime minDatetimeTicks = new DateTime(2000, 2, 1);

        static LIVINGKITZBUEHLEntities getData()
        {
            LIVINGKITZBUEHLEntities model = new LIVINGKITZBUEHLEntities();
            model.Configuration.LazyLoadingEnabled = false;
            ((IObjectContextAdapter)model).ObjectContext.CommandTimeout = 12000;

            return model;
        }

        #region Agent

        IQueryable<Agent> GetAgentTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<Agent> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.Agent
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.Agent
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncAgent(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            try
            {
                return processDbresult<Agent>(GetAgentTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region ContactPerson

        IQueryable<ContactPerson> GetContactPersonTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<ContactPerson> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.ContactPerson
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.ContactPerson
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncContactPerson(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            try
            {

                return processDbresult<ContactPerson>(GetContactPersonTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Client
        IQueryable<UserClient> GetClientTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<UserClient> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.UserClient
                         where a.UserID == UserID
                         && a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;
            else
                result = from a in this.currentData.UserClient
                         where a.UserID == UserID
                         && a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncClient(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            try
            {
                return processDbresult<UserClient>(GetClientTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Stock
        IQueryable<Stock> GetStockTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<Stock> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.Stock
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.Stock
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncStock(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            try
            {
                return processDbresult<Stock>(GetStockTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region CustomerFavorite
        IQueryable<CustomerFavorite> GetCustomerFavoriteTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            var result = from a in this.currentData.CustomerFavorite
                         where a.SyncDateTime > SyncDateTime && a.UserID == UserID && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncCustomerFavorite(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            try
            {
                return processDbresult<CustomerFavorite>(GetCustomerFavoriteTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region CustomerNote
        IQueryable<CustomerNote> GetCustomerNoteTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<CustomerNote> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.CustomerNote
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.CustomerNote
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncCustomerNote(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            try
            {
                return processDbresult<CustomerNote>(GetCustomerNoteTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Article
        IQueryable<Article> GetArticleTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {

            IQueryable<Article> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.Article
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.Article
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncArticle(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            try
            {
                return processDbresult<Article>(GetArticleTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion    

        #region Association
        IQueryable<Association> GetAssociationTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<Association> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.Association
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.Association
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncAssociation(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            try
            {
                return processDbresult<Association>(GetAssociationTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region AssociationMember
        IQueryable<AssociationMember> GetAssociationMemberTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<AssociationMember> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.AssociationMember
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.AssociationMember
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncAssociationMember(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            try
            {
                return processDbresult<AssociationMember>(GetAssociationMemberTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Assortment
        IQueryable<Assortment> GetAssortmentTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            LIVINGKITZBUEHLEntities serverdata = getData();

            IQueryable<Assortment> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in serverdata.Assortment
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in serverdata.Assortment
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncAssortment(string SyncDateTime, string UserID)
        {

            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            try
            {
                return processDbresult<Assortment>(GetAssortmentTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                SendMail("sascha.weber@myconveno.de", ex.Message, "TSTEIN ERROR: SyncAssortment", null);
                return string.Empty;
            }
        }
        #endregion

        #region Color
        IQueryable<Color> GetColorTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<Color> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.Color
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.Color
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncColor(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<Color>(GetColorTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Customer
        IQueryable<Customer> GetCustomerTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<Customer> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.Customer
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.Customer
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncCustomer(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<Customer>(GetCustomerTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region DeliveryAddress
        IQueryable<DeliveryAddress> GetDeliveryAddressTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {


            IQueryable<DeliveryAddress> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.DeliveryAddress
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.DeliveryAddress
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncDeliveryAddress(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<DeliveryAddress>(GetDeliveryAddressTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region InvoiceAddress
        IQueryable<InvoiceAddress> GetInvoiceAddressTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {

            IQueryable<InvoiceAddress> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.InvoiceAddress
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.InvoiceAddress
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncInvoiceAddress(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<InvoiceAddress>(GetInvoiceAddressTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Label
        IQueryable<Label> GetLabelTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<Label> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.Label
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.Label
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncLabel(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<Label>(GetLabelTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Model
        IQueryable<Model> GetModelTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            var result = from a in this.currentData.Model
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;


            if (SyncDateTime < minDatetimeTicks)
                result = result.Where(r => r.IsDeleted == false);

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncModel(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<Model>(GetModelTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Price
        IQueryable<Price> GetPriceTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<Price> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.Price
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.Price
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncPrice(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<Price>(GetPriceTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Pricelist
        IQueryable<Pricelist> GetPricelistTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            return this.currentData.Pricelist;
            if (SyncDateTime < minDatetimeTicks)
            {
                var pls = (from a in this.currentData.UserPriceList
                           where a.IsDeleted == false
                           select new
                           {
                               Currency = a.Pricelist.Currency,
                               IsDefault = a.Pricelist.IsDefault,
                               IsDeleted = a.IsDeleted,
                               PricelistClientID = a.Pricelist.PricelistClientID,
                               PricelistID = a.PricelistID,
                               PricelistName = a.Pricelist.PricelistName,
                               PricelistNumber = a.Pricelist.PricelistNumber,
                               SyncDateTimeTicks = a.SyncDateTime
                           }).ToList();

                List<Pricelist> newlists = new List<Pricelist>();
                foreach (var p in pls)
                {
                    newlists.Add(new Pricelist()
                    {
                        Currency = p.Currency,
                        IsDefault = p.IsDefault,
                        IsDeleted = p.IsDeleted.Value,

                        PricelistClientID = p.PricelistClientID,
                        PricelistID = p.PricelistID,
                        PricelistName = p.PricelistName,
                        PricelistNumber = p.PricelistNumber,
                        SyncDateTime = p.SyncDateTimeTicks.Value
                    });
                }

                return newlists.AsQueryable();
            }

            else
            {
                var pls = (from a in this.currentData.UserPriceList
                           select new
                           {
                               Currency = a.Pricelist.Currency,
                               IsDefault = a.Pricelist.IsDefault,
                               IsDeleted = a.IsDeleted,
                               PricelistClientID = a.Pricelist.PricelistClientID,
                               PricelistID = a.PricelistID,
                               PricelistName = a.Pricelist.PricelistName,
                               PricelistNumber = a.Pricelist.PricelistNumber,
                               SyncDateTimeTicks = a.SyncDateTime
                           }).ToList();
                List<Pricelist> newlists = new List<Pricelist>();
                foreach (var p in pls)
                {
                    newlists.Add(new Pricelist()
                    {
                        Currency = p.Currency,
                        IsDefault = p.IsDefault,
                        IsDeleted = p.IsDeleted.Value,
                        PricelistClientID = p.PricelistClientID,
                        PricelistID = p.PricelistID,
                        PricelistName = p.PricelistName,
                        PricelistNumber = p.PricelistNumber,
                        SyncDateTime = p.SyncDateTimeTicks.Value
                    });
                }

                return newlists.AsQueryable();
            }
        }


        public string SyncPricelist(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<Pricelist>(GetPricelistTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Season
        IQueryable<Season> GetSeasonTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<Season> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.Season
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.Season
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncSeason(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<Season>(GetSeasonTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Sizerun
        IQueryable<Sizerun> GetSizerunTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<Sizerun> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.Sizerun
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.Sizerun
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncSizerun(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<Sizerun>(GetSizerunTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region ProductImage

        IQueryable<ProductImage> GetProductImageTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<ProductImage> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.ProductImage
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.ProductImage
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncProductImage(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<ProductImage>(GetProductImageTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region User
        IQueryable<User> GetUserTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<User> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.User
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.UserID == UserID && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.User
                         where a.SyncDateTime > SyncDateTime && a.UserID == UserID && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncUser(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<User>(GetUserTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region ShoppingCart
        IQueryable<ShoppingCart> GetShoppingCartTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<ShoppingCart> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.ShoppingCart
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.UserID == UserID && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.ShoppingCart
                         where a.SyncDateTime > SyncDateTime && a.UserID == UserID && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncShoppingCart(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<ShoppingCart>(GetShoppingCartTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region ShoppingCartItem
        IQueryable<ShoppingCartItem> GetShoppingCartItemTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<ShoppingCartItem> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.ShoppingCartItem
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.UserID == UserID && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.ShoppingCartItem
                         where a.SyncDateTime > SyncDateTime && a.UserID == UserID && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncShoppingCartItem(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<ShoppingCartItem>(GetShoppingCartItemTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region PushChannel
        enum NotificationTypes
        {
            Tile, Toast, GhostToast, ToastDelete, Badge, Raw
        }


        public string UpdatePushChannel(string UserID, string OldChannelUri, string NewChannelUri)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            try
            {

                //OldChannelUri = FileSerializer.FromBase64String(OldChannelUri);
                //NewChannelUri = FileSerializer.FromBase64String(NewChannelUri);
                var channel = (from data in currentData.PushChannel
                               where data.ChannelUri == OldChannelUri
                               select data).FirstOrDefault();

                if (channel != null)
                {
                    channel.ChannelUri = NewChannelUri;
                    channel.UserID = UserID;
                }
                else
                {
                    var existingchannel = (from data in currentData.PushChannel
                                           where data.ChannelUri == NewChannelUri
                                           select data).FirstOrDefault();

                    if (existingchannel == null)
                    {
                        PushChannel newchannel = new PushChannel();
                        newchannel.UserID = UserID;
                        newchannel.ChannelUri = NewChannelUri;

                        currentData.PushChannel.Add(newchannel);
                    }
                }

                currentData.SaveChanges();


                return "true";
            }
            catch (Exception ex)
            {
                return "false";
            }
        }


        public string SendPushNotification(string UserIDs)
        {

            string secret = "KOYSmD5FHoOOyIx1MAs9/udkCsiORHFV";
            string sid = "ms-app://s-1-15-2-4049723377-2063029911-2665137763-2989306150-2776258536-2312981175-1490370008";
            try
            {
                foreach (string UserID in UserIDs.Split(';'))
                {
                    var channels = from data in currentData.PushChannel
                                   where data.UserID == UserID
                                   select data;

                    foreach (var c in channels)
                    {
                        if (!sendPush(NotificationTypes.Raw, c.ChannelUri, secret, sid))
                        {
                            currentData.PushChannel.Remove(c);
                        }
                    }
                }
                currentData.SaveChanges();
                return "true";
            }
            catch (Exception ex)
            {
                return "false";
            }

        }

        // Post to WNS
        string PostToWns(string secret, string sid, string uri, string xml, string notificationType, string contentType, NotificationTypes notType)
        {
            try
            {
                // You should cache this access token.
                var accessToken = GetAccessToken(secret, sid);

                byte[] contentInBytes = Encoding.UTF8.GetBytes(xml);

                var request = HttpWebRequest.Create(uri) as HttpWebRequest;
                //if (notType != NotificationTypes.ToastDelete)
                //{
                request.Method = "POST";
                request.Headers.Add("X-WNS-Type", notificationType);

                //Windows Phone
                //if (GroupAndTagList.SelectedIndex == 0)
                //{
                //    request.Headers.Add("X-WNS-Group", txtGroup.Text);
                //    request.Headers.Add("X-WNS-Tag", txtTag.Text);

                //}
                //else if (GroupAndTagList.SelectedIndex == 1) { request.Headers.Add("X-WNS-Group", txtGroupOnly.Text); }
                //else if (GroupAndTagList.SelectedIndex == 2) { request.Headers.Add("X-WNS-Tag", txtTagOnly.Text); }

                request.ContentType = contentType;
                request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken.AccessToken));
                //if (notType != NotificationTypes.GhostToast)
                //{ request.Headers.Add("X-WNS-SuppressPopup", "true"); }

                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(contentInBytes, 0, contentInBytes.Length);

                using (var webResponse = (HttpWebResponse)request.GetResponse())
                    return webResponse.StatusCode.ToString();

                //}
                //else
                //{
                //    string deleteArgs = "type=wns/toast;";
                //    if (GroupAndTagList.SelectedIndex == 0) { deleteArgs += ("group=" + txtGroup.Text + ";tag=" + txtTag.Text); }
                //    else if (GroupAndTagList.SelectedIndex == 1) { deleteArgs += ("group=" + txtGroupOnly.Text); }
                //    else if (GroupAndTagList.SelectedIndex == 2) { deleteArgs += ("tag=" + txtTagOnly.Text); }
                //    else if (GroupAndTagList.SelectedIndex == 3) { deleteArgs += "all"; }

                //    request.Method = "DELETE";
                //    request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken.AccessToken));
                //    request.Headers.Add("X-WNS-Match", deleteArgs);

                //    //using (Stream requestStream = request.GetRequestStream())
                //    //    requestStream.Write(contentInBytes, 0, contentInBytes.Length);

                //    using (var webResponse = (HttpWebResponse)request.GetResponse())
                //        return webResponse.StatusCode.ToString();
                //}


            }
            catch (WebException webException)
            {
                string exceptionDetails = webException.Response.Headers["WWW-Authenticate"];
                if (exceptionDetails.Contains("Token expired"))
                {
                    GetAccessToken(secret, sid);

                    // We suggest that you implement a maximum retry policy.
                    return PostToWns(uri, xml, secret, sid, notificationType, contentType, notType);
                }
                else
                {
                    // Log the response
                    return "EXCEPTION: " + webException.Message;
                }
            }
            catch (Exception ex)
            {
                return "EXCEPTION: " + ex.Message;
            }
        }

        // Authorization
        [DataContract]
        class OAuthToken
        {
            [DataMember(Name = "access_token")]
            public string AccessToken { get; set; }
            [DataMember(Name = "token_type")]
            public string TokenType { get; set; }
        }

        OAuthToken GetOAuthTokenFromJson(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                var ser = new DataContractJsonSerializer(typeof(OAuthToken));
                var oAuthToken = (OAuthToken)ser.ReadObject(ms);
                return oAuthToken;
            }
        }

        OAuthToken GetAccessToken(string secret, string sid)
        {
            var urlEncodedSecret = HttpUtility.UrlEncode(secret);
            var urlEncodedSid = HttpUtility.UrlEncode(sid);

            var body = String.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=notify.windows.com",
                                     urlEncodedSid,
                                     urlEncodedSecret);

            string response;
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                response = client.UploadString("https://login.live.com/accesstoken.srf", body);
            }
            return GetOAuthTokenFromJson(response);
        }

        bool sendPush(NotificationTypes notType, string channeluri, string secret, string sid)
        {
            try
            {
                string notificationType = string.Empty;
                string contentType = string.Empty;


                string uri = channeluri;

                string xml = getPayLoad(notType);

                if (xml != "")
                {
                    switch (notType)
                    {
                        case NotificationTypes.Tile:
                            notificationType = "wns/tile"; contentType = "text/xml";
                            break;

                        case NotificationTypes.Toast:
                        case NotificationTypes.GhostToast:
                        case NotificationTypes.ToastDelete:
                            notificationType = "wns/toast"; contentType = "text/xml";
                            break;

                        case NotificationTypes.Badge:
                            notificationType = "wns/badge"; contentType = "text/xml";
                            break;

                        case NotificationTypes.Raw:
                            notificationType = "wns/raw"; contentType = "application/octet-stream";
                            break;
                    }

                    var s = PostToWns(secret, sid, uri, xml, notificationType, contentType, notType);
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        string getPayLoad(NotificationTypes notType)
        {
            switch (notType)
            {
                case NotificationTypes.Tile:
                    return
                        "<tile>" +
                            "<visual version=\"2\">" +
                                "<binding template=\"TileSquare150x150Text01\" fallback=\"TileSquareText01\">" +
                                    "<text id=\"1\">Text Field 1 (larger text)</text>" +
                                    "<text id=\"2\">Text Field 2</text>" +
                                    "<text id=\"3\">Text Field 3</text>" +
                                    "<text id=\"4\">Text Field 4</text>" +
                                "</binding>" +
                            "</visual>" +
                        "</tile>";

                case NotificationTypes.Toast:
                    return
                        "<toast>" +
                            "<visual>" +
                                "<binding template=\"ToastText02\">" +
                                    "<text id=\"1\">headlineText</text>" +
                                    "<text id=\"2\">bodyText</text>" +
                                "</binding>" +
                            "</visual>" +
                        "</toast>";

                case NotificationTypes.GhostToast:
                    return
                        "<toast>" +
                            "<visual>" +
                                "<binding template=\"ToastText02\">" +
                                    "<text id=\"1\">Shhhhh...</text>" +
                                    "<text id=\"2\">So ghostly...</text>" +
                                "</binding>" +
                            "</visual>" +
                        "</toast>";

                case NotificationTypes.ToastDelete:
                    return "";

                case NotificationTypes.Badge:
                    return "<badge value=\"99\"/>";

                default: return "neue Daten";
            }
        }


        #endregion

        #region TextSnippet
        IQueryable<TextSnippet> GetTextSnippetTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<TextSnippet> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.TextSnippet
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.TextSnippet
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncTextSnippet(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<TextSnippet>(GetTextSnippetTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region SpecialDiscount
        IQueryable<SpecialDiscount> GetSpecialDiscountTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<SpecialDiscount> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.SpecialDiscount
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.SpecialDiscount
                         where a.SyncDateTime > SyncDateTime && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncSpecialDiscount(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<SpecialDiscount>(GetSpecialDiscountTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region UserAgent
        IQueryable<UserAgents> GetUserAgentTable(long SyncDateTimeSortStart, DateTime SyncDateTime, string UserID)
        {
            IQueryable<UserAgents> result = null;

            if (SyncDateTime < minDatetimeTicks)
                result = from a in this.currentData.UserAgents
                         where a.SyncDateTime > SyncDateTime && a.IsDeleted == false && a.UserID == UserID && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            else
                result = from a in this.currentData.UserAgents
                         where a.SyncDateTime > SyncDateTime && a.UserID == UserID && a.SyncDateTimeSort > SyncDateTimeSortStart
                         select a;

            return result.OrderBy(r => r.SyncDateTimeSort);
        }


        public string SyncUserAgent(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;


            try
            {
                return processDbresult<UserAgents>(GetUserAgentTable, new DateTime(Convert.ToInt64(SyncDateTime)), UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region GetChanges
        public string GetAvailableChangesBaseData(Dictionary<string, long> SyncDateTimes, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            var json = getJson(GetAvailableChangesBaseDataXML(SyncDateTimes, UserID));

            var zip = ZipHelper.ZipStringV2(json);

            return Convert.ToBase64String(zip);
        }

        List<SyncProgressItem> GetAvailableChangesBaseDataXML(Dictionary<string, long> SyncDateTimes, string UserID)
        {
            try
            {
                var user = from u in this.currentData.User
                           where u.UserID == UserID
                           select u;

                if (!user.Any())
                    return new List<SyncProgressItem>();

                long syncDateTimeAgent = SyncDateTimes.ContainsKey("SyncDateTimeAgent") ? SyncDateTimes["SyncDateTimeAgent"] : 630822816000000000;
                long syncDateTimeArticle = SyncDateTimes.ContainsKey("SyncDateTimeArticle") ? SyncDateTimes["SyncDateTimeArticle"] : 630822816000000000;
                long syncDateTimeArticleSizerun = SyncDateTimes.ContainsKey("SyncDateTimeArticleSizerun") ? SyncDateTimes["SyncDateTimeArticleSizerun"] : 630822816000000000;
                long syncDateTimeAssociation = SyncDateTimes.ContainsKey("SyncDateTimeAssociation") ? SyncDateTimes["SyncDateTimeAssociation"] : 630822816000000000;
                long syncDateTimeAssociationMember = SyncDateTimes.ContainsKey("SyncDateTimeAssociationMember") ? SyncDateTimes["SyncDateTimeAssociationMember"] : 630822816000000000;
                long syncDateTimeAssortment = SyncDateTimes.ContainsKey("SyncDateTimeAssortment") ? SyncDateTimes["SyncDateTimeAssortment"] : 630822816000000000;
                long syncDateTimeColor = SyncDateTimes.ContainsKey("SyncDateTimeColor") ? SyncDateTimes["SyncDateTimeColor"] : 630822816000000000;
                long syncDateTimeCustomer = SyncDateTimes.ContainsKey("SyncDateTimeCustomer") ? SyncDateTimes["SyncDateTimeCustomer"] : 630822816000000000;
                long syncDateTimeDeliveryAddress = SyncDateTimes.ContainsKey("SyncDateTimeDeliveryAddress") ? SyncDateTimes["SyncDateTimeDeliveryAddress"] : 630822816000000000;
                long syncDateTimeInvoiceAddress = SyncDateTimes.ContainsKey("SyncDateTimeInvoiceAddress") ? SyncDateTimes["SyncDateTimeInvoiceAddress"] : 630822816000000000;
                long syncDateTimeLabel = SyncDateTimes.ContainsKey("SyncDateTimeLabel") ? SyncDateTimes["SyncDateTimeLabel"] : 630822816000000000;
                long syncDateTimeModel = SyncDateTimes.ContainsKey("SyncDateTimeModel") ? SyncDateTimes["SyncDateTimeModel"] : 630822816000000000;
                long syncDateTimePrice = SyncDateTimes.ContainsKey("SyncDateTimePrice") ? SyncDateTimes["SyncDateTimePrice"] : 630822816000000000;
                long syncDateTimePricelist = SyncDateTimes.ContainsKey("SyncDateTimePricelist") ? SyncDateTimes["SyncDateTimePricelist"] : 630822816000000000;
                long syncDateTimeSeason = SyncDateTimes.ContainsKey("SyncDateTimeSeason") ? SyncDateTimes["SyncDateTimeSeason"] : 630822816000000000;
                long syncDateTimeSizerun = SyncDateTimes.ContainsKey("SyncDateTimeSizerun") ? SyncDateTimes["SyncDateTimeSizerun"] : 630822816000000000;
                long syncDateTimeUser = SyncDateTimes.ContainsKey("SyncDateTimeUser") ? SyncDateTimes["SyncDateTimeUser"] : 630822816000000000;
                long syncDateTimeContactPerson = SyncDateTimes.ContainsKey("SyncDateTimeContactPerson") ? SyncDateTimes["SyncDateTimeContactPerson"] : 630822816000000000;
                long syncDateTimeClient = SyncDateTimes.ContainsKey("SyncDateTimeClient") ? SyncDateTimes["SyncDateTimeClient"] : 630822816000000000;
                long syncDateTimeStock = SyncDateTimes.ContainsKey("SyncDateTimeStock") ? SyncDateTimes["SyncDateTimeStock"] : 630822816000000000;
                long syncDateTimeTextSnippet = SyncDateTimes.ContainsKey("SyncDateTimeTextSnippet") ? SyncDateTimes["SyncDateTimeTextSnippet"] : 630822816000000000;
                long syncDateTimeSpecialDiscount = SyncDateTimes.ContainsKey("SyncDateTimeSpecialDiscount") ? SyncDateTimes["SyncDateTimeSpecialDiscount"] : 630822816000000000;
                long syncDateTimeUserAgent = SyncDateTimes.ContainsKey("SyncDateTimeUserAgent") ? SyncDateTimes["SyncDateTimeUserAgent"] : 630822816000000000;
                long syncDateTimeProductImage = SyncDateTimes.ContainsKey("SyncDateTimeProductImage") ? SyncDateTimes["SyncDateTimeProductImage"] : 630822816000000000;
                long syncDateTimeShoppingCart = SyncDateTimes.ContainsKey("SyncDateTimeShoppingCart") ? SyncDateTimes["SyncDateTimeShoppingCart"] : 630822816000000000;
                long syncDateTimeShoppingCartItem = SyncDateTimes.ContainsKey("SyncDateTimeShoppingCartItem") ? SyncDateTimes["SyncDateTimeShoppingCartItem"] : 630822816000000000;
                long syncDateTimeSeasonValue = SyncDateTimes.ContainsKey("SyncDateTimeSeasonValue") ? SyncDateTimes["SyncDateTimeSeasonValue"] : 630822816000000000;
                long syncDateTimeCustomerNote = SyncDateTimes.ContainsKey("SyncDateTimeCustomerNote") ? SyncDateTimes["SyncDateTimeCustomerNote"] : 630822816000000000;
                long syncDateTimeCustomerFavorite = SyncDateTimes.ContainsKey("SyncDateTimeCustomerFavorite") ? SyncDateTimes["SyncDateTimeCustomerFavorite"] : 630822816000000000;


                List<SyncProgressItem> results = new List<SyncProgressItem>();

                int changes = 0;

                var currentUser = user.First();

                changes = GetAgentTable(0, new DateTime(syncDateTimeAgent), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Agent", changes));

                changes = GetArticleTable(0, new DateTime(syncDateTimeArticle), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Article", changes));

                //changes = GetArticleSizerunTable(syncDateTimeArticleSizerun).Select(a=>a.IsDeleted).Count();
                //if (changes > 0) results.Add(new SyncProgressItem("ArticleSizerun", changes));

                changes = GetAssociationTable(0, new DateTime(syncDateTimeAssociation), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Association", changes));

                changes = GetAssociationMemberTable(0, new DateTime(syncDateTimeAssociationMember), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("AssociationMember", changes));

                changes = GetAssortmentTable(0, new DateTime(syncDateTimeAssortment), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Assortment", changes));

                changes = GetColorTable(0, new DateTime(syncDateTimeColor), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Color", changes));

                changes = GetCustomerTable(0, new DateTime(syncDateTimeCustomer), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Customer", changes));

                changes = GetDeliveryAddressTable(0, new DateTime(syncDateTimeDeliveryAddress), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("DeliveryAddress", changes));

                changes = GetInvoiceAddressTable(0, new DateTime(syncDateTimeInvoiceAddress), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("InvoiceAddress", changes));

                changes = GetLabelTable(0, new DateTime(syncDateTimeLabel), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Label", changes));

                changes = GetClientTable(0, new DateTime(syncDateTimeClient), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Client", changes));

                changes = GetModelTable(0, new DateTime(syncDateTimeModel), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Model", changes));

                changes = GetPriceTable(0, new DateTime(syncDateTimePrice), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Price", changes));

                changes = GetPricelistTable(0, new DateTime(syncDateTimePricelist), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Pricelist", changes));

                changes = GetSeasonTable(0, new DateTime(syncDateTimeSeason), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Season", changes));

                changes = GetSizerunTable(0, new DateTime(syncDateTimeSizerun), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Sizerun", changes));

                changes = GetContactPersonTable(0, new DateTime(syncDateTimeContactPerson), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("ContactPerson", changes));

                changes = GetUserTable(0, new DateTime(syncDateTimeUser), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("User", changes));

                changes = GetStockTable(0, new DateTime(syncDateTimeStock), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("Stock", changes));

                changes = GetTextSnippetTable(0, new DateTime(syncDateTimeTextSnippet), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("TextSnippet", changes));

                changes = GetSpecialDiscountTable(0, new DateTime(syncDateTimeSpecialDiscount), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("SpecialDiscount", changes));

                changes = GetUserAgentTable(0, new DateTime(syncDateTimeUserAgent), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("UserAgents", changes));

                changes = GetProductImageTable(0, new DateTime(syncDateTimeProductImage), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("ProductImage", changes));

                //changes = GetSeasonValueTable(syncDateTimeSeasonValue);
                //if (changes > 0) results.Add(new SyncProgressItem("SeasonValue", changes));

                changes = GetCustomerNoteTable(0, new DateTime(syncDateTimeCustomerNote), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("CustomerNote", changes));

                changes = GetCustomerFavoriteTable(0, new DateTime(syncDateTimeCustomerFavorite), UserID).Select(a => a.CustomerFavoriteID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("CustomerFavorite", changes));

                changes = GetShoppingCartTable(0, new DateTime(syncDateTimeShoppingCart), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("ShoppingCart", changes));

                changes = GetShoppingCartItemTable(0, new DateTime(syncDateTimeShoppingCartItem), UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("ShoppingCartItem", changes));

                return results;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }

        #endregion

        string processDbresult<T>(Func<long, DateTime, string, IQueryable<T>> dbresult, DateTime SyncDateTime, string UserID)
        {
            IQueryable<T> items = dbresult(0, SyncDateTime, UserID);
            int dbresultCount = items.Count();
            long taken = 0;
            List<T> part = new List<T>();
            List<string> files = new List<string>();
            string fileName = string.Empty;
            int itemscount = 5000;
            while (taken < dbresultCount)
            {
                fileName = Guid.NewGuid().ToString() + ".data";

                items = dbresult(taken, SyncDateTime, UserID).Take(itemscount);

                part.AddRange(items);
                blobhandler.UploadText(Convert.ToBase64String(ZipHelper.ZipStringV2(getJson(part))), fileName);
                files.Add(fileName);

                dynamic lastÍtem = part.Last();
                taken = lastÍtem.SyncDateTimeSort;
                if (taken == 0)
                    taken = part.Count;
                part.Clear();
            }

            return Convert.ToBase64String(ZipHelper.ZipStringV2(getJson(files)));
        }

        string getJson(object Data)
        {
            string jsonClient = null;

            return Newtonsoft.Json.JsonConvert.SerializeObject(Data);

            return jsonClient;
        }

        #region Upload

        public bool UploadCustomerFavorite(string content, string UserID)
        {
            try
            {
                var user = from u in this.currentData.User
                           where u.UserID == UserID
                           select u;

                if (!user.Any())
                    return false;

                var contentArray = Convert.FromBase64String(content);

                var jsonData = ZipHelper.UnzipStream(new MemoryStream(contentArray));

                var uploadCustomerFavorite = JsonConvert.DeserializeObject<CustomerFavorite>(jsonData);

                var existingCF = currentData.CustomerFavorite.FirstOrDefault(dbcf => dbcf.CustomerID == uploadCustomerFavorite.CustomerID && dbcf.UserID == UserID);

                if (existingCF != null)
                {
                    existingCF.IsFavorite = uploadCustomerFavorite.IsFavorite;
                    existingCF.SyncDateTime = DateTime.UtcNow;
                }
                else
                {
                    uploadCustomerFavorite.SyncDateTime = DateTime.UtcNow;
                    currentData.CustomerFavorite.Add(uploadCustomerFavorite);
                }

                currentData.SaveChanges();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool UploadCustomerNote(string content, string UserID)
        {
            try
            {
                var user = from u in this.currentData.User
                           where u.UserID == UserID
                           select u;

                if (!user.Any())
                    return false;

                var contentArray = Convert.FromBase64String(content);

                var jsonData = ZipHelper.UnzipStream(new MemoryStream(contentArray));

                var uploadCustomerFavorite = JsonConvert.DeserializeObject<CustomerNote>(jsonData);

                var existingCF = currentData.CustomerNote.FirstOrDefault(dbcn => dbcn.CustomerNoteID == uploadCustomerFavorite.CustomerNoteID);

                if (existingCF != null)
                {
                    uploadCustomerFavorite.SyncDateTime = DateTime.UtcNow;
                    ClassFiller.PasteData(uploadCustomerFavorite, existingCF);
                }
                else
                {
                    uploadCustomerFavorite.SyncDateTime = DateTime.UtcNow;
                    currentData.CustomerNote.Add(uploadCustomerFavorite);
                }

                currentData.SaveChanges();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool UploadShoppingCart(string content, string UserID)
        {
            try
            {
                var user = from u in this.currentData.User
                           where u.UserID == UserID
                           select u;

                if (!user.Any())
                    return false;

                var contentArray = Convert.FromBase64String(content);

                var jsonData = ZipHelper.UnzipStream(new MemoryStream(contentArray));

                var uploadShoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(jsonData);

                var existingCF = currentData.ShoppingCart.FirstOrDefault(dbcn => dbcn.ShoppingCartID == uploadShoppingCart.ShoppingCartID);

                if (existingCF != null)
                {
                    if (existingCF.StatusID >= 10)
                    {
                        existingCF.SyncDateTime = DateTime.UtcNow;
                        ClassFiller.PasteData(uploadShoppingCart, existingCF);
                    }
                }
                else
                {
                    uploadShoppingCart.SyncDateTime = DateTime.UtcNow;
                    currentData.ShoppingCart.Add(uploadShoppingCart);
                }

                currentData.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }


        public bool UploadClosedShoppingCart(string content, string UserID)
        {
            try
            {
                var user = from u in this.currentData.User
                           where u.UserID == UserID
                           select u;

                if (!user.Any())
                    return false;

                var contentArray = Convert.FromBase64String(content);

                var jsonData = ZipHelper.UnzipStream(new MemoryStream(contentArray));

                var uploadShoppingCart = JsonConvert.DeserializeObject<UploadItemClosedOrder>(jsonData);

                var existingCF = currentData.ShoppingCart.FirstOrDefault(dbcn => dbcn.ShoppingCartID == uploadShoppingCart.ShoppingCart.ShoppingCartID);

                uploadShoppingCart.ShoppingCart.SyncDateTime = DateTime.UtcNow;
                uploadShoppingCart.ShoppingCart.Sent = true;
                uploadShoppingCart.ShoppingCart.StatusID = 0;

                if (existingCF != null)
                {
                    if (existingCF.StatusID < 10)
                        return true;


                    ClassFiller.PasteData(uploadShoppingCart.ShoppingCart, existingCF);
                }
                else
                {
                    currentData.ShoppingCart.Add(uploadShoppingCart.ShoppingCart);
                }

                Thread.Sleep(10);

                foreach (var sci in uploadShoppingCart.ShoppingCartItems)
                {
                    var existingSCItem = currentData.ShoppingCartItem.FirstOrDefault(dbcn => dbcn.ShoppingCartItemID == sci.ShoppingCartItemID);

                    if (existingSCItem != null)
                    {
                        sci.SyncDateTime = DateTime.UtcNow;
                        ClassFiller.PasteData(sci, existingSCItem);
                    }
                    else
                    {
                        sci.SyncDateTime = DateTime.UtcNow;
                        ShoppingCartItem newItem = new ShoppingCartItem();
                        ClassFiller.PasteData(sci, newItem);
                        currentData.ShoppingCartItem.Add(newItem);
                    }
                }

                currentData.SaveChanges();

                //Check alle Daten auf dem aktuellen Stand.
                ShoppingCart existingCart = currentData.ShoppingCart.FirstOrDefault(dbcn => dbcn.ShoppingCartID == uploadShoppingCart.ShoppingCart.ShoppingCartID);

                List<ShoppingCartItem> existingItems = currentData.ShoppingCartItem.Where(i => i.ShoppingCartID == uploadShoppingCart.ShoppingCart.ShoppingCartID && i.IsDeleted == false && i.SyncDateTime < existingCart.SyncDateTime).ToList();

                if (existingItems.Any())
                {
                    existingCart.StatusID = 10;
                    currentData.SaveChanges();
                    SendMail("sascha.weber@myconveno.de", "STEIN: \n\nShoppingCartID:" + existingCart.ShoppingCartID + "\n\nFehler Zeitüberschneidung Syncdatetime.", "BAUERFEIND CloseOrder Zeitüberschneidung", string.Empty);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }


        public bool UploadShoppingCartItem(string content, string UserID)
        {
            try
            {
                var user = from u in this.currentData.User
                           where u.UserID == UserID
                           select u;

                if (!user.Any())
                    return false;

                var contentArray = Convert.FromBase64String(content);

                var jsonData = ZipHelper.UnzipStream(new MemoryStream(contentArray));

                var uploadShoppingCartItem = JsonConvert.DeserializeObject<ShoppingCartItem>(jsonData);

                var existingSCItem = currentData.ShoppingCartItem.FirstOrDefault(dbcn => dbcn.ShoppingCartItemID == uploadShoppingCartItem.ShoppingCartItemID);
                var shoppingCart = currentData.ShoppingCart.FirstOrDefault(sc => sc.ShoppingCartID == uploadShoppingCartItem.ShoppingCartID);

                if (shoppingCart == null)
                    return false;

                if (existingSCItem != null)
                {
                    if (shoppingCart.StatusID >= 10)
                    {
                        existingSCItem.SyncDateTime = DateTime.UtcNow;
                        ClassFiller.PasteData(uploadShoppingCartItem, existingSCItem);
                    }
                }
                else
                {
                    if (shoppingCart.StatusID >= 10)
                    {
                        uploadShoppingCartItem.SyncDateTime = DateTime.UtcNow;
                        ShoppingCartItem newItem = new ShoppingCartItem();
                        ClassFiller.PasteData(uploadShoppingCartItem, newItem);
                        currentData.ShoppingCartItem.Add(newItem);
                    }
                }

                currentData.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }
        #endregion

        #region Export

        public byte[] GetIdocFromOrder(string ShoppingCartID)
        {
            MemoryStream idocstream = new MemoryStream();
            try
            {

                var order = (from o in this.currentData.ShoppingCart
                             where o.ShoppingCartID == ShoppingCartID
                             select o).FirstOrDefault();

                if (order != null)
                {
                    var items = (from pos in this.currentData.ShoppingCartItem
                                 where pos.ShoppingCartID == ShoppingCartID && pos.IsDeleted == false
                                 orderby pos.ShoppingCartItemSort
                                 select pos).ToList();

                    //if (order.ShoppingCartClientID == "1") //BERKEMANN
                    //{
                    //    return BerkemannExportIdoc.GetIdoc(order, items);
                    //}

                    //if (order.ShoppingCartClientID == "2") //SOLIDUS
                    //{
                    //    return SolidusExportIdoc.GetIdoc(order, items);
                    //}
                }

                return null;
            }
            catch (Exception ex)
            {
                //model.SaveChanges();
                //SendMail("sascha.weber@myconveno.de", ex.Message, "Fehler beim Erstellen des Idocs(" + AuftragsID.ToString() + ")", null);
                return null;
            }
        }

        public void CheckOrders4Export()
        {
            try
            {
                //DateTime ordersSyncDateTime = DateTime.UtcNow.AddMinutes(-10);

                //var orders = from order in this.currentData.ShoppingCart
                //             where order.IsDeleted == false && order.StatusID == 0
                //             && order.CustomerID != "-1" && order.Sent == true && order.SyncDateTime < ordersSyncDateTime
                //             && (order.ShoppingCartClientID == "1")
                //             select order;

                //foreach (var order in orders)
                //{
                //    var user = this.currentData.User.FirstOrDefault(u => u.UserID == order.UserID);
                //    if (user != null && user.UserNumber.CompareTo("610") >= 0)
                //    {
                //        byte[] idoc = GetIdocFromOrder(order.ShoppingCartID);

                //        if (idoc != null)
                //        {
                //            string destination = string.Empty;

                //            destination = order.ShoppingCartClientID == "1" ? "EXPORT/BERKEMANN/" : "EXPORT/SOLIDUS/";

                //            blobhandler.UploadFile(new MemoryStream(idoc), destination + order.OrderNumber + ".xml");

                //            order.StatusID = -4;
                //            order.SentDateTime = DateTime.UtcNow;
                //        }
                //    }
                //    else
                //    {
                //        order.StatusID = -4;
                //        order.SentDateTime = DateTime.UtcNow;
                //    }
                //}

                //this.currentData.SaveChanges();
            }
            catch { }
        }


        public void SendIdocMail(string ShoppingCartID, string Email)
        {
            MailMessage email = new MailMessage();

            var order = (from o in this.currentData.ShoppingCart
                         where o.ShoppingCartID == ShoppingCartID
                         select o).FirstOrDefault();

            if (order == null)
                return;

            foreach (string Adresse in Email.Split(';'))
            {
                email.To.Add(Adresse);
            }
            email.Subject = "IDOC";

            var xlsBinary = GetIdocFromOrder(ShoppingCartID).ToArray();

            email.Attachments.Add(new Attachment(new MemoryStream(xlsBinary), order.OrderNumber.ToString() + ".XML"));

            SendMail(email);

            order.StatusID = -4;
            order.SentDateTime = DateTime.UtcNow;

            currentData.SaveChanges();
        }

        #endregion

        #region Report
        public void CheckAEBs()
        {

            //try
            //{
            //    currentData.Database.ExecuteSqlCommand("UPDATE       ShoppingCart SET                InvoiceNumber = InvoiceAddress.InvoiceNumber, InvoiceAddressStreet = InvoiceAddress.InvoiceAddressStreet FROM            ShoppingCart INNER JOIN InvoiceAddress ON ShoppingCart.InvoiceAddressID = InvoiceAddress.InvoiceAddressID WHERE(ShoppingCart.InvoiceNumber IS NULL)");

            //    DateTime ab = new DateTime(2016, 7, 26);

            //    var aebs = from a in currentData.ShoppingCart
            //               where (a.ConfirmationStatus != -3)
            //               && a.StatusID < 10 && a.OrderDate > ab && a.IsDeleted == false
            //               select a;

            //    foreach (var auftrag in aebs)
            //    {

            //        var benutzer = (from b in currentData.User
            //                        where b.UserID == auftrag.UserID
            //                        select b).First();

            //        if (benutzer.UserNumber.CompareTo("610") >= 0)
            //        {
            //            var items = (from i in currentData.ShoppingCartItem
            //                         where i.ShoppingCartID == auftrag.ShoppingCartID
            //                         && i.IsDeleted == false
            //                         select i).ToList();

            //            var agent = (from i in currentData.Agent
            //                         where i.AgentID == auftrag.CustomerAgentID
            //                         select i).FirstOrDefault();

            //            List<string> empf = new List<string>();
            //            if (agent != null && !string.IsNullOrEmpty(agent.AgentConfirmEmail))
            //            {
            //                empf.AddRange(agent.AgentConfirmEmail.Split(';').ToList());
            //            }

            //            empf.AddRange(benutzer.ConfirmEmail.Split(';').ToList());

            //            var pdfData =  Confirm.Generate(auftrag, items, false, benutzer);

            //            if (pdfData == null)
            //            {
            //                SendMail("sascha.weber@myconveno.de", "ShopingCartID: " + auftrag.ShoppingCartID, "BAUERFEIND, AEB konnte nicht generiert werden.", null);
            //                continue;
            //            }

            //            if (!sendInnenDienstMail(new MemoryStream(pdfData.ToArray()), empf, auftrag.CustomerNumber, auftrag.OrderNumber, auftrag.CustomerName1 + " " + auftrag.CustomerName2 + " " + auftrag.CustomerName3 + " " + auftrag.CustomerCity, getInternText(auftrag), auftrag.ShoppingCartClientID))
            //            {
            //                SendMail("sascha.weber@myconveno.de", "ShopingCartID: " + auftrag.ShoppingCartID, "BAUERFEIND, Inndendienst AEB konnte nicht versendet werden.", null);
            //            }

            //            if (auftrag.ConfirmationEmail != string.Empty)
            //            {
            //                if (!sendKundeMail(new MemoryStream(pdfData.ToArray()), auftrag.ConfirmationEmail, auftrag.CustomerNumber, auftrag.OrderNumber, auftrag.CustomerName1 + " " + auftrag.CustomerName2 + " " + auftrag.CustomerName3 + " " + auftrag.CustomerCity, false, auftrag.ShoppingCartClientID))
            //                {
            //                    SendMail("sascha.weber@myconveno.de", "ShopingCartID: " + auftrag.ShoppingCartID, "BAUERFEIND, Kunden AEB konnte nicht versendet werden.", null);
            //                }
            //            }
            //        }
            //        auftrag.ConfirmationStatus = -3;
            //    }

            //    currentData.SaveChanges();

            //}
            //catch (Exception ex)
            //{
            //    currentData.SaveChanges();
            //    SendMail("sascha.weber@myconveno.de", ex.Message, "BAUERFEIND. Fehler beim Überprüfen der AEBS", null);
            //}
        }

        public void SendAEBMail(string ShoppingCartIDs, string Email)
        {

            //foreach (string ShoppingCartID in ShoppingCartIDs.Split(';'))
            //{

            //    var auftrag = (from a in currentData.ShoppingCart
            //                   where (a.ShoppingCartID == ShoppingCartID)
            //                   select a).FirstOrDefault();
            //    if (auftrag == null)
            //        return;

            //    var items = (from i in currentData.ShoppingCartItem
            //                 where i.ShoppingCartID == auftrag.ShoppingCartID
            //                 && i.IsDeleted == false
            //                 select i).ToList();

            //    var benutzer = (from b in currentData.User
            //                    where b.UserID == auftrag.UserID
            //                    select b).First();

            //    var agent = (from i in currentData.Agent
            //                 where i.AgentID == auftrag.CustomerAgentID
            //                 select i).First();              

            //    List<string> empf = new List<string>();


            //    if (string.IsNullOrEmpty(Email))
            //    {
            //        if (!string.IsNullOrEmpty(agent.AgentConfirmEmail))
            //        {
            //            empf.AddRange(agent.AgentConfirmEmail.Split(';').ToList());
            //        }
            //        else
            //        {
            //            empf.AddRange(benutzer.ConfirmEmail.Split(';').ToList());
            //        }
            //    }
            //    else
            //    {
            //        empf.AddRange(Email.Split(';').ToList());
            //    }

            //    if (!sendInnenDienstMail(Confirm.Generate(auftrag, items, false, benutzer), empf, auftrag.CustomerNumber, auftrag.OrderNumber, auftrag.CustomerName1 + " " + auftrag.CustomerName2 + " " + auftrag.CustomerName3 + " " + auftrag.CustomerCity, getInternText(auftrag), auftrag.ShoppingCartClientID))
            //    {
            //        SendMail("sascha.weber@myconveno.de", "", "BAUERFEIND, AEB konnte nicht versendet werden.", null);
            //    }

            //    auftrag.ConfirmationStatus = -3;

            //    currentData.SaveChanges();
            //}
        }

        #region Versand Emails, Fax Methoden
        private void SendMail(string EmpfaengerList, string Body, string Subject, string Datei)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                foreach (string empf in EmpfaengerList.Split(';'))
                {
                    if (IsValidEmail(empf))
                        mail.To.Add(new MailAddress(empf));
                }

                mail.Subject = Subject;

                mail.Body = Body;

                if (Datei != null && Datei != string.Empty)
                {
                    mail.Attachments.Add(new Attachment(Datei));
                }

                mail.IsBodyHtml = false;

                mail.From = new System.Net.Mail.MailAddress("service@myconveno.de", "BAUERFEIND Data Service");

                SmtpClient client = new SmtpClient("smtp.office365.com");
                client.Credentials = new NetworkCredential("service@myconveno.de", "Fisch6633");
                client.EnableSsl = true;
                client.Send(mail);
            }
            catch
            {

            }
        }

        private bool SendMail(MailMessage mail)
        {
            try
            {
                mail.From = new System.Net.Mail.MailAddress("service@myconveno.de", "BAUERFEIND Data Service");

                SmtpClient client = new SmtpClient("smtp.office365.com");
                client.Credentials = new NetworkCredential("service@myconveno.de", "Fisch6633");
                client.EnableSsl = true;
                client.Send(mail);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool SendFax(string Faxnummer, MemoryStream PDF)
        {
            try
            {
                string faxnr = string.Empty;

                Faxnummer = Faxnummer.Replace("+", "00");

                foreach (Char x in Faxnummer)
                {
                    switch (x)
                    {
                        case '+':
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            faxnr += x;
                            break;
                    }
                }

                //Session faxsession = new Session("MyCONVENO", "Fisch6633");
                //bool ok = faxsession.trySetClientIdentifier("MyCONVENOReportingService", "1.0.0.0", "MyCONVENO");
                //if (ok)
                //{
                //    string base64 = Convert.ToBase64String(PDF.ToArray());
                //    SessionInitiateResponse rp = faxsession.SendFax(faxnr, base64);

                //    if (rp.StatusCode == 200)
                //    {
                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool sendKundeMail(MemoryStream pdfstream, string EmpfaegerAdress, string Kundennummer, string Auftragsnummer, string KundeName, bool AGB, string ClientID)
        {
            try
            {
                foreach (var address in EmpfaegerAdress.Split(';'))
                {
                    if (address.Contains("@"))
                    {

                        MailMessage email = new MailMessage();
                        //email.Bcc.Add("sascha.weber@myconveno.de");
                        email.To.Add(address);
                        email.Subject = "AEB-" + Auftragsnummer + " KDNr.: " + Kundennummer + " " + KundeName;
                        // email.AlternateViews.Add(new AlternateView(new MemoryStream(Encoding.ASCII.GetBytes(McResources.Confirm)), "text/html"));
                        string textview = string.Empty;
                        textview += "EINGANGSBESTÄTIGUNG\n";
                        textview += "-------------------\n";
                        textview += "Als Anlage finden Sie die Eingangsbestaetigung Ihres Auftrags als ADOBE Reader PDF-Datei.\n\n";
                        //textview += "CONFIRMATION OF RECEIPT\n";
                        //textview += "-----------------------\n";
                        //textview += "Enclosed, please find your order receipt as PDF-file.\n\n";
                        //textview += "Orderbevestiging";
                        //textview += "-----------------------\n";
                        //textview += "In de bijlage vindt u de Solidus orderbevestiging van uw bestelling als Adobe Reader PDF bestand.";

                        string absender = "MyCONVENO";

                        //if (ClientID == "1")
                        //{
                        //    textview += ServiceResource.SignaturBerkemann;
                        //    absender = "BERKEMANN";
                        //}

                        //if (ClientID == "2")
                        //{
                        //    textview += ServiceResource.SignaturSolidus;
                        //    absender = "SOLIDUS";
                        //}


                        email.AlternateViews.Add(new AlternateView(new MemoryStream(Encoding.ASCII.GetBytes(textview)), "text/plain"));

                        email.Attachments.Add(new Attachment(new MemoryStream(pdfstream.ToArray()), Auftragsnummer + "_" + Kundennummer + ".pdf"));

                        if (AGB)
                        {
                            // email.Attachments.Add(new Attachment(new MemoryStream(McResources.MoreMoreAGB), "AGB.pdf"));
                            //email.Attachments.Add(new Attachment(new MemoryStream(McResources.agb_eng), "AGB_eng.pdf"));
                            //email.Attachments.Add(new Attachment(new MemoryStream(McResources.agb_it), "AGB_it.pdf"));
                        }

                        //email.Attachments.Add(new Attachment(new MemoryStream(McResources.TS_Neukundenformular), "TS_Neukundenformular.pdf"));
                        //email.Attachments.Add(new Attachment(new MemoryStream(McResources.TS_Sortimente), "TS_Sortimente.pdf"));
                        email.IsBodyHtml = false;
                        email.From = new System.Net.Mail.MailAddress("service@myconveno.de", absender + " Data Service");

                        SmtpClient client = new SmtpClient("smtp.office365.com");
                        client.Credentials = new NetworkCredential("service@myconveno.de", "Fisch6633");
                        client.EnableSsl = true;
                        client.Send(email);
                    }
                    else
                    {

                        if (!SendFax(EmpfaegerAdress, pdfstream))
                        {
                            SendMail("sascha.weber@myconveno.de", "Das Fax konnte nicht an " + address + " versendet werden.", "BAUERFEIND, FaxFehler", null);
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        private bool sendInnenDienstMail(MemoryStream Kundenpdfstream, List<string> EmpfaegerAdresses, string Kundennummer, string Auftragsnummer, string KundeName, string InternText, string ClientID)
        {
            try
            {
                MailMessage email = new MailMessage();
                foreach (string em in EmpfaegerAdresses)
                {
                    if (em != string.Empty)

                        email.To.Add(em);
                }

                //email.Bcc.Add("sascha.weber@myconveno.de");

                email.Subject = "Intern: AEB-" + Auftragsnummer + " KDNr.: " + Kundennummer + " " + KundeName;
                email.Body = InternText;

                if (InternText != string.Empty)
                {
                    email.Priority = MailPriority.High;
                }

                string absender = "MyCONVENO";

                //if (ClientID == "1")
                //{
                //    email.Body += ServiceResource.SignaturBerkemann;
                //    absender = "BERKEMANN";
                //}

                //if (ClientID == "2")
                //{
                //    email.Body += ServiceResource.SignaturSolidus;
                //    absender = "SOLIDUS";
                //}

                email.Attachments.Add(new Attachment(Kundenpdfstream, Auftragsnummer + "_" + Kundennummer + "_" + RemoveSonderzeichen(KundeName) + ".pdf"));

                email.IsBodyHtml = false;
                email.From = new System.Net.Mail.MailAddress("service@myconveno.de", absender + " Data Service");

                SmtpClient client = new SmtpClient("smtp.office365.com");
                client.Credentials = new NetworkCredential("service@myconveno.de", "Fisch6633");
                client.EnableSsl = true;
                client.Send(email);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string RemoveSonderzeichen(string input)
        {
            return Regex.Replace(input, @"[^a-zA-Z0-9]", string.Empty);
        }

        #region valid emailadress
        bool invalid = false;

        private bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format. 
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
        #endregion

        #endregion

        string getInternText(ShoppingCart auftrag)
        {
            string MyReturn = string.Empty;

            if (auftrag.CustomerID == "-2")
            {
                MyReturn = "\n===============================================================";
                MyReturn += "\n INTERNE EINGANGSBESTÄTIGUNG MIT ÄNDERUNG";
                MyReturn += "\n---------------------------------------------------------------";
                MyReturn += "\n Kundennummer : " + auftrag.CustomerNumber.ToString();
                MyReturn += "\n Name         : " + auftrag.CustomerName1.ToString();
                MyReturn += "\n              : " + auftrag.CustomerName2.ToString();
                MyReturn += "\n              : " + auftrag.CustomerName3.ToString();
                MyReturn += "\n Straße       : " + auftrag.CustomerStreet.ToString();
                MyReturn += "\n PLZ          : " + auftrag.CustomerZIP.ToString();
                MyReturn += "\n Ort          : " + auftrag.CustomerCity.ToString();
                MyReturn += "\n Ländercode   : " + auftrag.CustomerCountryCode.ToString();
                MyReturn += "\n Land         : " + auftrag.CustomerCountryName.ToString();
                MyReturn += "\n Telefon      : " + auftrag.CustomerPhone.ToString();
                MyReturn += "\n Fax          : " + auftrag.CustomerFax.ToString();
                MyReturn += "\n Email        : " + auftrag.CustomerEMail.ToString();
                MyReturn += "\n===============================================================\n";
            }
            if (auftrag.CustomerID == "-1")
            {
                MyReturn = "\n===============================================================";
                MyReturn += "\n NEUKUNDEN Auftrag";
                MyReturn += "\n---------------------------------------------------------------";
                MyReturn += "\n Name         : " + auftrag.CustomerName1.ToString();
                MyReturn += "\n              : " + auftrag.CustomerName2.ToString();
                MyReturn += "\n              : " + auftrag.CustomerName3.ToString();
                MyReturn += "\n Straße       : " + auftrag.CustomerStreet.ToString();
                MyReturn += "\n PLZ          : " + auftrag.CustomerZIP.ToString();
                MyReturn += "\n Ort          : " + auftrag.CustomerCity.ToString();
                MyReturn += "\n Ländercode   : " + auftrag.CustomerCountryCode.ToString();
                MyReturn += "\n Land         : " + auftrag.CustomerCountryName.ToString();
                MyReturn += "\n Telefon      : " + auftrag.CustomerPhone.ToString();
                MyReturn += "\n Fax          : " + auftrag.CustomerFax.ToString();
                MyReturn += "\n Email        : " + auftrag.CustomerEMail.ToString();
                MyReturn += "\n===============================================================\n";
            }
            if (auftrag.Remark2 != string.Empty)
            {
                MyReturn += "\n===============================================================";
                MyReturn += "\n Vertreter hat diesem Auftrag einen internen Bemerkungstext mitgegeben:";
                MyReturn += "\n " + auftrag.Remark2.ToString();
                MyReturn += "\n===============================================================\n";
            }
            if (auftrag.Remark1 != string.Empty)
            {
                MyReturn += "\n===============================================================";
                MyReturn += "\n Vertreter hat diesem Auftrag einen Bemerkungstext mitgegeben:";
                MyReturn += "\n " + auftrag.Remark1.ToString();
                MyReturn += "\n===============================================================\n";
            }

            return (MyReturn);
        }
        #endregion

        #region Received Methods
        public void MarkDataAsTransfered(List<string> DataPackages, string DataPackageType)
        {
            foreach (string file in DataPackages)
            {
                blobhandler.DeleteFile(file);
            }
            //ToDo
            //Bei einem Benutzer den aktuellen SyncStatus der tabelle setzen.
        }
        #endregion

        public void UpdateGeoData()
        {
            LIVINGKITZBUEHLEntities serverdata = getData();
            try
            {
                int counter = 0;
                foreach (var c in serverdata.Customer.Where(cust => cust.lat == 0).ToList())
                {
                    counter++;
                    GeoLocation geo = GetGoogleGeoResult(string.Empty, c.CustomerCity, c.CustomerZIP, c.CustomerStreet);

                    if (geo == null)
                    {

                        serverdata.SaveChanges();
                        return;
                    }


                    c.lat = geo.Lat;
                    c.lon = geo.Lon;
                    c.SyncDateTime = DateTime.UtcNow;

                    if (counter % 100 == 0)
                    {
                        serverdata.SaveChanges();
                    }
                }
                serverdata.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private GeoLocation GetGoogleGeoResult(string Country, string City, string PostCode, string Street)
        {
            GeoLocation loc = new GeoLocation();
            string AppID = "AIzaSyDTQtswXl6eMIbKMllNYk1sGWVdNOv1qjs";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/json?address=" + Country + "," + PostCode + "+" + City + "," + Street + "&key=" + AppID);
                string PointFromBingServiceURL = request.Address.AbsoluteUri;
                request.Proxy = WebRequest.GetSystemWebProxy();
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                        return loc;

                    using (StreamReader textread = new StreamReader(response.GetResponseStream()))
                    {
                        GoogleRootObject res = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleRootObject>(textread.ReadToEnd());

                        if (res.status == "OVER_QUERY_LIMIT")
                            return null;

                        var geo = res.results.First().geometry;

                        loc.Lat = geo.location.lat;
                        loc.Lon = geo.location.lng;

                    }

                    return loc;
                }
            }
            catch (Exception ex)
            {
                return new GeoLocation();
            }
        }

        class GeoLocation
        {
            public double Lon { get; set; }
            public double Lat { get; set; }
            public GeoLocation()
            {
                Lon = 0;
                Lat = 0;
            }
        }

        class AddressComponent
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public List<string> types { get; set; }
        }

        class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        class Northeast
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        class Southwest
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        class Viewport
        {
            public Northeast northeast { get; set; }
            public Southwest southwest { get; set; }
        }

        class Geometry
        {
            public Location location { get; set; }
            public string location_type { get; set; }
            public Viewport viewport { get; set; }
        }

        class Result
        {
            public List<AddressComponent> address_components { get; set; }
            public string formatted_address { get; set; }
            public Geometry geometry { get; set; }
            public string place_id { get; set; }
            public List<string> types { get; set; }
        }

        class GoogleRootObject
        {
            public List<Result> results { get; set; }
            public string status { get; set; }
        }
    }
}
