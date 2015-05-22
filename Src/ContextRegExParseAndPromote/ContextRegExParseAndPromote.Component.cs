using System;
using System.Collections;
using System.Linq;
using BizTalkComponents.Utils;

namespace BizTalkComponents.PipelineComponents.ContextRegExParseAndPromote
{
    public partial class ContextRegExParseAndPromote 
    {
        public string Name { get { return "ContextRegExParseAndPromote"; } }
        public string Version { get { return "1.0"; } }
        public string Description { get
        {
            return
                "Reads a property from context, apply a regular expression on it and promotes the result to another property. ";
        } }
        
        public void GetClassID(out Guid classID)
        {
            classID = Guid.Parse("62382B52-24F9-405C-8E2C-09CCDB42B156");
        }

        public void InitNew()
        {
            
        }

        public IEnumerator Validate(object projectSystem)
        {
            return ValidationHelper.Validate(this, false).ToArray().GetEnumerator();
        }

        public bool Validate(out string errorMessage)
        {
            var errors = ValidationHelper.Validate(this, true).ToArray();

            if (errors.Any())
            {
                errorMessage = string.Join(",", errors);

                return false;
            }

            errorMessage = string.Empty;

            return true;
        }

        public IntPtr Icon { get { return IntPtr.Zero; } }
         
    }
}