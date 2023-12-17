namespace festival.Shared.Services
{
    public class FakeAuthService
    {
        public string UserRole { get; private set; }

        public FakeAuthService()
        {
            Console.WriteLine("FakeAuthService is being constructed");
        }

        public void LoginAs(string role)
        {
            Console.WriteLine($"User is logging in as: {role}");
            UserRole = role;
        }

        public bool IsCoordinator() => UserRole == "Coordinator";
        public bool IsVolunteer() => UserRole == "Volunteer";
    }
}
//virker ikke.