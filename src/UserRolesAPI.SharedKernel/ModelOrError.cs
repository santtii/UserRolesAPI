using Microsoft.AspNetCore.Mvc;

namespace UserRolesAPI.SharedKernel;

public class ModelOrError<T>
{
    public T? Model { get; set; }
    public int Error { get; set; }

    public ModelOrError<T> AddError(ErrorCode error)
    {
        Error = (int)error;
        return this;
    }

    public ModelOrError<T> AddError(int error)
    {
        Error = error;
        return this;
    }

    public ModelOrError<T> AddModel(T model)
    {
        Model = model;
        return this;
    }

    public IActionResult GetResult()
    {
        return Error == 0 ? new OkObjectResult(Model) : new BadRequestObjectResult(Error);
    }
}
