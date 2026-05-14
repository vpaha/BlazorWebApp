public sealed class DamageSectionType
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsEmergency { get; set; }
}

public sealed class DamageEntrySection
{
    public int Id { get; set; }
    // FK → damage_entries.id
    public int DamageEntryId { get; set; }
    // FK → damage_section_types.id
    public int DamageSectionId { get; set; }

    public string? Entry { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public DamageSectionType? DamageSectionType { get; set; }
}

public class DamageState
{
    public DamageEntry Context { get; set; } = new();
}