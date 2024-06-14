using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace TPPizza.CustomAttributes
{
    public class TestAttribute : ValidationAttribute
    {
        public int Min { get; set; }
        public override bool IsValid(object value)
        {
            return value is IList list ? list.Count >= Min && list.Count > 0 : false;
        }
    }
}
