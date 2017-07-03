using CloudDataService.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CloudDataService.Export
{
    public class LVKBExportOrder
    {
        public static byte[] GetOrderStream(ShoppingCart cart, IEnumerable<ShoppingCartItem> Items)
        {
            EDI exportorder = new EDI();

            EDIHEADER header = new EDIHEADER();
            ClassFiller.FillDefaultValues(header);

            header.DOKUMENTTYP = "ORDERS";
            header.DOKUMENTID = cart.OrderNumber;
            header.DOKUMENTDATUM = DateTime.Now.ToString("yyyy.MM.dd");
            header.EMPFAENGERID = "DIAMOD";
            header.ABSENDERID = "WEB";

            EDIBODYORDERS body = new EDIBODYORDERS();
            ClassFiller.FillDefaultValues(body);

            var deliveryStart = Items.Min(i => i.ModifiedDeliveryDateStart);

            body.DocFunktion = "AUF";
            body.FirmaNr = "10";
            body.OrderNr = cart.OrderNumber;
            body.KundenOrderNr = cart.CustomerOrderNumber;
            body.OrderDatum = cart.OrderDate.ToString("yyyy.MM.dd");
            body.AuftragsDatum = cart.OrderDate.ToString("yyyy.MM.dd");
            body.Saison = cart.SeasonName;
            body.Auftragstyp = cart.OrderTypeID == "0" ? "0" : "1";
            body.Auftragsart = "5";
            body.Terminart = cart.OrderTypeID == "0" ? "2" : "0"; // OrderTypeID == 0 => Terminauftrag / OrderTypeID == 1 => Sofortauftrag 
            body.LieferDatumAnfang = deliveryStart.ToString("yyyy.MM.dd");
            body.LieferDatumEnde = deliveryStart.AddDays(14).ToString("yyyy.MM.dd");
            body.NichtLiefernVor = "";
            body.ILNBesteller = cart.CustomerNumber;

            if (!string.IsNullOrEmpty(cart.DeliveryNumber))
            {
                body.LiefAdressId = cart.DeliveryNumber;
            }

            body.Bestellart = "90";
            body.Versandart = "100";
            body.ZahlungsBed = "";
            body.LieferBed = "";
            body.Waehrung = "EUR";
            body.Bemerkung = cart.Remark1;

            body.MwSt1Satz = "0";
            body.MwSt2Satz = "0";
            body.BruttoRechnung = "N";
            body.PreislisteNr = cart.PricelistNumber;

            body.PreislisteProgramm = "0";


            body.VersandkostenGebuehr = "PO";

            body.ZANummer = "10";
            body.ZABezeichnung = "";
            body.ZAReferenz = "";

            body.Lagerort = "5";
            body.Shopnummer = "2";

            body.Kollektion = "-1";
            body.Produktbereich = "-1";

            body.Rechnungstyp = "0";            



            int counter = 0;

            List<EDIBODYORDERSORDPOS> poslist = new List<EDIBODYORDERSORDPOS>();
            ClassFiller.FillDefaultValues(poslist);

            int totalQty = 0;

            foreach (var p in Items)
            {

                if (p.Qty01 > 0)
                {
                    totalQty += p.Qty01;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN01, p.Qty01, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty02 > 0)
                {
                    totalQty += p.Qty02;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN02, p.Qty02, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty03 > 0)
                {

                    totalQty += p.Qty03;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN03, p.Qty03, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty04 > 0)
                {
                    totalQty += p.Qty04;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN04, p.Qty04, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty05 > 0)
                {
                    totalQty += p.Qty05;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN05, p.Qty05, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty06 > 0)
                {
                    totalQty += p.Qty06;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN06, p.Qty06, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty07 > 0)
                {
                    totalQty += p.Qty07;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN07, p.Qty07, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty08 > 0)
                {
                    totalQty += p.Qty08;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN08, p.Qty08, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty09 > 0)
                {
                    totalQty += p.Qty09;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN09, p.Qty09, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty10 > 0)
                {
                    totalQty += p.Qty10;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN10, p.Qty10, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty11 > 0)
                {
                    totalQty += p.Qty11;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN11, p.Qty11, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty12 > 0)
                {
                    totalQty += p.Qty12;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN12, p.Qty12, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty13 > 0)
                {
                    totalQty += p.Qty13;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN13, p.Qty13, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty14 > 0)
                {
                    totalQty += p.Qty14;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN14, p.Qty14, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty15 > 0)
                {
                    totalQty += p.Qty15;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN15, p.Qty15, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty16 > 0)
                {
                    totalQty += p.Qty16;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN16, p.Qty16, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty17 > 0)
                {
                    totalQty += p.Qty17;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN17, p.Qty17, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty18 > 0)
                {
                    totalQty += p.Qty18;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN18, p.Qty18, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty19 > 0)
                {
                    totalQty += p.Qty19;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN19, p.Qty19, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty20 > 0)
                {
                    totalQty += p.Qty20;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN20, p.Qty20, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty21 > 0)
                {
                    totalQty += p.Qty21;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN21, p.Qty21, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty22 > 0)
                {
                    totalQty += p.Qty22;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN22, p.Qty22, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty23 > 0)
                {

                    totalQty += p.Qty23;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN23, p.Qty23, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty24 > 0)
                {

                    totalQty += p.Qty24;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN24, p.Qty24, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty25 > 0)
                {
                    totalQty += p.Qty25;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN25, p.Qty25, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty26 > 0)
                {
                    totalQty += p.Qty26;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN26, p.Qty26, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty27 > 0)
                {
                    totalQty += p.Qty27;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN27, p.Qty27, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty28 > 0)
                {
                    totalQty += p.Qty28;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN28, p.Qty28, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty29 > 0)
                {
                    totalQty += p.Qty29;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN29, p.Qty29, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }

                if (p.Qty30 > 0)
                {
                    totalQty += p.Qty30;
                    counter++;
                    poslist.Add(GetFilledPos(counter, p.EAN30, p.Qty30, p.SalesPrice, p.BuyingPrice, p.DeliveryDecadeText));
                }
            }

            body.Versandkosten = cart.OrderTypeID == "0" ? "0" : (totalQty < 6 ? "6.00" : "0");

            body.ORDPOS = poslist.ToArray();

            EDIBODY ordersbody = new EDIBODY();
            ordersbody.ORDERS = new EDIBODYORDERS[] { body };


            exportorder.Items = new object[] { header, ordersbody };

            XmlSerializer ser = new XmlSerializer(typeof(EDI));
            MemoryStream mem = new MemoryStream();

            ser.Serialize(mem, exportorder);

            return mem.ToArray();
        }
        private static EDIBODYORDERSORDPOS GetFilledPos(int poscounter, string EAN, int qty, double SalesPrice, double BuyingPrice, string dekade)
        {
            EDIBODYORDERSORDPOS pos = new EDIBODYORDERSORDPOS();
            ClassFiller.FillDefaultValues(pos);

            int daystart = 1;
            int dayend = 1;
            int month = 1;
            int year = 2017;

            month = Convert.ToInt32(dekade.Split(' ')[1]);
            year = Convert.ToInt32(dekade.Split(' ')[2]);

            if (dekade.StartsWith("1"))
            {
                daystart = 1;
                dayend = 15;
            }

            

            if (dekade.StartsWith("2"))
            {
                daystart = 16;
                dayend = DateTime.DaysInMonth(year, month);
            }

            DateTime start = new DateTime(year, month, daystart);

            DateTime end = new DateTime(year, month, dayend);

            pos.PosFunktion = "VR";
            pos.PosNummer = poscounter.ToString();
            pos.Barcode = EAN;
            pos.PosMenge = qty.ToString();
            pos.PosMwStSchl = "1";
            pos.PreisReg = SalesPrice.ToString("0.00").Replace(",", ".");
            pos.PreisEff = BuyingPrice.ToString("0.00").Replace(",", ".");
            pos.VonLiefertermin = start.ToString("yyyy.MM.dd");
            pos.BisLiefertermin = end.ToString("yyyy.MM.dd");
            pos.PosBemerkung = "";
            pos.SortimentsAnzahl = "0";
            pos.Thema = "";
            pos.VE = "1";
            pos.Bestandskz = "J";

            return pos;
        }

    }
}