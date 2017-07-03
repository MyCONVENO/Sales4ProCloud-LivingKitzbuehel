using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CloudDataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICloudDataService" in both code and config file together.
    [ServiceContract]
    public interface ICloudDataService
    {

        [OperationContract]
        [WebGet]
        string SyncAgent(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncArticle(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncAssociation(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncAssociationMember(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncAssortment(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncColor(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncCustomer(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncCustomerNote(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncClient(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncContactPerson(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncDeliveryAddress(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncInvoiceAddress(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncLabel(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncModel(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncPrice(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncPricelist(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncSeason(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncSizerun(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncUser(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncStock(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncCustomerFavorite(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncShoppingCart(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncShoppingCartItem(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncProductImage(string SyncDateTime, string UserID, int take, int skip);

        [OperationContract]
        [WebGet]
        string SyncTextSnippet(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncSpecialDiscount(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncUserAgent(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string SyncSeasonValue(string SyncDateTime, string UserID);

        [OperationContract]
        [WebGet]
        string UpdatePushChannel(string UserID, string OldChannelUri, string NewChannelUri);

        [OperationContract]
        [WebGet]
        string SendPushNotification(string UserIDs);

        [OperationContract]
        [WebGet]
        string GetAvailableChangesStammdaten(string SyncDateTimeAgent, string SyncDateTimeArticle, string SyncDateTimeArticleSizerun, string SyncDateTimeAssociation, string SyncDateTimeAssociationMember, string SyncDateTimeAssortment, string SyncDateTimeColor, string SyncDateTimeCustomer, string SyncDateTimeDeliveryAddress, string SyncDateTimeInvoiceAddress, string SyncDateTimeLabel, string SyncDateTimeModel, string SyncDateTimePrice, string SyncDateTimePricelist, string SyncDateTimeSeason, string SyncDateTimeSizerun, string SyncDateTimeUser, string SyncDateTimeContactPerson, string SyncDateTimeClient, string SyncDateTimeStock, string SyncDateTimeTextSnippet, string SyncDateTimeSpecialDiscount, string SyncDateTimeUserAgent, string UserID);

        [OperationContract]
        [WebGet]
        string GetAvailableChangesBaseData(Dictionary<string, long> SyncDateTimes, string UserID);

        [OperationContract]
        [WebGet]
        string GetAvailableChangesBilder(string SyncDateTimeProductImage, string UserID);

        [OperationContract]
        [WebGet]
        string GetAvailableChangesBenutzerdaten(string SyncDateTimeCustomerNote, string SyncDateTimeCustomerFavorite, string UserID);

        [OperationContract]
        [WebGet]
        string GetAvailableChangesShoppingCartData(string SyncDateTimeShoppingCart, string SyncDateTimeShoppingCartItem, string UserID);

        [OperationContract]
        [WebGet]
        bool UploadCustomerFavorite(string content, string UserID);

        [OperationContract]
        [WebGet]
        bool UploadCustomerNote(string content, string UserID);

        [OperationContract]
        [WebGet]
        bool UploadShoppingCart(string content, string UserID);

        [OperationContract]
        [WebGet]
        bool UploadShoppingCartItem(string content, string UserID);

        [OperationContract]
        [WebGet]
        bool UploadClosedShoppingCart(string content, string UserID);

        [OperationContract]
        [WebGet]
        byte[] GetExportFromOrder(string ShoppingCartID);

        [OperationContract]
        [WebGet]
        void SendOrderExportMail(string ShoppingCartID, string Email);

        [OperationContract]
        [WebGet]
        void CheckAEBs();

        [OperationContract]
        [WebGet]
        void SendAEBMail(string ShoppingCartID, string Email);

        [OperationContract]
        [WebGet]
        void CheckOrders4Export();

        [OperationContract]
        [WebGet]
        void CheckUploadOrder(string content);
    }
}
