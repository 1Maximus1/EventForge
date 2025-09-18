namespace EventForge.API.Data.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasConversion(
            id => id.Value,               
            guid => EventId.Of(guid));    

        builder.Property(e => e.Category)
            .HasConversion(
                v => v.ToString(),
                s => (EventCategory)Enum.Parse(typeof(EventCategory), s))
            .IsRequired();

        builder.ComplexProperty(e => e.Name, nb =>
        {
            nb.Property(n => n.Value)
              .HasColumnName(nameof(Event.Name))
              .HasMaxLength(100)
              .IsRequired();
        });

        builder.ComplexProperty(e => e.Place, pb =>
        {
            pb.Property(p => p.Value)
              .HasColumnName(nameof(Event.Place))
              .HasMaxLength(200)
              .IsRequired();
        });

        builder.ComplexProperty(e => e.Schedule, sb =>
        {
            sb.Property(s => s.Date)
              .HasColumnName("Date")
              .IsRequired();

            sb.Property(s => s.Time)
              .HasColumnName("Time")
              .IsRequired();
        });

        builder.ComplexProperty(e => e.ImageUrl, ib =>
        {
            ib.Property(i => i.Value)
              .HasColumnName(nameof(Event.ImageUrl))
              .HasMaxLength(2048);
        });

        builder.Property(e => e.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(e => e.AdditionalInfo)
            .HasMaxLength(500);
    }
}

