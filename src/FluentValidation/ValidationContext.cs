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

namespace FluentValidation {
	using Internal;
	using JetBrains.Annotations;

	public class ValidationContext<T> : ValidationContext {
		public ValidationContext([NotNull] T instanceToValidate) : this(instanceToValidate, new PropertyChain(), new DefaultValidatorSelector()) {
			
		}

		public ValidationContext([NotNull] T instanceToValidate, [NotNull] PropertyChain propertyChain, [NotNull] IValidatorSelector validatorSelector)
			: base(instanceToValidate, propertyChain, validatorSelector) {

			InstanceToValidate = instanceToValidate;
		}

		[NotNull]
		public new T InstanceToValidate { get; private set; }
	}

	public class ValidationContext {

		public ValidationContext([NotNull] object instanceToValidate)
		 : this (instanceToValidate, new PropertyChain(), new DefaultValidatorSelector()){
			
		}

		public ValidationContext([NotNull] object instanceToValidate, [NotNull] PropertyChain propertyChain, [NotNull] IValidatorSelector validatorSelector) {
			PropertyChain = new PropertyChain(propertyChain);
			InstanceToValidate = instanceToValidate;
			Selector = validatorSelector;
		}

		[NotNull]
		public PropertyChain PropertyChain { get; private set; }
		[NotNull]
		public object InstanceToValidate { get; private set; }
		[NotNull]
		public IValidatorSelector Selector { get; private set; }
		public bool IsChildContext { get; internal set; }

		[NotNull]
		public ValidationContext Clone([CanBeNull] PropertyChain chain = null, [CanBeNull] object instanceToValidate = null, [CanBeNull] IValidatorSelector selector = null) {
			return new ValidationContext(instanceToValidate ?? this.InstanceToValidate, chain ?? this.PropertyChain, selector ?? this.Selector);
		}

		[NotNull]
		internal ValidationContext CloneForChildValidator([NotNull] object instanceToValidate) {
			return new ValidationContext(instanceToValidate, PropertyChain, Selector) {
				IsChildContext = true
			};
		}
	}
}