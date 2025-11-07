using Trackin.Domain.Entity;

namespace Trackin.Tests.Extensions
{
    public static class PatioExtensions
    {
        public static void SetId(this Patio patio, long id)
        {
            var idProperty = typeof(Patio).GetProperty("Id");
            if (idProperty != null)
            {
                idProperty.SetValue(patio, id);
            }
        }
    }
}