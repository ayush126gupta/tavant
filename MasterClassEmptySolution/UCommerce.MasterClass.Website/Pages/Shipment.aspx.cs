using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using UCommerce.Api;
using UCommerce.EntitiesV2;

// this will show all the shippment method that we want.
// this can give us normal shipment as well as fast shipment service.

namespace UCommerce.MasterClass.Pages
{
    public partial class Shipment : System.Web.UI.Page
    {
        protected int SelectedShipmentMethodId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            var basket = TransactionLibrary.GetBasket().PurchaseOrder;
            var firstShipment = basket.Shipments.First();

            var shippingMethods = TransactionLibrary.GetShippingMethods(firstShipment.ShipmentAddress.Country);

            AvailableShipmentMethods.DataSource = shippingMethods;
            AvailableShipmentMethods.DataBind();
        }
        protected void SaveShipmentAndGoToPaymentBtn_OnClick(object sender, EventArgs e)
        {
            var selectedShipmentMethodId = Int32.Parse(AvailableShipmentMethods.SelectedValue);
            UCommerce.Api.TransactionLibrary.CreateShipment(selectedShipmentMethodId, overwriteExisting: true);
            UCommerce.Api.TransactionLibrary.ExecuteBasketPipeline();

            Response.Redirect("/payment");
        }
    }
}