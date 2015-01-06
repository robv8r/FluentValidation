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
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using JetBrains.Annotations;
	using Validators;

	//TODO: Re-visit this interface for FluentValidation v3. Remove some of the duplication.

	/// <summary>
	/// Provides metadata about a validator.
	/// </summary>
	public interface IValidatorDescriptor {
		/// <summary>
		/// Gets the name display name for a property. 
		/// </summary>
		[CanBeNull]
		string GetName([NotNull] string property);
		
		/// <summary>
		/// Gets a collection of validators grouped by property.
		/// </summary>
		[NotNull]
		ILookup<string, IPropertyValidator> GetMembersWithValidators();
		
		/// <summary>
		/// Gets validators for a particular property.
		/// </summary>
		[NotNull]
		IEnumerable<IPropertyValidator> GetValidatorsForMember([NotNull] string name);

		/// <summary>
		/// Gets rules for a property.
		/// </summary>
		[NotNull]
		IEnumerable<IValidationRule> GetRulesForMember([NotNull] string name);
	}
}