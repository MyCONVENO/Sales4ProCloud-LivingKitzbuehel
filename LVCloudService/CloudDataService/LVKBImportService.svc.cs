using CloudDataService.CSVClasses;
using CloudDataService.Helper;
using CloudDataService.ImportClass;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace CloudDataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class LVKBImportService : ILVKBImportService
    {
        BlobstorageFileHandler blobhandler = new BlobstorageFileHandler("myconvenoftp", "ZZiN0Tl+eejzQc9ymh/vXBTziGa5n68/OVrLmGpA6FIN+Xm61yDVeadquGdSesRIoRtBXmUG586b9RjCERs5hg==", "livingkitzbuehel");
        LIVINGKITZBUEHLEntities dataModel = createDataModelInstance();

        static LIVINGKITZBUEHLEntities createDataModelInstance()
        {
            LIVINGKITZBUEHLEntities model = new LIVINGKITZBUEHLEntities();
            ((IObjectContextAdapter)model).ObjectContext.CommandTimeout = 12000;

            return model;
        }

        public string ImportSourceData()
        {
            try
            {


                ////##############
                ////###Artikel###
                ////##############

                var files = blobhandler.FileList("Stammdaten/myconveno_artikel_");
                List<List<object>> newdbitems = new List<List<object>>();

                if (files.Count > 0)
                {
                    dataModel.Database.ExecuteSqlCommand("Delete FROM source_Artikel");

                    foreach (var f in files)
                    {
                        using (StreamReader reader = new StreamReader(new MemoryStream(blobhandler.DownloadFile(f)), Encoding.Default))
                        {
                            var csv = new CsvReader(reader, new CsvConfiguration { CultureInfo = CultureInfo.GetCultureInfo("de-DE") });
                            csv.Configuration.Delimiter = ";";
                            csv.Configuration.RegisterClassMap<ArtikelCSVMap>();

                            var records = csv.GetRecords<source_Artikel>().ToList();

                            foreach (var a in records)
                            {
                                newdbitems.Add(ClassToListObjectItem.ToList(a));
                            }



                        }
                        SqlBulkInsertHelper.InsertBigData<source_Artikel>(newdbitems);
                        newdbitems.Clear();

                        blobhandler.MoveFileToArchive(f);
                    }

                }
                newdbitems.Clear();

                ////##############
                ////###Artikelbeschreibungen###
                ////##############

                files = blobhandler.FileList("Stammdaten/myconveno_artikelbeschreibung_");


                if (files.Count > 0)
                {
                    dataModel.Database.ExecuteSqlCommand("Delete FROM source_Artikelbeschreibung");

                    foreach (var f in files)
                    {
                        using (StreamReader reader = new StreamReader(new MemoryStream(blobhandler.DownloadFile(f)), Encoding.Default))
                        {
                            var csv = new CsvReader(reader, new CsvConfiguration { CultureInfo = CultureInfo.GetCultureInfo("de-DE") });
                            csv.Configuration.Delimiter = ";";
                            csv.Configuration.RegisterClassMap<ArtikelbeschreibungCSVMap>();

                            var records = csv.GetRecords<source_Artikelbeschreibung>().ToList();

                            foreach (var a in records)
                            {
                                newdbitems.Add(ClassToListObjectItem.ToList(a));
                            }



                        }
                        SqlBulkInsertHelper.InsertBigData<source_Artikelbeschreibung>(newdbitems);
                        newdbitems.Clear();

                        blobhandler.MoveFileToArchive(f);
                    }

                }
                newdbitems.Clear();


                ////##############
                ////###Vertreter###
                ////##############

                files = blobhandler.FileList("Stammdaten/myconveno_vertreter_");


                if (files.Count > 0)
                {
                    dataModel.Database.ExecuteSqlCommand("Delete FROM source_Vertreter");

                    foreach (var f in files)
                    {
                        using (StreamReader reader = new StreamReader(new MemoryStream(blobhandler.DownloadFile(f)), Encoding.Default))
                        {
                            var csv = new CsvReader(reader, new CsvConfiguration { CultureInfo = CultureInfo.GetCultureInfo("de-DE") });
                            csv.Configuration.Delimiter = ";";
                            csv.Configuration.RegisterClassMap<VertreterCSVMap>();

                            var records = csv.GetRecords<source_Vertreter>().ToList();

                            foreach (var a in records)
                            {
                                newdbitems.Add(ClassToListObjectItem.ToList(a));
                            }



                        }
                        SqlBulkInsertHelper.InsertBigData<source_Vertreter>(newdbitems);
                        newdbitems.Clear();

                        blobhandler.MoveFileToArchive(f);
                    }

                }
                newdbitems.Clear();


                ////##############
                ////###Kunden###
                ////##############

                files = blobhandler.FileList("Stammdaten/myconveno_kunde_");


                if (files.Count > 0)
                {
                    dataModel.Database.ExecuteSqlCommand("Delete FROM source_Kunde");

                    foreach (var f in files)
                    {
                        using (StreamReader reader = new StreamReader(new MemoryStream(blobhandler.DownloadFile(f)), Encoding.Default))
                        {
                            var csv = new CsvReader(reader, new CsvConfiguration { CultureInfo = CultureInfo.GetCultureInfo("de-DE") });
                            csv.Configuration.Delimiter = ";";
                            csv.Configuration.RegisterClassMap<KundeCSVMap>();

                            var records = csv.GetRecords<source_Kunde>().ToList();

                            foreach (var k in records)
                            {
                                newdbitems.Add(ClassToListObjectItem.ToList(k));
                            }



                        }
                        SqlBulkInsertHelper.InsertBigData<source_Kunde>(newdbitems);
                        newdbitems.Clear();

                        blobhandler.MoveFileToArchive(f);
                    }

                }
                newdbitems.Clear();


                ////##############
                ////###Preise###
                ////##############

                files = blobhandler.FileList("Stammdaten/myconveno_pl_");


                if (files.Count > 0)
                {
                    dataModel.Database.ExecuteSqlCommand("Delete FROM source_Preis");

                    foreach (var f in files)
                    {
                        using (StreamReader reader = new StreamReader(new MemoryStream(blobhandler.DownloadFile(f)), Encoding.Default))
                        {
                            var csv = new CsvReader(reader, new CsvConfiguration { CultureInfo = CultureInfo.GetCultureInfo("de-DE") });
                            csv.Configuration.Delimiter = ";";
                            csv.Configuration.RegisterClassMap<PreisCSVMap>();

                            var records = csv.GetRecords<source_Preis>().ToList();

                            foreach (var k in records)
                            {
                                newdbitems.Add(ClassToListObjectItem.ToList(k));
                            }



                        }
                        SqlBulkInsertHelper.InsertBigData<source_Preis>(newdbitems);
                        newdbitems.Clear();

                        blobhandler.MoveFileToArchive(f);
                    }

                }
                newdbitems.Clear();

                ////##############
                ////###Rabatte###
                ////##############

                files = blobhandler.FileList("Stammdaten/myconveno_rabatte_");


                if (files.Count > 0)
                {
                    dataModel.Database.ExecuteSqlCommand("Delete FROM source_Rabatt");

                    foreach (var f in files)
                    {
                        using (StreamReader reader = new StreamReader(new MemoryStream(blobhandler.DownloadFile(f)), Encoding.Default))
                        {
                            var csv = new CsvReader(reader, new CsvConfiguration { CultureInfo = CultureInfo.GetCultureInfo("de-DE") });
                            csv.Configuration.Delimiter = ";";
                            csv.Configuration.RegisterClassMap<RabattCSVMap>();

                            var records = csv.GetRecords<source_Rabatt>().ToList();

                            foreach (var k in records)
                            {
                                newdbitems.Add(ClassToListObjectItem.ToList(k));
                            }
                        }
                        SqlBulkInsertHelper.InsertBigData<source_Rabatt>(newdbitems);
                        newdbitems.Clear();

                        blobhandler.MoveFileToArchive(f);
                    }

                }
                newdbitems.Clear();


                ////##############
                ////###Ansprechpartner###
                ////##############

                files = blobhandler.FileList("Stammdaten/myconveno_asp_");


                if (files.Count > 0)
                {
                    dataModel.Database.ExecuteSqlCommand("Delete FROM source_Ansprechpartner");

                    foreach (var f in files)
                    {
                        using (StreamReader reader = new StreamReader(new MemoryStream(blobhandler.DownloadFile(f)), Encoding.Default))
                        {
                            var csv = new CsvReader(reader, new CsvConfiguration { CultureInfo = CultureInfo.GetCultureInfo("de-DE") });
                            csv.Configuration.Delimiter = ";";
                            csv.Configuration.RegisterClassMap<AnsprechpartnerCSVMap>();

                            var records = csv.GetRecords<source_Ansprechpartner>().ToList();

                            foreach (var k in records)
                            {
                                newdbitems.Add(ClassToListObjectItem.ToList(k));
                            }
                        }
                        SqlBulkInsertHelper.InsertBigData<source_Ansprechpartner>(newdbitems);
                        newdbitems.Clear();

                        blobhandler.MoveFileToArchive(f);
                    }

                }
                newdbitems.Clear();


                ////##############
                ////###Saisonumsatz###
                ////##############

                files = blobhandler.FileList("Stammdaten/myconveno_saisonumsatz");


                if (files.Count > 0)
                {
                    dataModel.Database.ExecuteSqlCommand("Delete FROM source_Saisonumsatz");

                    foreach (var f in files)
                    {
                        using (StreamReader reader = new StreamReader(new MemoryStream(blobhandler.DownloadFile(f)), Encoding.Default))
                        {
                            var csv = new CsvReader(reader, new CsvConfiguration { CultureInfo = CultureInfo.GetCultureInfo("de-DE") });
                            csv.Configuration.Delimiter = ";";
                            csv.Configuration.RegisterClassMap<SaisonCSVMap>();

                            var records = csv.GetRecords<source_Saisonumsatz>().ToList();

                            foreach (var k in records)
                            {
                                newdbitems.Add(ClassToListObjectItem.ToList(k));
                            }
                        }
                        SqlBulkInsertHelper.InsertBigData<source_Saisonumsatz>(newdbitems);
                        newdbitems.Clear();

                        blobhandler.MoveFileToArchive(f);
                    }

                }
                newdbitems.Clear();



                ////##############
                ////###Lieferadressen###
                ////##############

                files = blobhandler.FileList("Stammdaten/myconveno_Lieferadressen_");


                if (files.Count > 0)
                {
                    dataModel.Database.ExecuteSqlCommand("Delete FROM source_Lieferadresse");

                    foreach (var f in files)
                    {
                        using (StreamReader reader = new StreamReader(new MemoryStream(blobhandler.DownloadFile(f)), Encoding.Default))
                        {
                            var csv = new CsvReader(reader, new CsvConfiguration { CultureInfo = CultureInfo.GetCultureInfo("de-DE") });
                            csv.Configuration.Delimiter = ";";

                            csv.Configuration.RegisterClassMap<LieferadresseCSVMap>();

                            var records = csv.GetRecords<source_Lieferadresse>().ToList();

                            foreach (var k in records)
                            {
                                newdbitems.Add(ClassToListObjectItem.ToList(k));
                            }
                        }
                        SqlBulkInsertHelper.InsertBigData<source_Lieferadresse>(newdbitems);
                        newdbitems.Clear();

                        blobhandler.MoveFileToArchive(f);
                    }

                }
                newdbitems.Clear();


                return "OK";
            }
            catch (Exception ex)
            {
                SendMail("sascha.weber@myconveno.de", ex.Message.ToString(), "ERROR Source Data Import", "");
                throw;
            }
        }

        #region Article
        public string ImportArticle()
        {
            importSeason();

            importModel();

            importArticle();

            importColor();

            importSizerun();

            importAssortment();

            importImages();
            return "OK";
        }

        void importSeason()
        {

            string sourceImportSeasonQuery = "SELECT DISTINCT source_Artikel.Artikelsaison, Season.SeasonID FROM source_Artikel LEFT OUTER JOIN Season ON source_Artikel.Artikelsaison = Season.SeasonName";

            var models_source = dataModel.Database.SqlQuery<ImportSeason>(sourceImportSeasonQuery).ToList();


            List<Season> seasons = new List<Season>();

            foreach (var m in models_source)
            {
                Season newModel = new Season();
                ClassFiller.FillDefaultValues(newModel);

                newModel.IsDeleted = false;
                newModel.SeasonClientID = "1";
                newModel.SeasonID = string.Empty;
                newModel.SeasonLabelID = "1";
                newModel.SeasonName = m.Artikelsaison;
                newModel.SeasonLongName = (m.Artikelsaison.StartsWith("1") ? "Frühjahr/Sommer 20" : "Herbst/Winter 20") + m.Artikelsaison.Substring(1);
                newModel.SyncDateTime = DateTime.Now;


                if (m.SeasonID != null)
                {
                    newModel.SeasonID = m.SeasonID;
                }

                seasons.Add(newModel);
            }

            DatabaseModelHelper.ImportData<Season>(dataModel, seasons, dataModel.Season.Where(s => s.IsDeleted == false).ToList());
        }

        void importModel()
        {
            string sourceImportModelQuery = "SELECT DISTINCT Season.SeasonID, source_Artikel.Matchcode, Model.ModelID, source_Artikel.Produktbereich, source_Artikel.Produktgruppe FROM source_Artikel INNER JOIN Season ON source_Artikel.Artikelsaison = Season.SeasonName LEFT OUTER JOIN Model ON Season.SeasonID = Model.ModelSeasonID AND source_Artikel.Produktgruppe = Model.ProductGroup AND source_Artikel.Produktbereich = Model.FormName AND source_Artikel.Matchcode = Model.ModelName AND Model.IsDeleted = 0 WHERE(Season.IsDeleted = 0)";

            var models_source = dataModel.Database.SqlQuery<ImportModel>(sourceImportModelQuery).ToList();

            List<Model> Models = new List<Model>();

            foreach (var m in models_source)
            {
                Model newModel = new Model();
                ClassFiller.FillDefaultValues(newModel);

                newModel.IsDeleted = false;
                newModel.ModelClientID = "1";
                newModel.ModelID = string.Empty;
                newModel.ModelName = m.Matchcode;
                newModel.ProductGroup = m.Produktgruppe;
                newModel.FormName = m.Produktbereich;
                newModel.ModelSeasonID = m.SeasonID;
                newModel.SyncDateTime = DateTime.Now;


                if (m.ModelID != null)
                {
                    newModel.ModelID = m.ModelID;
                }

                Models.Add(newModel);
            }

            DatabaseModelHelper.ImportData<Model>(dataModel, Models, dataModel.Model.Where(s => s.IsDeleted == false).ToList());
        }

        void importArticle()
        {
            string sourceImportQuery = "SELECT DISTINCT Model.ModelID, source_Artikel.Artikelnr, source_Artikel.Artikelbez2, Article.ArticleID, source_Artikel.Artikelbez1, source_Artikelbeschreibung.Artikelbeschreibung FROM Model INNER JOIN source_Artikel INNER JOIN Season ON source_Artikel.Artikelsaison = Season.SeasonName ON Model.ProductGroup = source_Artikel.Produktgruppe AND Model.FormName = source_Artikel.Produktbereich AND Model.ModelName = source_Artikel.Matchcode AND Model.ModelSeasonID = Season.SeasonID LEFT OUTER JOIN source_Artikelbeschreibung ON source_Artikel.Artikelnr = source_Artikelbeschreibung.Artikelnr AND source_Artikel.Artikelsaison = source_Artikelbeschreibung.Artikelsaison LEFT OUTER JOIN Article ON Model.ModelID = Article.ModelID AND source_Artikel.Artikelnr = Article.ArticleNumber AND Article.IsDeleted = 0 WHERE EXISTS (SELECT Id, Preislistenr, Saison, Waehrung, ArtikelnrFarbnr, EAN, HEK, EmpfVK, Groesse FROM            source_Preis WHERE        (Saison = source_Artikel.Artikelsaison) AND(ArtikelnrFarbnr = source_Artikel.Artikelnr + '-0' + source_Artikel.Farbnr)) AND(Season.IsDeleted = 0) AND(Model.IsDeleted = 0)";

            List<Article> Articles = new List<Article>();

            var dbitems_source = dataModel.Database.SqlQuery<ImportArticle>(sourceImportQuery).ToList();

            foreach (var dbitem in dbitems_source)
            {
                Article newArticle = new Article();
                ClassFiller.FillDefaultValues(newArticle);

                newArticle.ArticleID = string.Empty;
                newArticle.ArticleClientID = "1";
                newArticle.IsDeleted = false;
                newArticle.ModelID = dbitem.ModelID;
                newArticle.ArticleNumber = dbitem.Artikelnr;
                newArticle.ArticleName = dbitem.Artikelbez1 + " " + dbitem.Artikelbez2;
                if (!string.IsNullOrEmpty(dbitem.Artikelbeschreibung))
                    newArticle.ArticleInfoText1 = dbitem.Artikelbeschreibung;
                newArticle.SyncDateTime = DateTime.UtcNow;

                if (dbitem.ArticleID != null)
                {
                    newArticle.ArticleID = dbitem.ArticleID;
                }

                Articles.Add(newArticle);
            }

            DatabaseModelHelper.ImportData<Article>(dataModel, Articles, dataModel.Article.Where(s => s.IsDeleted == false).ToList());
        }

        void importColor()
        {

            string sourceImportQuery = "SELECT        Article.ArticleID, Color.ColorID, source_Artikel.Kollektion, source_Artikel.Farbe, source_Artikel.Farbnr, source_Artikel.Artikelnr, source_Artikel.Artikelsaison, MIN(source_Artikel.Groesse) AS GroesseVon, MAX(source_Artikel.Groesse) AS GroesseBis, Season.DeliveryDateStart FROM            Article INNER JOIN Model INNER JOIN source_Artikel INNER JOIN Season ON source_Artikel.Artikelsaison = Season.SeasonName ON Model.ProductGroup = source_Artikel.Produktgruppe AND Model.FormName = source_Artikel.Produktbereich AND Model.ModelName = source_Artikel.Matchcode AND Model.ModelSeasonID = Season.SeasonID ON Article.ArticleNumber = source_Artikel.Artikelnr AND Article.ModelID = Model.ModelID LEFT OUTER JOIN Color ON Article.ArticleID = Color.ColorArticleID AND source_Artikel.Farbe = Color.ColorName AND source_Artikel.Farbnr = Color.ColorNumber AND Color.IsDeleted = 0 WHERE EXISTS (SELECT Id, Preislistenr, Saison, Waehrung, ArtikelnrFarbnr, EAN, HEK, EmpfVK, Groesse FROM            source_Preis WHERE        (Saison = source_Artikel.Artikelsaison) AND(ArtikelnrFarbnr = source_Artikel.Artikelnr + '-0' + source_Artikel.Farbnr)) AND(Season.IsDeleted = 0) AND(Model.IsDeleted = 0) AND(Article.IsDeleted = 0) GROUP BY Article.ArticleID, Color.ColorID, source_Artikel.Kollektion, source_Artikel.Farbe, source_Artikel.Farbnr, source_Artikel.Artikelnr, source_Artikel.Artikelsaison, Season.DeliveryDateStart";

            var dbitems_source = dataModel.Database.SqlQuery<ImportColor>(sourceImportQuery).ToList();

            List<Color> Colors = new List<Color>();

            foreach (var dbitem in dbitems_source)
            {
                Color newColor = new Color();
                ClassFiller.FillDefaultValues(newColor);

                newColor.ColorID = string.Empty;
                newColor.ColorClientID = "1";
                newColor.CollectionName = dbitem.Kollektion;
                newColor.ColorArticleID = dbitem.ArticleID;
                newColor.ColorName = dbitem.Farbe;
                newColor.ColorNumber = dbitem.Farbnr;

                newColor.DeliveryDateStart = dbitem.DeliveryDateStart;

                newColor.DeliveryDateEnd = newColor.DeliveryDateStart.AddMonths(10);
                newColor.IsDeleted = false;
                newColor.SyncDateTime = DateTime.UtcNow;
                newColor.ColorNumberSearchText = dbitem.Artikelnr + dbitem.Farbnr.TrimStart('0');
                newColor.ColorNumberText = dbitem.Artikelnr + "-" + dbitem.Farbnr.TrimStart('0');

                newColor.ColorImage = dbitem.Artikelnr + "-" + dbitem.Farbnr.TrimStart('0') + ".jpg";

                newColor.ColorText01 = dbitem.GroesseVon != dbitem.GroesseBis ? dbitem.GroesseVon + "-" + dbitem.GroesseBis : dbitem.GroesseVon;

                if (dbitem.ColorID != null)
                {
                    newColor.ColorID = dbitem.ColorID;
                }

                Colors.Add(newColor);
            }

            DatabaseModelHelper.ImportData<Color>(dataModel, Colors, dataModel.Color.Where(s => s.IsDeleted == false).ToList());

        }

        void importSizerun()
        {
            string sourceTempSizerunQuery = "SELECT        Groesse, Preislistenr, ArtikelnrFarbnr, Saison, HEK, EmpfVK, EAN FROM source_Preis ORDER BY Saison, ArtikelnrFarbnr, Groesse";
            var singleSizeruns = dataModel.Database.SqlQuery<ImportTempPriceSizerun>(sourceTempSizerunQuery).ToList();

            List<List<object>> newdbitems = new List<List<object>>();

            var sizerungroup = singleSizeruns.GroupBy(s => new { Mat = s.ArtikelnrFarbnr, Pricelist = s.Saison });

            foreach (var sr in sizerungroup)
            {

                var priceGroup = sr.GroupBy(s => s.HEK);

                foreach (var pricesr in priceGroup)
                {

                    source_TempSizerunPrice newSizerun = new source_TempSizerunPrice();
                    ClassFiller.FillDefaultValues(newSizerun);

                    var firstSR = pricesr.First();
                    newSizerun.Farbnr = firstSR.ArtikelnrFarbnr.Split('-')[1].TrimStart('0');
                    newSizerun.Artikelnr = firstSR.ArtikelnrFarbnr.Split('-')[0];
                    newSizerun.Saison = firstSR.Saison;
                    newSizerun.Preislistenr = firstSR.Preislistenr;
                    newSizerun.HEK = firstSR.HEK;
                    newSizerun.EmpfVK = firstSR.EmpfVK;

                    foreach (var size in pricesr.GroupBy(s => new { Size = s.Groesse, EAN = s.EAN }))
                    {
                        if (newSizerun.Size01 == string.Empty) { newSizerun.Size01 = size.Key.Size; newSizerun.EAN01 = size.Key.EAN; }
                        else
                           if (newSizerun.Size02 == string.Empty) { newSizerun.Size02 = size.Key.Size; newSizerun.EAN02 = size.Key.EAN; }
                        else
                           if (newSizerun.Size03 == string.Empty) { newSizerun.Size03 = size.Key.Size; newSizerun.EAN03 = size.Key.EAN; }
                        else
                           if (newSizerun.Size04 == string.Empty) { newSizerun.Size04 = size.Key.Size; newSizerun.EAN04 = size.Key.EAN; }
                        else
                           if (newSizerun.Size05 == string.Empty) { newSizerun.Size05 = size.Key.Size; newSizerun.EAN05 = size.Key.EAN; }
                        else
                           if (newSizerun.Size06 == string.Empty) { newSizerun.Size06 = size.Key.Size; newSizerun.EAN06 = size.Key.EAN; }
                        else
                           if (newSizerun.Size07 == string.Empty) { newSizerun.Size07 = size.Key.Size; newSizerun.EAN07 = size.Key.EAN; }
                        else
                           if (newSizerun.Size08 == string.Empty) { newSizerun.Size08 = size.Key.Size; newSizerun.EAN08 = size.Key.EAN; }
                        else
                           if (newSizerun.Size09 == string.Empty) { newSizerun.Size09 = size.Key.Size; newSizerun.EAN09 = size.Key.EAN; }
                        else
                           if (newSizerun.Size10 == string.Empty) { newSizerun.Size10 = size.Key.Size; newSizerun.EAN10 = size.Key.EAN; }
                        else
                           if (newSizerun.Size11 == string.Empty) { newSizerun.Size11 = size.Key.Size; newSizerun.EAN11 = size.Key.EAN; }
                        else
                           if (newSizerun.Size12 == string.Empty) { newSizerun.Size12 = size.Key.Size; newSizerun.EAN12 = size.Key.EAN; }
                        else
                           if (newSizerun.Size13 == string.Empty) { newSizerun.Size13 = size.Key.Size; newSizerun.EAN13 = size.Key.EAN; }
                        else
                           if (newSizerun.Size14 == string.Empty) { newSizerun.Size14 = size.Key.Size; newSizerun.EAN14 = size.Key.EAN; }
                        else
                           if (newSizerun.Size15 == string.Empty) { newSizerun.Size15 = size.Key.Size; newSizerun.EAN15 = size.Key.EAN; }
                        else
                           if (newSizerun.Size16 == string.Empty) { newSizerun.Size16 = size.Key.Size; newSizerun.EAN16 = size.Key.EAN; }
                        else
                           if (newSizerun.Size17 == string.Empty) { newSizerun.Size17 = size.Key.Size; newSizerun.EAN17 = size.Key.EAN; }
                        else
                           if (newSizerun.Size18 == string.Empty) { newSizerun.Size18 = size.Key.Size; newSizerun.EAN18 = size.Key.EAN; }
                        else
                           if (newSizerun.Size19 == string.Empty) { newSizerun.Size19 = size.Key.Size; newSizerun.EAN19 = size.Key.EAN; }
                        else
                           if (newSizerun.Size20 == string.Empty) { newSizerun.Size20 = size.Key.Size; newSizerun.EAN20 = size.Key.EAN; }
                        else
                           if (newSizerun.Size21 == string.Empty) { newSizerun.Size21 = size.Key.Size; newSizerun.EAN21 = size.Key.EAN; }
                        else
                           if (newSizerun.Size22 == string.Empty) { newSizerun.Size22 = size.Key.Size; newSizerun.EAN22 = size.Key.EAN; }
                        else
                           if (newSizerun.Size23 == string.Empty) { newSizerun.Size23 = size.Key.Size; newSizerun.EAN23 = size.Key.EAN; }
                        else
                           if (newSizerun.Size24 == string.Empty) { newSizerun.Size24 = size.Key.Size; newSizerun.EAN24 = size.Key.EAN; }
                        else
                           if (newSizerun.Size25 == string.Empty) { newSizerun.Size25 = size.Key.Size; newSizerun.EAN25 = size.Key.EAN; }
                        else
                           if (newSizerun.Size26 == string.Empty) { newSizerun.Size26 = size.Key.Size; newSizerun.EAN26 = size.Key.EAN; }
                        else
                           if (newSizerun.Size27 == string.Empty) { newSizerun.Size27 = size.Key.Size; newSizerun.EAN27 = size.Key.EAN; }
                        else
                           if (newSizerun.Size28 == string.Empty) { newSizerun.Size28 = size.Key.Size; newSizerun.EAN28 = size.Key.EAN; }
                        else
                           if (newSizerun.Size29 == string.Empty) { newSizerun.Size29 = size.Key.Size; newSizerun.EAN29 = size.Key.EAN; }
                        else
                           if (newSizerun.Size30 == string.Empty) { newSizerun.Size30 = size.Key.Size; newSizerun.EAN30 = size.Key.EAN; }
                    }

                    newdbitems.Add(ClassToListObjectItem.ToList(newSizerun));

                }
            }

            dataModel.Database.ExecuteSqlCommand("Delete FROM source_TempSizerunPrice");

            SqlBulkInsertHelper.InsertBigData<source_TempSizerunPrice>(newdbitems);
            newdbitems.Clear();

            string sourceImportQuery = "SELECT DISTINCT source_TempSizerunPrice.Artikelnr, source_TempSizerunPrice.Farbnr, source_TempSizerunPrice.Saison, source_TempSizerunPrice.Size01, source_TempSizerunPrice.Size02, source_TempSizerunPrice.Size03, source_TempSizerunPrice.Size04, source_TempSizerunPrice.Size05, source_TempSizerunPrice.Size06, source_TempSizerunPrice.Size07, source_TempSizerunPrice.Size08, source_TempSizerunPrice.Size09, source_TempSizerunPrice.Size10, source_TempSizerunPrice.Size11, source_TempSizerunPrice.Size12, source_TempSizerunPrice.Size13, source_TempSizerunPrice.Size14, source_TempSizerunPrice.Size15, source_TempSizerunPrice.Size16, source_TempSizerunPrice.Size17, source_TempSizerunPrice.Size18, source_TempSizerunPrice.Size19, source_TempSizerunPrice.Size20, source_TempSizerunPrice.Size21,  source_TempSizerunPrice.Size22, source_TempSizerunPrice.Size23, source_TempSizerunPrice.Size24, source_TempSizerunPrice.Size25, source_TempSizerunPrice.Size26, source_TempSizerunPrice.Size27,  source_TempSizerunPrice.Size28, source_TempSizerunPrice.Size29, source_TempSizerunPrice.Size30, source_TempSizerunPrice.HEK, source_TempSizerunPrice.EmpfVK,  source_TempSizerunPrice.Preislistenr, Color.ColorID, Sizerun.SizerunID, source_TempSizerunPrice.EAN01, source_TempSizerunPrice.EAN02, source_TempSizerunPrice.EAN03,  source_TempSizerunPrice.EAN04, source_TempSizerunPrice.EAN05, source_TempSizerunPrice.EAN06, source_TempSizerunPrice.EAN07, source_TempSizerunPrice.EAN08, source_TempSizerunPrice.EAN09,  source_TempSizerunPrice.EAN10, source_TempSizerunPrice.EAN11, source_TempSizerunPrice.EAN12, source_TempSizerunPrice.EAN13, source_TempSizerunPrice.EAN14, source_TempSizerunPrice.EAN15,  source_TempSizerunPrice.EAN16, source_TempSizerunPrice.EAN17, source_TempSizerunPrice.EAN18, source_TempSizerunPrice.EAN19, source_TempSizerunPrice.EAN20, source_TempSizerunPrice.EAN21,  source_TempSizerunPrice.EAN22, source_TempSizerunPrice.EAN23, source_TempSizerunPrice.EAN24, source_TempSizerunPrice.EAN25, source_TempSizerunPrice.EAN26, source_TempSizerunPrice.EAN27,  source_TempSizerunPrice.EAN28, source_TempSizerunPrice.EAN29, source_TempSizerunPrice.EAN30 FROM            Model INNER JOIN Season ON Model.ModelSeasonID = Season.SeasonID INNER JOIN Article ON Model.ModelID = Article.ModelID INNER JOIN Color ON Article.ArticleID = Color.ColorArticleID INNER JOIN source_TempSizerunPrice ON Season.SeasonName = source_TempSizerunPrice.Saison AND Article.ArticleNumber = source_TempSizerunPrice.Artikelnr AND Color.ColorNumber = source_TempSizerunPrice.Farbnr LEFT OUTER JOIN Sizerun ON Color.ColorID = Sizerun.SizerunColorID AND Sizerun.IsDeleted = 0 AND source_TempSizerunPrice.Size01 = Sizerun.Size01 AND source_TempSizerunPrice.Size02 = Sizerun.Size02 AND source_TempSizerunPrice.Size03 = Sizerun.Size03 AND source_TempSizerunPrice.Size04 = Sizerun.Size04 AND source_TempSizerunPrice.Size05 = Sizerun.Size05 AND source_TempSizerunPrice.Size06 = Sizerun.Size06 AND source_TempSizerunPrice.Size07 = Sizerun.Size07 AND source_TempSizerunPrice.Size08 = Sizerun.Size08 AND source_TempSizerunPrice.Size09 = Sizerun.Size09 AND source_TempSizerunPrice.Size10 = Sizerun.Size10 AND source_TempSizerunPrice.Size11 = Sizerun.Size11 AND source_TempSizerunPrice.Size12 = Sizerun.Size12 AND source_TempSizerunPrice.Size13 = Sizerun.Size13 AND source_TempSizerunPrice.Size14 = Sizerun.Size14 AND source_TempSizerunPrice.Size15 = Sizerun.Size15 AND source_TempSizerunPrice.Size16 = Sizerun.Size16 AND source_TempSizerunPrice.Size17 = Sizerun.Size17 AND source_TempSizerunPrice.Size18 = Sizerun.Size18 AND source_TempSizerunPrice.Size19 = Sizerun.Size19 AND source_TempSizerunPrice.Size20 = Sizerun.Size20 AND source_TempSizerunPrice.Size21 = Sizerun.Size21 AND source_TempSizerunPrice.Size22 = Sizerun.Size22 AND source_TempSizerunPrice.Size23 = Sizerun.Size23 AND source_TempSizerunPrice.Size24 = Sizerun.Size24 AND source_TempSizerunPrice.Size25 = Sizerun.Size25 AND source_TempSizerunPrice.Size26 = Sizerun.Size26 AND source_TempSizerunPrice.Size27 = Sizerun.Size27 AND source_TempSizerunPrice.Size28 = Sizerun.Size28 AND source_TempSizerunPrice.Size29 = Sizerun.Size29 AND source_TempSizerunPrice.Size30 = Sizerun.Size30 WHERE(Color.IsDeleted = 0) AND(Article.IsDeleted = 0) AND(Season.IsDeleted = 0) AND(Model.IsDeleted = 0) AND(source_TempSizerunPrice.Preislistenr = N'1') ORDER BY Sizerun.SizerunID";

            var dbitems_source = dataModel.Database.SqlQuery<ImportSizerun>(sourceImportQuery).ToList();

            List<Sizerun> Sizeruns = new List<Sizerun>();

            foreach (var dbitem in dbitems_source)
            {
                Sizerun newSizerun = new Sizerun();
                ClassFiller.FillDefaultValues(newSizerun);
                ClassFiller.PasteData(dbitem, newSizerun);

                newSizerun.SizerunID = string.Empty;
                newSizerun.SizerunClientID = "1";
                newSizerun.SizerunName = getSizerunName(newSizerun);
                newSizerun.SizerunNumber = string.Empty;
                newSizerun.IsDeleted = false;
                newSizerun.SyncDateTime = DateTime.UtcNow;

                newSizerun.SizerunColorID = dbitem.ColorID;


                if (dbitem.SizerunID != null)
                {
                    newSizerun.SizerunID = dbitem.SizerunID;
                }

                Sizeruns.Add(newSizerun);
            }

            DatabaseModelHelper.ImportData<Sizerun>(dataModel, Sizeruns, dataModel.Sizerun.Where(s => s.IsDeleted == false).ToList());
        }

        string getSizerunName(Sizerun run)
        {
            string firstSize = run.Size01;

            if (firstSize == string.Empty) firstSize = run.Size02;
            if (firstSize == string.Empty) firstSize = run.Size03;
            if (firstSize == string.Empty) firstSize = run.Size04;
            if (firstSize == string.Empty) firstSize = run.Size05;
            if (firstSize == string.Empty) firstSize = run.Size06;
            if (firstSize == string.Empty) firstSize = run.Size07;
            if (firstSize == string.Empty) firstSize = run.Size08;
            if (firstSize == string.Empty) firstSize = run.Size09;
            if (firstSize == string.Empty) firstSize = run.Size10;
            if (firstSize == string.Empty) firstSize = run.Size11;
            if (firstSize == string.Empty) firstSize = run.Size12;
            if (firstSize == string.Empty) firstSize = run.Size13;
            if (firstSize == string.Empty) firstSize = run.Size14;
            if (firstSize == string.Empty) firstSize = run.Size15;
            if (firstSize == string.Empty) firstSize = run.Size16;
            if (firstSize == string.Empty) firstSize = run.Size17;
            if (firstSize == string.Empty) firstSize = run.Size18;
            if (firstSize == string.Empty) firstSize = run.Size19;
            if (firstSize == string.Empty) firstSize = run.Size20;
            if (firstSize == string.Empty) firstSize = run.Size21;
            if (firstSize == string.Empty) firstSize = run.Size22;
            if (firstSize == string.Empty) firstSize = run.Size23;
            if (firstSize == string.Empty) firstSize = run.Size24;
            if (firstSize == string.Empty) firstSize = run.Size25;
            if (firstSize == string.Empty) firstSize = run.Size26;
            if (firstSize == string.Empty) firstSize = run.Size27;
            if (firstSize == string.Empty) firstSize = run.Size28;
            if (firstSize == string.Empty) firstSize = run.Size29;
            if (firstSize == string.Empty) firstSize = run.Size30;

            string lastSize = run.Size30;

            if (lastSize == string.Empty) lastSize = run.Size29;
            if (lastSize == string.Empty) lastSize = run.Size28;
            if (lastSize == string.Empty) lastSize = run.Size27;
            if (lastSize == string.Empty) lastSize = run.Size26;
            if (lastSize == string.Empty) lastSize = run.Size25;
            if (lastSize == string.Empty) lastSize = run.Size24;
            if (lastSize == string.Empty) lastSize = run.Size23;
            if (lastSize == string.Empty) lastSize = run.Size22;
            if (lastSize == string.Empty) lastSize = run.Size21;
            if (lastSize == string.Empty) lastSize = run.Size20;
            if (lastSize == string.Empty) lastSize = run.Size19;
            if (lastSize == string.Empty) lastSize = run.Size18;
            if (lastSize == string.Empty) lastSize = run.Size17;
            if (lastSize == string.Empty) lastSize = run.Size16;
            if (lastSize == string.Empty) lastSize = run.Size15;
            if (lastSize == string.Empty) lastSize = run.Size14;
            if (lastSize == string.Empty) lastSize = run.Size13;
            if (lastSize == string.Empty) lastSize = run.Size12;
            if (lastSize == string.Empty) lastSize = run.Size11;
            if (lastSize == string.Empty) lastSize = run.Size10;
            if (lastSize == string.Empty) lastSize = run.Size09;
            if (lastSize == string.Empty) lastSize = run.Size08;
            if (lastSize == string.Empty) lastSize = run.Size07;
            if (lastSize == string.Empty) lastSize = run.Size06;
            if (lastSize == string.Empty) lastSize = run.Size05;
            if (lastSize == string.Empty) lastSize = run.Size04;
            if (lastSize == string.Empty) lastSize = run.Size03;
            if (lastSize == string.Empty) lastSize = run.Size02;
            if (lastSize == string.Empty) lastSize = run.Size01;

            if (firstSize != lastSize)
            {
                return firstSize + " - " + lastSize;
            }
            else
                return firstSize;
        }

        void importAssortment()
        {


            List<Assortment> Assortments = new List<Assortment>();

            string sourceImportQuery = "SELECT Sizerun.SizerunID, Sizerun.SizerunName, Assortment.AssortmentID, Sizerun.IsDeleted FROM Sizerun LEFT OUTER JOIN Assortment ON Sizerun.SizerunID = Assortment.AssortmentSizerunID AND Sizerun.SizerunName = Assortment.AssortmentName AND Assortment.IsDeleted = 0 WHERE(Sizerun.IsDeleted = 0)";

            var dbitems_source = dataModel.Database.SqlQuery<ImportAssortment>(sourceImportQuery).ToList();



            foreach (var dbitem in dbitems_source)
            {
                Assortment newAssortment = new Assortment();
                newAssortment.AssortmentClientID = "1";
                newAssortment.AssortmentID = string.Empty;
                newAssortment.AssortmentName = dbitem.SizerunName;
                newAssortment.AssortmentSizerunID = dbitem.SizerunID;
                newAssortment.IsDeleted = false;
                newAssortment.IsFreeDisposition = true;
                newAssortment.SizerunRangeText = dbitem.SizerunName;
                newAssortment.SyncDateTime = DateTime.Now;

                if (dbitem.AssortmentID != null)
                {
                    newAssortment.AssortmentID = dbitem.AssortmentID;
                }

                Assortments.Add(newAssortment);
            }

            DatabaseModelHelper.ImportData<Assortment>(dataModel, Assortments, dataModel.Assortment.Where(s => s.IsDeleted == false).ToList());

        }

        void importImages()
        {
            BlobstorageFileHandler imageBlobHandler = new BlobstorageFileHandler("myconvenopicstorage", "astJJYrwS/JGBjpVTli5aHKUM0aL+MrI3z9XzDW99F+3BbR3ayXMPKPWGLDx5vJpFxWTEv/aWkgRsnVUCHolog==", "livingkbpics");

            var dbitems_source = imageBlobHandler.FileListWithDetails();

            List<ProductImage> images = new List<ProductImage>();

            foreach (var dbitem in dbitems_source)
            {
                ProductImage newImage = new ProductImage();
                newImage.ProductImageID = dbitem.Uri.ToString();
                newImage.ImageDateTime = dbitem.Properties.LastModifiedUtc;
                newImage.ImageName = dbitem.Name;
                newImage.IsDeleted = false;
                newImage.SyncDateTime = DateTime.Now;

                images.Add(newImage);
            }

            var newImages = (from i in images
                             join c in dataModel.Color
                             on i.ImageName equals c.ColorImage
                             select i.ColorID = c.ColorID).ToList();

            DatabaseModelHelper.ImportData<ProductImage>(dataModel, images, dataModel.ProductImage.Where(s => s.IsDeleted == false).ToList());
        }

        public string ImportPrice()
        {

            List<Price> Prices = new List<Price>();

            string sourceImportQuery = "SELECT DISTINCT Pricelist.PricelistID, Color.ColorID, Sizerun.SizerunID, source_TempSizerunPrice.HEK, source_TempSizerunPrice.EmpfVK, Price.PriceID, Color.IsDeleted FROM source_TempSizerunPrice INNER JOIN Article INNER JOIN Color ON Article.ArticleID = Color.ColorArticleID INNER JOIN Sizerun ON Color.ColorID = Sizerun.SizerunColorID ON source_TempSizerunPrice.Farbnr = Color.ColorNumber AND source_TempSizerunPrice.Artikelnr = Article.ArticleNumber AND source_TempSizerunPrice.Size01 = Sizerun.Size01 AND source_TempSizerunPrice.Size02 = Sizerun.Size02 AND source_TempSizerunPrice.Size03 = Sizerun.Size03 AND source_TempSizerunPrice.Size04 = Sizerun.Size04 AND source_TempSizerunPrice.Size05 = Sizerun.Size05 AND source_TempSizerunPrice.Size06 = Sizerun.Size06 AND source_TempSizerunPrice.Size07 = Sizerun.Size07 AND source_TempSizerunPrice.Size08 = Sizerun.Size08 AND source_TempSizerunPrice.Size09 = Sizerun.Size09 AND source_TempSizerunPrice.Size10 = Sizerun.Size10 AND source_TempSizerunPrice.Size11 = Sizerun.Size11 AND source_TempSizerunPrice.Size12 = Sizerun.Size12 AND source_TempSizerunPrice.Size13 = Sizerun.Size13 AND source_TempSizerunPrice.Size14 = Sizerun.Size14 AND source_TempSizerunPrice.Size15 = Sizerun.Size15 AND source_TempSizerunPrice.Size16 = Sizerun.Size16 AND source_TempSizerunPrice.Size17 = Sizerun.Size17 AND source_TempSizerunPrice.Size18 = Sizerun.Size18 AND source_TempSizerunPrice.Size19 = Sizerun.Size19 AND source_TempSizerunPrice.Size20 = Sizerun.Size20 AND source_TempSizerunPrice.Size21 = Sizerun.Size21 AND source_TempSizerunPrice.Size22 = Sizerun.Size22 AND source_TempSizerunPrice.Size23 = Sizerun.Size23 AND source_TempSizerunPrice.Size24 = Sizerun.Size24 AND source_TempSizerunPrice.Size25 = Sizerun.Size25 AND source_TempSizerunPrice.Size26 = Sizerun.Size26 AND source_TempSizerunPrice.Size27 = Sizerun.Size27 AND source_TempSizerunPrice.Size28 = Sizerun.Size28 AND source_TempSizerunPrice.Size29 = Sizerun.Size29 AND source_TempSizerunPrice.Size30 = Sizerun.Size30 INNER JOIN Pricelist ON source_TempSizerunPrice.Preislistenr = Pricelist.PricelistNumber INNER JOIN Model ON Article.ModelID = Model.ModelID INNER JOIN Season ON Model.ModelSeasonID = Season.SeasonID AND source_TempSizerunPrice.Saison = Season.SeasonName LEFT OUTER JOIN Price ON Color.ColorID = Price.PriceColorID AND Sizerun.SizerunID = Price.PriceSizerunID AND Pricelist.PricelistID = Price.PricePricelistID AND Price.IsDeleted = 0 WHERE(Color.IsDeleted = 0) AND(Article.IsDeleted = 0) AND(Sizerun.IsDeleted = 0) AND(Pricelist.IsDeleted = 0) AND(Season.IsDeleted = 0) AND(Model.IsDeleted = 0) ORDER BY Price.PriceID";

            var dbitems_source = dataModel.Database.SqlQuery<ImportPrice>(sourceImportQuery);

            dataModel.Database.ExecuteSqlCommand("Delete FROM Price_Compare");

            foreach (var p in dbitems_source)
            {
                Price newPrice = new Price();
                ClassFiller.FillDefaultValues(newPrice);

                newPrice.PriceID = string.Empty;
                newPrice.PriceClientID = "1";
                newPrice.PricePricelistID = p.PricelistID;
                newPrice.IsDeleted = false;
                newPrice.PriceSizerunID = p.SizerunID;
                newPrice.SyncDateTime = DateTime.UtcNow;
                newPrice.SalesPrice = Convert.ToDecimal(p.EmpfVK);
                newPrice.BuyingPrice = Convert.ToDecimal(p.HEK);
                newPrice.PriceArticleID = string.Empty;
                newPrice.PriceColorID = p.ColorID;

                if (p.PriceID != null)
                {
                    newPrice.PriceID = p.PriceID;
                }


                Prices.Add(newPrice);
            }

            DatabaseModelHelper.ImportData<Price>(dataModel, Prices, dataModel.Price.Where(s => s.IsDeleted == false).ToList());

            return "OK";
        }
        #endregion

        #region Customer
        public string ImportCustomer()
        {
            importAgent();

            importCustomer();

            importDeleiveryAddresses();

            importInvoiceAddresses();

            importAssociations();

            importAssociationMembers();

            importContactPerson();

            UpdateGeoData();

            return "OK";
        }

        void importAgent()
        {
            string sourceImportCustomerQuery = "SELECT        source_Vertreter.VertreterNr, source_Vertreter.Name1, source_Vertreter.Name2, source_Vertreter.Name3, source_Vertreter.Strasse, source_Vertreter.Land, source_Vertreter.Plz, source_Vertreter.Ort, source_Vertreter.Tel, source_Vertreter.Fax, source_Vertreter.Mobil, source_Vertreter.Email, source_Vertreter.Kundennr, source_Vertreter.UID, source_Vertreter.Steuernr, Agent.AgentID FROM            source_Vertreter LEFT OUTER JOIN Agent ON source_Vertreter.VertreterNr = Agent.AgentNumber";

            var customers_source = dataModel.Database.SqlQuery<ImportAgent>(sourceImportCustomerQuery).ToList();

            List<Agent> Agents = new List<Agent>();

            foreach (var k in customers_source)
            {

                Agent newAgent = new Agent();
                ClassFiller.FillDefaultValues(newAgent);

                newAgent.AgentCity = k.Ort;
                newAgent.AgentClientID = "1";
                newAgent.AgentCountryCode = k.Land;
                newAgent.AgentCountryName = k.Land;
                newAgent.AgentEMail = k.Email;
                newAgent.AgentID = string.Empty;
                newAgent.AgentMobile = k.Mobil;
                newAgent.AgentName1 = k.Name1;
                newAgent.AgentName2 = k.Name2;
                newAgent.AgentNumber = k.VertreterNr;
                newAgent.AgentPhone = k.Tel;
                newAgent.AgentStreet = k.Strasse;
                newAgent.AgentZIP = k.Plz;
                newAgent.IsDeleted = false;
                newAgent.SyncDateTime = DateTime.UtcNow;


                if (k.AgentID != null)
                {
                    newAgent.AgentID = k.AgentID;
                }

                Agents.Add(newAgent);
            }

            DatabaseModelHelper.ImportData<Agent>(dataModel, Agents, dataModel.Agent.Where(s => s.IsDeleted == false).ToList());
        }

        void importCustomer()
        {
            string sourceImportCustomerQuery = "SELECT        source_Kunde.Kundennr, source_Kunde.Name1, source_Kunde.Name2, source_Kunde.Name3, source_Kunde.Strasse, source_Kunde.Plz, source_Kunde.Ort, source_Kunde.Land, source_Kunde.Zahlungsbed, source_Kunde.Zahlungsart, source_Kunde.Tel, source_Kunde.Email, source_Kunde.Fax, source_Kunde.homepage, source_Kunde.Bonitaetsstufe, source_Kunde.Kundentyp, source_Kunde.Vertrerternr, source_Kunde.UID_Nummer, source_Kunde.Preislistennr, source_Kunde.Mwst_KZ, source_Kunde.Steuerschluessel, source_Kunde.Rechnungsempfaenger, source_Kunde.Bonitaet, Customer.CustomerID, Pricelist.PricelistID, Agent.AgentID, SUM(source_Rabatt.Wert) AS GrundRabatt, source_Kunde.Forderungen, Agent.AgentName1 FROM            source_Kunde INNER JOIN Pricelist ON source_Kunde.Preislistennr = Pricelist.PricelistNumber INNER JOIN Agent ON source_Kunde.Vertrerternr = Agent.AgentNumber LEFT OUTER JOIN source_Rabatt ON source_Kunde.Kundennr = source_Rabatt.Kundennr LEFT OUTER JOIN Customer ON source_Kunde.Kundennr = Customer.CustomerNumber GROUP BY source_Kunde.Kundennr, source_Kunde.Name1, source_Kunde.Name2, source_Kunde.Name3, source_Kunde.Strasse, source_Kunde.Plz, source_Kunde.Ort, source_Kunde.Land, source_Kunde.Zahlungsbed, source_Kunde.Zahlungsart, source_Kunde.Tel, source_Kunde.Email, source_Kunde.Fax, source_Kunde.homepage, source_Kunde.Bonitaetsstufe, source_Kunde.Kundentyp, source_Kunde.Vertrerternr, source_Kunde.Preislistennr, source_Kunde.UID_Nummer, source_Kunde.Mwst_KZ, source_Kunde.Steuerschluessel, source_Kunde.Rechnungsempfaenger, source_Kunde.Bonitaet, Customer.CustomerID, Pricelist.PricelistID, Agent.AgentID, source_Kunde.Forderungen, Agent.AgentName1";
           
            var customers_source = dataModel.Database.SqlQuery<ImportCustomer>(sourceImportCustomerQuery).ToList();

            List<Customer> Customers = new List<Customer>();

            foreach (var k in customers_source)
            {

                Customer newcustomer = new Customer();
                ClassFiller.FillDefaultValues(newcustomer);

                newcustomer.AgentName1 = k.AgentName1;
                newcustomer.AgentNumber = k.Vertrerternr;
                newcustomer.AssociationMemberNumber = string.Empty;
                newcustomer.Currency = string.Empty;
                newcustomer.CustomerAgentID = k.AgentID;
                newcustomer.CustomerCity = k.Ort;
                newcustomer.CustomerCountryCode = k.Land;
                newcustomer.CustomerCountryName = k.Land;
                newcustomer.CustomerDiscount = k.GrundRabatt.HasValue ? k.GrundRabatt.Value / 100.0 : 0;
                newcustomer.CustomerEmail = k.Email;
                newcustomer.CustomerFax = k.Fax;

                newcustomer.Customergroup = k.Kundentyp;
                newcustomer.CustomerID = string.Empty;
                newcustomer.CustomerMobile = string.Empty;
                newcustomer.CustomerName1 = k.Name3;
                newcustomer.CustomerName2 = k.Name1;
                newcustomer.CustomerName3 = k.Name2;
                newcustomer.CustomerNumber = k.Kundennr;
                newcustomer.CustomerPhone = k.Tel;
                newcustomer.CustomerFax = k.Fax;
                newcustomer.CustomerPricelistID = k.PricelistID;
                newcustomer.CustomerStreet = k.Strasse;
                newcustomer.CustomerTaxID = k.UID_Nummer;
                newcustomer.CustomerValuta = 0;
                newcustomer.CustomerZIP = k.Plz;
                newcustomer.IsDeleted = false;
                newcustomer.CustomerClientID = "1";
                newcustomer.PaymentTermNumber = k.Zahlungsbed;
                newcustomer.PaymentTermText = k.Zahlungsart;
                newcustomer.PricelistName = k.Preislistennr;
                newcustomer.PricelistNumber = k.Preislistennr;
                newcustomer.SyncDateTime = DateTime.UtcNow;
                newcustomer.IsTestData = false;
                newcustomer.CustomerHeader01 = "Informationen";
                newcustomer.CustomerHeader02 = "Offene Forderungen";

                newcustomer.CustomerText01 = k.Bonitaet;
                newcustomer.CustomerText02 = k.Forderungen.HasValue ? k.Forderungen.Value.ToString("0.00").Replace(".", ",") + " EUR" : string.Empty;

                newcustomer.OpenReceivables = k.Forderungen.HasValue ? k.Forderungen.Value : 0;


                if (k.CustomerID != null)
                {
                    newcustomer.CustomerID = k.CustomerID;
                }

                Customers.Add(newcustomer);
            }

            DatabaseModelHelper.ImportData<Customer>(dataModel, Customers, dataModel.Customer.Where(s => s.IsDeleted == false).ToList());
        }

        void importDeleiveryAddresses()
        {
            string sourceImportDeleiveryAddressQuery = "SELECT        source_Lieferadresse.Kundennr, source_Lieferadresse.Name1, source_Lieferadresse.Name2, source_Lieferadresse.Name3, source_Lieferadresse.Strasse, source_Lieferadresse.Plz, source_Lieferadresse.Ort, source_Lieferadresse.Land, source_Lieferadresse.Telefon, source_Lieferadresse.Email, source_Lieferadresse.Fax, source_Lieferadresse.Bemerkung, Customer.CustomerID, source_Lieferadresse.Lieferort as Warenempfaengernr, DeliveryAddress.DeliveryAddressID FROM            source_Lieferadresse INNER JOIN Customer ON source_Lieferadresse.Kundennr = Customer.CustomerNumber LEFT OUTER JOIN DeliveryAddress ON Customer.CustomerID = DeliveryAddress.DeliveryAddressCustomerID AND source_Lieferadresse.Lieferort = DeliveryAddress.DeliveryNumber";
         
            var deliveryAddresses_source = dataModel.Database.SqlQuery<ImportDeliveryAddress>(sourceImportDeleiveryAddressQuery);

            List<DeliveryAddress> DeliveryAddresses = new List<DeliveryAddress>();

            foreach (var d in deliveryAddresses_source)
            {
                DeliveryAddress newdeliveryAddress = new DeliveryAddress();
                ClassFiller.FillDefaultValues(newdeliveryAddress);

                newdeliveryAddress.DeliveryAddressCity = d.Ort;
                newdeliveryAddress.DeliveryAddressClientID = "1";
                newdeliveryAddress.DeliveryAddressCountryCode = d.Land;
                newdeliveryAddress.DeliveryAddressCountryName = d.Land;
                newdeliveryAddress.DeliveryAddressCustomerID = d.CustomerID;
                newdeliveryAddress.DeliveryAddressEMail = string.Empty;
                newdeliveryAddress.DeliveryAddressFax = d.Fax;
                newdeliveryAddress.DeliveryAddressID = string.Empty;
                newdeliveryAddress.DeliveryAddressName1 = d.Name3;
                newdeliveryAddress.DeliveryAddressName2 = d.Name1;
                newdeliveryAddress.DeliveryAddressName3 = d.Name2;
                newdeliveryAddress.DeliveryAddressPhone = d.Telefon;
                newdeliveryAddress.DeliveryAddressStreet = d.Strasse;
                newdeliveryAddress.DeliveryAddressZIP = d.Plz;
                newdeliveryAddress.DeliveryNumber = d.Warenempfaengernr;
                newdeliveryAddress.IsDeleted = false;
                newdeliveryAddress.SyncDateTime = DateTime.UtcNow;
                newdeliveryAddress.IsTestData = false;

                if (d.DeliveryAddressID != null)
                {
                    newdeliveryAddress.DeliveryAddressID = d.DeliveryAddressID;
                }

                DeliveryAddresses.Add(newdeliveryAddress);
            }

            DatabaseModelHelper.ImportData<DeliveryAddress>(dataModel, DeliveryAddresses, dataModel.DeliveryAddress.Where(s => s.IsDeleted == false).ToList());
        }

        void importInvoiceAddresses()
        {
            string sourceImportInvoiceAddressQuery = "SELECT        source_Kunde.Kundennr, source_Kunde.Name1, source_Kunde.Name2, source_Kunde.Name3, source_Kunde.Strasse, source_Kunde.Plz, source_Kunde.Ort, source_Kunde.Land, source_Kunde.Zahlungsbed, source_Kunde.Zahlungsart, source_Kunde.Tel, source_Kunde.Email, source_Kunde.Fax, source_Kunde.homepage, source_Kunde.Bonitaetsstufe, source_Kunde.Kundentyp, source_Kunde.Vertrerternr, source_Kunde.UID_Nummer, source_Kunde.Preislistennr, source_Kunde.Mwst_KZ, source_Kunde.Steuerschluessel, source_Kunde.Rechnungsempfaenger, source_Kunde.Bonitaet, Customer.CustomerID, InvoiceAddress.InvoiceAddressID FROM            source_Kunde INNER JOIN Customer ON source_Kunde.Kundennr = Customer.CustomerNumber LEFT OUTER JOIN InvoiceAddress ON Customer.CustomerID = InvoiceAddress.InvoiceAddressCustomerID AND source_Kunde.Rechnungsempfaenger = InvoiceAddress.InvoiceNumber WHERE(source_Kunde.Rechnungsempfaenger <> N'0') UNION ALL SELECT source_Kunde_1.Kundennr, source_Kunde_1.Name1, source_Kunde_1.Name2, source_Kunde_1.Name3, source_Kunde_1.Strasse, source_Kunde_1.Plz, source_Kunde_1.Ort, source_Kunde_1.Land, source_Kunde_1.Zahlungsbed, source_Kunde_1.Zahlungsart, source_Kunde_1.Tel, source_Kunde_1.Email, source_Kunde_1.Fax, source_Kunde_1.homepage, source_Kunde_1.Bonitaetsstufe, source_Kunde_1.Kundentyp, source_Kunde_1.Vertrerternr, source_Kunde_1.UID_Nummer, source_Kunde_1.Preislistennr, source_Kunde_1.Mwst_KZ, source_Kunde_1.Steuerschluessel, source_Kunde_1.Kundennr AS Rechnungsempfaenger, source_Kunde_1.Bonitaet, Customer_1.CustomerID, InvoiceAddress_1.InvoiceAddressID FROM            source_Kunde AS source_Kunde_1 INNER JOIN Customer AS Customer_1 ON source_Kunde_1.Kundennr = Customer_1.CustomerNumber LEFT OUTER JOIN InvoiceAddress AS InvoiceAddress_1 ON Customer_1.CustomerID = InvoiceAddress_1.InvoiceAddressCustomerID AND source_Kunde_1.Kundennr = InvoiceAddress_1.InvoiceNumber WHERE(source_Kunde_1.Rechnungsempfaenger = N'0')";

            var deliveryAddresses_source = dataModel.Database.SqlQuery<ImportInvoiceAddress>(sourceImportInvoiceAddressQuery);

            List<InvoiceAddress> InvoiceAddresses = new List<InvoiceAddress>();

            foreach (var d in deliveryAddresses_source)
            {
                InvoiceAddress newInvoiceAddress = new InvoiceAddress();
                ClassFiller.FillDefaultValues(newInvoiceAddress);

                newInvoiceAddress.InvoiceAddressCity = d.Ort;
                newInvoiceAddress.InvoiceAddressClientID = "1";
                newInvoiceAddress.InvoiceAddressCountryCode = d.Land;
                newInvoiceAddress.InvoiceAddressCountryName = d.Land;
                newInvoiceAddress.InvoiceAddressCustomerID = d.CustomerID;
                newInvoiceAddress.InvoiceAddressEMail = d.Email;
                newInvoiceAddress.InvoiceAddressFax = d.Fax;
                newInvoiceAddress.InvoiceAddressID = string.Empty;
                newInvoiceAddress.InvoiceAddressName1 = d.Name3;
                newInvoiceAddress.InvoiceAddressName2 = d.Name1;
                newInvoiceAddress.InvoiceAddressName3 = d.Name2;
                newInvoiceAddress.InvoiceAddressPhone = d.Tel;
                newInvoiceAddress.InvoiceAddressStreet = d.Strasse;
                newInvoiceAddress.InvoiceAddressZIP = d.Plz;
                newInvoiceAddress.IsDeleted = false;
                newInvoiceAddress.InvoiceNumber = d.Rechnungsempfaenger;
                newInvoiceAddress.SyncDateTime = DateTime.UtcNow;
                newInvoiceAddress.IsTestData = false;

                if (d.InvoiceAddressID != null)
                {
                    newInvoiceAddress.InvoiceAddressID = d.InvoiceAddressID;
                }

                InvoiceAddresses.Add(newInvoiceAddress);
            }

            DatabaseModelHelper.ImportData<InvoiceAddress>(dataModel, InvoiceAddresses, dataModel.InvoiceAddress.Where(s => s.IsDeleted == false).ToList());
        }

        void importAssociations()
        {
            string sourceImportAssociationQuery = "SELECT DISTINCT source_Kunde.EKV AS Name, Association.AssociationID FROM source_Kunde LEFT OUTER JOIN Association ON source_Kunde.EKV = Association.AssociationName1 WHERE(source_Kunde.EKV <> N'')";
          
            var associationMembers_source = dataModel.Database.SqlQuery<ImportAssociation>(sourceImportAssociationQuery);

            List<Association> Associations = new List<Association>();

            foreach (var aM in associationMembers_source)
            {
                Association newAssociation = new Association();
                ClassFiller.FillDefaultValues(newAssociation);

                newAssociation.AssociationID = string.Empty;
                newAssociation.AssociationName1 = aM.Name;
                newAssociation.IsDeleted = false;
                newAssociation.AssociationClientID = "1";
                newAssociation.SyncDateTime = DateTime.UtcNow;

                if (aM.AssociationID != null)
                {
                    newAssociation.AssociationID = aM.AssociationID;
                }

                Associations.Add(newAssociation);
            }


            DatabaseModelHelper.ImportData<Association>(dataModel, Associations, dataModel.Association.Where(s => s.IsDeleted == false).ToList());
        }

        void importAssociationMembers()
        {
            string sourceImportAssociationMemberQuery = "SELECT DISTINCT Customer.CustomerID, source_Kunde.EKV_KTO, Association.AssociationID, AssociationMember.AssociationMemberID FROM source_Kunde INNER JOIN Customer ON source_Kunde.Kundennr = Customer.CustomerNumber INNER JOIN Association ON source_Kunde.EKV = Association.AssociationName1 LEFT OUTER JOIN AssociationMember ON Customer.CustomerID = AssociationMember.AssociationMemberCustomerID AND Association.AssociationID = AssociationMember.AssociationMemberAssociationID AND source_Kunde.EKV_KTO = AssociationMember.AssociationMemberNumber WHERE(source_Kunde.EKV <> N'')";

            var associationMembers_source = dataModel.Database.SqlQuery<ImportAssociationMember>(sourceImportAssociationMemberQuery);

            List<AssociationMember> AssociationMembers = new List<AssociationMember>();

            foreach (var aM in associationMembers_source)
            {
                AssociationMember newAssociationMember = new AssociationMember();
                ClassFiller.FillDefaultValues(newAssociationMember);

                newAssociationMember.AssociationMemberID = string.Empty;
                newAssociationMember.AssociationMemberClientID = "1";
                newAssociationMember.AssociationMemberAssociationID = aM.AssociationID;
                newAssociationMember.AssociationMemberCustomerID = aM.CustomerID;
                newAssociationMember.AssociationMemberNumber = aM.EKV_KTO;
                newAssociationMember.IsDeleted = false;
                newAssociationMember.SyncDateTime = DateTime.UtcNow;
                newAssociationMember.IsTestData = false;

                if (aM.AssociationMemberID != null)
                {
                    newAssociationMember.AssociationMemberID = aM.AssociationMemberID;
                }

                AssociationMembers.Add(newAssociationMember);
            }

            DatabaseModelHelper.ImportData<AssociationMember>(dataModel, AssociationMembers, dataModel.AssociationMember.Where(s => s.IsDeleted == false).ToList());
        }

        void importContactPerson()
        {
            string sourceImportContactPersonQuery = "SELECT        source_Ansprechpartner.Kundennr, source_Ansprechpartner.ASP_Nr, source_Ansprechpartner.ASP_Bereich, source_Ansprechpartner.ASP_Vorname, source_Ansprechpartner.ASP_Name1, source_Ansprechpartner.ASP_Name2, source_Ansprechpartner.ASP_Telefon, source_Ansprechpartner.ASP_Mobiltelefon, source_Ansprechpartner.ASP_Fax, source_Ansprechpartner.ASP_Bemerkung, source_Ansprechpartner.ASP_Email, Customer.CustomerID, ContactPerson.ContactPersonID FROM            source_Ansprechpartner INNER JOIN Customer ON source_Ansprechpartner.Kundennr = Customer.CustomerNumber LEFT OUTER JOIN ContactPerson ON Customer.CustomerID = ContactPerson.ContactPersonCustomerID AND source_Ansprechpartner.ASP_Nr = ContactPerson.ContactPersonNumber";
          
            var contactPersons_source = dataModel.Database.SqlQuery<ImportContactPerson>(sourceImportContactPersonQuery);

            List<ContactPerson> ContactPersons = new List<ContactPerson>();

            foreach (var cp in contactPersons_source)
            {
                ContactPerson newContactPerson = new ContactPerson();
                ClassFiller.FillDefaultValues(newContactPerson);

                newContactPerson.ContactPersonClientID = "1";
                newContactPerson.ContactPersonCustomerID = cp.CustomerID;
                newContactPerson.ContactPersonEmail = cp.ASP_Email;
                newContactPerson.ContactPersonFax = cp.ASP_Fax;
                newContactPerson.ContactPersonID = Guid.NewGuid().ToString();
                newContactPerson.ContactPersonFunction = cp.ASP_Bereich;
                newContactPerson.ContactPersonName1 = cp.ASP_Vorname;
                newContactPerson.ContactPersonName2 = cp.ASP_Name1;
                newContactPerson.ContactPersonName3 = cp.ASP_Name2;
                newContactPerson.ContactPersonPhone = cp.ASP_Telefon;
                newContactPerson.ContactPersonNumber = cp.ASP_Nr;
                newContactPerson.IsDeleted = false;
                newContactPerson.SyncDateTime = DateTime.Now;

                if (cp.ContactPersonID != null)
                {
                    newContactPerson.ContactPersonID = cp.ContactPersonID;
                }

                ContactPersons.Add(newContactPerson);
            }

            DatabaseModelHelper.ImportData<ContactPerson>(dataModel, ContactPersons, dataModel.ContactPerson.Where(s => s.IsDeleted == false).ToList());

        }
        #endregion

        #region Stock
        public string ImportSourceDataStock()
        {
            //##############
            //###Bestände###
            //##############

            var files = blobhandler.FileList("Stammdaten/myconveno_bestand");
            List<List<object>> newdbitems = new List<List<object>>();

            if (files.Count > 0)
            {
                dataModel.Database.ExecuteSqlCommand("Delete FROM source_Stock");

                foreach (var f in files)
                {
                    using (StreamReader reader = new StreamReader(new MemoryStream(blobhandler.DownloadFile(f)), Encoding.Default))
                    {
                        var csv = new CsvReader(reader);
                        csv.Configuration.Delimiter = ";";
                        csv.Configuration.RegisterClassMap<BestandCSVMap>();

                        var records = csv.GetRecords<source_Stock>().ToList();

                        foreach (var a in records)
                        {
                            newdbitems.Add(ClassToListObjectItem.ToList(a));
                        }



                    }
                    SqlBulkInsertHelper.InsertBigData<source_Stock>(newdbitems);
                    newdbitems.Clear();

                    blobhandler.MoveFileToArchive(f);
                }

            }
            newdbitems.Clear();

            return "OK";
        }

        public string ImportStock()
        {

            string sourceTempStockQuery = "SELECT        Sizerun.SizerunID, source_Stock.EAN, source_Stock.Freilagerbestand, Assortment.AssortmentID, CASE WHEN EAN = Sizerun.EAN01 THEN 1 WHEN EAN = Sizerun.EAN02 THEN 2 WHEN EAN = Sizerun.EAN03 THEN 3 WHEN EAN = Sizerun.EAN04 THEN 4 WHEN EAN = Sizerun.EAN05 THEN 5 WHEN EAN = Sizerun.EAN06 THEN 6 WHEN EAN = Sizerun.EAN07 THEN 7 WHEN EAN = Sizerun.EAN08 THEN 8 WHEN EAN = Sizerun.EAN09 THEN 9 WHEN EAN = Sizerun.EAN10 THEN 10 WHEN EAN = Sizerun.EAN11 THEN 11 WHEN EAN = Sizerun.EAN12 THEN 12 WHEN EAN = Sizerun.EAN13 THEN 13 WHEN EAN = Sizerun.EAN14 THEN 14 WHEN EAN = Sizerun.EAN15 THEN 15 WHEN EAN = Sizerun.EAN16 THEN 16 WHEN EAN = Sizerun.EAN17 THEN 17 WHEN EAN = Sizerun.EAN18 THEN 18 WHEN EAN = Sizerun.EAN19 THEN 19 WHEN EAN = Sizerun.EAN20 THEN 20 WHEN EAN = Sizerun.EAN21 THEN 21 WHEN EAN = Sizerun.EAN22 THEN 22 WHEN EAN = Sizerun.EAN23 THEN 23 WHEN EAN = Sizerun.EAN24 THEN 24 WHEN EAN = Sizerun.EAN25 THEN 25 WHEN EAN = Sizerun.EAN26 THEN 26 WHEN EAN = Sizerun.EAN27 THEN 27 WHEN EAN = Sizerun.EAN28 THEN 28 WHEN EAN = Sizerun.EAN29 THEN 29 WHEN EAN = Sizerun.EAN30 THEN 30 END AS SizeIndex FROM            source_Stock INNER JOIN Sizerun ON source_Stock.EAN = Sizerun.EAN01 OR source_Stock.EAN = Sizerun.EAN02 OR source_Stock.EAN = Sizerun.EAN03 OR source_Stock.EAN = Sizerun.EAN04 OR source_Stock.EAN = Sizerun.EAN05 OR source_Stock.EAN = Sizerun.EAN06 OR source_Stock.EAN = Sizerun.EAN07 OR source_Stock.EAN = Sizerun.EAN08 OR source_Stock.EAN = Sizerun.EAN09 OR source_Stock.EAN = Sizerun.EAN10 OR source_Stock.EAN = Sizerun.EAN11 OR source_Stock.EAN = Sizerun.EAN12 OR source_Stock.EAN = Sizerun.EAN13 OR source_Stock.EAN = Sizerun.EAN14 OR source_Stock.EAN = Sizerun.EAN15 OR source_Stock.EAN = Sizerun.EAN16 OR source_Stock.EAN = Sizerun.EAN17 OR source_Stock.EAN = Sizerun.EAN18 OR source_Stock.EAN = Sizerun.EAN19 OR source_Stock.EAN = Sizerun.EAN20 OR source_Stock.EAN = Sizerun.EAN21 OR source_Stock.EAN = Sizerun.EAN22 OR source_Stock.EAN = Sizerun.EAN23 OR source_Stock.EAN = Sizerun.EAN24 OR source_Stock.EAN = Sizerun.EAN25 OR source_Stock.EAN = Sizerun.EAN26 OR source_Stock.EAN = Sizerun.EAN27 OR source_Stock.EAN = Sizerun.EAN28 OR source_Stock.EAN = Sizerun.EAN29 OR source_Stock.EAN = Sizerun.EAN30 INNER JOIN Assortment ON Sizerun.SizerunID = Assortment.AssortmentSizerunID WHERE(source_Stock.Freilagerbestand > 0) AND(Sizerun.IsDeleted = 0) AND(Assortment.IsDeleted = 0)";

            var tempStocks = dataModel.Database.SqlQuery<ImportTempStock>(sourceTempStockQuery).ToList();

            dataModel.Database.ExecuteSqlCommand("Delete FROM source_tempStock");

            List<List<object>> newdbitems = new List<List<object>>();



            foreach (var m in tempStocks.GroupBy(ts => ts.AssortmentID))
            {
                source_tempStock newStock = new source_tempStock();
                ClassFiller.FillDefaultValues(newStock);

                newStock.AssortmentID = m.Key;
                newStock.ID = Guid.NewGuid().ToString();

                foreach (var s in m)
                {
                    switch (s.SizeIndex)
                    {
                        case 1: newStock.Qty01 = s.Freilagerbestand; break;
                        case 2: newStock.Qty02 = s.Freilagerbestand; break;
                        case 3: newStock.Qty03 = s.Freilagerbestand; break;
                        case 4: newStock.Qty04 = s.Freilagerbestand; break;
                        case 5: newStock.Qty05 = s.Freilagerbestand; break;
                        case 6: newStock.Qty06 = s.Freilagerbestand; break;
                        case 7: newStock.Qty07 = s.Freilagerbestand; break;
                        case 8: newStock.Qty08 = s.Freilagerbestand; break;
                        case 9: newStock.Qty09 = s.Freilagerbestand; break;
                        case 10: newStock.Qty10 = s.Freilagerbestand; break;
                        case 11: newStock.Qty11 = s.Freilagerbestand; break;
                        case 12: newStock.Qty12 = s.Freilagerbestand; break;
                        case 13: newStock.Qty13 = s.Freilagerbestand; break;
                        case 14: newStock.Qty14 = s.Freilagerbestand; break;
                        case 15: newStock.Qty15 = s.Freilagerbestand; break;
                        case 16: newStock.Qty16 = s.Freilagerbestand; break;
                        case 17: newStock.Qty17 = s.Freilagerbestand; break;
                        case 18: newStock.Qty18 = s.Freilagerbestand; break;
                        case 19: newStock.Qty19 = s.Freilagerbestand; break;
                        case 20: newStock.Qty20 = s.Freilagerbestand; break;
                        case 21: newStock.Qty21 = s.Freilagerbestand; break;
                        case 22: newStock.Qty22 = s.Freilagerbestand; break;
                        case 23: newStock.Qty23 = s.Freilagerbestand; break;
                        case 24: newStock.Qty24 = s.Freilagerbestand; break;
                        case 25: newStock.Qty25 = s.Freilagerbestand; break;
                        case 26: newStock.Qty26 = s.Freilagerbestand; break;
                        case 27: newStock.Qty27 = s.Freilagerbestand; break;
                        case 28: newStock.Qty28 = s.Freilagerbestand; break;
                        case 29: newStock.Qty29 = s.Freilagerbestand; break;
                        case 30: newStock.Qty30 = s.Freilagerbestand; break;
                    }
                }


                newdbitems.Add(ClassToListObjectItem.ToList(newStock));

            }

            SqlBulkInsertHelper.InsertBigData<source_tempStock>(newdbitems);
            newdbitems.Clear();


            string sourceImportQuery = "SELECT        Stock.StockID, source_tempStock.Qty01, source_tempStock.Qty02, source_tempStock.Qty03, source_tempStock.Qty04, source_tempStock.Qty05, source_tempStock.Qty06, source_tempStock.Qty07, source_tempStock.Qty08, source_tempStock.Qty09, source_tempStock.Qty10, source_tempStock.Qty11, source_tempStock.Qty12, source_tempStock.Qty13, source_tempStock.Qty14, source_tempStock.Qty15, source_tempStock.Qty16, source_tempStock.Qty17, source_tempStock.Qty18, source_tempStock.Qty19, source_tempStock.Qty20, source_tempStock.Qty21, source_tempStock.Qty22, source_tempStock.Qty23, source_tempStock.Qty24, source_tempStock.Qty25, source_tempStock.Qty26, source_tempStock.Qty27, source_tempStock.Qty28, source_tempStock.Qty29, source_tempStock.Qty30,  source_tempStock.AssortmentID FROM            source_tempStock LEFT OUTER JOIN Stock ON source_tempStock.AssortmentID = Stock.StockAssortmentID";

            string updateQuery = "UPDATE Stock SET StockID = Stock_Compare.StockID, StockAssortmentID = Stock_Compare.StockAssortmentID, StockQty01 = Stock_Compare.StockQty01, StockQty02 = Stock_Compare.StockQty02, StockQty03 = Stock_Compare.StockQty03, StockQty04 = Stock_Compare.StockQty04, StockQty05 = Stock_Compare.StockQty05, StockQty06 = Stock_Compare.StockQty06, StockQty07 = Stock_Compare.StockQty07, StockQty08 = Stock_Compare.StockQty08, StockQty09 = Stock_Compare.StockQty09, StockQty10 = Stock_Compare.StockQty10, StockQty11 = Stock_Compare.StockQty11, StockQty12 = Stock_Compare.StockQty12, StockQty13 = Stock_Compare.StockQty13, StockQty14 = Stock_Compare.StockQty14, StockQty15 = Stock_Compare.StockQty15, StockQty16 = Stock_Compare.StockQty16, StockQty17 = Stock_Compare.StockQty17, StockQty18 = Stock_Compare.StockQty18, StockQty19 = Stock_Compare.StockQty19, StockQty20 = Stock_Compare.StockQty20, StockQty21 = Stock_Compare.StockQty21, StockQty22 = Stock_Compare.StockQty22, StockQty23 = Stock_Compare.StockQty23, StockQty24 = Stock_Compare.StockQty24, StockQty25 = Stock_Compare.StockQty25, StockQty26 = Stock_Compare.StockQty26, StockQty27 = Stock_Compare.StockQty27, StockQty28 = Stock_Compare.StockQty28, StockQty29 = Stock_Compare.StockQty29, StockQty30 = Stock_Compare.StockQty30, SyncDateTime = Stock_Compare.SyncDateTime, IsDeleted = Stock_Compare.IsDeleted, StockClientID = Stock_Compare.StockClientID, IsFreeDisposition = Stock_Compare.IsFreeDisposition, StockQtyAssortment = Stock_Compare.StockQtyAssortment FROM Stock INNER JOIN Stock_Compare ON Stock.StockID = Stock_Compare.StockID WHERE (Stock.StockID <> Stock_Compare.StockID OR Stock.StockAssortmentID <> Stock_Compare.StockAssortmentID OR Stock.StockQty01 <> Stock_Compare.StockQty01 OR Stock.StockQty02 <> Stock_Compare.StockQty02 OR Stock.StockQty03 <> Stock_Compare.StockQty03 OR Stock.StockQty04 <> Stock_Compare.StockQty04 OR Stock.StockQty05 <> Stock_Compare.StockQty05 OR Stock.StockQty06 <> Stock_Compare.StockQty06 OR Stock.StockQty07 <> Stock_Compare.StockQty07 OR Stock.StockQty08 <> Stock_Compare.StockQty08 OR Stock.StockQty09 <> Stock_Compare.StockQty09 OR Stock.StockQty10 <> Stock_Compare.StockQty10 OR Stock.StockQty11 <> Stock_Compare.StockQty11 OR Stock.StockQty12 <> Stock_Compare.StockQty12 OR Stock.StockQty13 <> Stock_Compare.StockQty13 OR Stock.StockQty14 <> Stock_Compare.StockQty14 OR Stock.StockQty15 <> Stock_Compare.StockQty15 OR Stock.StockQty16 <> Stock_Compare.StockQty16 OR Stock.StockQty17 <> Stock_Compare.StockQty17 OR Stock.StockQty18 <> Stock_Compare.StockQty18 OR Stock.StockQty19 <> Stock_Compare.StockQty19 OR Stock.StockQty20 <> Stock_Compare.StockQty20 OR Stock.StockQty21 <> Stock_Compare.StockQty21 OR Stock.StockQty22 <> Stock_Compare.StockQty22 OR Stock.StockQty23 <> Stock_Compare.StockQty23 OR Stock.StockQty24 <> Stock_Compare.StockQty24 OR Stock.StockQty25 <> Stock_Compare.StockQty25 OR Stock.StockQty26 <> Stock_Compare.StockQty26 OR Stock.StockQty27 <> Stock_Compare.StockQty27 OR Stock.StockQty28 <> Stock_Compare.StockQty28 OR Stock.StockQty29 <> Stock_Compare.StockQty29 OR Stock.StockQty30 <> Stock_Compare.StockQty30 OR Stock.IsDeleted <> Stock_Compare.IsDeleted OR Stock.StockClientID <> Stock_Compare.StockClientID OR Stock.IsFreeDisposition <> Stock_Compare.IsFreeDisposition OR Stock.StockQtyAssortment <> Stock_Compare.StockQtyAssortment )";

            string deleteQuery = "UPDATE Stock SET SyncDateTime = GETDATE(), IsDeleted = 1 FROM Stock LEFT OUTER JOIN Stock_Compare ON Stock.StockID = Stock_Compare.StockID WHERE(Stock_Compare.StockID IS NULL) AND (Stock.StockClientID = N'1') AND (Stock.IsDeleted = 0)";

            string insertQuery = "INSERT INTO Stock SELECT Stock_Compare.* FROM Stock_Compare LEFT OUTER JOIN Stock AS Stock_1 ON Stock_Compare.StockID = Stock_1.StockID WHERE(Stock_1.StockID IS NULL)";

            var dbitems_source = dataModel.Database.SqlQuery<ImportStock>(sourceImportQuery).ToList();



            foreach (var m in dbitems_source)
            {
                Stock newStock = new Stock();
                ClassFiller.FillDefaultValues(newStock);
                ClassFiller.PasteData(m, newStock);

                newStock.StockID = Guid.NewGuid().ToString();
                newStock.StockClientID = "1";
                newStock.StockQty01 = m.Qty01;
                newStock.StockQty02 = m.Qty02;
                newStock.StockQty03 = m.Qty03;
                newStock.StockQty04 = m.Qty04;
                newStock.StockQty05 = m.Qty05;
                newStock.StockQty06 = m.Qty06;
                newStock.StockQty07 = m.Qty07;
                newStock.StockQty08 = m.Qty08;
                newStock.StockQty09 = m.Qty09;
                newStock.StockQty10 = m.Qty10;
                newStock.StockQty11 = m.Qty11;
                newStock.StockQty12 = m.Qty12;
                newStock.StockQty13 = m.Qty13;
                newStock.StockQty14 = m.Qty14;
                newStock.StockQty15 = m.Qty15;
                newStock.StockQty16 = m.Qty16;
                newStock.StockQty17 = m.Qty17;
                newStock.StockQty18 = m.Qty18;
                newStock.StockQty19 = m.Qty19;
                newStock.StockQty20 = m.Qty20;
                newStock.StockQty21 = m.Qty21;
                newStock.StockQty22 = m.Qty22;
                newStock.StockQty23 = m.Qty23;
                newStock.StockQty24 = m.Qty24;
                newStock.StockQty25 = m.Qty25;
                newStock.StockQty26 = m.Qty26;
                newStock.StockQty27 = m.Qty27;
                newStock.StockQty28 = m.Qty28;
                newStock.StockQty29 = m.Qty29;
                newStock.StockQty30 = m.Qty30;
                newStock.StockAssortmentID = m.AssortmentID;
                newStock.IsDeleted = false;
                newStock.SyncDateTime = DateTime.UtcNow;
                newStock.IsFreeDisposition = true;

                if (m.StockID != null)
                {
                    newStock.StockID = m.StockID;
                }

                newdbitems.Add(ClassToListObjectItem.ToList(newStock));
            }

            dataModel.Database.ExecuteSqlCommand("Delete FROM Stock_Compare");

            SqlBulkInsertHelper.InsertBigData<Stock_Compare>(newdbitems);
            newdbitems.Clear();

            var insertResult = dataModel.Database.ExecuteSqlCommand(insertQuery);

            var updateResult = dataModel.Database.ExecuteSqlCommand(updateQuery);

            var deleteResult = dataModel.Database.ExecuteSqlCommand(deleteQuery);

            //Farbe als Bestandsartikel markieren/setzen
            dataModel.Database.ExecuteSqlCommand("UPDATE       Color SET                StockArticle = 1, SyncDateTime = GETDATE() FROM            Stock INNER JOIN Assortment ON Stock.StockAssortmentID = Assortment.AssortmentID INNER JOIN Sizerun ON Assortment.AssortmentSizerunID = Sizerun.SizerunID INNER JOIN Color ON Sizerun.SizerunColorID = Color.ColorID WHERE(Color.StockArticle = 0)");

            return "OK";
        }
        #endregion

        #region Saisonumsatz
        public string ImportSeasonValue()
        {

            string sourceQuery = "SELECT        Season.SeasonID, Customer.CustomerID, source_Saisonumsatz.MengeVO AS QtyPreOrder, source_Saisonumsatz.UmsatzVO AS ValuePreOrder, source_Saisonumsatz.MengeGesamt AS Qty, source_Saisonumsatz.UmsatzGesamt AS[Value], SeasonValue.SeasonValueID, GETDATE() AS SyncDateTime, CAST(0 AS bit) AS IsDeleted FROM source_Saisonumsatz INNER JOIN Customer ON source_Saisonumsatz.Kundennr = Customer.CustomerNumber INNER JOIN Season ON source_Saisonumsatz.Saison = Season.SeasonName LEFT OUTER JOIN SeasonValue ON Customer.CustomerID = SeasonValue.CustomerID AND Season.SeasonID = SeasonValue.SeasonID WHERE(Customer.IsDeleted = 0) AND(Season.IsDeleted = 0)";

            var sourceItems = dataModel.Database.SqlQuery<ImportSeasonValue>(sourceQuery).ToList();

            List<List<object>> newdbitems = new List<List<object>>();

            List<List<object>> insertItems = new List<List<object>>();

            List<ImportSeasonValue> updateItems = new List<ImportSeasonValue>();

            var existingItems = dataModel.SeasonValue.Where(m => m.IsDeleted == false).ToList();


            foreach (var newitem in sourceItems)
            {
                if (string.IsNullOrEmpty(newitem.SeasonValueID))
                {
                    newitem.SeasonValueID = Guid.NewGuid().ToString();
                    insertItems.Add(ClassToListObjectItem.ToList(newitem));
                }
                else
                {
                    var existingDBItem = existingItems.FirstOrDefault(dbitem => dbitem.SeasonValueID == newitem.SeasonValueID);
                    if (existingDBItem != null && DatabaseModelHelper.Update(existingDBItem, newitem))
                    {
                        updateItems.Add(newitem);
                    }
                    existingItems.Remove(existingDBItem);
                }

            }

            SqlBulkInsertHelper.InsertBigData<SeasonValue>(insertItems);

            return "OK";
        }
        #endregion

        public string ImportAllData()
        {
            try
            {
                ImportSourceData();
                ImportArticle();
                ImportCustomer();
                ImportPrice();
                return "OK";
            }
            catch (Exception ex)
            {
                SendMail("sascha.weber@myconveno.de", ex.Message, "LIVING KB ERROR ImportAllData", "");
                throw;
            }
        }

        public string ImportAllDataStock()
        {
            ImportSourceDataStock();
            ImportStock();
            return "OK";
        }

        public string UpdateGeoData()
        {
            try
            {
                int counter = 0;
                foreach (var c in dataModel.Customer.Where(cust => cust.lat == 0).ToList())
                {
                    counter++;
                    GeoLocation geo = GetGoogleGeoResult(string.Empty, c.CustomerCity, c.CustomerZIP, c.CustomerStreet);

                    c.lat = geo.Lat;
                    c.lon = geo.Lon;
                    c.SyncDateTime = DateTime.UtcNow;

                    if (counter % 100 == 0)
                    {
                        dataModel.SaveChanges();
                    }
                }
                dataModel.SaveChanges();

                return "OK";
            }
            catch (Exception ex)
            {
                ex.ToString();
                return ex.ToString();
            }
        }
        private GeoLocation GetGeoResult(string Country, string City, string PostCode, string Street)
        {
            GeoLocation loc = new GeoLocation();
            string AppID = "AqJgOyV4notDOw8F2LvEFCD5Zx12q_RPfl-CSm-JDHIImfb9Ca1EhYTkUg6JpUXE";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://dev.virtualearth.net/REST/v1/Locations?" + (Country != string.Empty ? "CountryRegion=" + Country + "&" : string.Empty) + (City != string.Empty ? "locality=" + City + "&" : string.Empty) + (PostCode != string.Empty ? "postalCode=" + PostCode + "&" : string.Empty) + (Street != string.Empty ? "addressLine=" + Street + "&" : string.Empty) + "&key=" + AppID + "&o=xml");
                string PointFromBingServiceURL = request.Address.AbsoluteUri;
                request.Proxy = WebRequest.GetSystemWebProxy();
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                        return loc;

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(response.GetResponseStream());

                    loc.Lat = Convert.ToDouble(xmldoc.GetElementsByTagName("Latitude")[0].FirstChild.Value.ToString());
                    loc.Lon = Convert.ToDouble(xmldoc.GetElementsByTagName("Longitude")[0].FirstChild.Value.ToString());
                    response.Close();
                    return loc;
                }
            }
            catch (Exception ex)
            {
                return new GeoLocation();
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

        private void SendMail(string EmpfaengerList, string Body, string Subject, string Datei)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                foreach (string empf in EmpfaengerList.Split(';'))
                {
                    mail.To.Add(new MailAddress(empf));
                }

                mail.Subject = Subject;

                mail.Body = Body;

                if (Datei != null && Datei != string.Empty)
                {
                    mail.Attachments.Add(new Attachment(Datei));
                }

                mail.IsBodyHtml = false;

                mail.From = new System.Net.Mail.MailAddress("service@myconveno.de", "Livingkitzbühl Data Service");

                SmtpClient client = new SmtpClient("smtp.office365.com");
                client.Credentials = new NetworkCredential("service@myconveno.de", "Fisch6633");
                client.EnableSsl = true;
                client.Send(mail);
            }
            catch
            {

            }
        }

        public class AddressComponent
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public List<string> types { get; set; }
        }

        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Northeast
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Southwest
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Viewport
        {
            public Northeast northeast { get; set; }
            public Southwest southwest { get; set; }
        }

        public class Geometry
        {
            public Location location { get; set; }
            public string location_type { get; set; }
            public Viewport viewport { get; set; }
        }

        public class Result
        {
            public List<AddressComponent> address_components { get; set; }
            public string formatted_address { get; set; }
            public Geometry geometry { get; set; }
            public string place_id { get; set; }
            public List<string> types { get; set; }
        }

        public class GoogleRootObject
        {
            public List<Result> results { get; set; }
            public string status { get; set; }
        }
    }
}
