#region License
// Copyright (c) Jeremy Skinner (http://www.jeremyskinner.co.uk)
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://www.codeplex.com/FluentValidation
#endregion

namespace FluentValidation.Validators {
	using System;
	using System.Collections;
	using System.Reflection;
	using Attributes;
	using Internal;
	using JetBrains.Annotations;
	using Resources;

	public class NotEqualValidator : PropertyValidator, IComparisonValidator {
		[CanBeNull]
		readonly IEqualityComparer comparer;
		
		[CanBeNull]
		readonly Func<object, object> func;

		public NotEqualValidator([NotNull] Func<object, object> func, [CanBeNull] MemberInfo memberToCompare)
			: base(() => Messages.notequal_error) {
			this.func = func;
			MemberToCompare = memberToCompare;
		}

		public NotEqualValidator([NotNull] Func<object, object> func, MemberInfo memberToCompare, [CanBeNull] IEqualityComparer equalityComparer)
			: base(() => Messages.notequal_error) {
			this.func = func;
			this.comparer = equalityComparer;
			MemberToCompare = memberToCompare;
		}

		public NotEqualValidator([CanBeNull] object comparisonValue)
			: base(() => Messages.notequal_error) {
			ValueToCompare = comparisonValue;
		}

		public NotEqualValidator([CanBeNull] object comparisonValue, [CanBeNull] IEqualityComparer equalityComparer)
			: base(() => Messages.notequal_error) {
			ValueToCompare = comparisonValue;
			comparer = equalityComparer;
		}

		protected override bool IsValid(PropertyValidatorContext context) {
			var comparisonValue = GetComparisonValue(context);
			bool success = !Compare(comparisonValue, context.PropertyValue);

			if (!success) {
				context.MessageFormatter.AppendArgument("ComparisonValue", comparisonValue);
				return false;
			}

			return true;
		}

		private object GetComparisonValue([NotNull] PropertyValidatorContext context) {
			if (func != null) {
				return func(context.Instance);
			}

			return ValueToCompare;
		}

		public Comparison Comparison {
			get { return Comparison.NotEqual; }
		}

		public MemberInfo MemberToCompare { get; private set; }
		public object ValueToCompare { get; private set; }

		protected bool Compare([CanBeNull] object comparisonValue, [CanBeNull] object propertyValue) {
			if(comparer != null) {
				return comparer.Equals(comparisonValue, propertyValue);
			}

			if (comparisonValue is IComparable && propertyValue is IComparable) {
				return Internal.Comparer.GetEqualsResult((IComparable)comparisonValue, (IComparable)propertyValue);
			}

			return comparisonValue == propertyValue;
		}
	}
}