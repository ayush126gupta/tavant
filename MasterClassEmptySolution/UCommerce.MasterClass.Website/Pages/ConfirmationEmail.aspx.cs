using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterClass.Pages
{
    /// <summary>
    /// This page can be used in the Ucommerce Settings> Settings > Email to send a summary of the order to the customer after he has completed the checkout flow.
    /// </summary>
    public partial class EmailConfirmationPageaspx : System.Web.UI.Page
    {
        /// <summary>
        /// Using the Ucommerce APIs, create an email with the items that were purchased by a customer after checkout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         protected void Page_Load(object sender, EventArgs e)
        {
            
            var orderGuid = Request.QueryString["orderGuid"];
            var orderNumber = Request.QueryString["orderNumber"];

            //TODO
        }

    }
}