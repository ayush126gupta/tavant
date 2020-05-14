using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UCommerce.EntitiesV2;
using UCommerce.Infrastructure.Globalization;
using UCommerce.Pipelines;
using UCommerce.Runtime;
using UCommerce.Transactions;

// this is to notify customer via email when he purchases more than 500 //

namespace UCommerce.MasterClass.BusinessLogic.Pipelines
{
    public class NotifyOnVipCustomerTask : IPipelineTask<PurchaseOrder>
    {
        public decimal OrderTotal { get; set; }

        private readonly ILocalizationContext localizationContext;
        private readonly IEmailService emailService;
        private readonly ICatalogContext catalogContext;

        public NotifyOnVipCustomerTask(IEmailService emailService, ILocalizationContext localizationContext, ICatalogContext catalogContext)
        {
            this.emailService = emailService;
            this.localizationContext = localizationContext;
            this.catalogContext = catalogContext;

        }

        //public IEmailService emailService { get; }

        public PipelineExecutionResult Execute(PurchaseOrder subject)

        {
            if (subject.OrderTotal.GetValueOrDefault() > 500)
            {
                EmailProfile emailProfile = catalogContext.CurrentCatalogGroup.EmailProfile;
                IDictionary<string, string> templateParameters = new Dictionary<string, string>();
                templateParameters.Add("ordereValue", subject.OrderTotal.GetValueOrDefault().ToString());
                emailService.Send(localizationContext, emailProfile, "vip", new MailAddress("ayush126gupta@gmail.com"), templateParameters);
            }

            return PipelineExecutionResult.Success;
        }
    }
}
