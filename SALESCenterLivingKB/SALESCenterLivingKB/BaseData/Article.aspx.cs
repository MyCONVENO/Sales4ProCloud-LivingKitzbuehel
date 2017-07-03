using SALESCenterLivingKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SALESCenterLivingKB.BaseData
{
    public partial class Article : System.Web.UI.Page
    {
        ServerEntities entityModel = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            entityModel = new ServerEntities();
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression      

        protected void articleGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = (sender as GridView).SelectedValue.ToString();

            FillColor(selectedValue);
        }

        protected void GridViewModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = (sender as GridView).SelectedValue.ToString();

            FillArticle(selectedValue);
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<SALESCenterLivingKB.Models.Client> GridViewClient_GetData()
        {
            return entityModel.Client.Where(c => c.IsDeleted == false).OrderBy(c => c.ClientID);
        }

        protected void GridViewClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = (sender as GridView).SelectedValue.ToString();

            FillModelle(selectedValue);
        }


        void FillModelle(string ClientID)
        {
            var modelle = entityModel.Model.Where(a => a.IsDeleted == false && a.ModelClientID == ClientID);

            //if (!string.IsNullOrEmpty(tbSearch.Text))
            //{
            //    modelle = modelle.Where(m => m.ModelName.StartsWith(tbSearch.Text) || m.ModelNumber.StartsWith(tbSearch.Text));
            //}


            GridViewModel.DataSource = modelle.OrderBy(a => a.ModelName).ToList();
            GridViewModel.DataBind();
            GridViewModel.SelectedIndex = 0;

            if (modelle.Any())
            {
                FillArticle(modelle.First().ModelID);
            }
            else
            {
                FillArticle("");
            }
        }

        void FillArticle(string ModelID)
        {
            var article = entityModel.Article.Where(a => a.IsDeleted == false && a.ModelID == ModelID).OrderBy(a => a.ArticleNumber).ToList();
            articleGrid.DataSource = article;
            articleGrid.DataBind();
            articleGrid.SelectedIndex = 0;

            if (article.Any())
            {
                FillColor(article.First().ArticleID);
            }
            else
            {
                FillColor("");
            }

        }

        private void FillColor(string articleID)
        {
            var colors = entityModel.Color.Where(a => a.IsDeleted == false && a.ColorArticleID == articleID).OrderBy(a => a.ColorNumber).ToList();
            GridViewColor.DataSource = colors;
            GridViewColor.DataBind();
            GridViewColor.SelectedIndex = 0;

            if (colors.Any())
            {
                FillSizerun(colors.First().ColorID);
            }
            else
            {
                FillSizerun("");
            }
        }

        protected void GridViewColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = (sender as GridView).SelectedValue.ToString();

            FillSizerun(selectedValue);
        }

        private void FillSizerun(string ColorID)
        {
            var sizeruns = entityModel.Sizerun.Where(a => a.IsDeleted == false && a.SizerunColorID == ColorID).OrderBy(a => a.SizerunName).ToList();
            GridViewSizerun.DataSource = sizeruns;
            GridViewSizerun.DataBind();
            GridViewSizerun.SelectedIndex = 0;
        }

        protected void GridViewSizerun_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void searchButton_Click(object sender, EventArgs e)
        {            
        }
    }
}