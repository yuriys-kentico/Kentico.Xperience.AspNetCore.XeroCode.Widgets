using System;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Internal;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.UIControls;

[Title("Xero code widgets")]
[UIElement("XeroCodeWidgets", "Widgets")]
public partial class Widgets : CMSDeskPage
{
    protected void Page_Init()
    {
        var relativePath = "/xerocode/widgets";

        relativePath = "~/" + VirtualContext.AddPathHash(relativePath).TrimStart('/');

        var virtualContextParameters = VirtualContext.GetPreviewModeParameters(
            MembershipContext.AuthenticatedUser.UserGUID,
            LocalizationContext.CurrentCulture.CultureCode,
            DocumentHelper.GetDocuments().Path("/").First().DocumentWorkflowCycleGUID,
            false,
            true
            );

        relativePath = VirtualContext.GetVirtualContextPath(relativePath, virtualContextParameters);
        relativePath = URLHelper.AddParameterToUrl(
            relativePath,
            VirtualContext.ADMINISTRATION_DOMAIN_PARAMETER,
            Uri.EscapeDataString($"{RequestContext.CurrentScheme}://{RequestContext.CurrentDomain}")
            );

        var presentationUrl = new PresentationUrlRetriever().RetrieveForAdministration(SiteContext.CurrentSiteID);

        widgetsIframe.Src = URLHelper.CombinePath(relativePath, '/', presentationUrl, null); ;

        RegisterCookiePolicyDetection();
    }
}