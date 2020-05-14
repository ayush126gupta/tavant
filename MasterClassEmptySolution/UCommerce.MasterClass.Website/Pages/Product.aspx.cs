using System;
using UCommerce.MasterClass.Models;
using UCommerce.Extensions;
using UCommerce.Api;
using UCommerce.Runtime;
using UCommerce.EntitiesV2;

// this page will show all the products variants to be added to the basket with add to basket button //

namespace UCommerce.MasterClass.Pages
{
    public partial class Product : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            ProductDetailsModel model = new ProductDetailsModel();
            var currentproduct = UCommerce.Runtime.SiteContext.Current.CatalogContext.CurrentProduct;
            model = MapProduct(currentproduct);

            BuildPage(model);

        }

        private ProductDetailsModel MapProduct(EntitiesV2.Product currentproduct)
        {
            var model = new ProductDetailsModel();
            model.Sku = currentproduct.Sku;
            model.Name = currentproduct.DisplayName();
            model.LongDescription = currentproduct.LongDescription();
            model.Url = $"/Product?product={currentproduct.Id}";
            model.PriceCalculation = UCommerce.Api.CatalogLibrary.CalculatePrice(currentproduct);
            model.VariantSku = currentproduct.VariantSku;

            model.IsVariant = currentproduct.IsVariant;
            foreach (EntitiesV2.Product currentProductVariant in currentproduct.Variants)
            {

                model.Variants.Add(MapProduct(currentProductVariant));
            }
            return model;
        }



        private void BuildPage(ProductDetailsModel model)
        {
            ProductName.Text = model.Name;
            ShowPrice.Visible = (model.PriceCalculation != null);
            if (model.PriceCalculation != null)
            {
                YourPriceAmount.Text = model.PriceCalculation.YourPrice.Amount.ToString();
            }

            HiddenSku.Value = model.Sku;
            LongDescription.Text = model.LongDescription;
            VariantsRepeater.DataSource = model.Variants;
            VariantsRepeater.DataBind();
        }

        protected void AddToBasketButton_OnClick(object sender, EventArgs e)
        {
            string sku = HiddenSku.Value;
            string variantSku = Request.Form["VariantSku"];
            OrderLine orderline = TransactionLibrary.AddToBasket(1, sku, variantSku);

        }
    }
}
