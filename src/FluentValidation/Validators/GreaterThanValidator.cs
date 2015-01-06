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
	using System.Reflection;
	using Attributes;
	using Internal;
	using JetBrains.Annotations;
	using Resources;

	public class GreaterThanValidator : AbstractComparisonValidator {
		public GreaterThanValidator([NotNull] IComparable value) : base(value, () => Messages.greaterthan_error) {
		}

		public GreaterThanValidator([NotNull] Func<object, object> valueToCompareFunc, [CanBeNull] MemberInfo member)
			: base(valueToCompareFunc, member, () => Messages.greaterthan_error) {
		}

		public override bool IsValid(IComparable value, IComparable valueToCompare) {
			if (valueToCompare == null)
				return false;

			return Comparer.GetComparisonResult(value, valueToCompare) > 0;
		}

		public override Comparison Comparison {
			get { return Validators.Comparison.GreaterThan; }
		}
	}
}