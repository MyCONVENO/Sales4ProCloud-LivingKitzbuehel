using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CloudDataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICloudDataServiceV2" in both code and config file together.
    [ServiceContract]
    public interface ICloudDataServiceV2
    {
        #region Sync Methods
        [OperationContract]
        string SyncAgent(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncArticle(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncAssociation(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncAssociationMember(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncAssortment(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncColor(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncCustomer(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncCustomerNote(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncClient(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncContactPerson(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncDeliveryAddress(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncInvoiceAddress(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncLabel(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncModel(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncPrice(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncPricelist(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncSeason(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncSizerun(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncUser(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncStock(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncCustomerFavorite(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncShoppingCart(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncShoppingCartItem(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncProductImage(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncTextSnippet(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncSpecialDiscount(string SyncDateTime, string UserID);

        [OperationContract]
        string SyncUserAgent(string SyncDateTime, string UserID);
        #endregion

        [OperationContract]
        string UpdatePushChannel(string UserID, string OldChannelUri, string NewChannelUri);

        [OperationContract]
        string SendPushNotification(string UserIDs);

        [OperationContract]
        string GetAvailableChangesBaseData(Dictionary<string, long> SyncDateTimes, string UserID);

        [OperationContract]
        bool UploadCustomerFavorite(string content, string UserID);

        [OperationContract]
        bool UploadCustomerNote(string content, string UserID);

        [OperationContract]
        bool UploadShoppingCart(string content, string UserID);

        [OperationContract]
        bool UploadShoppingCartItem(string content, string UserID);

        [OperationContract]
        bool UploadClosedShoppingCart(string content, string UserID);

        [OperationContract]
        byte[] GetIdocFromOrder(string ShoppingCartID);

        [OperationContract]
        void SendIdocMail(string ShoppingCartID, string Email);

        [OperationContract]
        void CheckAEBs();

        [OperationContract]
        void SendAEBMail(string ShoppingCartID, string Email);

        [OperationContract]
        void CheckOrders4Export();

        [OperationContract]
        void MarkDataAsTransfered(List<string> DataPackages, string DataPackageType);

        [OperationContract]
        void UpdateGeoData();
    }
}
