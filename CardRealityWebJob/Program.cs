namespace CardRealityWebJob
{
    using Microsoft.Azure.WebJobs;

    class Program
    {
        static void Main()
        {
            var host = new JobHost();
            host.RunAndBlock();
        }
    }
}
