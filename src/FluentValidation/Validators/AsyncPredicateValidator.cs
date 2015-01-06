namespace FluentValidation.Validators
{
	using System;
	using System.Threading.Tasks;
	using FluentValidation.Internal;
	using FluentValidation.Resources;
	using JetBrains.Annotations;

	public class AsyncPredicateValidator : AsyncValidatorBase
	{
		[NotNull]
		private readonly Func<object, object, PropertyValidatorContext, Task<bool>> predicate;
		public AsyncPredicateValidator([NotNull] Func<object, object, PropertyValidatorContext, Task<bool>> predicate)
			: base(() => Messages.predicate_error)
		{
			predicate.Guard("A predicate must be specified.");
			this.predicate = predicate;
		}

		protected override Task<bool> IsValidAsync(PropertyValidatorContext context)
		{
			return predicate(context.Instance, context.PropertyValue, context);
		}
	}
}