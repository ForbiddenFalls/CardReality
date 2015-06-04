namespace CardReality.WebJob
{
    using Microsoft.Azure.WebJobs;

    class Program
    {
        static void Main()
        {
            JobHost host = new JobHost();
            host.RunAndBlock();
        }
    }
}
