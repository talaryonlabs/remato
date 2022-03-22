namespace Remato
{
    public interface IIssueParams
    {
        IIssueParams Id(string issueId);
        IIssueParams Issuer(string issuer);
    }
}