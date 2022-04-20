using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public enum QuestionType
    {
        [Display(Name="close ended single choice")]
        CloseEndedSingleChoice,
        [Display(Name = "close ended multiple choice")]
        CloseEndedMultipleChoice
    }
}
