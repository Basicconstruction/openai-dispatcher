using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dispatcher.Models;


//api key 用户，可以登录用户自己的后台来查看自己的密钥
// 消费记录等
public class KeyUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long KeyUserId
    {
        get;
        set;
    }

    [Required]
    public string UserName
    {
        get;
        set;
    }

    [Required]
    public string Password
    {
        get;
        set;
    }

    [Required]
    [Column(TypeName = "decimal(16,8)")]
    public decimal Balance
    {
        get;
        set;
    }

    public IEnumerable<OpenKey>? OpenKeys
    {
        get;
        set;
    }

    
}