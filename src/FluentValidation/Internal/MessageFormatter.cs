namespace FluentValidation.Internal {
	using System.Collections.Generic;
	using JetBrains.Annotations;

	/// <summary>
	/// Assists in the construction of validation messages.
	/// </summary>
	public class MessageFormatter {
		[NotNull]
		readonly Dictionary<string, object> placeholderValues = new Dictionary<string, object>();
		object[] additionalArgs;

		/// <summary>
		/// Default Property Name placeholder.
		/// </summary>
		public const string PropertyName = "PropertyName";

		/// <summary>
		/// Adds a value for a validation message placeholder.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		[NotNull]
		public MessageFormatter AppendArgument([NotNull] string name, object value) {
			placeholderValues[name] = value;
			return this;
		}

		/// <summary>
		/// Appends a property name to the message.
		/// </summary>
		/// <param name="name">The name of the property</param>
		/// <returns></returns>
		[NotNull]
		public MessageFormatter AppendPropertyName(string name) {
			return AppendArgument(PropertyName, name);
		}

		/// <summary>
		/// Adds additional arguments to the message for use with standard string placeholders.
		/// </summary>
		/// <param name="additionalArgs">Additional arguments</param>
		/// <returns></returns>
		[NotNull]
		public MessageFormatter AppendAdditionalArguments(params object[] additionalArgs) {
			this.additionalArgs = additionalArgs;
			return this;
		}

		/// <summary>
		/// Constructs the final message from the specified template. 
		/// </summary>
		/// <param name="messageTemplate">Message template</param>
		/// <returns>The message with placeholders replaced with their appropriate values</returns>
		[NotNull]
		public string BuildMessage([NotNull] string messageTemplate) {

			string result = messageTemplate;

			foreach(var pair in placeholderValues) {
				result = ReplacePlaceholderWithValue(result, pair.Key, pair.Value);
			}

			if(ShouldUseAdditionalArgs) {
				return string.Format(result, additionalArgs);
			}
			return result;
		}

		private bool ShouldUseAdditionalArgs {
			get { return additionalArgs != null && additionalArgs.Length > 0; }
		}

		[NotNull]
		string ReplacePlaceholderWithValue([NotNull] string template, [NotNull] string key, [CanBeNull] object value) {
			string placeholder = "{" + key + "}";
			return template.Replace(placeholder, value == null ? null : value.ToString());
		}
	}
}