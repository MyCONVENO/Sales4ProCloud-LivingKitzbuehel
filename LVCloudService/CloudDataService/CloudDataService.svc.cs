using CloudDataService.Export;
using CloudDataService.Helper;
using CloudDataService.Report;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CloudDataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CloudDataService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CloudDataService.svc or CloudDataService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CloudDataService : ICloudDataService
    {
        BlobstorageFileHandler blobhandler = new BlobstorageFileHandler("myconvenoftp", "ZZiN0Tl+eejzQc9ymh/vXBTziGa5n68/OVrLmGpA6FIN+Xm61yDVeadquGdSesRIoRtBXmUG586b9RjCERs5hg==", "livingkitzbuehel");
        LIVINGKITZBUEHLEntities currentData = createDataSource();

        static LIVINGKITZBUEHLEntities createDataSource()
        {
            var datasource = new LIVINGKITZBUEHLEntities();
            datasource.Configuration.LazyLoadingEnabled = false;
            ((IObjectContextAdapter)datasource).ObjectContext.CommandTimeout = 12000;
            return datasource;
        }

        #region Agent

        //const string agentQueryString = "SELECT Agent.* FROM Agent INNER JOIN UserAgents ON Agent.AgentID = UserAgents.AgentID WHERE UserAgents.UserID = @UserID AND Agent.SyncDateTime > @SyncDateTime";
        //const string agentCountQueryString = "SELECT Agent.AgentID FROM Agent INNER JOIN UserAgents ON Agent.AgentID = UserAgents.AgentID WHERE UserAgents.UserID = @UserID AND Agent.SyncDateTime > @SyncDateTime";

        //const string agentQueryString = "SELECT Agent.* FROM Agent INNER JOIN UserAgents ON Agent.AgentID = UserAgents.AgentID WHERE UserAgents.UserID = @UserID AND Agent.SyncDateTime > @SyncDateTime";
        //const string agentCountQueryString = "SELECT Agent.AgentID FROM Agent INNER JOIN UserAgents ON Agent.AgentID = UserAgents.AgentID WHERE UserAgents.UserID = @UserID AND Agent.SyncDateTime > @SyncDateTime";

        IQueryable<Agent> GetAgentTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            return from a in this.currentData.Agent
                   where a.SyncDateTime > SyncDateTime
                   select a;
        }

        int GetAgentTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            // return currentData.Database.SqlQuery<string>(agentCountQueryString, new object[] { new SqlParameter("@UserID", UserID), new SqlParameter("@SyncDateTime", SyncDateTime) }).Count();

            return (from a in this.currentData.Agent
                    where a.SyncDateTime > SyncDateTime
                    select a.AgentID).Count();
        }

        [WebGet]
        public string SyncAgent(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetAgentTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region ContactPerson
        IEnumerable<ContactPerson> GetContactPersonTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = from a in this.currentData.ContactPerson
                             //where a.SyncDateTime > SyncDateTime && a.Customer.Agent.UserAgents.Any(ua => ua.UserID == UserID)
                         where a.SyncDateTime > SyncDateTime
                         select a;

            if (IsTestData.HasValue && IsTestData.Value)
                result = result.Where(r => r.IsTestData == true);

            return result;
        }

        int GetContactPersonTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = (from a in this.currentData.ContactPerson
                              //where a.SyncDateTime > SyncDateTime && a.Customer.Agent.UserAgents.Any(ua => ua.UserID == UserID)
                          where a.SyncDateTime > SyncDateTime
                          select a);

            if (IsTestData.HasValue && IsTestData.Value)
                result = result.Where(r => r.IsTestData == true);

            return result.Select(r => r.ContactPersonID).Count();
        }

        [WebGet]
        public string SyncContactPerson(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetContactPersonTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Client
        IEnumerable<UserClient> GetClientTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            return from a in this.currentData.UserClient
                   where a.UserID == UserID
                   && a.SyncDateTime > SyncDateTime
                   select a;
        }

        int GetClientTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            return (from a in this.currentData.UserClient
                    where a.UserID == UserID
                    && a.SyncDateTime > SyncDateTime
                    select a.UserMandantID).Count();
        }

        [WebGet]
        public string SyncClient(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetClientTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Stock
        IEnumerable<Stock> GetStockTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = (from a in this.currentData.Stock
                          where a.SyncDateTime > SyncDateTime
                          select a).ToList();

            result = result.Where(r => r.SyncDateTime.Ticks > SyncDateTime.Ticks).ToList();

            return result;

        }

        int GetStockTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = GetStockTable(SyncDateTime, UserID, IsTestData).ToList();

            return result.Count();
        }

        [WebGet]
        public string SyncStock(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetStockTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region CustomerFavorite
        IEnumerable<CustomerFavorite> GetCustomerFavoriteTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            return from a in this.currentData.CustomerFavorite
                   where a.SyncDateTime > SyncDateTime && a.UserID == UserID
                   select a;
        }

        int GetCustomerFavoriteTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            return (from a in this.currentData.CustomerFavorite
                    where a.SyncDateTime > SyncDateTime && a.UserID == UserID
                    select a.CustomerFavoriteID).Count();
        }

        [WebGet]
        public string SyncCustomerFavorite(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetCustomerFavoriteTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region CustomerNote
        IEnumerable<CustomerNote> GetCustomerNoteTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            return from a in this.currentData.CustomerNote
                   join userclients in this.currentData.UserClient on a.CustomerNoteClientID equals userclients.UserClientClientID
                   where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                   select a;
        }

        int GetCustomerNoteTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            return (from a in this.currentData.CustomerNote
                    join userclients in this.currentData.UserClient on a.CustomerNoteClientID equals userclients.UserClientClientID
                    where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                    select a.CustomerNoteID).Count();
        }

        [WebGet]
        public string SyncCustomerNote(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetCustomerNoteTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Article
        IEnumerable<Article> GetArticleTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {

            var result = from a in this.currentData.Article
                         join userclients in this.currentData.UserClient on a.ArticleClientID equals userclients.UserClientClientID
                         where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                         select a;

            //if (IsTestData.HasValue && IsTestData.Value)
            //    result = result.Where(r => r.IsTestData == true);
            //else
            //    result = result.Where(r => r.IsTestData == null);

            return result;
        }

        int GetArticleTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {

            var result = (from a in this.currentData.Article
                          join userclients in this.currentData.UserClient on a.ArticleClientID equals userclients.UserClientClientID
                          where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                          select a);

            //if (IsTestData.HasValue && IsTestData.Value)
            //    result = result.Where(r => r.IsTestData == true);
            //else
            //    result = result.Where(r => r.IsTestData == null);

            return result.Select(r => r.ArticleID).Count();
        }

        [WebGet]
        public string SyncArticle(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetArticleTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion    

        #region Association
        IEnumerable<Association> GetAssociationTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = from a in this.currentData.Association
                         join userclients in this.currentData.UserClient on a.AssociationClientID equals userclients.UserClientClientID
                         where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                         select a;

            if (IsTestData.HasValue && IsTestData.Value)
                result = result.Where(r => r.IsTestData == true);

            return result;
        }

        int GetAssociationTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = (from a in this.currentData.Association
                          join userclients in this.currentData.UserClient on a.AssociationClientID equals userclients.UserClientClientID
                          where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                          select a);

            if (IsTestData.HasValue && IsTestData.Value)
                result = result.Where(r => r.IsTestData == true);

            return result.Select(r => r.AssociationID).Count();
        }

        [WebGet]
        public string SyncAssociation(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetAssociationTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region AssociationMember
        IEnumerable<AssociationMember> GetAssociationMemberTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = from a in this.currentData.AssociationMember
                         where a.SyncDateTime > SyncDateTime
                         select a;


            return result;
        }

        int GetAssociationMemberTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = (from a in this.currentData.AssociationMember
                          where a.SyncDateTime > SyncDateTime
                          select a);


            return result.Select(r => r.AssociationMemberID).Count();
        }

        [WebGet]
        public string SyncAssociationMember(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetAssociationMemberTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Assortment
        IEnumerable<Assortment> GetAssortmentTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = (from a in this.currentData.Assortment
                          where a.SyncDateTime > SyncDateTime
                          select a).ToList();

            return result.Where(a => a.SyncDateTime.Ticks > SyncDateTime.Ticks);
        }

        int GetAssortmentTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = (from a in this.currentData.Assortment
                          where a.SyncDateTime > SyncDateTime
                          select a).ToList();

            return result.Where(a => a.SyncDateTime.Ticks > SyncDateTime.Ticks).Select(r => r.AssortmentID).Count();
        }

        [WebGet]
        public string SyncAssortment(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetAssortmentTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Color
        IEnumerable<Color> GetColorTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {


            var result = from a in this.currentData.Color
                         join userclients in this.currentData.UserClient on a.ColorClientID equals userclients.UserClientClientID
                         where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                         select a;

            //if (IsTestData.HasValue && IsTestData.Value)
            //    result = result.Where(r => r.IsTestData == true);
            //else
            //    result = result.Where(r => r.IsTestData == null);

            return result;
        }

        int GetColorTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {


            var result = (from a in this.currentData.Color
                          join userclients in this.currentData.UserClient on a.ColorClientID equals userclients.UserClientClientID
                          where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                          select a);

            //if (IsTestData.HasValue && IsTestData.Value)
            //    result = result.Where(r => r.IsTestData == true);
            //else
            //    result = result.Where(r => r.IsTestData == null);

            return result.Select(r => r.ColorID).Count();
        }

        [WebGet]
        public string SyncColor(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetColorTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Customer
        IEnumerable<Customer> GetCustomerTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {


            var result = from a in this.currentData.Customer
                         join userclients in this.currentData.UserClient on a.CustomerClientID equals userclients.UserClientClientID
                         //join useragents in this.currentData.UserAgents on a.CustomerAgentID equals useragents.AgentID
                         //where a.SyncDateTime > SyncDateTime && useragents.UserID == UserID
                         where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                         select a;

            if (IsTestData.HasValue && IsTestData.Value)
                result = result.Where(r => r.IsTestData == true);

            return result;
        }

        int GetCustomerTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {


            var result = (from a in this.currentData.Customer
                          join userclients in this.currentData.UserClient on a.CustomerClientID equals userclients.UserClientClientID
                          //join useragents in this.currentData.UserAgents on a.CustomerAgentID equals useragents.AgentID
                          //where a.SyncDateTime > SyncDateTime && useragents.UserID == UserID
                          where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                          select a);

            if (IsTestData.HasValue && IsTestData.Value)
                result = result.Where(r => r.IsTestData == true);

            return result.Select(r => r.CustomerID).Count();
        }

        [WebGet]
        public string SyncCustomer(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetCustomerTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region DeliveryAddress
        IEnumerable<DeliveryAddress> GetDeliveryAddressTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {


            var result = from a in this.currentData.DeliveryAddress
                             //join useragents in this.currentData.UserAgents on a.Customer.CustomerAgentID equals useragents.AgentID
                             //where a.SyncDateTime > SyncDateTime && useragents.UserID == UserID
                         where a.SyncDateTime > SyncDateTime
                         select a;

            if (IsTestData.HasValue && IsTestData.Value)
                result = result.Where(r => r.IsTestData == true);

            return result;
        }

        int GetDeliveryAddressTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {


            var result = (from a in this.currentData.DeliveryAddress
                              //join useragents in this.currentData.UserAgents on a.Customer.CustomerAgentID equals useragents.AgentID
                              //where a.SyncDateTime > SyncDateTime && useragents.UserID == UserID
                          where a.SyncDateTime > SyncDateTime
                          select a);

            if (IsTestData.HasValue && IsTestData.Value)
                result = result.Where(r => r.IsTestData == true);

            return result.Select(r => r.DeliveryAddressID).Count();
        }

        [WebGet]
        public string SyncDeliveryAddress(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetDeliveryAddressTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region InvoiceAddress
        IEnumerable<InvoiceAddress> GetInvoiceAddressTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = from a in this.currentData.InvoiceAddress
                             //join useragents in this.currentData.UserAgents on a.Customer.CustomerAgentID equals useragents.AgentID
                             //where a.SyncDateTime > SyncDateTime && useragents.UserID == UserID
                         where a.SyncDateTime > SyncDateTime
                         select a;

            if (IsTestData.HasValue && IsTestData.Value)
                result = result.Where(r => r.IsTestData == true);

            return result;
        }

        int GetInvoiceAddressTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = (from a in this.currentData.InvoiceAddress
                              //join useragents in this.currentData.UserAgents on a.Customer.CustomerAgentID equals useragents.AgentID
                              //where a.SyncDateTime > SyncDateTime && useragents.UserID == UserID
                          where a.SyncDateTime > SyncDateTime
                          select a);

            if (IsTestData.HasValue && IsTestData.Value)
                result = result.Where(r => r.IsTestData == true);

            return result.Select(r => r.InvoiceAddressID).Count();
        }

        [WebGet]
        public string SyncInvoiceAddress(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetInvoiceAddressTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Label
        IEnumerable<Label> GetLabelTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {


            return from a in this.currentData.Label
                   join userclients in this.currentData.UserClient on a.LabelClientID equals userclients.UserClientClientID
                   where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                   select a;
        }

        int GetLabelTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            return (from a in this.currentData.Label
                    join userclients in this.currentData.UserClient on a.LabelClientID equals userclients.UserClientClientID
                    where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                    select a.LabelID).Count();
        }

        [WebGet]
        public string SyncLabel(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetLabelTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Model
        IEnumerable<Model> GetModelTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = (from a in this.currentData.Model
                          where a.SyncDateTime > SyncDateTime
                          select a).ToList();


            return result.Where(a => a.SyncDateTime.Ticks > SyncDateTime.Ticks);
        }

        int GetModelTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {


            var result = (from a in this.currentData.Model
                          where a.SyncDateTime > SyncDateTime
                          select a).ToList();

            return result.Where(a => a.SyncDateTime.Ticks > SyncDateTime.Ticks).Select(r => r.ModelID).Count();
        }

        [WebGet]
        public string SyncModel(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetModelTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Price
        IEnumerable<Price> GetPriceTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = (from a in this.currentData.Price
                          where a.SyncDateTime > SyncDateTime
                          select a).ToList();

            return result.Where(a => a.SyncDateTime.Ticks > SyncDateTime.Ticks);
        }

        int GetPriceTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = (from a in this.currentData.Price
                          where a.SyncDateTime > SyncDateTime
                          select a).ToList();

            return result.Where(a => a.SyncDateTime.Ticks > SyncDateTime.Ticks).Select(r => r.PriceID).Count();
        }

        [WebGet]
        public string SyncPrice(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetPriceTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Pricelist
        IEnumerable<Pricelist> GetPricelistTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {


            return from a in this.currentData.Pricelist
                   join userclients in this.currentData.UserClient on a.PricelistClientID equals userclients.UserClientClientID
                   //join userpricelists in this.currentData.UserPriceList on a.PricelistID equals userpricelists.PricelistID
                   //where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID && userpricelists.UserID == UserID && userpricelists.UserPricelistClientID == userclients.UserClientClientID
                   where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                   select a;
        }

        int GetPricelistTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {


            return (from a in this.currentData.Pricelist
                    join userclients in this.currentData.UserClient on a.PricelistClientID equals userclients.UserClientClientID
                    //join userpricelists in this.currentData.UserPriceList on a.PricelistID equals userpricelists.PricelistID
                    //where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID && userpricelists.UserID == UserID && userpricelists.UserPricelistClientID == userclients.UserClientClientID
                    where a.SyncDateTime > SyncDateTime && userclients.UserClientClientID == a.PricelistClientID
                    select a.PricelistID).Count();
        }

        [WebGet]
        public string SyncPricelist(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetPricelistTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Season
        IEnumerable<Season> GetSeasonTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {


            return from a in this.currentData.Season
                   join userclients in this.currentData.UserClient on a.SeasonClientID equals userclients.UserClientClientID
                   where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                   select a;
        }

        int GetSeasonTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {


            return (from a in this.currentData.Season
                    join userclients in this.currentData.UserClient on a.SeasonClientID equals userclients.UserClientClientID
                    where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                    select a.SeasonID).Count();
        }


        [WebGet]
        public string SyncSeason(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetSeasonTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region Sizerun
        IEnumerable<Sizerun> GetSizerunTable(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = from a in this.currentData.Sizerun
                         join userclients in this.currentData.UserClient on a.SizerunClientID equals userclients.UserClientClientID
                         where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                         select a;

            if (IsTestData.HasValue && IsTestData.Value)
                result = result.Where(r => r.IsTestData == true);

            return result;
        }

        int GetSizerunTableChangedItemsCount(DateTime SyncDateTime, string UserID, bool? IsTestData)
        {
            var result = (from a in this.currentData.Sizerun
                          join userclients in this.currentData.UserClient on a.SizerunClientID equals userclients.UserClientClientID
                          where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                          select a);

            if (IsTestData.HasValue && IsTestData.Value)
                result = result.Where(r => r.IsTestData == true);

            return result.Select(r => r.SizerunID).Count();
        }

        [WebGet]
        public string SyncSizerun(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetSizerunTable(syncdatetime, UserID, user.First().UseTestData).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region ProductImage

        int GetProductImageChanges(DateTime SyncDateTime)
        {
            return (from a in this.currentData.ProductImage
                    where a.SyncDateTime > SyncDateTime
                    select a.ProductImageID).Count();
        }
        IEnumerable<ProductImage> GetProductImageTable(DateTime SyncDateTime, int take, int skip)
        {
            return (from a in this.currentData.ProductImage
                    where a.SyncDateTime > SyncDateTime
                    orderby a.SyncDateTime
                    select a).Skip(skip).Take(take);
        }

        [WebGet]
        public string SyncProductImage(string SyncDateTime, string UserID, int take, int skip)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetProductImageTable(syncdatetime, take, skip).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region User
        IOrderedQueryable<User> GetUserTable(DateTime SyncDateTime, string UserID)
        {


            return from a in this.currentData.User
                   where a.SyncDateTime > SyncDateTime && a.UserID == UserID
                   orderby a.Name1
                   select a;
        }

        int GetUserTableChangedItemsCount(DateTime SyncDateTime, string UserID)
        {
            return (from a in this.currentData.User
                    where a.SyncDateTime > SyncDateTime && a.UserID == UserID
                    orderby a.Name1
                    select a.UserID).Count();
        }

        [WebGet]
        public string SyncUser(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetUserTable(syncdatetime, UserID).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region ShoppingCart
        int GethoppingCartTableChangedItemsCount(DateTime SyncDateTime, string UserID)
        {
            var result = (from a in this.currentData.ShoppingCart
                          where a.SyncDateTime > SyncDateTime && a.UserID == UserID
                          select a).ToList();

            return result.Where(a => a.SyncDateTime.Ticks > SyncDateTime.Ticks).Count();
        }

        IEnumerable<ShoppingCart> GetShoppingCartTable(DateTime SyncDateTime, string UserID)
        {
            var result = (from a in this.currentData.ShoppingCart
                          where a.SyncDateTime > SyncDateTime && a.UserID == UserID
                          select a).ToList();

            return result.Where(a => a.SyncDateTime.Ticks > SyncDateTime.Ticks);
        }

        [WebGet]
        public string SyncShoppingCart(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetShoppingCartTable(syncdatetime, UserID).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region ShoppingCartItem
        IEnumerable<ShoppingCartItem> GetShoppingCartItemTable(DateTime SyncDateTime, string UserID)
        {
            return from a in this.currentData.ShoppingCartItem
                   where a.SyncDateTime > SyncDateTime && a.UserID == UserID
                   select a;
        }

        [WebGet]
        public string SyncShoppingCartItem(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetShoppingCartItemTable(syncdatetime, UserID).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
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

        [WebGet]
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

        [WebGet]
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
        IEnumerable<TextSnippet> GetTextSnippetTable(DateTime SyncDateTime)
        {
            return from a in this.currentData.TextSnippet
                   where a.SyncDateTime > SyncDateTime
                   select a;
        }

        int GetTextSnippetTableChangedItemsCount(DateTime SyncDateTime)
        {
            return (from a in this.currentData.TextSnippet
                    where a.SyncDateTime > SyncDateTime
                    select a.TextSnippetID).Count();
        }

        [WebGet]
        public string SyncTextSnippet(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetTextSnippetTable(syncdatetime).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region SpecialDiscount
        IEnumerable<SpecialDiscount> GetSpecialDiscountTable(DateTime SyncDateTime, string UserID)
        {
            return from a in this.currentData.SpecialDiscount
                   join userclients in this.currentData.UserClient on a.SpecialDiscountClientID equals userclients.UserClientClientID
                   where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                   select a;
        }

        int GetSpecialDiscountTableChangedItemsCount(DateTime SyncDateTime, string UserID)
        {
            return (from a in this.currentData.SpecialDiscount
                    join userclients in this.currentData.UserClient on a.SpecialDiscountClientID equals userclients.UserClientClientID
                    where a.SyncDateTime > SyncDateTime && userclients.UserID == UserID
                    select a.SpecialDiscountID).Count();
        }

        [WebGet]
        public string SyncSpecialDiscount(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetSpecialDiscountTable(syncdatetime, UserID).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region UserAgent
        IEnumerable<UserAgents> GetUserAgentTable(DateTime SyncDateTime, string UserID)
        {
            return from a in this.currentData.UserAgents
                   where a.SyncDateTime > SyncDateTime && a.UserID == UserID
                   select a;
        }

        int GetUserAgentTableChangedItemsCount(DateTime SyncDateTime, string UserID)
        {
            return (from a in this.currentData.UserAgents
                    where a.SyncDateTime > SyncDateTime && a.UserID == UserID
                    select a.UserAgentsID).Count();
        }

        [WebGet]
        public string SyncUserAgent(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetUserAgentTable(syncdatetime, UserID).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        #endregion

        #region SeasonValue
        IEnumerable<SeasonValue> GetSeasonValueTable(DateTime SyncDateTime)
        {
            var result = from a in this.currentData.SeasonValue
                         where a.SyncDateTime > SyncDateTime
                         select a;


            //if (IsTestData.HasValue && IsTestData.Value)
            //    result = result.Where(r => r.IsTestData == true);
            //else
            //    result = result.Where(r => r.IsTestData == null);

            return result;
        }

        int GetSeasonValueTableChangedItemsCount(DateTime SyncDateTime)
        {


            var result = (from a in this.currentData.SeasonValue
                          where a.SyncDateTime > SyncDateTime
                          select a);

            if (SyncDateTime.Year == 2000)
                result = result.Where(r => r.IsDeleted == false);

            return result.Select(r => r.SeasonValueID).Count();
        }

        [WebGet]
        public string SyncSeasonValue(string SyncDateTime, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            DateTime syncdatetime = new DateTime(Convert.ToInt64(SyncDateTime));
            try
            {
                var dbresult = GetSeasonValueTable(syncdatetime).ToList();

                return Convert.ToBase64String(ZipHelper.ZipString(getJson(dbresult)));
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

            var zip = ZipHelper.ZipString(json);

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

                DateTime syncDateTimeAgent = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeAgent") ? SyncDateTimes["SyncDateTimeAgent"] : 630822816000000000);
                DateTime syncDateTimeArticle = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeArticle") ? SyncDateTimes["SyncDateTimeArticle"] : 630822816000000000);
                DateTime syncDateTimeArticleSizerun = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeArticleSizerun") ? SyncDateTimes["SyncDateTimeArticleSizerun"] : 630822816000000000);
                DateTime syncDateTimeAssociation = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeAssociation") ? SyncDateTimes["SyncDateTimeAssociation"] : 630822816000000000);
                DateTime syncDateTimeAssociationMember = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeAssociationMember") ? SyncDateTimes["SyncDateTimeAssociationMember"] : 630822816000000000);
                DateTime syncDateTimeAssortment = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeAssortment") ? SyncDateTimes["SyncDateTimeAssortment"] : 630822816000000000);
                DateTime syncDateTimeColor = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeColor") ? SyncDateTimes["SyncDateTimeColor"] : 630822816000000000);
                DateTime syncDateTimeCustomer = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeCustomer") ? SyncDateTimes["SyncDateTimeCustomer"] : 630822816000000000);
                DateTime syncDateTimeDeliveryAddress = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeDeliveryAddress") ? SyncDateTimes["SyncDateTimeDeliveryAddress"] : 630822816000000000);
                DateTime syncDateTimeInvoiceAddress = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeInvoiceAddress") ? SyncDateTimes["SyncDateTimeInvoiceAddress"] : 630822816000000000);
                DateTime syncDateTimeLabel = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeLabel") ? SyncDateTimes["SyncDateTimeLabel"] : 630822816000000000);
                DateTime syncDateTimeModel = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeModel") ? SyncDateTimes["SyncDateTimeModel"] : 630822816000000000);
                DateTime syncDateTimePrice = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimePrice") ? SyncDateTimes["SyncDateTimePrice"] : 630822816000000000);
                DateTime syncDateTimePricelist = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimePricelist") ? SyncDateTimes["SyncDateTimePricelist"] : 630822816000000000);
                DateTime syncDateTimeSeason = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeSeason") ? SyncDateTimes["SyncDateTimeSeason"] : 630822816000000000);
                DateTime syncDateTimeSizerun = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeSizerun") ? SyncDateTimes["SyncDateTimeSizerun"] : 630822816000000000);
                DateTime syncDateTimeUser = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeUser") ? SyncDateTimes["SyncDateTimeUser"] : 630822816000000000);
                DateTime syncDateTimeContactPerson = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeContactPerson") ? SyncDateTimes["SyncDateTimeContactPerson"] : 630822816000000000);
                DateTime syncDateTimeClient = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeClient") ? SyncDateTimes["SyncDateTimeClient"] : 630822816000000000);
                DateTime syncDateTimeStock = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeStock") ? SyncDateTimes["SyncDateTimeStock"] : 630822816000000000);
                DateTime syncDateTimeTextSnippet = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeTextSnippet") ? SyncDateTimes["SyncDateTimeTextSnippet"] : 630822816000000000);
                DateTime syncDateTimeSpecialDiscount = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeSpecialDiscount") ? SyncDateTimes["SyncDateTimeSpecialDiscount"] : 630822816000000000);
                DateTime syncDateTimeUserAgent = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeUserAgent") ? SyncDateTimes["SyncDateTimeUserAgent"] : 630822816000000000);
                DateTime syncDateTimeProductImage = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeProductImage") ? SyncDateTimes["SyncDateTimeProductImage"] : 630822816000000000);
                DateTime syncDateTimeShoppingCart = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeShoppingCart") ? SyncDateTimes["SyncDateTimeShoppingCart"] : 630822816000000000);
                DateTime syncDateTimeShoppingCartItem = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeShoppingCartItem") ? SyncDateTimes["SyncDateTimeShoppingCartItem"] : 630822816000000000);
                DateTime syncDateTimeSeasonValue = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeSeasonValue") ? SyncDateTimes["SyncDateTimeSeasonValue"] : 630822816000000000);
                DateTime syncDateTimeCustomerNote = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeCustomerNote") ? SyncDateTimes["SyncDateTimeCustomerNote"] : 630822816000000000);
                DateTime syncDateTimeCustomerFavorite = new DateTime(SyncDateTimes.ContainsKey("SyncDateTimeCustomerFavorite") ? SyncDateTimes["SyncDateTimeCustomerFavorite"] : 630822816000000000);


                List<SyncProgressItem> results = new List<SyncProgressItem>();

                int changes = 0;

                changes = GetAgentTableChangedItemsCount(syncDateTimeAgent, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Agent", changes));

                changes = GetArticleTableChangedItemsCount(syncDateTimeArticle, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Article", changes));

                //changes = GetArticleSizerunTable(syncDateTimeArticleSizerun).Count();
                //if (changes > 0) results.Add(new SyncProgressItem("ArticleSizerun", changes));

                changes = GetAssociationTableChangedItemsCount(syncDateTimeAssociation, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Association", changes));

                changes = GetAssociationMemberTableChangedItemsCount(syncDateTimeAssociationMember, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("AssociationMember", changes));

                changes = GetAssortmentTableChangedItemsCount(syncDateTimeAssortment, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Assortment", changes));

                changes = GetColorTableChangedItemsCount(syncDateTimeColor, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Color", changes));

                changes = GetCustomerTableChangedItemsCount(syncDateTimeCustomer, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Customer", changes));

                changes = GetDeliveryAddressTableChangedItemsCount(syncDateTimeDeliveryAddress, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("DeliveryAddress", changes));

                changes = GetInvoiceAddressTableChangedItemsCount(syncDateTimeInvoiceAddress, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("InvoiceAddress", changes));

                changes = GetLabelTableChangedItemsCount(syncDateTimeLabel, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Label", changes));

                changes = GetClientTableChangedItemsCount(syncDateTimeClient, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Client", changes));

                changes = GetModelTableChangedItemsCount(syncDateTimeModel, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Model", changes));

                changes = GetPriceTableChangedItemsCount(syncDateTimePrice, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Price", changes));

                changes = GetPricelistTableChangedItemsCount(syncDateTimePricelist, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Pricelist", changes));

                changes = GetSeasonTableChangedItemsCount(syncDateTimeSeason, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Season", changes));

                changes = GetSizerunTableChangedItemsCount(syncDateTimeSizerun, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Sizerun", changes));

                changes = GetContactPersonTableChangedItemsCount(syncDateTimeContactPerson, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("ContactPerson", changes));

                changes = GetUserTableChangedItemsCount(syncDateTimeUser, UserID);
                if (changes > 0) results.Add(new SyncProgressItem("User", changes));

                changes = GetStockTableChangedItemsCount(syncDateTimeStock, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Stock", changes));

                changes = GetTextSnippetTableChangedItemsCount(syncDateTimeTextSnippet);
                if (changes > 0) results.Add(new SyncProgressItem("TextSnippet", changes));

                changes = GetSpecialDiscountTableChangedItemsCount(syncDateTimeSpecialDiscount, UserID);
                if (changes > 0) results.Add(new SyncProgressItem("SpecialDiscount", changes));

                changes = GetUserAgentTableChangedItemsCount(syncDateTimeUserAgent, UserID);
                if (changes > 0) results.Add(new SyncProgressItem("UserAgents", changes));

                changes = GetProductImageChanges(syncDateTimeProductImage);
                if (changes > 0) results.Add(new SyncProgressItem("ProductImage", changes));

                changes = GetSeasonValueTableChangedItemsCount(syncDateTimeSeasonValue);
                if (changes > 0) results.Add(new SyncProgressItem("SeasonValue", changes));

                changes = GetCustomerNoteTableChangedItemsCount(syncDateTimeCustomerNote, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("CustomerNote", changes));

                changes = GetCustomerFavoriteTableChangedItemsCount(syncDateTimeCustomerFavorite, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("CustomerFavorite", changes));

                changes = GethoppingCartTableChangedItemsCount(syncDateTimeShoppingCart, UserID);
                if (changes > 0) results.Add(new SyncProgressItem("ShoppingCart", changes));

                changes = GetShoppingCartItemTable(syncDateTimeShoppingCartItem, UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("ShoppingCartItem", changes));

                return results;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }

        #endregion

        List<SyncProgressItem> GetAvailableChangesBenutzerdatenXML(string SyncDateTimeCustomerNote, string SyncDateTimeCustomerFavorite, string UserID)
        {
            try
            {

                var user = from u in this.currentData.User
                           where u.UserID == UserID
                           select u;

                if (!user.Any())
                    return new List<SyncProgressItem>();

                DateTime syncDateTimeCustomerNote = new DateTime(Convert.ToInt64(SyncDateTimeCustomerNote));
                DateTime syncDateTimeCustomerFavorite = new DateTime(Convert.ToInt64(SyncDateTimeCustomerFavorite));



                List<SyncProgressItem> results = new List<SyncProgressItem>();

                int changes = 0;

                changes = GetCustomerNoteTable(syncDateTimeCustomerNote, UserID, user.First().UseTestData).Count();
                if (changes > 0) results.Add(new SyncProgressItem("CustomerNote", changes));

                changes = GetCustomerFavoriteTable(syncDateTimeCustomerFavorite, UserID, user.First().UseTestData).Count();
                if (changes > 0) results.Add(new SyncProgressItem("CustomerFavorite", changes));


                return results;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }

        [WebGet]
        public string GetAvailableChangesBenutzerdaten(string SyncDateTimeCustomerNote, string SyncDateTimeCustomerFavorite, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            var json = getJson(GetAvailableChangesBenutzerdatenXML(SyncDateTimeCustomerNote, SyncDateTimeCustomerFavorite, UserID));

            var zip = ZipHelper.ZipString(json);

            return Convert.ToBase64String(zip);
        }

        List<SyncProgressItem> GetAvailableChangesShoppingCartDataXML(string SyncDateTimeShoppingCart, string SyncDateTimeShoppingCartItem, string UserID)
        {
            try
            {

                DateTime syncDateTimeShoppingCart = new DateTime(Convert.ToInt64(SyncDateTimeShoppingCart));
                DateTime syncDateTimeShoppingCartItem = new DateTime(Convert.ToInt64(SyncDateTimeShoppingCartItem));


                List<SyncProgressItem> results = new List<SyncProgressItem>();

                int changes = 0;

                changes = GetShoppingCartTable(syncDateTimeShoppingCart, UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("ShoppingCart", changes));

                changes = GetShoppingCartItemTable(syncDateTimeShoppingCartItem, UserID).Count();
                if (changes > 0) results.Add(new SyncProgressItem("ShoppingCartItem", changes));


                return results;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }

        [WebGet]
        public string GetAvailableChangesShoppingCartData(string SyncDateTimeShoppingCart, string SyncDateTimeShoppingCartItem, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            var json = getJson(GetAvailableChangesShoppingCartDataXML(SyncDateTimeShoppingCart, SyncDateTimeShoppingCartItem, UserID));

            var zip = ZipHelper.ZipString(json);

            return Convert.ToBase64String(zip);
        }


        List<SyncProgressItem> GetAvailableChangesStammdatenXML(string SyncDateTimeAgent, string SyncDateTimeArticle, string SyncDateTimeArticleSizerun, string SyncDateTimeAssociation, string SyncDateTimeAssociationMember, string SyncDateTimeAssortment, string SyncDateTimeColor, string SyncDateTimeCustomer, string SyncDateTimeDeliveryAddress, string SyncDateTimeInvoiceAddress, string SyncDateTimeLabel, string SyncDateTimeModel, string SyncDateTimePrice, string SyncDateTimePricelist, string SyncDateTimeSeason, string SyncDateTimeSizerun, string SyncDateTimeUser, string SyncDateTimeContactPerson, string SyncDateTimeClient, string SyncDateTimeStock, string SyncDateTimeTextSnippet, string SyncDateTimeSpecialDiscount, string SyncDateTimeUserAgent, string UserID)
        {
            try
            {
                var user = from u in this.currentData.User
                           where u.UserID == UserID
                           select u;

                if (!user.Any())
                    return new List<SyncProgressItem>();

                DateTime syncDateTimeAgent = new DateTime(Convert.ToInt64(SyncDateTimeAgent));
                DateTime syncDateTimeArticle = new DateTime(Convert.ToInt64(SyncDateTimeArticle));
                DateTime syncDateTimeArticleSizerun = new DateTime(Convert.ToInt64(SyncDateTimeArticleSizerun));
                DateTime syncDateTimeAssociation = new DateTime(Convert.ToInt64(SyncDateTimeAssociation));
                DateTime syncDateTimeAssociationMember = new DateTime(Convert.ToInt64(SyncDateTimeAssociationMember));
                DateTime syncDateTimeAssortment = new DateTime(Convert.ToInt64(SyncDateTimeAssortment));
                DateTime syncDateTimeColor = new DateTime(Convert.ToInt64(SyncDateTimeColor));
                DateTime syncDateTimeCustomer = new DateTime(Convert.ToInt64(SyncDateTimeCustomer));
                DateTime syncDateTimeDeliveryAddress = new DateTime(Convert.ToInt64(SyncDateTimeDeliveryAddress));
                DateTime syncDateTimeInvoiceAddress = new DateTime(Convert.ToInt64(SyncDateTimeInvoiceAddress));
                DateTime syncDateTimeLabel = new DateTime(Convert.ToInt64(SyncDateTimeLabel));
                DateTime syncDateTimeModel = new DateTime(Convert.ToInt64(SyncDateTimeModel));
                DateTime syncDateTimePrice = new DateTime(Convert.ToInt64(SyncDateTimePrice));
                DateTime syncDateTimePricelist = new DateTime(Convert.ToInt64(SyncDateTimePricelist));
                DateTime syncDateTimeSeason = new DateTime(Convert.ToInt64(SyncDateTimeSeason));
                DateTime syncDateTimeSizerun = new DateTime(Convert.ToInt64(SyncDateTimeSizerun));
                DateTime syncDateTimeUser = new DateTime(Convert.ToInt64(SyncDateTimeUser));
                DateTime syncDateTimeContactPerson = new DateTime(Convert.ToInt64(SyncDateTimeContactPerson));
                DateTime syncDateTimeClient = new DateTime(Convert.ToInt64(SyncDateTimeClient));
                DateTime syncDateTimeStock = new DateTime(Convert.ToInt64(SyncDateTimeStock));
                DateTime syncDateTimeTextSnippet = new DateTime(Convert.ToInt64(SyncDateTimeTextSnippet));
                DateTime syncDateTimeSpecialDiscount = new DateTime(Convert.ToInt64(SyncDateTimeSpecialDiscount));
                DateTime syncDateTimeUserAgent = new DateTime(Convert.ToInt64(SyncDateTimeUserAgent));

                List<SyncProgressItem> results = new List<SyncProgressItem>();

                int changes = 0;

                changes = GetAgentTableChangedItemsCount(syncDateTimeAgent, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Agent", changes));

                changes = GetArticleTableChangedItemsCount(syncDateTimeArticle, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Article", changes));

                //changes = GetArticleSizerunTable(syncDateTimeArticleSizerun).Count();
                //if (changes > 0) results.Add(new SyncProgressItem("ArticleSizerun", changes));

                changes = GetAssociationTableChangedItemsCount(syncDateTimeAssociation, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Association", changes));

                changes = GetAssociationMemberTableChangedItemsCount(syncDateTimeAssociationMember, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("AssociationMember", changes));

                changes = GetAssortmentTableChangedItemsCount(syncDateTimeAssortment, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Assortment", changes));

                changes = GetColorTableChangedItemsCount(syncDateTimeColor, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Color", changes));

                changes = GetCustomerTableChangedItemsCount(syncDateTimeCustomer, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Customer", changes));

                changes = GetDeliveryAddressTableChangedItemsCount(syncDateTimeDeliveryAddress, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("DeliveryAddress", changes));

                changes = GetInvoiceAddressTableChangedItemsCount(syncDateTimeInvoiceAddress, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("InvoiceAddress", changes));

                changes = GetLabelTableChangedItemsCount(syncDateTimeLabel, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Label", changes));

                changes = GetClientTableChangedItemsCount(syncDateTimeClient, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Client", changes));

                changes = GetModelTableChangedItemsCount(syncDateTimeModel, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Model", changes));

                changes = GetPriceTableChangedItemsCount(syncDateTimePrice, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Price", changes));

                changes = GetPricelistTableChangedItemsCount(syncDateTimePricelist, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Pricelist", changes));

                changes = GetSeasonTableChangedItemsCount(syncDateTimeSeason, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Season", changes));

                changes = GetSizerunTableChangedItemsCount(syncDateTimeSizerun, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Sizerun", changes));

                changes = GetContactPersonTableChangedItemsCount(syncDateTimeContactPerson, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("ContactPerson", changes));

                changes = GetUserTableChangedItemsCount(syncDateTimeUser, UserID);
                if (changes > 0) results.Add(new SyncProgressItem("User", changes));

                changes = GetStockTableChangedItemsCount(syncDateTimeStock, UserID, user.First().UseTestData);
                if (changes > 0) results.Add(new SyncProgressItem("Stock", changes));

                changes = GetTextSnippetTableChangedItemsCount(syncDateTimeTextSnippet);
                if (changes > 0) results.Add(new SyncProgressItem("TextSnippet", changes));

                changes = GetSpecialDiscountTableChangedItemsCount(syncDateTimeSpecialDiscount, UserID);
                if (changes > 0) results.Add(new SyncProgressItem("SpecialDiscount", changes));

                changes = GetUserAgentTableChangedItemsCount(syncDateTimeUserAgent, UserID);
                if (changes > 0) results.Add(new SyncProgressItem("UserAgents", changes));

                return results;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }

        [WebGet]
        public string GetAvailableChangesStammdaten(string SyncDateTimeAgent, string SyncDateTimeArticle, string SyncDateTimeArticleSizerun, string SyncDateTimeAssociation, string SyncDateTimeAssociationMember, string SyncDateTimeAssortment, string SyncDateTimeColor, string SyncDateTimeCustomer, string SyncDateTimeDeliveryAddress, string SyncDateTimeInvoiceAddress, string SyncDateTimeLabel, string SyncDateTimeModel, string SyncDateTimePrice, string SyncDateTimePricelist, string SyncDateTimeSeason, string SyncDateTimeSizerun, string SyncDateTimeUser, string SyncDateTimeContactPerson, string SyncDateTimeClient, string SyncDateTimeStock, string SyncDateTimeTextSnippet, string SyncDateTimeSpecialDiscount, string SyncDateTimeUserAgent, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            var json = getJson(GetAvailableChangesStammdatenXML(SyncDateTimeAgent, SyncDateTimeArticle, SyncDateTimeArticleSizerun, SyncDateTimeAssociation, SyncDateTimeAssociationMember, SyncDateTimeAssortment, SyncDateTimeColor, SyncDateTimeCustomer, SyncDateTimeDeliveryAddress, SyncDateTimeInvoiceAddress, SyncDateTimeLabel, SyncDateTimeModel, SyncDateTimePrice, SyncDateTimePricelist, SyncDateTimeSeason, SyncDateTimeSizerun, SyncDateTimeUser, SyncDateTimeContactPerson, SyncDateTimeClient, SyncDateTimeStock, SyncDateTimeTextSnippet, SyncDateTimeSpecialDiscount, SyncDateTimeUserAgent, UserID));

            var zip = ZipHelper.ZipString(json);

            return Convert.ToBase64String(zip);
        }

        List<SyncProgressItem> GetAvailableChangesBilderXML(string SyncDateTimeProductImage, string UserID)
        {
            try
            {
                DateTime syncDateTimeProductImage = new DateTime(Convert.ToInt64(SyncDateTimeProductImage));

                List<SyncProgressItem> results = new List<SyncProgressItem>();

                int changes = 0;

                changes = GetProductImageChanges(syncDateTimeProductImage);

                if (changes > 0) results.Add(new SyncProgressItem("ProductImage", changes));

                return results;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred: {0}", ex.Message));
            }
        }

        [WebGet]
        public string GetAvailableChangesBilder(string SyncDateTimeProductImage, string UserID)
        {
            var user = from u in this.currentData.User
                       where u.UserID == UserID
                       select u;

            if (!user.Any())
                return string.Empty;

            var json = getJson(GetAvailableChangesBilderXML(SyncDateTimeProductImage, UserID));

            var zip = ZipHelper.ZipString(json);

            return Convert.ToBase64String(zip);
        }

        string getJson(object Data)
        {
            string jsonClient = null;
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.NullValueHandling = NullValueHandling.Ignore;
            jsonSerializer.MissingMemberHandling = MissingMemberHandling.Error;
            jsonSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Error;

            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (JsonTextWriter jtw = new JsonTextWriter(sw))
                    {
                        jsonSerializer.Serialize(jtw, Data);
                    }
                    jsonClient = sw.ToString();
                }
            }
            catch (Exception ex)
            {
                ex = ex; // have a breakpoint here so can inspect exception
            }
            return jsonClient;
        }

        IEnumerable<T> sqlWrapper<T>(SqlDataReader reader)
        {
            List<T> returnlist = new List<T>();
            Type returnType = typeof(T);

            var props = returnType.GetProperties();

            while (reader.Read())
            {
                T newitem = (T)Activator.CreateInstance(returnType);

                foreach (var property in props)
                {

                    var destprop = newitem.GetType().GetProperty(property.Name);

                    if (destprop.CanWrite)
                    {
                        var sourceValue = reader[property.Name];

                        if (reader[property.Name] != System.DBNull.Value)
                            destprop.SetValue(newitem, reader[property.Name]);
                    }
                }

                returnlist.Add(newitem);
            }

            return returnlist;
        }

        #region Upload
        [WebGet]
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
                    existingCF.SyncDateTime = DateTime.Now;
                }
                else
                {
                    uploadCustomerFavorite.SyncDateTime = DateTime.Now;
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

        [WebGet]
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

        [WebGet]
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

                SendMail("sascha.weber@myconveno.de", "Message:" + ex.Message + "\n\nInner:" + ex.InnerException.ToString(), "LvKB-Upload Error ShoppingCart", null);
                ex.ToString();
                return false;
            }
        }

        [WebGet]
        public bool UploadClosedShoppingCart(string content, string UserID)
        {
            string jsonData = string.Empty;
            try
            {
                var user = from u in this.currentData.User
                           where u.UserID == UserID
                           select u;

                if (!user.Any())
                    return false;

                var contentArray = Convert.FromBase64String(content);

                jsonData = ZipHelper.UnzipStream(new MemoryStream(contentArray));

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

                foreach (var sci in uploadShoppingCart.ShoppingCartItems)
                {
                    var existingSCItem = currentData.ShoppingCartItem.FirstOrDefault(dbcn => dbcn.ShoppingCartItemID == sci.ShoppingCartItemID);

                    if (existingSCItem != null)
                    {
                        existingSCItem.SyncDateTime = DateTime.UtcNow;
                        ClassFiller.PasteData(sci, existingSCItem);
                    }
                    else
                    {
                        sci.SyncDateTime = DateTime.UtcNow;
                        currentData.ShoppingCartItem.Add(sci);
                    }
                }

                currentData.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                SendMail("sascha.weber@myconveno.de", "Exception" + ex.Message.ToString() + "\n\nJSON:" + jsonData, "LivingKB Error Upload Closed Order", "");
                ex.ToString();
                return false;
            }
        }

        [WebGet]
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

                if (existingSCItem != null)
                {
                    if (shoppingCart == null || shoppingCart.StatusID >= 10)
                    {
                        existingSCItem.SyncDateTime = DateTime.UtcNow;
                        ClassFiller.PasteData(uploadShoppingCartItem, existingSCItem);
                    }
                }
                else
                {
                    if (shoppingCart == null || shoppingCart.StatusID >= 10)
                    {
                        uploadShoppingCartItem.SyncDateTime = DateTime.UtcNow;
                        currentData.ShoppingCartItem.Add(uploadShoppingCartItem);
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

        public void CheckUploadOrder(string content)
        {

            try
            {
                var uploadShoppingCart = JsonConvert.DeserializeObject<UploadItemClosedOrder>(TestResource.UploadContent);

                var existingCF = currentData.ShoppingCart.FirstOrDefault(dbcn => dbcn.ShoppingCartID == uploadShoppingCart.ShoppingCart.ShoppingCartID);

                uploadShoppingCart.ShoppingCart.SyncDateTime = DateTime.UtcNow;
                uploadShoppingCart.ShoppingCart.Sent = true;
                uploadShoppingCart.ShoppingCart.StatusID = 0;

                if (existingCF != null)
                {
                    if (existingCF.StatusID < 10)
                        return;

                    ClassFiller.PasteData(uploadShoppingCart.ShoppingCart, existingCF);
                }
                else
                {
                    currentData.ShoppingCart.Add(uploadShoppingCart.ShoppingCart);
                }

                foreach (var sci in uploadShoppingCart.ShoppingCartItems)
                {
                    var existingSCItem = currentData.ShoppingCartItem.FirstOrDefault(dbcn => dbcn.ShoppingCartItemID == sci.ShoppingCartItemID);

                    if (existingSCItem != null)
                    {
                        existingSCItem.SyncDateTime = DateTime.UtcNow;
                        ClassFiller.PasteData(sci, existingSCItem);
                    }
                    else
                    {
                        sci.SyncDateTime = DateTime.UtcNow;
                        currentData.ShoppingCartItem.Add(sci);
                    }
                }

                currentData.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        #endregion

        #region Export
        [WebGet]
        public byte[] GetExportFromOrder(string ShoppingCartID)
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
                                 select pos).ToList().OrderBy(p=>p.ShoppingCartItemSort);

                    string seasonID = items.First().ModelSeasonID;

                    Season season = currentData.Season.First(s => s.SeasonID == seasonID);

                    var result = LVKBExportOrder.GetOrderStream(order, items, season.SeasonName);

                    if (result != null)
                    {
                        order.StatusID = -4;
                        order.Sent = true;
                        order.SentDateTime = DateTime.Now;
                        this.currentData.SaveChanges();
                    }

                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                //model.SaveChanges();
                SendMail("sascha.weber@myconveno.de", ex.Message, "LIVINGKB Fehler beim EXPORT eines Auftrags (" + ShoppingCartID + ")", null);
                return null;
            }
        }

        public void CheckOrders4Export()
        {
            try
            {
                var orders = (from order in this.currentData.ShoppingCart
                              where order.IsDeleted == false && order.StatusID == 0 && order.CustomerID != "-1" && order.CustomerNumber != "-1" && order.Sent == true
                              select order).ToList();

                foreach (var order in orders)
                {
                    var user = this.currentData.User.FirstOrDefault(u => u.UserID == order.UserID);
                    if (user != null && user.UserNumber.CompareTo("610") >= 0)
                    {
                        byte[] idoc = GetExportFromOrder(order.ShoppingCartID);

                        if (idoc != null)
                        {
                            string destination = string.Empty;

                            destination = "Auftraege/";

                            blobhandler.UploadFile(new MemoryStream(idoc), destination + order.OrderNumber + ".XML");
                        }
                    }
                }
            }
            catch { }
        }

        [WebGet]
        public void SendOrderExportMail(string ShoppingCartID, string Email)
        {
            MailMessage email = new MailMessage();

            foreach (var s in ShoppingCartID.Split(';'))
            {

                var order = (from o in this.currentData.ShoppingCart
                             where o.ShoppingCartID == s
                             select o).FirstOrDefault();

                if (order == null)
                    return;

                var xlsBinary = GetExportFromOrder(s).ToArray();

                email.Attachments.Add(new Attachment(new MemoryStream(xlsBinary), order.OrderNumber.ToString() + ".XML"));

            }

            foreach (string Adresse in Email.Split(';'))
            {
                email.To.Add(Adresse);
            }
            email.Subject = "Export Order";
            SendMail(email);
        }

        #endregion

        #region Report
        public void CheckAEBs()
        {

            try
            {

                DateTime ab = new DateTime(2016, 7, 26);

                var aebs = from a in currentData.ShoppingCart
                           where (a.ConfirmationStatus != -3)
                           && a.StatusID < 10 && a.OrderDate > ab && a.IsDeleted == false
                           select a;

                foreach (var auftrag in aebs)
                {

                    var benutzer = (from b in currentData.User
                                    where b.UserID == auftrag.UserID
                                    select b).First();

                    if (benutzer.UserNumber.CompareTo("610") >= 0)
                    {
                        var items = (from i in currentData.ShoppingCartItem
                                     where i.ShoppingCartID == auftrag.ShoppingCartID
                                     && i.IsDeleted == false
                                     select i).ToList();

                        var agent = (from i in currentData.Agent
                                     where i.AgentID == auftrag.CustomerAgentID
                                     select i).FirstOrDefault();

                        List<string> empf = new List<string>();
                        if (agent != null && !string.IsNullOrEmpty(agent.AgentConfirmEmail))
                        {
                            empf.AddRange(agent.AgentConfirmEmail.Split(';').ToList());
                        }
                        else
                        {
                            empf.AddRange(benutzer.ConfirmEmail.Split(';').ToList());
                        }

                        if (!sendInnenDienstMail(ConfirmPDF.Generate(auftrag, items, false, benutzer), empf, auftrag.CustomerNumber, auftrag.OrderNumber, auftrag.CustomerName1 + " " + auftrag.CustomerName2 + " " + auftrag.CustomerName3 + " " + auftrag.CustomerCity, getInternText(auftrag)))
                        {
                            SendMail("sascha.weber@myconveno.de", "", "Living Kitzbuehl, AEB konnte nicht versendet werden.", null);
                        }

                        if (auftrag.ConfirmationEmail != string.Empty)
                        {
                            if (!sendKundeMail(ConfirmPDF.Generate(auftrag, items, false, benutzer), auftrag.ConfirmationEmail, auftrag.CustomerNumber, auftrag.OrderNumber, auftrag.CustomerName1 + " " + auftrag.CustomerName2 + " " + auftrag.CustomerName3 + " " + auftrag.CustomerCity, false))
                            {
                                SendMail("sascha.weber@myconveno.de", "", "Living Kitzbuehl, AEB konnte nicht versendet werden.", null);
                            }
                        }
                    }
                    auftrag.ConfirmationStatus = -3;
                }

                currentData.SaveChanges();

            }
            catch (Exception ex)
            {
                currentData.SaveChanges();
                SendMail("sascha.weber@myconveno.de", ex.Message, "Living Kitzbuehl. Fehler beim Überprüfen der AEBS", null);
            }
        }

        public void SendAEBMail(string ShoppingCartIDs, string Email)
        {

            foreach (string ShoppingCartID in ShoppingCartIDs.Split(';'))
            {

                var auftrag = (from a in currentData.ShoppingCart
                               where (a.ShoppingCartID == ShoppingCartID)
                               select a).FirstOrDefault();
                if (auftrag == null)
                    return;

                var items = (from i in currentData.ShoppingCartItem
                             where i.ShoppingCartID == auftrag.ShoppingCartID
                             && i.IsDeleted == false
                             select i).ToList();

                var benutzer = (from b in currentData.User
                                where b.UserID == auftrag.UserID
                                select b).First();

                var agent = (from i in currentData.Agent
                             where i.AgentID == auftrag.CustomerAgentID
                             select i).FirstOrDefault();


                //if (!sendKundeMail(Confirm.Generate(auftrag, items, false, benutzer), Email, auftrag.CustomerNumber, auftrag.OrderNumber, auftrag.CustomerName1 + " " + auftrag.CustomerName2 + " " + auftrag.CustomerName3 + " " + auftrag.CustomerCity, false))
                //{
                //    SendMail("sascha.weber@myconveno.de", "", "Living Kitzbuehl, AEB konnte nicht versendet werden.", null);
                //}

                List<string> empf = new List<string>();


                if (string.IsNullOrEmpty(Email))
                {
                    if (!string.IsNullOrEmpty(agent.AgentConfirmEmail))
                    {
                        empf.AddRange(agent.AgentConfirmEmail.Split(';').ToList());
                    }
                    else
                    {
                        empf.AddRange(benutzer.ConfirmEmail.Split(';').ToList());
                    }
                }
                else
                {
                    empf.AddRange(Email.Split(';').ToList());
                }

                if (!sendInnenDienstMail(ConfirmPDF.Generate(auftrag, items, false, benutzer), empf, auftrag.CustomerNumber, auftrag.OrderNumber, auftrag.CustomerName1 + " " + auftrag.CustomerName2 + " " + auftrag.CustomerName3 + " " + auftrag.CustomerCity, getInternText(auftrag)))
                {
                    SendMail("sascha.weber@myconveno.de", "", "Living Kitzbuehl, AEB konnte nicht versendet werden.", null);
                }

                //auftrag.ConfirmationStatus = -3;

                currentData.SaveChanges();
            }
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

                mail.From = new System.Net.Mail.MailAddress("service@myconveno.de", "Living Kitzbuehl Data Service");

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
                mail.From = new System.Net.Mail.MailAddress("service@myconveno.de", "Living Kitzbuehl Data Service");

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

        private bool sendKundeMail(MemoryStream pdfstream, string EmpfaegerAdress, string Kundennummer, string Auftragsnummer, string KundeName, bool AGB)
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
                        textview += "CONFIRMATION OF RECEIPT\n";
                        textview += "-----------------------\n";
                        textview += "Enclosed, please find your order receipt as PDF-file.";
                        //textview += "Orderbevestiging";
                        //textview += "-----------------------\n";
                        //textview += "In de bijlage vindt u de Solidus orderbevestiging van uw bestelling als Adobe Reader PDF bestand.";

                        textview += "\n\n";

                        textview += Confirm.ConfirmResource.LivingKBSignatur;
                        email.Body = textview;

                        //email.AlternateViews.Add(new AlternateView(new MemoryStream(Encoding.ASCII.GetBytes(textview)), "text/plain"));

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
                        email.From = new System.Net.Mail.MailAddress("service@myconveno.de", "Living Kitzbühel Data Service");

                        SmtpClient client = new SmtpClient("smtp.office365.com");
                        client.Credentials = new NetworkCredential("service@myconveno.de", "Fisch6633");
                        client.EnableSsl = true;
                        client.Send(email);
                    }
                    else
                    {

                        if (!SendFax(EmpfaegerAdress, pdfstream))
                        {
                            SendMail("sascha.weber@myconveno.de", "Das Fax konnte nicht an " + address + " versendet werden.", "Living Kitzbuehl, FaxFehler", null);
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

        private bool sendInnenDienstMail(MemoryStream Kundenpdfstream, List<string> EmpfaegerAdresses, string Kundennummer, string Auftragsnummer, string KundeName, string InternText)
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

                email.Attachments.Add(new Attachment(new MemoryStream(Kundenpdfstream.ToArray()), Auftragsnummer + "_" + Kundennummer + "_" + RemoveSonderzeichen(KundeName) + ".pdf"));

                email.IsBodyHtml = false;
                email.From = new System.Net.Mail.MailAddress("service@myconveno.de", "Living Kitzbühel Data Service");

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

            if (auftrag.CustomerID.StartsWith("-"))
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

    }

    public class SyncProgressItem
    {
        public SyncProgressItem()
        {
        }

        public SyncProgressItem(string tableName, int totalChanges)
        {
            TableName = tableName;
            TotalChanges = totalChanges;
        }

        public string TableName { get; set; }

        public int TotalChanges { get; set; }
    }

}
