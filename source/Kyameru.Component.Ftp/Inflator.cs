using Kyameru.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kyameru.Component.Ftp
{
    public class Inflator : IOasis
    {
        public IFromComponent CreateFromComponent(Dictionary<string, string> headers)
        {
            return new From(headers);
        }

        public IToComponent CreateToComponent(Dictionary<string, string> headers)
        {
            throw new NotImplementedException();
        }
    }
}