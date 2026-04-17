using Microsoft.AspNetCore.Identity;

// Identity roots
public sealed class AppUser : IdentityUser<int> { }
public sealed class AppRole : IdentityRole<int> { }
