using Domain.Entity.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Mapping
{
    internal partial class UserEntityMap
    {
        public class PersonEntityMap : IEntityTypeConfiguration<PersonEntity>
        {
            public void Configure(EntityTypeBuilder<PersonEntity> builder)
            {
                builder.ToTable("Person");

                builder.HasKey(x => x.Id);
                builder.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                builder.Property(x => x.FullName).IsRequired();
                builder.Property(x => x.BirthDate).IsRequired();
                builder.Property(x => x.IncomeValue).IsRequired();
                builder.Property(x => x.Cpf).IsRequired();
                builder.Property(x => x.CreateDate).IsRequired();
                builder.Property(x => x.UpdateDate);
            }
        }
    }
}
