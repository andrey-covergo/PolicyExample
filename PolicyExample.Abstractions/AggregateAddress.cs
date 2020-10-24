using System;

namespace PolicyExample.Abstractions
{
    public class AggregateAddress : IAggregateAddress
    {
        public AggregateAddress(string id, string type)
        {
            Type = type;
            Id = id;
        }

        public bool Equals(IAggregateAddress? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type && Id == other.Id;
        }

        public string Type { get; }
        public string Id { get; }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            //  if (obj.GetType() != this.GetType()) return false;
            return Equals((AggregateAddress) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Id);
        }

        public static bool operator ==(AggregateAddress? left, IAggregateAddress? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AggregateAddress? left, IAggregateAddress? right)
        {
            return !Equals(left, right);
        }
    }

    public class AggregateAddress<T> : AggregateAddress
    {
        public AggregateAddress(string? id = null) : base(id ?? Guid.NewGuid().ToString(), typeof(T).Name)
        {
        }

        public static bool operator ==(AggregateAddress<T>? left, IAggregateAddress? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AggregateAddress<T>? left, IAggregateAddress? right)
        {
            return !Equals(left, right);
        }
    }
}