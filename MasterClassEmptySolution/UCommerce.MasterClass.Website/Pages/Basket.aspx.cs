using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using UCommerce.MasterClass.Models;
using UCommerce.Api;
using UCommerce.EntitiesV2;
using UCommerce.Extensions;

// this will give all the basket details such as product name, quantity and product description.
// this will give total amount to be paid to pcomplete order.
// this will update and delete order details.


namespace UCommerce.MasterClass.Pages
{
    public partial class Basket : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                return;
            }
            var basketModel = new PurchaseOrderModel();
            UCommerce.EntitiesV2.PurchaseOrder basket = UCommerce.Api.TransactionLibrary.GetBasket().PurchaseOrder;
            basketModel = MapBasket(basket);

            BuildPage(basketModel);
        }

        private PurchaseOrderModel MapBasket(PurchaseOrder basket)
        {
            var basketModel = new PurchaseOrderModel();
            UCommerce.EntitiesV2.Currency billingCurrency = basket.BillingCurrency;
            basketModel.OrderTotal = new Money(basket.OrderTotal.GetValueOrDefault(), billingCurrency).ToString();
            foreach (UCommerce.EntitiesV2.OrderLine basketOrderLine in basket.OrderLines)
            {
                var orderlineModel = new OrderlineModel();
                orderlineModel.Quantity = basketOrderLine.Quantity;
                orderlineModel.ProductName = basketOrderLine.ProductName;
                var total = new Money(basketOrderLine.Total.GetValueOrDefault(), basket.BillingCurrency);
                orderlineModel.Sku = basketOrderLine.Sku;
                orderlineModel.OrderLineId = basketOrderLine.OrderLineId;
                orderlineModel.VariantSku = basketOrderLine.VariantSku;
                orderlineModel.Total = new Money(basketOrderLine.Total.GetValueOrDefault(), billingCurrency).ToString();
                basketModel.OrderLines.Add(orderlineModel);
            }
            return (basketModel);
        }

        protected void UpdateBasketButton_OnClick(object sender, EventArgs e)
        {
            var basket = TransactionLibrary.GetBasket().PurchaseOrder;
            foreach (RepeaterItem item in OrderLinesRepeater.Items)
            {
                var orderlineIdHidden = item.FindControl("OrderLineId") as HiddenField;
                int orderlineId = Convert.ToInt32(orderlineIdHidden.Value);

                var quantityTextBox = item.FindControl("OrderLineQuantity") as TextBox;
                int quantity = Convert.ToInt32(quantityTextBox.Text);

                UCommerce.Api.TransactionLibrary.UpdateLineItem(orderlineId, quantity);

            }
            UCommerce.Api.TransactionLibrary.ExecuteBasketPipeline();
            basket = TransactionLibrary.GetBasket().PurchaseOrder;

            PurchaseOrderModel basketModel = MapBasket(basket);

            BuildPage(basketModel);
        }

        protected void RemoveOrderLineButton_OnClick(object sender, EventArgs e)
        {
            var orderlineId = Convert.ToInt32((sender as Button).Attributes["Value"]);
            TransactionLibrary.UpdateLineItem(orderlineId, 0);
            UCommerce.Api.TransactionLibrary.ExecuteBasketPipeline();
        }

        protected void ContinueToBillingBtn_OnClick(object sender, EventArgs e)
        {
            string url = $"/basket/address-(2)";
            Response.Redirect(url);
        }
        private void BuildPage(PurchaseOrderModel basketModel)
        {
            OrderLinesRepeater.DataSource = basketModel.OrderLines;
            OrderLinesRepeater.DataBind();

            OrderTotal.Text = basketModel.OrderTotal;
        }
    }
}
