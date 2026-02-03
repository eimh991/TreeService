namespace TreeService.DTOs
{
    public class ChangeUserRoleDto
    {
        public int UserId { get; set; }
        public string Role { get; set; } = null!;
    }
}
