using System;
using System.Web;
using System.Web.Util;

namespace PGD.UI.Mvc.Helpers
{
    public class ProeticaRequestValidator : RequestValidator
    {
        protected override bool IsValidRequestString(HttpContext context, string value, RequestValidationSource requestValidationSource, string collectionKey, out int validationFailureIndex)
        {
            validationFailureIndex = 0;

            if (requestValidationSource == RequestValidationSource.Form && !String.IsNullOrEmpty(collectionKey) && collectionKey.Equals(WSFederationConstants.Parameters.Result, StringComparison.Ordinal))
            {
                var unvalidatedFormValues = context.Request.Unvalidated.Form;
                SignInResponseMessage message = WSFederationMessage.CreateFromNameValueCollection(WSFederationMessage.GetBaseUrl(context.Request.Url), unvalidatedFormValues) as SignInResponseMessage;

                if (message != null)
                {
                    return true;
                }
            }

            return base.IsValidRequestString(context, value, requestValidationSource, collectionKey, out validationFailureIndex);
        }
    }
}