using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace StackUnderflow.ViewModels
{
    public class QuestionViewModel
    {
		public string Title { get; set; }
		public string Body {  get; set; }
		public string SelectedTags { get; set; }
		[ValidateNever]
		public List<string> AvailableTags { get; set; }
	}
}   
