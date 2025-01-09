namespace Activator
{
    public interface IOfficeOnlineActivator
    {
        public bool SetOfficeActivationKey(string key, string? osppDirectory);

        public bool SetVisioActivationKey(string key, string? osppDirectory);

        public bool SetKMSServer(string serverName, string? osppDirectory);

        public bool ApplyActivation(string? osppDirectory);

        public bool RemoveActivation(string? osppDirectory);

        public bool RegisterManualRenewalTask();

        public bool RemoveManualRenewalTask();
    }
}
