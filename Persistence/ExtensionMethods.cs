using FastMember;
using Npgsql;
namespace robot_controller_api.Persistence {
    public static class ExtensionMethods {
        public static void MapTo<T>(this NpgsqlDataReader reader, T entity) 
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            
            var fastMember = TypeAccessor.Create(entity.GetType());
            var props = fastMember.GetMembers().Select(x => x.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < reader.FieldCount; i++) {
                var prop = props.FirstOrDefault(x =>
                x.Equals(reader.GetName(i), StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(prop))fastMember[entity, prop] = reader.IsDBNull(i) ? null : reader.GetValue(i);
            }
        }
    }
}