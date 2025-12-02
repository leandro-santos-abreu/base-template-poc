using Microsoft.EntityFrameworkCore;

namespace BaseTemplatePoc.DbContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options);