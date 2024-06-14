using System.ComponentModel.DataAnnotations;

namespace TPPizza.CustomAttributes
{
    /// <summary>
    /// 
    /// </summary>
    public class IgnoreAttribute : ValidationAttribute
    {
        public override bool IsValid(object value) => true;
    }
}
