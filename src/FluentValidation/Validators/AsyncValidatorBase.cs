namespace FluentValidation.Validators {
	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	public abstract class AsyncValidatorBase : PropertyValidator {
		public override bool IsAsync {
			get { return true; }
		}

		protected AsyncValidatorBase([NotNull] string errorMessageResourceName, [NotNull] Type errorMessageResourceType)
			: base(errorMessageResourceName, errorMessageResourceType) {
		}

		protected AsyncValidatorBase([NotNull] string errorMessage)
			: base(errorMessage) {
		}

		protected AsyncValidatorBase([NotNull] Expression<Func<string>> errorMessageResourceSelector)
			: base(errorMessageResourceSelector) {
		}

		protected override bool IsValid(PropertyValidatorContext context) {
			return IsValidAsync(context).Result;
		}

		protected abstract override Task<bool> IsValidAsync(PropertyValidatorContext context);
	}
}