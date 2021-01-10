namespace Xant.MVC.Models.Constants
{
    public class ConstantValidationErrorMessages
    {
        public const string RequiredMsg = "{0} " + "را وارد نمایید";
        public const string MaxLengthMsg = "{0} " + "نباید بیشتر از " + "{1} " + "کاراکتر باشد";
        public const string MinLengthMsg = "{0} " + "نباید کمتر از " + "{1} " + "کاراکتر باشد";
        public const string RegexMsg = "قالب " + "{0} " + "اشتباه است";
        public const string RangeMsg = "مقدار " + "{0} " + "باید بین " + "{1} " + "تا " + "{2} " + "باشد";
        public const string RemoteMsg = "این " + "{0} " + "تکراری می باشد. " + "{0} " + "دیگری را وارد نمایید";
        public const string CompareMsg = "مقدار " + "{0} " + "و " + "{1} " + "برابر نیستند";
        public const string StringLengthMsg = "طول " + "{0} " + "وارد شده صحیح نمی باشد";
        public const string DuplicateMsg = "{0} " + "تکراری می باشد. مقدار دیگری را وارد نمایید.";
        public const string IsNotValid = "{0} " + "تکراری می باشد. مقدار دیگری را وارد نمایید.";
    }
}
