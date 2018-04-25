using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading;
using BizTalkComponents.Utils;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using IComponent = Microsoft.BizTalk.Component.Interop.IComponent;

namespace BizTalkComponents.PipelineComponents.ContextRegExParseAndPromote
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("BCDEAB4B-9E1D-4374-8282-077717B0F431")]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    public partial class ContextRegExParseAndPromote : IComponent, IBaseComponent,
                                        IPersistPropertyBag, IComponentUI
    {
        private const string PropertyToParsePropertyName = "PropertyToParse";
        private const string RegExPatternPropertyName = "RegExPattern";
        private const string DestinationPropertyPropertyName = "DestinationProperty";
        private const string ThrowIfNoMatchPropertyName = "ThrowIfNoMatch";

        [RequiredRuntime]
        [DisplayName("Property to parse")]
        [Description("The property path of the property to parse.")]
        [RegularExpression(@"^.*#.*$",
        ErrorMessage = "A property path should be formatted as namespace#property.")]
        public string PropertyToParse { get; set; }

        [RequiredRuntime]
        [DisplayName("RegExPattern Pattern")]
        [Description("The regular expression pattern to use to parse the property value.")]
        public string RegExPattern { get; set; }

        [RequiredRuntime]
        [DisplayName("Destination property")]
        [Description("The property path to promote the result to.")]
        [RegularExpression(@"^.*#.*$",
        ErrorMessage = "A property path should be formatted as namespace#property.")]
        public string DestinationProperty { get; set; }

        [RequiredRuntime]
        [DisplayName("Throw if no match")]
        [Description("Specified whether an ArgumentException should be thrown if the pattern does not match any value.")]
        public bool ThrowIfNoMatch { get; set; }

        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            string errorMessage;

            if (!Validate(out errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            var propertyToParse = new ContextProperty(PropertyToParse);
            var destinationProperty = new ContextProperty(DestinationProperty);

            string propertyToParseValue;

            if (!pInMsg.Context.TryRead(propertyToParse, out propertyToParseValue))
            {
                throw new InvalidOperationException("The specified property to parse does not exist in message context.");
            }

            var parsedValue = Regex.Match(propertyToParseValue, RegExPattern).Groups[0].Value;

            if (!string.IsNullOrEmpty(parsedValue))
            {
                pInMsg.Context.Promote(destinationProperty, parsedValue);    
            }
            else if (string.IsNullOrEmpty(parsedValue) && ThrowIfNoMatch)
            {
                throw new ArgumentException("The specified pattern did not match any value.");
            }
            

            return pInMsg;

        }

        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            PropertyToParse = PropertyBagHelper.ReadPropertyBag(propertyBag, PropertyToParsePropertyName, PropertyToParse);
            RegExPattern = PropertyBagHelper.ReadPropertyBag(propertyBag, RegExPatternPropertyName, RegExPattern);
            DestinationProperty = PropertyBagHelper.ReadPropertyBag(propertyBag, DestinationPropertyPropertyName, DestinationProperty);
            ThrowIfNoMatch = PropertyBagHelper.ReadPropertyBag(propertyBag, ThrowIfNoMatchPropertyName, ThrowIfNoMatch);
        }

        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            PropertyBagHelper.WritePropertyBag(propertyBag, PropertyToParsePropertyName, PropertyToParse);
            PropertyBagHelper.WritePropertyBag(propertyBag, RegExPatternPropertyName, RegExPattern);
            PropertyBagHelper.WritePropertyBag(propertyBag, DestinationPropertyPropertyName, DestinationProperty);
            PropertyBagHelper.WritePropertyBag(propertyBag, ThrowIfNoMatchPropertyName, ThrowIfNoMatch);
        }

      
    }
}
