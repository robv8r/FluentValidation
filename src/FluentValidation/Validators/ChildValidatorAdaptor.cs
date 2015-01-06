namespace FluentValidation.Validators {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Results;

	public class ChildValidatorAdaptor : NoopPropertyValidator {
		[CanBeNull]
		readonly IValidator validator;

		[NotNull]
		static readonly IEnumerable<ValidationFailure> EmptyResult = Enumerable.Empty<ValidationFailure>();

		[NotNull]
		static readonly Task<IEnumerable<ValidationFailure>> AsyncEmptyResult = TaskHelpers.FromResult(Enumerable.Empty<ValidationFailure>());

		[CanBeNull]
		public IValidator Validator {
			get { return validator; }
		}

		public override bool IsAsync {
			get { return true; }
		}

		public ChildValidatorAdaptor([CanBeNull] IValidator validator) {
			this.validator = validator;
		}

		public override IEnumerable<ValidationFailure> Validate(PropertyValidatorContext context) {
			return ValidateInternal(
				context, 
				(ctx, v) => v.Validate(ctx).Errors,
				EmptyResult
			);
		}

		public override Task<IEnumerable<ValidationFailure>> ValidateAsync(PropertyValidatorContext context) {
			return ValidateInternal(
				context, 
				(ctx, v) => v.ValidateAsync(ctx).Then(r => r.Errors.AsEnumerable(), runSynchronously:true),
				AsyncEmptyResult
			);
		}

		TResult ValidateInternal<TResult>([NotNull] PropertyValidatorContext context, [NotNull] Func<ValidationContext, IValidator, TResult> validationApplicator, [CanBeNull] TResult emptyResult) {
			if (context.Rule.Member == null) {
				throw new InvalidOperationException(string.Format("Nested validators can only be used with Member Expressions."));
			}

			var instanceToValidate = context.PropertyValue;

			if (instanceToValidate == null) {
				return emptyResult;
			}

			var validator = GetValidator(context);

			if (validator == null) {
				return emptyResult;
			}

			var newContext = CreateNewValidationContextForChildValidator(instanceToValidate, context);

			return validationApplicator(newContext, validator);
		}

		[CanBeNull]
		protected virtual IValidator GetValidator([NotNull] PropertyValidatorContext context) {
			return Validator;
		}

		[NotNull]
		protected ValidationContext CreateNewValidationContextForChildValidator([NotNull] object instanceToValidate, [NotNull] PropertyValidatorContext context) {
			var newContext = context.ParentContext.CloneForChildValidator(instanceToValidate);
			newContext.PropertyChain.Add(context.Rule.PropertyName);
			return newContext;
		}
	}
}