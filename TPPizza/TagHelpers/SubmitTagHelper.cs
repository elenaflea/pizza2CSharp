using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TPPizza.TagHelpers {
    public class SubmitTagHelper : TagHelper {

        public string? Label { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "input";
            output.Attributes.SetAttribute("type", "submit");
            if(Label != null) {
                output.Attributes.SetAttribute("value", Label);
            }
        }
    }
}
