namespace Application.Common.Constants;

public static class Permissions
{
    public static class Users
    {
        public const string Read = "user:read";
        public const string Create = "user:create";
        public const string CreateOperator = "user:create-operator";
        public const string CreateCoordinator = "user:create-coordinator";
    }

    public static class Consultas
    {
        public const string Read = "consultas:read";
    }

    public static class Vehiculos
    {
        public const string Read = "vehiculo:read";
        public const string Create = "vehiculo:create";
        public const string Update = "vehiculo:update";
        public const string Delete = "vehiculo:delete";
    }
}