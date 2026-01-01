using CleanModularTemplate.Accounts.Domain.Customers;
using CleanModularTemplate.Accounts.Domain.Customers.Entities;
using CleanModularTemplate.Accounts.Domain.Customers.ValueObjects;
using CleanModularTemplate.Accounts.Domain.Shared;
using CleanModularTemplate.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanModularTemplate.Accounts.Infrastructure.Config;

public class CustomerConfigurations : IEntityTypeConfiguration<Customer>
{

  public void Configure(EntityTypeBuilder<Customer> builder)
  {
	builder.ToTable("Customers");
	builder.HasKey(a => a.Id);
	builder.Property(a => a.Id)
		.ValueGeneratedNever()
		.IsRequired();

	builder.Property(a => a.FullName)
		.HasMaxLength(AccountConstants.MaxNameLength)
		.IsRequired();

	builder.Property(a => a.DefaultAddressId)
		.ValueGeneratedNever();

	builder.Property(a => a.UserId)
		.ValueGeneratedNever()
		.IsRequired();

	builder.HasIndex(x => x.UserId)
		.IsUnique();

	builder.ConfigureAuditable();

	// ADDRESSES CONFIGURATION
	builder.OwnsMany(e => e.Addresses, addressBuilder =>
	{
	  addressBuilder.ToTable("Addresses");

	  addressBuilder.WithOwner().HasForeignKey("CustomerId");
	  addressBuilder.HasKey(a => a.Id);
	  addressBuilder.Property(a => a.Id)
			  .ValueGeneratedNever()
			  .IsRequired();

	  addressBuilder.Property(a => a.Alias)
			  .HasMaxLength(CustomerConstants.MaxAliasLength);

	  addressBuilder.OwnsOne(a => a.PostalDetails, pd =>
		  {
		  pd.Property(x => x.Street)
				  .HasMaxLength(CustomerConstants.MaxStreetLength)
				  .IsRequired()
				  .HasColumnName(nameof(PostalDetails.Street));


		  pd.Property(x => x.BuildingNumber)
				  .HasColumnName(nameof(PostalDetails.BuildingNumber));

		  pd.Property(x => x.SecondaryNumber)
				  .HasColumnName(nameof(PostalDetails.SecondaryNumber));


		  pd.Property(x => x.District)
				  .HasMaxLength(CustomerConstants.MaxStateLength)
				  .IsRequired()
				  .HasColumnName(nameof(PostalDetails.District));
		  pd.Property(x => x.City)
				  .HasMaxLength(CustomerConstants.MaxCityLength)
				  .IsRequired()
				  .HasColumnName(nameof(PostalDetails.City));


		  pd.Property(x => x.PostalCode)
				  .HasMaxLength(CustomerConstants.MaxPostalCodeLength)
				  .IsRequired()
				  .HasColumnName(nameof(PostalDetails.PostalCode));
		});


	  addressBuilder.OwnsOne(a => a.Coordinates, c =>
		  {
		  c.Property(x => x.Latitude).IsRequired().HasColumnName("Latitude");
		  c.Property(x => x.Longitude).IsRequired().HasColumnName("Longitude");
		});
	});

	builder.Navigation(e => e.Addresses)
		.UsePropertyAccessMode(PropertyAccessMode.Field);
  }
}
