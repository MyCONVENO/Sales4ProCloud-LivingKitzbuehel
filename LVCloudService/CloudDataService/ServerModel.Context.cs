﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CloudDataService
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LIVINGKITZBUEHLEntities : DbContext
    {
        public LIVINGKITZBUEHLEntities()
            : base("name=LIVINGKITZBUEHLEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Agent> Agent { get; set; }
        public DbSet<Agent_Compare> Agent_Compare { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Association> Association { get; set; }
        public DbSet<Association_Compare> Association_Compare { get; set; }
        public DbSet<AssociationMember> AssociationMember { get; set; }
        public DbSet<AssociationMember_Compare> AssociationMember_Compare { get; set; }
        public DbSet<Assortment> Assortment { get; set; }
        public DbSet<Assortment_Compare> Assortment_Compare { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<ContactPerson> ContactPerson { get; set; }
        public DbSet<ContactPerson_Compare> ContactPerson_Compare { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<CustomerFavorite> CustomerFavorite { get; set; }
        public DbSet<CustomerNote> CustomerNote { get; set; }
        public DbSet<DeliveryAddress> DeliveryAddress { get; set; }
        public DbSet<DeliveryAddress_Compare> DeliveryAddress_Compare { get; set; }
        public DbSet<InvoiceAddress> InvoiceAddress { get; set; }
        public DbSet<InvoiceAddress_Compare> InvoiceAddress_Compare { get; set; }
        public DbSet<Label> Label { get; set; }
        public DbSet<Label_Compare> Label_Compare { get; set; }
        public DbSet<Model> Model { get; set; }
        public DbSet<Model_Compare> Model_Compare { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<pricat> pricat { get; set; }
        public DbSet<Price> Price { get; set; }
        public DbSet<Price_Compare> Price_Compare { get; set; }
        public DbSet<Pricelist> Pricelist { get; set; }
        public DbSet<Pricelist_Compare> Pricelist_Compare { get; set; }
        public DbSet<PushChannel> PushChannel { get; set; }
        public DbSet<Season> Season { get; set; }
        public DbSet<Season_Compare> Season_Compare { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItem { get; set; }
        public DbSet<Sizerun> Sizerun { get; set; }
        public DbSet<Sizerun_Compare> Sizerun_Compare { get; set; }
        public DbSet<SpecialDiscount> SpecialDiscount { get; set; }
        public DbSet<Stock_Compare> Stock_Compare { get; set; }
        public DbSet<TextSnippet> TextSnippet { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserAgents> UserAgents { get; set; }
        public DbSet<UserClient> UserClient { get; set; }
        public DbSet<UserPriceList> UserPriceList { get; set; }
        public DbSet<Article_Compare> Article_Compare { get; set; }
        public DbSet<Color_Compare> Color_Compare { get; set; }
        public DbSet<Customer_Compare> Customer_Compare { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<source_Artikel> source_Artikel { get; set; }
        public DbSet<source_Artikelbeschreibung> source_Artikelbeschreibung { get; set; }
        public DbSet<source_Kunde> source_Kunde { get; set; }
        public DbSet<source_TempSizerun> source_TempSizerun { get; set; }
        public DbSet<source_Preis> source_Preis { get; set; }
        public DbSet<source_TempSizerunPrice> source_TempSizerunPrice { get; set; }
        public DbSet<source_Rabatt> source_Rabatt { get; set; }
        public DbSet<source_Ansprechpartner> source_Ansprechpartner { get; set; }
        public DbSet<source_Stock> source_Stock { get; set; }
        public DbSet<source_tempStock> source_tempStock { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductImage_Compare> ProductImage_Compare { get; set; }
        public DbSet<source_Vertreter> source_Vertreter { get; set; }
        public DbSet<source_Lieferadresse> source_Lieferadresse { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<SeasonValue> SeasonValue { get; set; }
        public DbSet<source_Saisonumsatz> source_Saisonumsatz { get; set; }
    }
}
