namespace Activator
{
    public interface IOnlineActivator
    {
        public bool SetActivationKey(string key);

        public bool SetKMSServer(string serverName);

        public bool ApplyActivation();

        public bool RemoveActivation();

        public bool RegisterManualRenewalTask();

        public bool RemoveManualRenewalTask();
    }
}
