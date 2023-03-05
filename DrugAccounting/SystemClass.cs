namespace DrugAccounting
{
    public static class SystemClass
    {
        public static bool IsTableExists
        {
            get
            {
                try
                {
                    SqlDataAccess.LoadPatients();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
