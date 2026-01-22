using System;

namespace Rexar.Toolbox.Blueprint
{
    public sealed class BlueprintParameterAttribute : Attribute
    {
        private string parameterHeader;
        internal string ParameterHeader => parameterHeader;

        public BlueprintParameterAttribute(string parameterHeader)
        {
            this.parameterHeader = parameterHeader;
        }

    }

}
