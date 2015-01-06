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

namespace FluentValidation.Validators
{
    using Internal;
    using JetBrains.Annotations;

    public class PropertyValidatorContext
    {
        [NotNull]
        private readonly MessageFormatter messageFormatter = new MessageFormatter();
        private bool propertyValueSet;
        [CanBeNull]
        private object propertyValue;

        [NotNull]
        public ValidationContext ParentContext { get; private set; }
        [NotNull]
        public PropertyRule Rule { get; private set; }
        [NotNull]
        public string PropertyName { get; private set; }

        [NotNull]
        public string PropertyDescription
        {
            get { return Rule.GetDisplayName(); }
        }

        [CanBeNull]
        public object Instance
        {
            get { return ParentContext.InstanceToValidate; }
        }

        [NotNull]
        public MessageFormatter MessageFormatter
        {
            get { return messageFormatter; }
        }

        //Lazily load the property value
        //to allow the delegating validator to cancel validation before value is obtained
        [CanBeNull]
        public object PropertyValue
        {
            get
            {
                if (!propertyValueSet)
                {
                    propertyValue = Rule.PropertyFunc(Instance);
                    propertyValueSet = true;
                }

                return propertyValue;
            }
            set
            {
                propertyValue = value;
                propertyValueSet = true;
            }
        }

        public PropertyValidatorContext([NotNull] ValidationContext parentContext, [NotNull] PropertyRule rule, [NotNull] string propertyName)
        {
            ParentContext = parentContext;
            Rule = rule;
            PropertyName = propertyName;
        }
    }
}