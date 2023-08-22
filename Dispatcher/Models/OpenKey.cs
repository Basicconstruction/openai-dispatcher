using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dispatcher.Models;

public class OpenKey
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long OpenKeyId
    {
        get;
        set;
    }

    [Required]
    public string Key
    {
        get;
        set;
    }

    public PricingMethod? PricingMethod
    {
        get;
        set;
    }

    public int? AvailableRequest
    {
        get;
        set;
    }

    public long? AvailableRequestToken
    {
        get;
        set;
    }

    [Column(TypeName = "decimal(16,8)")]
    public decimal? Balance
    {
        get;
        set;
    }

    public int? KeyUserId
    {
        get;
        set;
    }


    public KeyUser? KeyUser
    {
        get;
        set;
    }

    public bool? Available
    {
        get;
        set;
    }

}