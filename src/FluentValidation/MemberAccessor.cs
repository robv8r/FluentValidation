namespace FluentValidation
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using Internal;
	using JetBrains.Annotations;

	public static class MemberAccessor<TObject> {
		[NotNull]
		public static MemberAccessor<TObject, TValue> From<TValue>([NotNull] Expression<Func<TObject, TValue>> getExpression) {
			return new MemberAccessor<TObject, TValue>(getExpression);
		} 
	}

	public class MemberAccessor<TObject, TValue> {
		[NotNull]
		readonly Expression<Func<TObject, TValue>> getExpression;
		[NotNull]
		readonly Func<TObject, TValue> getter;
		[NotNull]
		readonly Action<TObject, TValue> setter;

		public MemberAccessor([NotNull] Expression<Func<TObject, TValue>> getExpression) {
			this.getExpression = getExpression;
			getter = getExpression.Compile();
			setter = CreateSetExpression(getExpression).Compile();

			Member = getExpression.GetMember();
		}

		[NotNull]
		static Expression<Action<TObject, TValue>> CreateSetExpression([NotNull] Expression<Func<TObject, TValue>> getExpression) {
			var valueParameter = Expression.Parameter(getExpression.Body.Type);
			var assignExpression = Expression.Lambda<Action<TObject, TValue>>(
				Expression.Assign(getExpression.Body, valueParameter),
				getExpression.Parameters.First(), valueParameter);
			return assignExpression;
		}

		[NotNull]
		public MemberInfo Member { get; private set; }

		[NotNull]
		public TValue Get([NotNull] TObject target) {
			return getter(target);
		}

		public void Set([NotNull] TObject target, [CanBeNull] TValue value) {
			setter(target, value);
		}

		protected bool Equals([NotNull] MemberAccessor<TObject, TValue> other) {
			return Member.Equals(other.Member);
		}

		public override bool Equals([CanBeNull] object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((MemberAccessor<TObject, TValue>)obj);
		}

		public override int GetHashCode() {
			return Member.GetHashCode();
		}

		public static implicit operator Expression<Func<TObject, TValue>>([NotNull] MemberAccessor<TObject, TValue> @this) {
			return @this.getExpression;
		}

		public static implicit operator MemberAccessor<TObject, TValue>([NotNull] Expression<Func<TObject, TValue>> @this) {
			return new MemberAccessor<TObject, TValue>(@this);
		}
	}
}
