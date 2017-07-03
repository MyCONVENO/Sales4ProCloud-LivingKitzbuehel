using CloudDataService;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace CloudDataService.Report
{
    class ConfirmPDF
    {
        //PdfBrushes
        static PdfBrush brush_black = new PdfSolidBrush(System.Drawing.Color.FromArgb(255, 0, 0, 0));
        static PdfBrush brush_gray = new PdfSolidBrush(System.Drawing.Color.FromArgb(255, 200, 200, 200));

        //PdfFonts
        static PdfFont font_smallAddressline = new PdfStandardFont(PdfFontFamily.Helvetica, 6);
        static PdfFont font_ConfirmHeader = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold);
        static PdfFont font_tableHeader = new PdfStandardFont(PdfFontFamily.Helvetica, 9, PdfFontStyle.Bold);
        static PdfFont font_normal = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
        static PdfFont font_normal_strikeout = new PdfStandardFont(PdfFontFamily.Helvetica, 9, PdfFontStyle.Strikeout);
        static PdfFont font_size = new PdfStandardFont(PdfFontFamily.Helvetica, 7);
        static PdfFont font_footer = new PdfStandardFont(PdfFontFamily.Helvetica, 6);

        //PdfPen
        static PdfPen pdfPen_black = new PdfPen(brush_black, 0.1f);
        static PdfPen pdfPen_gray = new PdfPen(brush_gray, 0.1f);
        static PdfPen pdfPen_blackTableLine = new PdfPen(brush_black, 0.5f);

        //PdfStringFormat
        static PdfStringFormat pdfStringFormat_AlignRight = new PdfStringFormat(PdfTextAlignment.Right);
        static PdfStringFormat pdfStringFormat_AlignLeft = new PdfStringFormat(PdfTextAlignment.Left);
        static PdfStringFormat pdfStringFormat_AlignCenter = new PdfStringFormat(PdfTextAlignment.Center);

        //PdfLayoutResult
        static PdfLayoutResult drawResult;

        //PDFPageCount
        static int PageCount = 0;

        static bool printTableHeader = true;

        static string CartCurrency = string.Empty;

        static bool drawUVP = false;
        static bool ExistsItemDiscount = false;

        //Tabelleaufteilung
        static List<float> tableSizes = new List<float>() { 0, 75, 130, 180, 260, 150, 100, 50, 0 };

        public static  MemoryStream Generate(ShoppingCart Cart, List<ShoppingCartItem> Items, bool suggestion, User user)
        {
            //CultureInfo ci = new CultureInfo("de-DE");

            //Thread.CurrentThread.CurrentCulture = ci;
            //Thread.CurrentThread.CurrentUICulture = ci;

            ResourceManager rm =  new ResourceManager("CloudDataService.res.ConfirmResource", Assembly.GetExecutingAssembly());

            //PdfTextElements
            PdfTextElement textElement = new PdfTextElement();

            CartCurrency = Cart.Currency;

            drawUVP = Cart.CustomerCountryCode == "DE" || Cart.CustomerCountryCode == "AT";
            ExistsItemDiscount = Items.Any(i => i.ShoppingCartItemDiscount > 0);

            Client currentclient = Clients.FirstOrDefault(c => c.ClientID == Cart.ShoppingCartClientID);

            PdfDocument doc = new PdfDocument();
            PageCount = 0;
            doc.PageSettings.Orientation = PdfPageOrientation.Portrait;
            doc.PageSettings.Margins.Left = 40;
            doc.PageSettings.Margins.Right = 40;
            doc.PageSettings.Margins.Top = 40;
            doc.PageSettings.Height = PdfPageSize.A4.Height;
            doc.PageSettings.Width = PdfPageSize.A4.Width;
            doc.Pages.PageAdded += Pages_PageAdded;

            PdfPage page = doc.Pages.Add();
            PdfGraphics g = page.Graphics;

            printTableHeader = true;


            addHeader(doc, currentclient);

            addFooter(doc, currentclient, font_footer);

            textElement = new PdfTextElement(currentclient.Addressline, font_smallAddressline, brush_black);
            drawResult = textElement.Draw(page, 0, 65);

            textElement = new PdfTextElement(Cart.CustomerName1, font_tableHeader, brush_black);
            drawResult = textElement.Draw(page, 0, drawResult.Bounds.Y + font_tableHeader.Height + 3);
            textElement = new PdfTextElement(Cart.CustomerName2, font_tableHeader, brush_black);
            drawResult = textElement.Draw(page, 0, drawResult.Bounds.Y + font_tableHeader.Height);
            textElement = new PdfTextElement(Cart.CustomerName3, font_normal, brush_black);
            drawResult = textElement.Draw(page, 0, drawResult.Bounds.Y + font_normal.Height);
            textElement = new PdfTextElement(Cart.CustomerStreet, font_normal, brush_black);
            drawResult = textElement.Draw(page, 0, drawResult.Bounds.Y + font_normal.Height + 3);
            textElement = new PdfTextElement(Cart.CustomerZIP + " " + Cart.CustomerCity, font_normal, brush_black);
            drawResult = textElement.Draw(page, 0, drawResult.Bounds.Y + font_normal.Height);
            textElement = new PdfTextElement(Cart.CustomerCountryName, font_normal, brush_black);
            drawResult = textElement.Draw(page, 0, drawResult.Bounds.Y + font_normal.Height);

            textElement = new PdfTextElement(Cart.StatusID < 10 ? rm.GetString("OrderProposal") : (suggestion ? rm.GetString("OrderProposalForm") : rm.GetString("OrderProposalPreview")), font_ConfirmHeader, brush_black);
            drawResult = textElement.Draw(page, g.ClientSize.Width - 200, 100);

            float tablespace = 90;

            textElement = new PdfTextElement(rm.GetString("OrderType"), font_tableHeader, brush_black);
            textElement.Draw(page, g.ClientSize.Width - 200, drawResult.Bounds.Y + font_tableHeader.Height + 3);
            textElement = new PdfTextElement(Cart.OrderTypeText, font_normal, brush_black);
            drawResult = textElement.Draw(page, g.ClientSize.Width - tablespace, drawResult.Bounds.Y + font_normal.Height + 3);

            textElement = new PdfTextElement(rm.GetString("PreliminaryOrderNo"), font_tableHeader, brush_black);
            textElement.Draw(page, g.ClientSize.Width - 200, drawResult.Bounds.Y + font_tableHeader.Height);
            textElement = new PdfTextElement(Cart.OrderNumber, font_normal, brush_black);
            drawResult = textElement.Draw(page, g.ClientSize.Width - tablespace, drawResult.Bounds.Y + font_normal.Height);

            textElement = new PdfTextElement(rm.GetString("Date"), font_tableHeader, brush_black);
            textElement.Draw(page, g.ClientSize.Width - 200, drawResult.Bounds.Y + font_tableHeader.Height);
            textElement = new PdfTextElement(Cart.OrderDate.ToString("dd.MM.yyyy"), font_normal, brush_black);
            drawResult = textElement.Draw(page, g.ClientSize.Width - tablespace, drawResult.Bounds.Y + font_normal.Height);

            textElement = new PdfTextElement(rm.GetString("CustomerNumber"), font_tableHeader, brush_black);
            textElement.Draw(page, g.ClientSize.Width - 200, drawResult.Bounds.Y + font_tableHeader.Height);
            textElement = new PdfTextElement(Cart.CustomerNumber, font_normal, brush_black);
            drawResult = textElement.Draw(page, g.ClientSize.Width - tablespace, drawResult.Bounds.Y + font_normal.Height);

            if (!string.IsNullOrEmpty(Cart.CustomerOrderNumber))
            {
                textElement = new PdfTextElement(rm.GetString("YourOrderNumber"), font_tableHeader, brush_black);
                textElement.Draw(page, g.ClientSize.Width - 200, drawResult.Bounds.Y + font_normal.Height);
                textElement = new PdfTextElement(Cart.CustomerOrderNumber, font_normal, brush_black);
                drawResult = textElement.Draw(page, g.ClientSize.Width - tablespace, drawResult.Bounds.Y + font_normal.Height);
            }

            if (!string.IsNullOrEmpty(Cart.AgentName1))
            {
                textElement = new PdfTextElement(rm.GetString("Agent"), font_tableHeader, brush_black);
                textElement.Draw(page, g.ClientSize.Width - 200, drawResult.Bounds.Y + font_tableHeader.Height);
                textElement = new PdfTextElement(Cart.AgentName1, font_normal, brush_black);
                drawResult = textElement.Draw(page, g.ClientSize.Width - tablespace, drawResult.Bounds.Y + font_normal.Height);
            }

            textElement = new PdfTextElement(rm.GetString("Season"), font_tableHeader, brush_black);
            textElement.Draw(page, g.ClientSize.Width - 200, drawResult.Bounds.Y + font_tableHeader.Height);
            textElement = new PdfTextElement(Cart.SeasonName, font_normal, brush_black);
            drawResult = textElement.Draw(page, g.ClientSize.Width - tablespace, drawResult.Bounds.Y + font_normal.Height);

            if (!string.IsNullOrEmpty(Cart.AssociationMemberNumber))
            {
                textElement = new PdfTextElement(rm.GetString("Association"), font_tableHeader, brush_black);
                textElement.Draw(page, g.ClientSize.Width - 200, drawResult.Bounds.Y + font_tableHeader.Height);
                textElement = new PdfTextElement(Cart.AssociationName1, font_normal, brush_black);
                drawResult = textElement.Draw(page, g.ClientSize.Width - tablespace, drawResult.Bounds.Y + font_normal.Height);

                textElement = new PdfTextElement(rm.GetString("AssoMemberNumber"), font_tableHeader, brush_black);
                textElement.Draw(page, g.ClientSize.Width - 200, drawResult.Bounds.Y + font_tableHeader.Height);
                textElement = new PdfTextElement(Cart.AssociationMemberNumber, font_normal, brush_black);
                drawResult = textElement.Draw(page, g.ClientSize.Width - tablespace, drawResult.Bounds.Y + font_normal.Height);
            }


            drawTableHeader(drawResult.Page, drawResult.Bounds.Y + font_normal.Height + 40);

            double totalqty = 0;
            double totalsum = 0;
            int poscount = 0;
            foreach (ShoppingCartItem item in Items.OrderBy(i => i.ShoppingCartItemSort))
            {
                //foreach (var formGroup in deliveryDateGroup.OrderBy(i => i.FormName).GroupBy(i => i.FormName))
                //{               

                //foreach (var item in formGroup)
                //{
                poscount++;

                if (poscount == Items.Count)
                {
                    printTableHeader = false;
                }

                textElement = new PdfTextElement(poscount.ToString(), font_normal, brush_black);
                drawResult = textElement.Draw(drawResult.Page, tableSizes[0], drawResult.Bounds.Y + font_normal.Height);

                Stream imgStream = tryOpenImage(item.ColorImage);

                if (imgStream != null)
                {
                    PdfImage img = new PdfBitmap(imgStream);
                    //g.DrawImage(img, 15, drawResult.Bounds.Y, img.Width / 5, img.Height / 5);
                    float res = (float)img.Width / (float)img.Height * 30;
                    g.DrawImage(img, 15, drawResult.Bounds.Y, res, 30);
                }

                textElement = new PdfTextElement(item.ArticleNumber, font_normal, brush_black);
                textElement.Draw(drawResult.Page, tableSizes[1], drawResult.Bounds.Y);

                textElement = new PdfTextElement(item.ColorNumber, font_normal, brush_black);
                textElement.Draw(drawResult.Page, tableSizes[1], drawResult.Bounds.Y + font_normal.Height);

                textElement = new PdfTextElement(rm.GetString("SizeRange") + ": " + item.SizerunName, font_normal, brush_black);
                textElement.Draw(drawResult.Page, tableSizes[1], drawResult.Bounds.Y + font_normal.Height * 2);


                List<KeyValuePair<string, int>> sizes = getSizes(item);

                float horizontalOffset = 0;
                foreach (KeyValuePair<string, int> s in sizes)
                {
                    textElement = new PdfTextElement(s.Key, font_size, null, brush_black, pdfStringFormat_AlignCenter);
                    textElement.Draw(drawResult.Page, tableSizes[1] + horizontalOffset + (suggestion ? 11 : 8), drawResult.Bounds.Y + font_normal.Height * 3);
                    //Trennlinie Größe
                    //g.DrawLine(pdfPen_black, tableSizes[1] + horizontalOffset, drawResult.Bounds.Y + font_tableHeader.Height * 4 - 2, tableSizes[1] + horizontalOffset + 16, drawResult.Bounds.Y + font_tableHeader.Height * 4 - 2);
                    textElement = new PdfTextElement(s.Value == 0 ? string.Empty : s.Value.ToString(), font_size, null, brush_black, pdfStringFormat_AlignCenter);
                    textElement.Draw(drawResult.Page, tableSizes[1] + horizontalOffset + (suggestion ? 11 : 8), drawResult.Bounds.Y + font_normal.Height * 4 - 2);

                    if (suggestion)
                    {
                        //PdfTextBoxField firstNameTextBox = new PdfTextBoxField(page, s.Key);
                        //firstNameTextBox.Bounds = new RectangleF(tableSizes[1] + horizontalOffset, drawResult.Bounds.Y  + (doc.PageCount > 1 ? 50 : 0), 24, 20);
                        //firstNameTextBox.Font = font_normal;
                        //firstNameTextBox.TextAlignment = PdfTextAlignment.Center;
                        //doc.Form.Fields.Add(firstNameTextBox);
                        g.DrawLine(pdfPen_gray, tableSizes[1] + horizontalOffset, drawResult.Bounds.Y + font_normal.Height * 5, tableSizes[1] + horizontalOffset + 22, drawResult.Bounds.Y + font_normal.Height * 5);
                        g.DrawLine(pdfPen_gray, tableSizes[1] + horizontalOffset + 22, drawResult.Bounds.Y + font_normal.Height * 5, tableSizes[1] + horizontalOffset + 22, drawResult.Bounds.Y + font_normal.Height * 7);
                        g.DrawLine(pdfPen_gray, tableSizes[1] + horizontalOffset + 22, drawResult.Bounds.Y + font_normal.Height * 7, tableSizes[1] + horizontalOffset, drawResult.Bounds.Y + font_normal.Height * 7);
                        g.DrawLine(pdfPen_gray, tableSizes[1] + horizontalOffset, drawResult.Bounds.Y + font_normal.Height * 7, tableSizes[1] + horizontalOffset, drawResult.Bounds.Y + font_normal.Height * 5);
                    }


                    horizontalOffset += (suggestion ? 22 : 18);
                }



                textElement = new PdfTextElement(item.Width, font_normal, brush_black);
                textElement.Draw(drawResult.Page, tableSizes[2], drawResult.Bounds.Y);

                textElement = new PdfTextElement(item.ColorName, font_normal, brush_black);
                textElement.Draw(drawResult.Page, tableSizes[2], drawResult.Bounds.Y + font_normal.Height);

                textElement = new PdfTextElement(item.CustomerArticleNumber, font_normal, brush_black);
                textElement.Draw(drawResult.Page, tableSizes[3], drawResult.Bounds.Y);

                textElement = new PdfTextElement(item.DeliveryDecadeText, font_normal, brush_black);
                textElement.Draw(drawResult.Page, tableSizes[4], drawResult.Bounds.Y);

                //if (!string.IsNullOrEmpty(item.ArticleMaterial))
                //{
                //    textElement = new PdfTextElement("Material: " + item.ArticleMaterial, font_normal, brush_black);
                //    textElement.Draw(drawResult.Page, tableSizes[3], drawResult.Bounds.Y + font_tableHeader.Height * 2);
                //}

                int qty = (item.Qty01 + item.Qty02 + item.Qty03 + item.Qty04 + item.Qty05 + item.Qty06 + item.Qty07 + item.Qty08 + item.Qty09 + item.Qty10 + item.Qty11 + item.Qty12 + item.Qty13 + item.Qty14 + item.Qty15 + item.Qty16 + item.Qty17 + item.Qty18 + item.Qty19 + item.Qty20 + item.Qty21 + item.Qty22 + item.Qty23 + item.Qty24 + item.Qty25 + item.Qty26 + item.Qty27 + item.Qty28 + item.Qty29 + item.Qty30) * item.AssortmentQty;

                totalqty += qty;

                textElement = new PdfTextElement(qty.ToString(), font_normal, null, brush_black, pdfStringFormat_AlignRight);
                textElement.Draw(drawResult.Page, g.ClientSize.Width - tableSizes[5], drawResult.Bounds.Y);

                if (item.ShoppingCartItemDiscount > 0 || item.IsFreeOfCharge)
                {
                    if (item.IsFreeOfCharge)
                    {
                        textElement = new PdfTextElement(rm.GetString("FoC"), font_normal, null, brush_black, pdfStringFormat_AlignRight);
                        textElement.Draw(drawResult.Page, g.ClientSize.Width - tableSizes[5], drawResult.Bounds.Y + font_normal.Height);
                    }
                    else
                    {
                        textElement = new PdfTextElement(item.ShoppingCartItemDiscount.ToString("0.0%"), font_normal, null, brush_black, pdfStringFormat_AlignRight);
                        textElement.Draw(drawResult.Page, g.ClientSize.Width - tableSizes[5], drawResult.Bounds.Y + font_normal.Height);
                    }
                }

                if (drawUVP)
                {
                    textElement = new PdfTextElement(item.SalesPrice.ToString("n2"), font_normal, null, brush_black, pdfStringFormat_AlignRight);
                    textElement.Draw(drawResult.Page, g.ClientSize.Width - tableSizes[6], drawResult.Bounds.Y);
                }


                if (item.ShoppingCartItemDiscount == 0 && !item.IsFreeOfCharge)
                {
                    textElement = new PdfTextElement(item.BuyingPrice.ToString("n2"), font_normal, null, brush_black, pdfStringFormat_AlignRight);
                    textElement.Draw(drawResult.Page, g.ClientSize.Width - tableSizes[7], drawResult.Bounds.Y);

                    double total = (double)qty * item.BuyingPrice * (1 - item.ShoppingCartItemDiscount);
                    totalsum += total;

                    textElement = new PdfTextElement(Math.Round(total, 2).ToString("n2"), font_normal, null, brush_black, pdfStringFormat_AlignRight);
                    textElement.Draw(drawResult.Page, g.ClientSize.Width - tableSizes[8], drawResult.Bounds.Y);
                }
                else
                {
                    textElement = new PdfTextElement(item.BuyingPrice.ToString("n2"), font_normal_strikeout, null, brush_black, pdfStringFormat_AlignRight);
                    textElement.Draw(drawResult.Page, g.ClientSize.Width - tableSizes[7], drawResult.Bounds.Y);

                    double total = (double)qty * item.BuyingPrice;

                    textElement = new PdfTextElement(Math.Round(total, 2).ToString("n2"), font_normal_strikeout, null, brush_black, pdfStringFormat_AlignRight);
                    textElement.Draw(drawResult.Page, g.ClientSize.Width - tableSizes[8], drawResult.Bounds.Y);

                    if (item.IsFreeOfCharge)
                    {
                        item.BuyingPrice = 0;
                    }

                    double discountValue = item.BuyingPrice * (1 - item.ShoppingCartItemDiscount);

                    textElement = new PdfTextElement(discountValue.ToString("n2"), font_normal, null, brush_black, pdfStringFormat_AlignRight);
                    textElement.Draw(drawResult.Page, g.ClientSize.Width - tableSizes[7], drawResult.Bounds.Y + font_normal.Height);

                    total = (double)qty * discountValue;
                    totalsum += total;

                    textElement = new PdfTextElement(Math.Round(total, 2).ToString("n2"), font_normal, null, brush_black, pdfStringFormat_AlignRight);
                    textElement.Draw(drawResult.Page, g.ClientSize.Width - tableSizes[8], drawResult.Bounds.Y + font_normal.Height);
                }

                textElement = new PdfTextElement(" ", font_normal, pdfPen_black, brush_black, pdfStringFormat_AlignRight);
                drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width, drawResult.Bounds.Y + font_normal.Height * (suggestion ? 7 : 5));

                drawResult.Page.Graphics.DrawLine(pdfPen_gray, 0, drawResult.Bounds.Y + 3, drawResult.Page.Graphics.ClientSize.Width, drawResult.Bounds.Y + 3);



                if (drawResult.Bounds.Y + 150 > g.ClientSize.Height)
                {
                    page = doc.Pages.Add();
                }
                g = drawResult.Page.Graphics;
                // }
                // }
            }

            if (!suggestion)
            {
                textElement = new PdfTextElement(rm.GetString("TotalQuantity"), font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
                drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width - 120, drawResult.Bounds.Y + font_tableHeader.Height);
                textElement = new PdfTextElement(totalqty.ToString(), font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
                drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width, drawResult.Bounds.Y);


                textElement = new PdfTextElement(rm.GetString("TotalValue"), font_normal, null, brush_black, pdfStringFormat_AlignRight);
                drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width - 120, drawResult.Bounds.Y + font_normal.Height);
                textElement = new PdfTextElement(totalsum.ToString("n2") + " " + Cart.Currency, font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
                drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width, drawResult.Bounds.Y);

                double rabatt = 0;


                double ShoppingCartCustomerDiscount = totalsum * Cart.ShoppingCartCustomerDiscount.Value;
                rabatt += ShoppingCartCustomerDiscount;

                if (Cart.ShoppingCartCustomerDiscount > 0)
                {
                    textElement = new PdfTextElement(rm.GetString("CustomerDiscount") + " (" + Cart.ShoppingCartCustomerDiscount.Value.ToString("p1") + ")", font_normal, null, brush_black, pdfStringFormat_AlignRight);
                    drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width - 120, drawResult.Bounds.Y + font_normal.Height);
                    textElement = new PdfTextElement(ShoppingCartCustomerDiscount.ToString("n2") + " " + Cart.Currency, font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
                    drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width, drawResult.Bounds.Y);
                }

                double ShoppingCartCollectionDiscount = totalsum * Cart.ShoppingCartCollectionDiscount.Value;
                rabatt += ShoppingCartCollectionDiscount;

                if (Cart.ShoppingCartCollectionDiscount > 0)
                {
                    textElement = new PdfTextElement(rm.GetString("CollectionDiscount") + " (" + Cart.ShoppingCartCollectionDiscount.Value.ToString("p1") + ")", font_normal, null, brush_black, pdfStringFormat_AlignRight);
                    drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width - 120, drawResult.Bounds.Y + font_normal.Height);
                    textElement = new PdfTextElement(ShoppingCartCollectionDiscount.ToString("n2") + " " + Cart.Currency, font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
                    drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width, drawResult.Bounds.Y);
                }

                double ShoppingCartQuantityDiscount = totalsum * Cart.ShoppingCartQuantityDiscount.Value;
                rabatt += ShoppingCartQuantityDiscount;

                if (Cart.ShoppingCartQuantityDiscount > 0)
                {
                    textElement = new PdfTextElement(rm.GetString("QuantityDiscount") + " (" + Cart.ShoppingCartQuantityDiscount.Value.ToString("p1") + ")", font_normal, null, brush_black, pdfStringFormat_AlignRight);
                    drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width - 120, drawResult.Bounds.Y + font_normal.Height);
                    textElement = new PdfTextElement(ShoppingCartQuantityDiscount.ToString("n2") + " " + Cart.Currency, font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
                    drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width, drawResult.Bounds.Y);
                }

                double ShoppingCartEarlyBirdDiscount = totalsum * Cart.ShoppingCartEarlyBirdDiscount.Value;
                rabatt += ShoppingCartEarlyBirdDiscount;

                if (Cart.ShoppingCartEarlyBirdDiscount > 0)
                {
                    textElement = new PdfTextElement(rm.GetString("EarlyBirdDiscount") + " (" + Cart.ShoppingCartEarlyBirdDiscount.Value.ToString("p1") + ")", font_normal, null, brush_black, pdfStringFormat_AlignRight);
                    drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width - 120, drawResult.Bounds.Y + font_normal.Height);
                    textElement = new PdfTextElement(ShoppingCartEarlyBirdDiscount.ToString("n2") + " " + Cart.Currency, font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
                    drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width, drawResult.Bounds.Y);
                }

                double ShoppingCartPreorderDiscount = totalsum * Cart.ShoppingCartPreorderDiscount.Value;
                rabatt += ShoppingCartPreorderDiscount;

                if (Cart.ShoppingCartPreorderDiscount > 0)
                {
                    textElement = new PdfTextElement(rm.GetString("PreOrderDiscount") + " (" + Cart.ShoppingCartPreorderDiscount.Value.ToString("p1") + ")", font_normal, null, brush_black, pdfStringFormat_AlignRight);
                    drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width - 120, drawResult.Bounds.Y + font_normal.Height);
                    textElement = new PdfTextElement(ShoppingCartPreorderDiscount.ToString("n2") + " " + Cart.Currency, font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
                    drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width, drawResult.Bounds.Y);
                }


                textElement = new PdfTextElement(rm.GetString("OrderValue"), font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
                drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width - 120, drawResult.Bounds.Y + font_tableHeader.Height);
                textElement = new PdfTextElement((totalsum - rabatt).ToString("n2") + " " + Cart.Currency, font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
                drawResult = textElement.Draw(drawResult.Page, g.ClientSize.Width, drawResult.Bounds.Y);


                textElement = new PdfTextElement(rm.GetString("PaymentTerms") + ": " + Cart.PaymentTermText, font_normal, brush_black);
                drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height + 10);
            }

            if (!string.IsNullOrEmpty(Cart.Remark1))
            {
                textElement = new PdfTextElement(rm.GetString("Remarks") + ": " + Cart.Remark1, font_normal, brush_black);
                drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height + 10);
            }

            float spacingInvoiceAddress = drawResult.Page.GetClientSize().Width / 2;


            textElement = new PdfTextElement(rm.GetString("DeliveryTo") + ": ", font_tableHeader, brush_black);
            drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height + 15);

            textElement = new PdfTextElement(rm.GetString("InvoiceTo") + ": ", font_tableHeader, brush_black);
            drawResult = textElement.Draw(drawResult.Page, spacingInvoiceAddress, drawResult.Bounds.Y);


            textElement = new PdfTextElement(Cart.DeliveryNumber == null ? " " : Cart.DeliveryNumber, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height);

            textElement = new PdfTextElement(Cart.InvoiceNumber == null ? " " : Cart.InvoiceNumber, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, spacingInvoiceAddress, drawResult.Bounds.Y);


            textElement = new PdfTextElement(string.IsNullOrEmpty(Cart.DeliveryAddressName1) ? " " : Cart.DeliveryAddressName1, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height);
            textElement = new PdfTextElement(string.IsNullOrEmpty(Cart.InvoiceAddressName1) ? " " : Cart.InvoiceAddressName1, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, spacingInvoiceAddress, drawResult.Bounds.Y);


            textElement = new PdfTextElement(string.IsNullOrEmpty(Cart.DeliveryAddressName2) ? " " : Cart.DeliveryAddressName2, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height);
            textElement = new PdfTextElement(string.IsNullOrEmpty(Cart.InvoiceAddressName2) ? " " : Cart.InvoiceAddressName2, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, spacingInvoiceAddress, drawResult.Bounds.Y);


            textElement = new PdfTextElement(string.IsNullOrEmpty(Cart.DeliveryAddressName3) ? " " : Cart.DeliveryAddressName3, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height);
            textElement = new PdfTextElement(string.IsNullOrEmpty(Cart.InvoiceAddressName3) ? " " : Cart.InvoiceAddressName3, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, spacingInvoiceAddress, drawResult.Bounds.Y);


            textElement = new PdfTextElement(string.IsNullOrEmpty(Cart.DeliveryAddressStreet) ? " " : Cart.DeliveryAddressStreet, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height + 3);
            textElement = new PdfTextElement(string.IsNullOrEmpty(Cart.InvoiceAddressStreet) ? " " : Cart.InvoiceAddressStreet, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, spacingInvoiceAddress, drawResult.Bounds.Y);


            textElement = new PdfTextElement((string.IsNullOrEmpty(Cart.DeliveryAddressZIP) ? " " : Cart.DeliveryAddressZIP) + " " + (string.IsNullOrEmpty(Cart.DeliveryAddressCity) ? " " : Cart.DeliveryAddressCity), font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height);
            textElement = new PdfTextElement((string.IsNullOrEmpty(Cart.InvoiceAddressZIP) ? " " : Cart.InvoiceAddressZIP) + " " + (string.IsNullOrEmpty(Cart.InvoiceAddressCity) ? " " : Cart.InvoiceAddressCity), font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, spacingInvoiceAddress, drawResult.Bounds.Y);


            textElement = new PdfTextElement(string.IsNullOrEmpty(Cart.DeliveryAddressCountryName) ? " " : Cart.DeliveryAddressCountryName, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height);
            textElement = new PdfTextElement(string.IsNullOrEmpty(Cart.InvoiceAddressCountryName) ? " " : Cart.InvoiceAddressCountryName, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, spacingInvoiceAddress, drawResult.Bounds.Y);


            textElement = new PdfTextElement(rm.GetString("ContactPerson") + ": ", font_tableHeader, brush_black);
            drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_tableHeader.Height + 15);

            textElement = new PdfTextElement(user.Name1 + " " + user.Name2, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height);

            textElement = new PdfTextElement(user.Email, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height);

            textElement = new PdfTextElement(user.Street, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height + 3);

            textElement = new PdfTextElement(user.ZIP + " " + user.City, font_normal, brush_black);
            drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_normal.Height);

            if (!suggestion && Cart.StatusID < 10)
            {
                textElement = new PdfTextElement(rm.GetString("ThankYouForYourOrder"), font_tableHeader, brush_black);
                drawResult = textElement.Draw(drawResult.Page, 0, drawResult.Bounds.Y + font_tableHeader.Height + 10);
            }

            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);

            return new MemoryStream(stream.ToArray());

            ////////////Save(stream, "Sales4Pro_Ausdruck.pdf");
        }

        private static List<KeyValuePair<string, int>> getSizes(ShoppingCartItem item)
        {
            List<KeyValuePair<string, int>> sizes = new List<KeyValuePair<string, int>>();

            if (!string.IsNullOrEmpty(item.Size01)) { sizes.Add(new KeyValuePair<string, int>(item.Size01, item.Qty01)); }
            if (!string.IsNullOrEmpty(item.Size02)) { sizes.Add(new KeyValuePair<string, int>(item.Size02, item.Qty02)); }
            if (!string.IsNullOrEmpty(item.Size03)) { sizes.Add(new KeyValuePair<string, int>(item.Size03, item.Qty03)); }
            if (!string.IsNullOrEmpty(item.Size04)) { sizes.Add(new KeyValuePair<string, int>(item.Size04, item.Qty04)); }
            if (!string.IsNullOrEmpty(item.Size05)) { sizes.Add(new KeyValuePair<string, int>(item.Size05, item.Qty05)); }
            if (!string.IsNullOrEmpty(item.Size06)) { sizes.Add(new KeyValuePair<string, int>(item.Size06, item.Qty06)); }
            if (!string.IsNullOrEmpty(item.Size07)) { sizes.Add(new KeyValuePair<string, int>(item.Size07, item.Qty07)); }
            if (!string.IsNullOrEmpty(item.Size08)) { sizes.Add(new KeyValuePair<string, int>(item.Size08, item.Qty08)); }
            if (!string.IsNullOrEmpty(item.Size09)) { sizes.Add(new KeyValuePair<string, int>(item.Size09, item.Qty09)); }
            if (!string.IsNullOrEmpty(item.Size10)) { sizes.Add(new KeyValuePair<string, int>(item.Size10, item.Qty10)); }
            if (!string.IsNullOrEmpty(item.Size11)) { sizes.Add(new KeyValuePair<string, int>(item.Size11, item.Qty11)); }
            if (!string.IsNullOrEmpty(item.Size12)) { sizes.Add(new KeyValuePair<string, int>(item.Size12, item.Qty12)); }
            if (!string.IsNullOrEmpty(item.Size13)) { sizes.Add(new KeyValuePair<string, int>(item.Size13, item.Qty13)); }
            if (!string.IsNullOrEmpty(item.Size14)) { sizes.Add(new KeyValuePair<string, int>(item.Size14, item.Qty14)); }
            if (!string.IsNullOrEmpty(item.Size15)) { sizes.Add(new KeyValuePair<string, int>(item.Size15, item.Qty15)); }
            if (!string.IsNullOrEmpty(item.Size16)) { sizes.Add(new KeyValuePair<string, int>(item.Size16, item.Qty16)); }
            if (!string.IsNullOrEmpty(item.Size17)) { sizes.Add(new KeyValuePair<string, int>(item.Size17, item.Qty17)); }
            if (!string.IsNullOrEmpty(item.Size18)) { sizes.Add(new KeyValuePair<string, int>(item.Size18, item.Qty18)); }
            if (!string.IsNullOrEmpty(item.Size19)) { sizes.Add(new KeyValuePair<string, int>(item.Size19, item.Qty19)); }
            if (!string.IsNullOrEmpty(item.Size20)) { sizes.Add(new KeyValuePair<string, int>(item.Size20, item.Qty20)); }
            if (!string.IsNullOrEmpty(item.Size21)) { sizes.Add(new KeyValuePair<string, int>(item.Size21, item.Qty21)); }
            if (!string.IsNullOrEmpty(item.Size22)) { sizes.Add(new KeyValuePair<string, int>(item.Size22, item.Qty22)); }
            if (!string.IsNullOrEmpty(item.Size23)) { sizes.Add(new KeyValuePair<string, int>(item.Size23, item.Qty23)); }
            if (!string.IsNullOrEmpty(item.Size24)) { sizes.Add(new KeyValuePair<string, int>(item.Size24, item.Qty24)); }
            if (!string.IsNullOrEmpty(item.Size25)) { sizes.Add(new KeyValuePair<string, int>(item.Size25, item.Qty25)); }
            if (!string.IsNullOrEmpty(item.Size26)) { sizes.Add(new KeyValuePair<string, int>(item.Size26, item.Qty26)); }
            if (!string.IsNullOrEmpty(item.Size27)) { sizes.Add(new KeyValuePair<string, int>(item.Size27, item.Qty27)); }
            if (!string.IsNullOrEmpty(item.Size28)) { sizes.Add(new KeyValuePair<string, int>(item.Size28, item.Qty28)); }
            if (!string.IsNullOrEmpty(item.Size29)) { sizes.Add(new KeyValuePair<string, int>(item.Size29, item.Qty29)); }
            if (!string.IsNullOrEmpty(item.Size30)) { sizes.Add(new KeyValuePair<string, int>(item.Size30, item.Qty30)); }

            return sizes;
        }

        private static void Pages_PageAdded(object sender, PageAddedEventArgs args)
        {
            PageCount++;
            if (PageCount > 1)
            {
                if (printTableHeader)
                {
                    drawTableHeader(args.Page, 20);
                }
                else
                {
                    PdfTextElement textElement = new PdfTextElement(" ", font_tableHeader, pdfPen_black, brush_black, pdfStringFormat_AlignRight);
                    drawResult = textElement.Draw(args.Page, 0, 0);
                }
            }
        }

        private static void drawTableHeader(PdfPage page, float y)
        {
            ResourceManager rm = new ResourceManager("CloudDataService.res.ConfirmResource", Assembly.GetExecutingAssembly());

            PdfTextElement textElement = new PdfTextElement("Pos.", font_tableHeader, null, brush_black, pdfStringFormat_AlignLeft);
            drawResult = textElement.Draw(page, tableSizes[0], y);

            textElement = new PdfTextElement(rm.GetString("ArticleNo"), font_tableHeader, null, brush_black, pdfStringFormat_AlignLeft);
            textElement.Draw(drawResult.Page, tableSizes[1], y);
            textElement = new PdfTextElement(rm.GetString("Color"), font_tableHeader, null, brush_black, pdfStringFormat_AlignLeft);
            textElement.Draw(drawResult.Page, tableSizes[1], y + font_tableHeader.Height);

            textElement = new PdfTextElement(rm.GetString("Width"), font_tableHeader, null, brush_black, pdfStringFormat_AlignLeft);
            textElement.Draw(drawResult.Page, tableSizes[2], y);

            textElement = new PdfTextElement(rm.GetString("CustomerArticle"), font_tableHeader, null, brush_black, pdfStringFormat_AlignLeft);
            textElement.Draw(drawResult.Page, tableSizes[3], y);

            textElement = new PdfTextElement(rm.GetString("DesiredDeliveryDate"), font_tableHeader, null, brush_black, pdfStringFormat_AlignLeft);
            textElement.Draw(drawResult.Page, tableSizes[4], y);

            textElement = new PdfTextElement(rm.GetString("Quantity"), font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
            textElement.Draw(drawResult.Page, drawResult.Page.Graphics.ClientSize.Width - tableSizes[5], y);
            if (ExistsItemDiscount)
            {
                textElement = new PdfTextElement(rm.GetString("Discount"), font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
                textElement.Draw(drawResult.Page, drawResult.Page.Graphics.ClientSize.Width - tableSizes[5], y + font_tableHeader.Height);
            }

            if (drawUVP)
            {
                textElement = new PdfTextElement(rm.GetString("RRP"), font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
                textElement.Draw(drawResult.Page, drawResult.Page.Graphics.ClientSize.Width - tableSizes[6], y);
            }

            textElement = new PdfTextElement(rm.GetString("BP"), font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
            textElement.Draw(drawResult.Page, drawResult.Page.Graphics.ClientSize.Width - tableSizes[7], y);

            textElement = new PdfTextElement(rm.GetString("Total"), font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
            drawResult = textElement.Draw(drawResult.Page, drawResult.Page.Graphics.ClientSize.Width - tableSizes[8], y);

            //Währung UVP
            if (drawUVP)
            {
                textElement = new PdfTextElement(rm.GetString("EUR"), font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
                textElement.Draw(drawResult.Page, drawResult.Page.Graphics.ClientSize.Width - tableSizes[6], y + font_tableHeader.Height);
            }

            //Währung EK
            textElement = new PdfTextElement(CartCurrency, font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
            textElement.Draw(drawResult.Page, drawResult.Page.Graphics.ClientSize.Width - tableSizes[7], y + font_tableHeader.Height);

            //Währung GES
            textElement = new PdfTextElement(CartCurrency, font_tableHeader, null, brush_black, pdfStringFormat_AlignRight);
            drawResult = textElement.Draw(drawResult.Page, drawResult.Page.Graphics.ClientSize.Width - tableSizes[8], y + font_tableHeader.Height);

            drawResult.Page.Graphics.DrawLine(pdfPen_blackTableLine, 0, y + font_tableHeader.Height * 2, drawResult.Page.Graphics.ClientSize.Width, y + font_tableHeader.Height * 2);

            textElement = new PdfTextElement(" ", font_tableHeader, pdfPen_black, brush_black, pdfStringFormat_AlignRight);
            drawResult = textElement.Draw(drawResult.Page, 0, y + font_tableHeader.Height * 2 - 5);
        }

        private static void addHeader(PdfDocument doc, Client currentClient)
        {
            RectangleF rect = new RectangleF(0, 0, doc.Pages[0].GetClientSize().Width, 50);

            PdfPageTemplateElement header = new PdfPageTemplateElement(rect);

            Stream imgStream = typeof(ConfirmPDF).GetTypeInfo().Assembly.GetManifestResourceStream("CloudDataService.Confirm.Logos.LivingKitzbuehel_Logo.png");//currentClient.ImagePathString);

            PdfImage img = new PdfBitmap(imgStream);
            float res = (float)img.Width / (float)img.Height * 50;

            header.Graphics.DrawImage(img, doc.Pages[0].GetClientSize().Width - res, 0, res, 50);

            //Add header template at the top.
            doc.Template.Top = header;

        }

        private static void addFooter(PdfDocument doc, Client CurrentClient, PdfFont footer_font)
        {
            ResourceManager rm = new ResourceManager("CloudDataService.res.ConfirmResource", Assembly.GetExecutingAssembly());

            RectangleF rect = new RectangleF(0, 0, doc.Pages[0].GetClientSize().Width, 30);
            PdfColor blueColor = new PdfColor(System.Drawing.Color.FromArgb(255, 0, 0, 255));
            PdfColor GrayColor = new PdfColor(System.Drawing.Color.FromArgb(255, 128, 128, 128));
            //Create a page template
            PdfPageTemplateElement footer = new PdfPageTemplateElement(rect);


            PdfSolidBrush brush = new PdfSolidBrush(GrayColor);

            PdfStringFormat format = new PdfStringFormat();
            format.Alignment = PdfTextAlignment.Left;
            format.LineAlignment = PdfVerticalAlignment.Top;
            footer.Graphics.DrawString(CurrentClient.FooterLeftLine1, footer_font, brush, 0, 0, format);
            footer.Graphics.DrawString(CurrentClient.FooterLeftLine2, footer_font, brush, 0, 6, format);
            footer.Graphics.DrawString(CurrentClient.FooterLeftLine3, footer_font, brush, 0, 12, format);
            footer.Graphics.DrawString(CurrentClient.FooterLeftLine4, footer_font, brush, 0, 18, format);


            format = new PdfStringFormat();
            format.Alignment = PdfTextAlignment.Right;
            format.LineAlignment = PdfVerticalAlignment.Top;
            footer.Graphics.DrawString(CurrentClient.FooterRightLine1, footer_font, brush, rect.Width, 0, format);
            footer.Graphics.DrawString(CurrentClient.FooterRightLine2, footer_font, brush, rect.Width, 6, format);
            footer.Graphics.DrawString(CurrentClient.FooterRightLine3, footer_font, brush, rect.Width, 12, format);
            footer.Graphics.DrawString(CurrentClient.FooterRightLine4, footer_font, brush, rect.Width, 18, format);


            format = new PdfStringFormat();
            format.Alignment = PdfTextAlignment.Right;
            format.LineAlignment = PdfVerticalAlignment.Bottom;

            //Create page number field
            PdfPageNumberField pageNumber = new PdfPageNumberField(footer_font, brush);

            //Create page count field
            PdfPageCountField count = new PdfPageCountField(footer_font, brush);

            PdfCompositeField compositeField = new PdfCompositeField(footer_font, brush, rm.GetString("PageXofX"), pageNumber, count);
            compositeField.Bounds = footer.Bounds;
            compositeField.Draw(footer.Graphics, new PointF(rect.Width / 2, 24));

            //Add the footer template at the bottom
            doc.Template.Bottom = footer;

        }



        private static List<Client> clients;
        public static List<Client> Clients
        {
            get
            {
                if (clients == null)
                {

                    clients = new List<Client>();
                    Client clientBerkemann = new Client();
                    clientBerkemann.ClientID = "1";
                    clientBerkemann.ClientName = "LIVING KITZBÜHEL";
                    clientBerkemann.AccentColorString = "#FF990000";
                    clientBerkemann.ImagePathString = "ms-appx:///S4PDataService/Assets/LivingKitzbuehel_Logo.png";

                    clientBerkemann.AddressName1 = "Living Kitzbühel GmbH";
                    clientBerkemann.AddressName2 = "";
                    clientBerkemann.AddressStreet = "Josef-Pichl-Str. 9";
                    clientBerkemann.AddressZip = "6370";
                    clientBerkemann.AddressCity = "Kitzbühel";
                    clientBerkemann.AddressCountryCode = "AT";
                    clientBerkemann.AddressCountryName = "Österreich";
                    clientBerkemann.Addressline = "Living Kitzbühel GmbH - Josef-Pichl-Str. 9 - AT6380 Kitzbühel";
                    clientBerkemann.FooterLeftLine1 = "UID Nr. ATU 629 789 23";
                    clientBerkemann.FooterLeftLine2 = "FN 285 656 h LG Innsbruck";
                    clientBerkemann.FooterLeftLine3 = "Geschäftsführer:";
                    clientBerkemann.FooterLeftLine4 = "Jürgen Langensiepen & Dr. Walter Niedermair";

                    clientBerkemann.FooterRightLine1 = "Südtiroler Volksbank, Fil. Bruneck";
                    clientBerkemann.FooterRightLine2 = "IBAN: IT61 F058 5658 2400 1057 1156 910";
                    clientBerkemann.FooterRightLine3 = "SWIFT/BIC: BPAAIT2B010";
                    clientBerkemann.FooterRightLine4 = "ABI:05856 - CAB: 58240";

                    clients.Add(clientBerkemann);
                }

                return clients;
            }
        }

        private static Stream tryOpenImage(string colorImage)
        {
            try
            {
                WebClient client = new WebClient();
                var data = client.DownloadData("http://myconvenopicstorage.blob.core.windows.net/livingkbpics/" + colorImage);

                Image img = Image.FromStream(new MemoryStream(data));
                MemoryStream saveStream = new MemoryStream();
                if (data != null)
                {
                    ScaleImage(img, 300, 300).Save(saveStream, colorImage.Contains("png") ? ImageFormat.Png : ImageFormat.Jpeg);
                    return saveStream;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        static byte[] resizeImage(byte[] imageArray, int maxWidth, int maxHeight, int quality, bool png)
        {
            using (MemoryStream imageStream = new MemoryStream(imageArray))
            {
                Bitmap image = new Bitmap(imageStream);
                // Get the image's original width and height
                int originalWidth = image.Width;
                int originalHeight = image.Height;

                // To preserve the aspect ratio
                float ratioX = (float)maxWidth / (float)originalWidth;
                float ratioY = (float)maxHeight / (float)originalHeight;
                float ratio = Math.Min(ratioX, ratioY);

                // New width and height based on aspect ratio
                int newWidth = (int)(originalWidth * ratio);
                int newHeight = (int)(originalHeight * ratio);

                // Convert other formats (including CMYK) to RGB.
                Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppRgb);

                // Draws the image in the specified size with quality mode set to HighQuality
                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                }

                // Get an ImageCodecInfo object that represents the JPEG codec.
                ImageCodecInfo imageCodecInfo = GetEncoderInfo(png ? ImageFormat.Png : ImageFormat.Jpeg);

                // Create an Encoder object for the Quality parameter.
                Encoder encoder = Encoder.Quality;

                // Create an EncoderParameters object. 
                EncoderParameters encoderParameters = new EncoderParameters(1);

                // Save the image as a JPEG file with quality level.
                EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
                encoderParameters.Param[0] = encoderParameter;

                MemoryStream resultStream = new MemoryStream();

                newImage.Save(resultStream, imageCodecInfo, encoderParameters);

                return resultStream.ToArray();
            }
        }

        static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        /// <summary>
        /// Method to get encoder infor for given image format.
        /// </summary>
        /// <param name="format">Image format</param>
        /// <returns>image codec info.</returns>
        private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }
    }
}
