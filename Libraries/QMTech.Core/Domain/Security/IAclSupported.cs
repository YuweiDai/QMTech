namespace QMTech.Core.Domain.Security
{
    /// <summary>
    /// 实体是否支持ACL
    /// </summary>
    public interface IAclSupported
    {
        bool SubjectToAcl { get; set; }
    }
}
