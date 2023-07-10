using ExtraDrug.Controllers.Resources;
using Microsoft.AspNetCore.Mvc;

namespace ExtraDrug.Persistence.Services;

public class ResponceBuilder
{
    public SuccessResponce<T> CreateSuccess<T>( string message , T data  , RequestMetaResource? meta = null) 
    {
        return new SuccessResponce<T>()
        {
            Message = message ,
            Data = data ,
            Meta= meta
        };    
    }

    public ErrorResponce CreateFailure(string message,ICollection<string>? errors )
    {
        return new ErrorResponce()
        {
            Message = message,
            Errors= errors
        };
    }

   

}
