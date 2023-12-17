namespace festival.Shared.Services
{
    public class FakeAuthService
    {
        public string UserRole { get; private set; }

        public void LoginAs(string role)
        {
            UserRole = role;
        }

        public bool IsCoordinator() => UserRole == "Coordinator";
        public bool IsVolunteer() => UserRole == "Volunteer";
    }
}
